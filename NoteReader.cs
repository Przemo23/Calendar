using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using P01.Models;

namespace P01

{
    public static class NoteReader{
        public static string dirName;
        public static int noteToDisplayId;
        public static List<Note> notes;
        static NoteReader()
        {
            notes = new List<Note>();
            dirName = Environment.CurrentDirectory+"/Notes";
        }
        public static void ReadFiles(int recentFileId)
        {
            string noteToDisplayTitle = null;
            if(notes.Count >0)    
                noteToDisplayTitle = notes.ElementAt(recentFileId).title;
            notes.Clear();
            string[] fileNames = Directory.GetFiles(dirName);
            int fileCounter = 0;
            foreach(string name in fileNames)
            {
                ReadFile(name);
                fileCounter++;
            }
            assignNewIds();
            if(String.IsNullOrEmpty(noteToDisplayTitle))
                noteToDisplayId = 0;
            else
                noteToDisplayId = notes.Find(note => note.title == noteToDisplayTitle).id;
            
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
                curNote.title = name.Substring(name.LastIndexOf('/')+1);
                addInDateOrder(curNote);
            }
            file.Close();
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
            if(notes.Count == 0)
            {
                notes.Add(curNote);
                return;
            }
            int i = -1;
            foreach(Note note in notes )
            {
                i++;
                if(DateTime.Compare(curNote.date,note.date) <= 0)
                {    
                    notes.Insert(i,curNote);
                    return;
                }
            }
            notes.Add(curNote);
            
        }
        private static void assignNewIds()
        {
            int i = 0;
            foreach(Note note in notes)
            {
                note.id = i;
                i++;
            }
        }
    }
}