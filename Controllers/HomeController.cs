using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JiraDashboard.Models;

namespace JiraDashboard.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Jira()
        {
            JiraDBContext context = HttpContext.RequestServices.GetService(typeof(JiraDashboard.Models.JiraDBContext)) as JiraDBContext;
            context.GetTasksCreatedAndCompletedThisPeriodAndYTD();
            context.GetLoggedHoursByProjectAndResource();
            context.ProcessTasks();
            context.ProcessIndividualTasks();
            return View(context);
        }
    }
}
