using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JiraDashboard.Models
{
    public class JiraDBContext
    {
        public string ConnectionString { get; set; }
        public DateTime ReportStartDate { get; set; }
        public DateTime ReportEnddate { get; set; }
        public DateTime AllTimeStartDate { get; set; }
        public DateTime AllTimeEndDate { get; set; }
        public List<Tasks> TasksCreatedAndCompletedThisPeriodAndYTD { get; set; }
        public List<IndividualTask> IndividualTasks { get; set; }
        public List<List<List<IndividualTask>>> LoggedHoursByProject { get; set; }
        public List<List<List<IndividualTask>>> LoggedHoursByResource { get; set; }
        public Tasks TotalCountProjectTasks { get; set; }
        public List<Tasks> MoreThan10 { get; set; }
        public List<String> GraphLabel { get; set; }
        public List<int> GraphMonthlyTask { get; set; }
        public List<int> GraphYTDTask { get; set; }
        public List<int> ByResource { get; set; }
        public List<int> ByProject { get; set; }
        public List<String> SelectedProject { get; set; }
        public List<String> SelectedResource { get; set; }
        public List<List<List<IndividualTask>>> LoggedHoursByProjectDisplay { get; set; }
        public List<List<List<IndividualTask>>> LoggedHoursByResourceDisplay { get; set; }
        public List<int> EngagementByProjectTotal { get; set; }
        public List<int> EngagementByResourceTotal { get; set; }


        public JiraDBContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public void GetTasksCreatedAndCompletedThisPeriodAndYTD()
        {
            List<Tasks> list = new List<Tasks>();

            using (MySqlConnection conn = GetConnection())
            {

                string sql = @"
                    select YTD.Project,  ThisPeriod.TasksCreatedThisMonth, ThisPeriod.Completed, YTD.TasksCreatedYTD, YTD.TasksCompletedYTD, YTD.Backlog
                    from
                    (
                    select CompletedProjectIssuesThisPeriod.Project, SprintProjectIssues.TasksCreatedThisMonth, CompletedProjectIssuesThisPeriod.Completed from
                    (
                    select project.pname as 'Project', count(*) as 'TasksCreatedThisMonth'
                    from
                    jiraissue, project
                    where
                    #joins
                    jiraissue.PROJECT = project.ID and
                    #selecions
                    (
                    (project.ID = 10000 and jiraissue.CREATED > @report_start_date and  jiraissue.CREATED < @report_end_date) #ALL tasks created for CLSD
                    or
                    (project.ID != 10000 and jiraissue.DUEDATE > @report_start_date and  jiraissue.DUEDATE < @report_end_date) #Any tasks with a due date
                    or
                    (
                    project.ID != 10000 and jiraissue.id in  # Tasks included in sprints finished in the selected period
                    (select ISSUE
                    from
                    customfieldvalue
                    where
                    customfieldvalue.stringvalue  in
                    (
                    SELECT  ID from
                    jiradb.AO_60DB71_SPRINT
                    where
                    DATE(FROM_UNIXTIME(COMPLETE_DATE / 1000)) > @report_start_date and
                    DATE(FROM_UNIXTIME(COMPLETE_DATE / 1000)) < @report_end_date
                    )
                    and
                    customfieldvalue.CUSTOMFIELD = 10008
                    )
                    )
                    )
                    group by
                    project.pname
                    )
                    AS SprintProjectIssues RIGHT OUTER JOIN
                    (
                    #Tasks completed in this period 
                    select project.pname as 'Project', count(*) as 'Completed'
                    from
                    jiraissue, project
                    where
                    #joins
                    jiraissue.PROJECT = project.ID and
                    #selecions
                    (
                    (
                    # Tasks included in sprints that are finished
                    jiraissue.RESOLUTION is not null and jiraissue.id in
                    (select ISSUE
                    from
                    customfieldvalue
                    where
                    customfieldvalue.stringvalue  in
                    (
                    SELECT  ID from
                    jiradb.AO_60DB71_SPRINT
                    where
                    DATE(FROM_UNIXTIME(COMPLETE_DATE / 1000)) > @report_start_date and
                    DATE(FROM_UNIXTIME(COMPLETE_DATE / 1000)) < @report_end_date
                    )
                    and
                    customfieldvalue.CUSTOMFIELD = 10008
                    )
                    )
                    or
                    (
                    #tasks completed this reporting period 
                    jiraissue.RESOLUTIONDATE > @report_start_date and  jiraissue.RESOLUTIONDATE < @report_end_date
                    )
                    )
                    group by
                    project.pname
                    )
                    AS CompletedProjectIssuesThisPeriod
                    ON
                    SprintProjectIssues.Project = CompletedProjectIssuesThisPeriod.Project
                    )
                    AS ThisPeriod RIGHT OUTER JOIN
                    (

                     SELECT
                        ProjectIssuesYTD.Project,
                        ProjectIssuesYTD.TasksCreatedYTD,
                        CompletedProjectIssuesYTD.TasksCompletedYTD,
                        ProjectIssuesYTD.TasksCreatedYTD - CompletedProjectIssuesYTD.TasksCompletedYTD AS 'Backlog'
                    FROM
                        (SELECT
                            project.pname AS 'Project', COUNT(*) AS 'TasksCreatedYTD'
                        FROM
                            jiraissue, project
                        WHERE
                            jiraissue.PROJECT = project.ID
                                AND jiraissue.CREATED > @all_time_start_date
                        GROUP BY project.pname) AS ProjectIssuesYTD
                            LEFT OUTER JOIN
                        (SELECT
                            project.pname AS 'Project', COUNT(*) AS 'TasksCompletedYTD'
                        FROM
                            jiraissue, project
                        WHERE
                            jiraissue.PROJECT = project.ID
                                AND jiraissue.CREATED > @all_time_start_date
                                AND jiraissue.RESOLUTION IS NOT NULL
                        GROUP BY project.pname) AS CompletedProjectIssuesYTD ON ProjectIssuesYTD.Project = CompletedProjectIssuesYTD.Project
                    )
                    AS YTD
                    ON
                    ThisPeriod.Project = YTD.Project";
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@report_start_date", this.ReportStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    cmd.Parameters.AddWithValue("@report_end_date", this.ReportEnddate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    cmd.Parameters.AddWithValue("@all_time_start_date", this.AllTimeStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    cmd.Parameters.AddWithValue("@all_time_end_date", this.AllTimeEndDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            list.Add(new Tasks
                                (
                                reader.GetString(0),
                                SafeGetint(reader, 1),
                                SafeGetint(reader, 2),
                                SafeGetint(reader, 3),
                                SafeGetint(reader, 4),
                                SafeGetint(reader, 5)
                                )
                            );
                        }
                    }
                }
            }
            list = list.OrderByDescending(x => x.Backlog).ToList();
            this.TasksCreatedAndCompletedThisPeriodAndYTD = list;
            //return list;
        }

        public int SafeGetint(MySqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
            {
                return reader.GetInt32(colIndex);
            }
            else
            {
                return 0;
            }
        }

        public void ProcessTasks()
        {
            var temp1 = 0;
            var temp2 = 0;
            var temp3 = 0;
            var temp4 = 0;
            var temp5 = 0;
            List<Tasks> TempTasks = new List<Tasks>();
            List<String> Lables = new List<String>();
            List<int> Monthly = new List<int>();
            List<int> YPD = new List<int>();
            foreach (var item in this.TasksCreatedAndCompletedThisPeriodAndYTD)
            {
                temp1 += item.TasksCreatedThisMonth;
                temp2 += item.Completed;
                temp3 += item.TasksCreatedYTD;
                temp4 += item.TasksCompletedYTD;
                temp5 += item.Backlog;
                if (item.TasksCreatedThisMonth > 20 && item.Project!= "Clinical Laboratories Service Desk")
                {
                    TempTasks.Add(item);
                    Lables.Add(item.Project);
                    Monthly.Add(item.Completed);
                    YPD.Add(item.TasksCompletedYTD);
                }
            }
            this.TotalCountProjectTasks = new Tasks("Total", temp1, temp2, temp3, temp4, temp5);
            this.MoreThan10 = TempTasks;
            this.GraphLabel = Lables;
            this.GraphMonthlyTask = Monthly;
            this.GraphYTDTask = YPD;
            //this.TasksCreatedAndCompletedThisPeriodAndYTD.Add(totalCount);
        }

        public void GetLoggedHoursByProjectAndResource()
        {
            List<IndividualTask> list = new List<IndividualTask>();

            using (MySqlConnection conn = GetConnection())
            {
                string sql = @"
                    Select 
                    HoursLoggedYTD.Resource, HoursLoggedYTD.Project, HoursLoggedWeek1.TotalLoggedHoursWeek1, HoursLoggedWeek2.TotalLoggedHoursWeek2,
                     HoursLoggedWeek3.TotalLoggedHoursWeek3,  HoursLoggedWeek4.TotalLoggedHoursWeek4,
                      HoursLoggedMonth.TotalLoggedHoursMonth,
                    HoursLoggedYTD.TotalLoggedHoursTasksThisProject
                    from 
                    (
                    select author as 'Resource', jiradb.project.pname as 'Project', sum(jiradb.worklog.timeworked/3600)  as 'TotalLoggedHoursTasksThisProject'
                    from jiradb.worklog, jiradb.jiraissue, jiradb.project 
                    where 
                    jiradb.worklog.issueid = jiradb.jiraissue.id and
                    jiradb.jiraissue.PROJECT = jiradb.project.id and
                    worklog.STARTDATE > @report_start_date_ytd and 
                    worklog.STARTDATE < @report_end_date_ytd
                    group by AUTHOR,jiradb.project.pname
                    )
                    as HoursLoggedYTD
                    LEFT OUTER JOIN
                    (
                    select author as 'Resource', jiradb.project.pname as 'Project', sum(jiradb.worklog.timeworked/3600)  as 'TotalLoggedHoursWeek1'
                    from jiradb.worklog, jiradb.jiraissue, jiradb.project 
                    where 
                    jiradb.worklog.issueid = jiradb.jiraissue.id and
                    jiradb.jiraissue.PROJECT = jiradb.project.id and
                    worklog.STARTDATE >= @report_start_date and
                    worklog.STARTDATE <= DATE_ADD(@report_start_date, INTERVAL 1 WEEK)   
                    group by AUTHOR,jiradb.project.pname
                    )
                    as HoursLoggedWeek1
                    ON HoursLoggedYTD.Resource = HoursLoggedWeek1.Resource and HoursLoggedYTD.project = HoursLoggedWeek1.Project 
                    LEFT OUTER JOIN
                    (
                    select author as 'Resource', jiradb.project.pname as 'Project', sum(jiradb.worklog.timeworked/3600)  as 'TotalLoggedHoursWeek2'
                    from jiradb.worklog, jiradb.jiraissue, jiradb.project 
                    where 
                    jiradb.worklog.issueid = jiradb.jiraissue.id and
                    jiradb.jiraissue.PROJECT = jiradb.project.id and
                    worklog.STARTDATE > DATE_ADD(@report_start_date, INTERVAL 1 WEEK)  and  
                    worklog.STARTDATE <= DATE_ADD(@report_start_date, INTERVAL 2 WEEK)   
                    group by AUTHOR,jiradb.project.pname
                    )
                    as HoursLoggedWeek2
                    ON HoursLoggedYTD.Resource = HoursLoggedWeek2.Resource and HoursLoggedYTD.project = HoursLoggedWeek2.Project 
                    LEFT OUTER JOIN
                    (
                    select author as 'Resource', jiradb.project.pname as 'Project', sum(jiradb.worklog.timeworked/3600)  as 'TotalLoggedHoursWeek3'
                    from jiradb.worklog, jiradb.jiraissue, jiradb.project 
                    where 
                    jiradb.worklog.issueid = jiradb.jiraissue.id and
                    jiradb.jiraissue.PROJECT = jiradb.project.id and
                    worklog.STARTDATE > DATE_ADD(@report_start_date, INTERVAL 2 WEEK)  and  
                    worklog.STARTDATE <= DATE_ADD(@report_start_date, INTERVAL 3 WEEK)   
                    group by AUTHOR,jiradb.project.pname
                    )
                    as HoursLoggedWeek3
                    ON HoursLoggedYTD.Resource = HoursLoggedWeek3.Resource and HoursLoggedYTD.project = HoursLoggedWeek3.Project 
                    LEFT OUTER JOIN
                    (
                    select author as 'Resource', jiradb.project.pname as 'Project', sum(jiradb.worklog.timeworked/3600)  as 'TotalLoggedHoursWeek4'
                    from jiradb.worklog, jiradb.jiraissue, jiradb.project 
                    where 
                    jiradb.worklog.issueid = jiradb.jiraissue.id and
                    jiradb.jiraissue.PROJECT = jiradb.project.id and
                    worklog.STARTDATE > DATE_ADD(@report_start_date, INTERVAL 3 WEEK)  and  
                    worklog.STARTDATE <= DATE_ADD(@report_start_date, INTERVAL 4 WEEK)   
                    group by AUTHOR,jiradb.project.pname
                    )
                    as HoursLoggedWeek4
                    ON HoursLoggedYTD.Resource = HoursLoggedWeek4.Resource and HoursLoggedYTD.project = HoursLoggedWeek4.Project 
                    LEFT OUTER JOIN
                    (
                    select author as 'Resource', jiradb.project.pname as 'Project', sum(jiradb.worklog.timeworked/3600)  as 'TotalLoggedHoursMonth'
                    from jiradb.worklog, jiradb.jiraissue, jiradb.project 
                    where 
                    jiradb.worklog.issueid = jiradb.jiraissue.id and
                    jiradb.jiraissue.PROJECT = jiradb.project.id and
                    worklog.STARTDATE >= @report_start_date and
                    worklog.STARTDATE <= DATE_ADD(@report_start_date, INTERVAL 4 WEEK)   
                    group by AUTHOR,jiradb.project.pname
                    )
                    as HoursLoggedMonth
                    ON HoursLoggedYTD.Resource = HoursLoggedMonth.Resource and HoursLoggedYTD.project = HoursLoggedMonth.Project 
                    where
                    HoursLoggedYTD.Resource in 
                     (SELECT lower_child_name FROM 
                      jiradb.cwd_membership 
                      where
                      parent_name = 'PATH-IT Staff')
  
                    Order by 
                    HoursLoggedYTD.Project;";
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@report_start_date", this.ReportStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    cmd.Parameters.AddWithValue("@report_end_date", this.ReportEnddate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    cmd.Parameters.AddWithValue("@report_start_date_ytd", this.AllTimeStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    cmd.Parameters.AddWithValue("@report_end_date_ytd", this.AllTimeEndDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            list.Add(new IndividualTask
                                (
                                reader.GetString(0), 
                                reader.GetString(1),
                                SafeGetint(reader, 2),
                                SafeGetint(reader, 3),
                                SafeGetint(reader, 4),
                                SafeGetint(reader, 5),
                                SafeGetint(reader, 6),
                                SafeGetint(reader, 7)
                                )
                            );
                        }
                    }
                }
            }

            this.IndividualTasks = list;
        }

        public void ProcessIndividualTasks()
        {
            this.LoggedHoursByProject = new List<List<List<IndividualTask>>>();
            this.LoggedHoursByResource = new List<List<List<IndividualTask>>>();
            foreach (IndividualTask item in this.IndividualTasks)
            {
                List<List<IndividualTask>> ByProject = InLoggedHoursByProject(item);
                List<List<IndividualTask>> ByResource = InLoggedHoursByResource(item);

                if (ByProject != null)
                {
                    AddITToGroup(ByProject, item);
                }
                else
                {
                    IndividualTask newItem = new IndividualTask
                    (
                        item.Resource,
                        item.Project,
                        item.TotalLoggedHoursWeek1,
                        item.TotalLoggedHoursWeek2,
                        item.TotalLoggedHoursWeek3,
                        item.TotalLoggedHoursWeek4,
                        item.TotalLoggedHoursMonth,
                        item.TotalLoggedHoursTasksThisProject
                    );
                    this.LoggedHoursByProject.Add(new List<List<IndividualTask>> { new List<IndividualTask> { newItem }, new List<IndividualTask> { item } });
                }
                if (ByResource != null)
                {
                    AddITToGroup(ByResource, item);
                }
                else
                {
                    IndividualTask newItem = new IndividualTask
                    (
                        item.Resource,
                        item.Project,
                        item.TotalLoggedHoursWeek1,
                        item.TotalLoggedHoursWeek2,
                        item.TotalLoggedHoursWeek3,
                        item.TotalLoggedHoursWeek4,
                        item.TotalLoggedHoursMonth,
                        item.TotalLoggedHoursTasksThisProject
                    );

                    this.LoggedHoursByResource.Add(new List<List<IndividualTask>> { new List<IndividualTask> { newItem }, new List<IndividualTask> { item } });
                }
            }

            this.LoggedHoursByProjectDisplay = this.LoggedHoursByProject.OrderByDescending(x => x[0][0].TotalLoggedHoursMonth).ToList();
            this.LoggedHoursByResourceDisplay = this.LoggedHoursByResource.OrderByDescending(x => x[0][0].TotalLoggedHoursMonth).ToList();
            CountTotalByProject();
            CountTotalByResource();
            var TempSelectedProject = new List<string> { };
            foreach (var item in this.LoggedHoursByProject)
            {
                TempSelectedProject.Add(item[0][0].Project);
            }
            var TempSelectedResource = new List<string> { };
            foreach (var item in this.LoggedHoursByResource)
            {
                TempSelectedResource.Add(item[0][0].Resource);
            }
            this.SelectedProject = TempSelectedProject;
            this.SelectedResource = TempSelectedResource;
        }

        public void AddITToGroup(List<List<IndividualTask>> set, IndividualTask item)
        {
            set[1].Add(item);
            set[0][0].TotalLoggedHoursWeek1 += item.TotalLoggedHoursWeek1;
            set[0][0].TotalLoggedHoursWeek2 += item.TotalLoggedHoursWeek2;
            set[0][0].TotalLoggedHoursWeek3 += item.TotalLoggedHoursWeek3;
            set[0][0].TotalLoggedHoursWeek4 += item.TotalLoggedHoursWeek4;
            set[0][0].TotalLoggedHoursMonth = set[0][0].TotalLoggedHoursMonth + item.TotalLoggedHoursMonth;
            set[0][0].TotalLoggedHoursTasksThisProject += item.TotalLoggedHoursTasksThisProject;
        }
        public List<List<IndividualTask>> InLoggedHoursByResource(IndividualTask item)
        {
            foreach (List<List<IndividualTask>> set in this.LoggedHoursByResource)
            {
                if (item.Resource == set[0][0].Resource)
                {
                    return set;
                }
            }
            return null;
        }

        public List<List<IndividualTask>> InLoggedHoursByProject(IndividualTask item)
        {
            foreach (List<List<IndividualTask>> set in this.LoggedHoursByProject)
            {
                if (item.Project == set[0][0].Project)
                {
                    return set;
                }
            }
            return null;
        }

        public void ProcessProject()
        {
            var temp = new List<List<List<IndividualTask>>> { };
            foreach (var item in this.LoggedHoursByProject)
            {
                if (this.SelectedProject.Contains(item[0][0].Project))
                {
                    temp.Add(item);
                }
            }
            temp = temp.OrderByDescending(x => x[0][0].TotalLoggedHoursMonth).ToList();
            this.LoggedHoursByProjectDisplay = temp;
            CountTotalByProject();
        }

        public void ProcessResource()
        {
            var temp = new List<List<List<IndividualTask>>> { };
            foreach (var item in this.LoggedHoursByResource)
            {
                if (this.SelectedResource.Contains(item[0][0].Resource))
                {
                    temp.Add(item);
                }
            }
            temp = temp.OrderByDescending(x => x[0][0].TotalLoggedHoursMonth).ToList();
            this.LoggedHoursByResourceDisplay = temp;
            CountTotalByResource();
        }

        public void CountTotalByProject()
        {
            var temp = new List<int> { 0, 0 };
            foreach (var item in this.LoggedHoursByProjectDisplay)
            {
                temp[0] += item[1].Count;
                temp[1] += item[0][0].TotalLoggedHoursMonth;
            }
            this.EngagementByProjectTotal = temp;
        }

        public void CountTotalByResource()
        {
            var temp = new List<int> { 0, 0, 0, 0, 0, 0 };
            foreach (var item in this.LoggedHoursByResourceDisplay)
            {
                temp[0] += item[1].Count;
                temp[1] += item[0][0].TotalLoggedHoursWeek1;
                temp[2] += item[0][0].TotalLoggedHoursWeek2;
                temp[3] += item[0][0].TotalLoggedHoursWeek3;
                temp[4] += item[0][0].TotalLoggedHoursWeek4;
                temp[5] += item[0][0].TotalLoggedHoursMonth;
            }
            this.EngagementByResourceTotal = temp;
        }
        public void SetDate(DateTime date)
        {
            this.ReportStartDate = date.AddDays(-28);
            this.ReportEnddate = date;
            this.AllTimeStartDate = date.AddDays(-365);
            this.AllTimeEndDate = date;
        }
    }
}