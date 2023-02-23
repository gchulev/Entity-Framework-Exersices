namespace SoftUni
{
    using System.Text;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using SoftUni.Models;

    public static class StartUp
    {
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var result = context.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.FirstName,
                    e.MiddleName,
                    e.LastName,
                    e.JobTitle,
                    Salary = Math.Round(e.Salary, 2)
                })
                .OrderBy(e => e.EmployeeId);

            var sb = new StringBuilder();

            foreach (var emp in result)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} {emp.MiddleName} {emp.JobTitle} {emp.Salary:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var result = context.Employees
                .Where(e => e.Salary > 50000m)
                .Select(e => new { e.FirstName, Salary = Math.Round(e.Salary, 2)})
                .OrderBy(e => e.FirstName);

            var sb = new StringBuilder();

            foreach (var emp in result)
            {
                sb.AppendLine($"{emp.FirstName} - {emp.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var result = context.Employees
                .AsNoTracking()
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Department.Name,
                    Salary = Math.Round(e.Salary, 2)
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName);

            var sb = new StringBuilder();

            foreach (var item in result)
            {
                sb.AppendLine($"{item.FirstName} {item.LastName} from {item.Name} - ${item.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
