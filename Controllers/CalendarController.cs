using Microsoft.AspNetCore.Mvc;
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
        // 
        // GET: /Calendar/

        public IActionResult Index()
        {            
            return  View(ReadAllNotes().allNotes);
        }
      //  [Route("/Calendar/Edit/", Name = "Edit")]
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
                                

                                    });
            return  View("Edit",notes.Find(note => note.id == notes.Count -1));
        }
        
        public IActionResult SubmitChanges(int id, String title,string text,DateTime date,string category)
        {
            Note modifiedNote = notes.Find(note => note.id == id);
            string path = Path.Combine(dirName,modifiedNote.title);
            text = "category:"+category+'\n'+"date:"+date.ToString("MM/dd/yyyy")+'\n'+text;
            if (System.IO.File.Exists(path))
                System.IO.File.WriteAllText(path,text);
            else
                System.IO.File.WriteAllText(Path.Combine(dirName,title),text);
             
            
            return View("Index",ReadAllNotes().allNotes);
        }

        public IActionResult DeleteFile(int id)
        {
            Note modifiedNote = notes.Find(note => note.id == id);
            string path = Path.Combine(dirName,modifiedNote.title);
            System.IO.File.Delete(path);
            return View("Index",ReadAllNotes().allNotes);
        }

        private Models.Notebook ReadAllNotes()
        {
            ReadFiles();
            var notebook = new Models.Notebook
            {
                allNotes = notes
            };
            return notebook;
        }
        private void EditFile(string path)
        {
            
        }




        
    }
}