using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Collections.Generic;
using static P01.NoteReader;


namespace P01.Controllers
{
    public class CalendarController : Controller
    {
        // 
        // GET: /Calendar/

        public IActionResult Index()
        {
            ReadFiles();
            var notebook = new Models.Notebook
            {
                allNotes = notes
            };
            
            return  View(notebook.allNotes);
        }
      //  [Route("/Calendar/Edit/", Name = "Edit")]
        public IActionResult Edit(uint ID)
        {
            return  View(notes.Find(note => note.id == ID));
        }
        public IActionResult New()
        {
            return  View("Edit");
        }



        
    }
}