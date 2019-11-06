﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Collections.Generic;
using static P01.NoteReader;
using P01.Models;


namespace P01.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {            
            return  View(ReadAllNotes(0));
        }
        public IActionResult NextPage()
        {
            if( noteToDisplayId/10 * 10 + 10 < notes.Count )
                noteToDisplayId = noteToDisplayId/10 * 10 + 10;

            return View("Index",ReadAllNotes(noteToDisplayId));
        }
        public IActionResult PreviousPage()
        {
            if( noteToDisplayId - 10 >= 0)
                noteToDisplayId = noteToDisplayId - 10;
            return View("Index",ReadAllNotes(noteToDisplayId));
        }
        public IActionResult Edit(int id)
        {
            return  View(notes.Find(note => note.id == id));
        }
        public IActionResult New()
        {
            notes.Add(new Note{
                                    id = notes.Count,
                                    title = "New",
                                    text = "Your text",
                                    category = "Category",
                                    date = DateTime.Now,
                                    isMarkdown = false                                
                                    });
            return  View("Edit",notes.Find(note => note.id == notes.Count -1));
        }        
        public IActionResult SubmitChanges(int id, String title,string text,DateTime date,string category,bool markdown)
        {
            if(!IsNameUnique(id,title))
            {
                notes.Find(note => note.id == id).title = "A file with such a name already exists";
                return View("Edit",notes.Find(note => note.id == id));
            }
            Note modifiedNote = notes.Find(note => note.id == id);
            modifiedNote.date = date;
            modifiedNote.isMarkdown = markdown;           
            text = "category:"+category+'\n'+"date:"+date.ToString("MM/dd/yyyy")+'\n'+text;
            if (IsEditing()) // Editing note
            {
                string path = Path.Combine(dirName,modifiedNote.title);
                System.IO.File.WriteAllText(path,text);
                System.IO.File.Move(path, Path.Combine(dirName,title));
                modifiedNote.title = title;
            }
            else //creating new
            {
                modifiedNote.title = title;
                System.IO.File.WriteAllText(Path.Combine(dirName,title),text);        
            }
            if(modifiedNote.isMarkdown)
                modifiedNote.title += ".md";
            return View("Index",ReadAllNotes(id));
        }
        public IActionResult DeleteFile(int id)
        {
            Note modifiedNote = notes.Find(note => note.id == id);
            string path = Path.Combine(dirName,modifiedNote.title);
            System.IO.File.Delete(path);
            int indexToPass = id == 0 ? 1 : id - 1;
            return View("Index",ReadAllNotes(indexToPass));
        }
        public IActionResult Filter(DateTime from, DateTime to, string category)
        {
            
            if(DateTime.Compare(from,to) > 0)
                return View("Index",ReadAllNotes(0));
            ReadAllNotes(0); // Update the notes list
            Notebook filteredNotes = new Notebook{ allNotes = notes};

            for(var i = filteredNotes.allNotes.Count -1 ; i>=0; i--)
            {
                var note = filteredNotes.allNotes.Find(fnote => fnote.id == i);
                //if(!string.IsNullOrEmpty(category))
                if(DateTime.Compare(from, note.date ) > 0 || DateTime.Compare(note.date, to) > 0 || (note.category != category && !string.IsNullOrEmpty(category)))
                    filteredNotes.allNotes.RemoveAt(note.id);
            }
            return View("Index",filteredNotes);
        }
        private bool IsNameUnique(int id, string title)
        {
            if(notes.Exists(note => note.title == title))
                if(notes.Find(note => note.id == id).title != title)
                    return false;
            return true;
        }
        private bool IsEditing()
        {
            return notes.Count == Directory.GetFiles(dirName).Length;
        }
        private Models.Notebook ReadAllNotes(int recentNoteId)
        {
            ReadFiles(recentNoteId);
            var notebook = new Models.Notebook
            {
                allNotes = notes,
                currentNoteId = noteToDisplayId
            };
            return notebook;
        }
        
        




        
    }
}