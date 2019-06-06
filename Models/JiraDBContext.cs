using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace JiraDashboard.Models
{
    public class JiraDBContext
    {
        public string ConnectionString { get; set; }

        public JiraDBContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<Tasks> GetTasksCreatedAndCompletedThisPeriodAndYTD()
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
                    cmd.Parameters.AddWithValue("@report_start_date", new DateTime(2019, 04, 22));
                    cmd.Parameters.AddWithValue("@report_end_date", new DateTime(2019, 05, 17));
                    cmd.Parameters.AddWithValue("@all_time_start_date", new DateTime(2018, 05, 17));
                    cmd.Parameters.AddWithValue("@all_time_end_date", new DateTime(2019, 05, 17));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Tasks
                                (
                                reader["Project"].ToString(),
                                Convert.ToInt32(reader["TasksCreatedThisMonth"]),
                                Convert.ToInt32(reader["Completed"]),
                                Convert.ToInt32(reader["TasksCreatedYTD"]),
                                Convert.ToInt32(reader["TasksCompletedYTDrice"]),
                                Convert.ToInt32(reader["Backlog"])
                                )
                            );
                        }
                    }
                }
                
            }
            return list;
        }
    }
}