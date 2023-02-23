namespace SoftUni
{
    using System.Text;

    using Data;
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

    }
}
