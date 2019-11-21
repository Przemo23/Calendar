using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P01.Models
{
    public class Note
    {
       // Note(){noteCategories = new List<String>();}
        public String title {get;set;}
        public DateTime date {get;set;}
        public List<String> noteCategories {get;set;}
        public int id{get;set;}
        public String text {get;set;}
        public bool isMarkdown {get;set;}
        
    }
}