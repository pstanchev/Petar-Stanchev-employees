using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestTeam.BusinessObjects
{
    public class Employee
    {
        private DateTime? end;

        public int Id { get; private set; }

        public int ProjectId { get; private set; }

        public DateTime Start { get; private set; }

        public Employee(int empId, int projectId, DateTime dateFrom, DateTime? dateTo)
        {
            this.Id = empId;
            this.ProjectId = projectId;

            if (dateTo != null && dateFrom > dateTo)
            {
                throw new ArgumentException("Periods start and end do not match");
            }

            this.Start = dateFrom;
            this.end = dateTo;
        }

        public DateTime End
        {
            get
            {
                if (this.end == null)
                {
                    return DateTime.Now.Date;
                }

                return (DateTime)this.end;
            }

            set
            {
                this.end = value;
            }
        }
    }
}
