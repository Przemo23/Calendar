using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace P01.Controllers
{
    public class CalendarController : Controller
    {
        // 
        // GET: /Calendar/

        public IActionResult Index()
        {
            return  View();
        }
        [Route("/Calendar/Edit", Name = "Edit")]
        public IActionResult Edit()
        {
            return  View();
        }
        public IActionResult New()
        {
            return  View("Edit");
        }



        // 
        // GET: /Calendar/Welcome/ 

         public string Welcome()
        {
            return "This is the Welcome action method...";
        }
    }
}