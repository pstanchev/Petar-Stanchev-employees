using BestTeam.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestTeam.BusinessLogic
{
    public class CSVParser
    {
        public static Employee Parse(string line, string separatorString =  ", ")
        {
            string[] newData = line.Split(new string[]{ separatorString }, StringSplitOptions.RemoveEmptyEntries);

            int empId = int.Parse(newData[0]);
            int projectId = int.Parse(newData[1]);
            DateTime dateFrom = DateTime.Parse(newData[2]).Date;

            DateTime? dateTo = newData[3].ToUpper() == "NULL" ? (DateTime?)null : DateTime.Parse(newData[3].Replace('/', '-'));

            var newEmployee = new Employee(empId, projectId, dateFrom, dateTo);
            return newEmployee;
        }
    }
}
