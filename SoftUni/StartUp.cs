using System.Text;
using SoftUni.Data;

namespace SoftUni
{
    public class StartUp
    {
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var result = context.Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    MiddleName = e.MiddleName,
                    JobTitle = e.JobTitle,
                    Salary = Math.Round(e.Salary, 2)
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var emp in result)
            {
                sb.AppendLine($"{emp.LastName} {emp.FirstName} {emp.MiddleName} {emp.JobTitle} {emp.Salary:f2}");
            }
            return sb.ToString().TrimEnd();
        }
    }
}
