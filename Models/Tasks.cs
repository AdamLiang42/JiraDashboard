using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiraDashboard.Models
{
    public class Tasks
    {
        
        public string Project { get; set; }

        public int TasksCreatedThisMonth { get; set; }

        public int Completed { get; set; }

        public int TasksCreatedYTD { get; set; }

        public int TasksCompletedYTD { get; set; }

        public int Backlog { get; set; }

        public Tasks(string project, int tasksCreatedThisMonth, int completed, int tasksCreatedYTD, int tasksCompletedYTD, int backlog)
        {
            this.Project = project;
            this.TasksCreatedThisMonth = tasksCreatedThisMonth;
            this.Completed = completed;
            this.TasksCreatedYTD = tasksCreatedYTD;
            this.TasksCompletedYTD = tasksCompletedYTD;
            this.Backlog = backlog;
        }
    }
}
