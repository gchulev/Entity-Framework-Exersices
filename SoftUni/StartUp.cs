using System.Globalization;
using System.Text;

using Microsoft.EntityFrameworkCore;

using SoftUni.Data;
using SoftUni.Models;

namespace SoftUni
{
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
                .Select(e => new { e.FirstName, Salary = Math.Round(e.Salary, 2) })
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

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var newAddress = new Address()
            {
                TownId = 4,
                AddressText = "Vitoshka 15"
            };

            Employee? nakov = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");

            nakov!.Address = newAddress;

            context.SaveChanges();

            string[] result = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => e.Address!.AddressText)
                .ToArray();

            return string.Join(Environment.NewLine, result);
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var resultCollection = context.Employees
                .AsNoTracking()
                .Take(10)
                //.Where(e => e.EmployeesProjects.Any(ep => ep.Project.StartDate.Year >= 2001 &&
                //                                          ep.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager!.FirstName,
                    ManagerLastName = e.Manager!.LastName,
                    Projects = e.EmployeesProjects
                        .Where(ep => ep.Project.StartDate.Year >= 2001 &&
                                     ep.Project.StartDate.Year <= 2003)
                    .Select(ep => new
                    {
                        ProjectName = ep.Project.Name,
                        StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                        EndDate = ep.Project.EndDate.HasValue
                                  ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                                  : "not finished"
                    }).ToArray()
                }).ToArray();

            var sb = new StringBuilder();

            foreach (var e in resultCollection)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

                foreach (var p in e.Projects)
                {
                    sb.AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town!.Name)
                .ThenBy(a => a.AddressText)
                .Select(a => new
                {
                    a.AddressText,
                    TownName = a.Town!.Name,
                    a.Employees,
                })
                .Take(10)
                .ToArray();

            var sb = new StringBuilder();

            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.Employees.Count} employees");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var result = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects
                        .Select(ep => new { ep.Project.Name })
                        .OrderBy(p => p.Name)
                        .ToArray()
                }).ToArray();

            var sb = new StringBuilder();

            foreach (var e in result)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                foreach (var p in e.Projects)
                {
                    sb.AppendLine($"{p.Name}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    d.Manager.FirstName,
                    d.Manager.LastName,
                    d.Employees
                })
                .ToArray();

            var sb = new StringBuilder();

            foreach (var dep in departments)
            {
                sb.AppendLine($"{dep.DepartmentName} - {dep.FirstName} {dep.LastName}");

                foreach (var emp in dep.Employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
                {
                    sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            List<Project> latestProjects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .ToList();

            latestProjects.Sort((a, b) => a.Name.CompareTo(b.Name));

            var sb = new StringBuilder();

            foreach (Project project in latestProjects)
            {
                sb.AppendLine($"{project.Name}");
                sb.AppendLine($"{project.Description}");
                sb.AppendLine($"{project.StartDate.ToString("M/d/yyyy h:mm:ss tt")}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            string[] departmentList = new string[] { "Engineering", "Tool Design", "Marketing", "Information Services" };

            var employeesWithIncreasedSalary = context.Employees
                .Where(e => departmentList.Contains(e.Department.Name))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    Salary = e.Salary * 1.12m
                })
                .OrderBy(x => x.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();

            // Not commiting changes here
            //context.SaveChanges();

            var sb = new StringBuilder();

            foreach (var e in employeesWithIncreasedSalary)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employeesWithNameStartingWithSA = context.Employees
                .AsNoTracking()
                .ToList()
                .Where(e => e.FirstName.StartsWith("Sa", StringComparison.Ordinal))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();

            var sb = new StringBuilder();

            foreach (var e in employeesWithNameStartingWithSA)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
