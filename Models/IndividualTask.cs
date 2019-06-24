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
        public int TotalLoggedHoursWeek1 { get; set; }
        public int TotalLoggedHoursWeek2 { get; set; }
        public int TotalLoggedHoursWeek3 { get; set; }
        public int TotalLoggedHoursWeek4 { get; set; }
        public int TotalLoggedHoursMonth { get; set; }
        public int TotalLoggedHoursTasksThisProject { get; set; }

        public IndividualTask(String Resource, String Project, int TotalLoggedHoursWeek1, int TotalLoggedHoursWeek2, int TotalLoggedHoursWeek3, int TotalLoggedHoursWeek4, int TotalLoggedHoursMonth, int TotalLoggedHoursTasksThisProject)
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
