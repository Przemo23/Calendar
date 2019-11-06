using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P01.Models
{
    public class Notebook
    {
        public List<Note> allNotes {get;set;}
        public int currentNoteId {get;set;}
    }
}

