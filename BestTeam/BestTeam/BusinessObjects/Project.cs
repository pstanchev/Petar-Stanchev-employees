using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestTeam.BusinessObjects
{
    public class Project
    {
        public int Id { get; set; }

        public List<Employee> Employees { get; set; }

        public Project(int projectId)
        {
            this.Id = projectId;
            this.Employees = new List<Employee>();
        }
    }
}
