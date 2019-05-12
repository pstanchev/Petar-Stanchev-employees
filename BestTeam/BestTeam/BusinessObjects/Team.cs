using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestTeam.BusinessObjects
{
    public class Team
    {
        public int EmployeeId1 { get; private set; }

        public int EmployeeId2 { get; private set; }

        public Dictionary<int, int> Projects { get; private set; }

        public Team(int empId1, int empId2)
        {
            this.EmployeeId1 = empId1;
            this.EmployeeId2 = empId2;
            this.Projects = new Dictionary<int, int>();
        }

        public int TotalDaysDuration
        {
            get
            {
                int total = 0;

                foreach (var project in this.Projects)
                {
                    total += project.Value;
                }

                return total;
            }
        }
    }
}
