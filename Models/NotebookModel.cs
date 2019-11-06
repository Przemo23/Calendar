using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P01.Models
{
    public static class Notebook
    {
        static Notebook()
        {
            displayedNotes = allNotes = new List<Note>();
            currentNoteId = 0;

        }
        public static List<Note> displayedNotes {get;set;}
        public static int currentNoteId {get;set;}
        public static List<Note> allNotes {get;set;}
        public static DateTime fromFilterDate {get;set;}
        public static DateTime toFilterDate {get;set;}
        public static string filteredCategory {get;set;}


        
    }
}

