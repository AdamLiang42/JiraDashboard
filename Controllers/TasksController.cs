using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JiraDashboard.Models;

namespace JiraDashboard.Controllers
{
    public class TasksController : Controller
    {
        public IActionResult Index()
        {
            JiraDBContext context = HttpContext.RequestServices.GetService(typeof(JiraDashboard.Models.JiraDBContext)) as JiraDBContext;
            return View(context.GetTasksCreatedAndCompletedThisPeriodAndYTD());
        }
    }
}