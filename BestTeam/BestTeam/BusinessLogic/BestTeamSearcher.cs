using BestTeam.BusinessObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BestTeam.BusinessLogic
{
    public class BestTeamSearcher
    {
        public BestTeamSearcher()
        {
            Projects = new List<Project>();
            Teams = new List<Team>();
        }

        public static List<Project> Projects { get; set;  }

        public static List<Team> Teams { get; set; }

        public Team Search(string fileName)
        {
            this.ExtractFileData(fileName);

            this.FindTeamwork();

            var bestTeam = this.FindBestTeam();
            return bestTeam;
        }

        private void ExtractFileData(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    var newEmployee = CSVParser.Parse(line);
                    this.AddEmployeeToProjects(newEmployee);
                }
            }
        }

        private void AddEmployeeToProjects(Employee employee)
        {
            var projectFound = BestTeamSearcher.Projects.Find(p => p.Id == employee.ProjectId);

            if (projectFound != null)
            {
                projectFound.Employees.Add(employee);
            }
            else
            {
                var newProject = new Project(employee.ProjectId);
                newProject.Employees.Add(employee);
                BestTeamSearcher.Projects.Add(newProject);
            }
        }

        private void FindTeamwork()
        {
            foreach (var project in Projects)
            {
                // Sort employees to reduce avoid cases of overlapping
                project.Employees.Sort((e1, e2) => e1.Start.CompareTo(e2.Start));

                for (var i = 0; i < project.Employees.Count; i++)
                {
                    for (int j = i + 1; j < project.Employees.Count; j++)
                    {
                        var employee1 = project.Employees[i];
                        var employee2 = project.Employees[j];

                        int daysTeamwork = this.CalculateTeamDays(employee1, employee2);

                        if (daysTeamwork > 0)
                        {
                            this.CreateTeams(employee1, employee2, project, daysTeamwork);
                        }
                    }
                }
            }
        }

        private int CalculateTeamDays(Employee employee1, Employee employee2)
        {
            var overlapStart = new TimeSpan();
            var overlapEnd = new TimeSpan();

            if (employee1.End > employee2.Start)
            {
                overlapStart = employee1.End - employee2.Start;

                if (employee2.End < employee1.End)
                {
                    overlapEnd = employee2.End - employee1.End;
                }
            }

            int totalDays = overlapStart.Days + overlapEnd.Days;
            return totalDays;
        }

        private void CreateTeams(Employee employee1, Employee employee2, Project proj, int daysTeamwork)
        {
            // Sorting to avoid duplication of teams
            var leftEmployee = employee1.Id < employee2.Id ? employee1.Id : employee2.Id;
            var rightEmployee = employee1.Id < employee2.Id ? employee2.Id : employee1.Id;

            var teamFound = Teams.Find(x => x.EmployeeId1 == leftEmployee && x.EmployeeId2 == rightEmployee);

            if (teamFound != null)
            {
                if (teamFound.Projects.ContainsKey(proj.Id))
                {
                    teamFound.Projects[proj.Id] += daysTeamwork;
                }
                else
                {
                    teamFound.Projects.Add(proj.Id, daysTeamwork);
                }
            }
            else
            {
                var newTeam = new Team(leftEmployee, rightEmployee);
                newTeam.Projects.Add(proj.Id, daysTeamwork);
                Teams.Add(newTeam);
            }
        }

        private Team FindBestTeam()
        {
            var bestTeam = Teams.OrderByDescending(t => t.TotalDaysDuration).FirstOrDefault();

            return bestTeam;
        }
    }
}
