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
        public JiraDBContext context { get; set; }
        [HttpPost]
        public ActionResult Index(List<String> project, List<String> resource, DateTime setdate)
        {
            this.context = HttpContext.RequestServices.GetService(typeof(JiraDashboard.Models.JiraDBContext)) as JiraDBContext;
            this.context.SetDate(setdate);
            this.context.GetTasksCreatedAndCompletedThisPeriodAndYTD();
            this.context.GetLoggedHoursByProjectAndResource();
            this.context.ProcessTasks();
            this.context.ProcessIndividualTasks();
            this.context.SelectedProject = project;
            this.context.ProcessProject();
            this.context.SelectedResource = resource;
            this.context.ProcessResource();
            return View(this.context);
        }

        public ActionResult Index()
        {
            this.context = HttpContext.RequestServices.GetService(typeof(JiraDashboard.Models.JiraDBContext)) as JiraDBContext;
            this.context.SetDate(DateTime.Now);
            this.context.GetTasksCreatedAndCompletedThisPeriodAndYTD();
            this.context.GetLoggedHoursByProjectAndResource();
            this.context.ProcessTasks();
            this.context.ProcessIndividualTasks();
            return View(this.context);
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

        /*public IActionResult Jira()
        {
            JiraDBContext context = HttpContext.RequestServices.GetService(typeof(JiraDashboard.Models.JiraDBContext)) as JiraDBContext;
            context.GetTasksCreatedAndCompletedThisPeriodAndYTD();
            context.GetLoggedHoursByProjectAndResource();
            context.ProcessTasks();
            context.ProcessIndividualTasks();
            return View(context);
        }*/
        public IActionResult test(List<String> ProjectList)
        {
            //JiraDBContext context = HttpContext.RequestServices.GetService(typeof(JiraDashboard.Models.JiraDBContext)) as JiraDBContext;
            //context.GetTasksCreatedAndCompletedThisPeriodAndYTD();
            //context.GetLoggedHoursByProjectAndResource();
            //context.ProcessTasks();
            //context.ProcessIndividualTasks();
            Console.WriteLine("success");
            return View(this.context);
        }
    }
}
