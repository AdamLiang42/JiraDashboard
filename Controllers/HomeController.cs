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


        public ActionResult Index(List<String> project, List<String> resource)
        {
            this.context = HttpContext.RequestServices.GetService(typeof(JiraDashboard.Models.JiraDBContext)) as JiraDBContext;
            this.context.GetTasksCreatedAndCompletedThisPeriodAndYTD();
            this.context.GetLoggedHoursByProjectAndResource();
            this.context.ProcessTasks();
            this.context.ProcessIndividualTasks();
            if (project.Count != 0 && resource.Count == 0)
            {
                this.context.SelectedProject = project;
                this.context.ProcessProject();
            }
            if (resource.Count != 0 && project.Count == 0)
            {
                this.context.SelectedResource = resource;
                this.context.ProcessResource();
            }
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
