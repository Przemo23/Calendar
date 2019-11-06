using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using P01.Models;
using static P01.Models.Notebook;
using static P01.NoteReader;
using static P01.Controllers.CalendarController;


namespace P01
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ReadAllNotes(0);
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
