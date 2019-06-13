using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiraDashboard.Models
{
    public class IndividualTask
    {
        public String Resource { get; set; }
        public String Project { get; set; }
        public double TotalLoggedHoursWeek1 { get; set; }
        public double TotalLoggedHoursWeek2 { get; set; }
        public double TotalLoggedHoursWeek3 { get; set; }
        public double TotalLoggedHoursWeek4 { get; set; }
        public double TotalLoggedHoursMonth { get; set; }
        public double TotalLoggedHoursTasksThisProject { get; set; }

        public IndividualTask(String Resource, String Project, double TotalLoggedHoursWeek1, double TotalLoggedHoursWeek2, double TotalLoggedHoursWeek3, double TotalLoggedHoursWeek4, double TotalLoggedHoursMonth, double TotalLoggedHoursTasksThisProject)
        {
            this.Resource = Resource;
            this.Project = Project;
            this.TotalLoggedHoursWeek1 = TotalLoggedHoursWeek1;
            this.TotalLoggedHoursWeek2 = TotalLoggedHoursWeek2;
            this.TotalLoggedHoursWeek3 = TotalLoggedHoursWeek3;
            this.TotalLoggedHoursWeek4 = TotalLoggedHoursWeek4;
            this.TotalLoggedHoursMonth = TotalLoggedHoursMonth;
            this.TotalLoggedHoursTasksThisProject = TotalLoggedHoursTasksThisProject;
        }
    }
}
