using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using P01.Models;
using static P01.Models.Notebook;

namespace P01

{
    public static class NoteReader{
        public static string dirName;
        
        static NoteReader()
        {
            allNotes = new List<Note>();
            dirName = Environment.CurrentDirectory+"/Notes";
        }
        public static void ReadFiles(int recentFileId)
        {
            string noteToDisplayTitle = null;
            if(allNotes.Count >0)    
                noteToDisplayTitle = allNotes.ElementAt(recentFileId).title;
            allNotes.Clear();
            string[] fileNames = Directory.GetFiles(dirName);
            foreach(string name in fileNames)
            {
                ReadFile(name);
            }
            assignNewIds();
            if(String.IsNullOrEmpty(noteToDisplayTitle))
                currentNoteId = 0;
            else
                currentNoteId = allNotes.Find(note => note.title == noteToDisplayTitle).id;
            
        }
        private static void ReadFile(string name)
        {
            StreamReader file = new StreamReader(name);
            string line;
            int lineCounter = 0;
            Note curNote = new Note();
            bool properFormat = true;
            while((line = file.ReadLine())!= null && properFormat)
            {
                if(lineCounter == 0) // decoding line by line
                {
                    
                    if(line.Substring(0,9)!="category:")
                        properFormat = false;
                    else
                        curNote.category = line.Substring(9);  
                }
                else if(lineCounter ==1)
                {
                    if(line.Substring(0,5)!="date:")
                        properFormat = false;
                    else
                        curNote.date = parseToDate(line);
                      
                }
                else
                    curNote.text += line;                
                lineCounter++;
            }
            if(properFormat && lineCounter > 0)
            {
                curNote = MdCheck(name.Substring(name.LastIndexOf('/')+1),curNote);
                addInDateOrder(curNote);
            }
            file.Close();
            
        }
        private static Note MdCheck(string name, Note note)
            {
                if(name.EndsWith(".md") )
                {
                    note.title = name.Remove(name.Length-3,3);
                    note.isMarkdown = true;
                }
                else
                {    
                    note.isMarkdown = false;
                    note.title = name;
                }
                return note;
            }
        
        private static DateTime parseToDate(string line)
        {
            int year = Int32.Parse(line.Substring(11));
            int month = Int32.Parse(line.Substring(5,2));
            int day = Int32.Parse(line.Substring(8,2));
            return new DateTime(year,month,day); 
        }
        private static void addInDateOrder(Note curNote)
        {
            if(allNotes.Count == 0)
            {
                allNotes.Add(curNote);
                return;
            }
            int i = -1;
            foreach(Note note in allNotes )
            {
                i++;
                if(DateTime.Compare(curNote.date,note.date) <= 0)
                {    
                    allNotes.Insert(i,curNote);
                    return;
                }
            }
            allNotes.Add(curNote);
            
        }
        private static void assignNewIds()
        {
            int i = 0;
            foreach(Note note in allNotes)
            {
                note.id = i;
                i++;
            }
        }
    }
}