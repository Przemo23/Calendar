using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Collections.Generic;
using static P01.NoteReader;
using static P01.Models.Notebook;
using P01.Models;


namespace P01.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {            
           // ReadAllNotes(0);
            return  View();
        }
        public IActionResult NextPage()
        {
            if(currentNoteId/10 * 10 + 10 < allNotes.Count )
                currentNoteId = currentNoteId/10 * 10 + 10;

            return View("Index");
        }
        public IActionResult PreviousPage()
        {
            if(currentNoteId - 10 >= 0)
                currentNoteId = currentNoteId - 10;
            return View("Index");
        }
        public IActionResult Edit(int id)
        {
            return  View(allNotes.Find(note => note.id == id));
        }
        public IActionResult New()
        {
            allNotes.Add(new Note{
                                    id = allNotes.Count,
                                    title = "New",
                                    text = "Your text",
                                    category = "Category",
                                    date = DateTime.Now,
                                    isMarkdown = false                               
                                    });
            return  View("Edit",allNotes.Find(note => note.id == allNotes.Count -1));
        }        
        public IActionResult SubmitChanges(int id, String title,string text,DateTime date,string category,bool markdown)
        {
            if(!IsNameUnique(id,title))
            {
                allNotes.Find(note => note.id == id).title = "A file with such a name already exists";
                return View("Edit",allNotes.Find(note => note.id == id));
            }
            Note modifiedNote = allNotes.Find(note => note.id == id);
            modifiedNote.date = date;
            modifiedNote.isMarkdown = markdown;           
            text = "category:"+category+'\n'+"date:"+date.ToString("MM/dd/yyyy")+'\n'+text;
            if (IsEditing()) // Editing note
            {
                string path = Path.Combine(dirName,modifiedNote.title);
                if(modifiedNote.isMarkdown)
                    path += ".md";
                System.IO.File.WriteAllText(path,text);
                System.IO.File.Move(path, Path.Combine(dirName,title));
                modifiedNote.title = title;
            }
            else //creating new
            {
                modifiedNote.title = title;
                if(modifiedNote.isMarkdown)
                    title += ".md";
                System.IO.File.WriteAllText(Path.Combine(dirName,title),text);        
            }
            
            ReadAllNotes(id);
            return View("Index");
        }
        public IActionResult DeleteFile(int id)
        {
            Note modifiedNote = allNotes.Find(note => note.id == id);
            string path = Path.Combine(dirName,modifiedNote.title);
            System.IO.File.Delete(path);
            allNotes.RemoveAt(modifiedNote.id);
            displayedNotes.Remove(displayedNotes.Find(note=>note.id == id));
            return View("Index");
        }
        public IActionResult AddCategory(int id, string category)
        {
            AddCategory(category);
            allNotes.Find(note => note.id == id).category = category;
            return  View("Edit",allNotes.Find(note => note.id == id));
        }
        public IActionResult DeleteFilter()
        {
            
            ReadAllNotes(0);
            return View("Index");
        }
        public IActionResult Filter(DateTime from, DateTime to, string category)
        {
            
            if(DateTime.Compare(from,to) > 0)
                return View("Index");
            ReadAllNotes(0); // Update the notes list
            CopyToDisplayNotes();
            fromFilterDate = from;
            toFilterDate = to;
            filteredCategory = category;

            for(var i = displayedNotes.Count -1 ; i>=0; i--)
            {
                var note = displayedNotes.Find(fnote => fnote.id == i);
                //if(!string.IsNullOrEmpty(category))
                if(DateTime.Compare(from, note.date ) > 0 || DateTime.Compare(note.date, to) > 0 || (note.category != category && !string.IsNullOrEmpty(category)))
                    displayedNotes.RemoveAt(note.id);
            }
            return View("Index");
        }
        private bool IsNameUnique(int id, string title)
        {
            if(allNotes.Exists(note => note.title == title))
                if(allNotes.Find(note => note.id == id).title != title)
                    return false;
            return true;
        }
        private bool IsEditing()
        {
            return allNotes.Count == Directory.GetFiles(dirName).Length;
        }
        public static void ReadAllNotes(int recentNoteId)
        {
            ReadFiles(recentNoteId);
            CopyToDisplayNotes();
            fromFilterDate = DateTime.Now;
            toFilterDate = DateTime.Parse("12/12/2030");
            filteredCategory = "";
            
            
            
        }
        private static void AddCategory(string category)
        {
            AddCategoryToList(category);
        }
        private static void CopyToDisplayNotes()
        {
            displayedNotes.Clear();
            foreach(Note note in allNotes)
            {
                displayedNotes.Add(note);
            }
        }
        
        




        
    }
}