using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

using SoftUni;
using SoftUni.Data;
using SoftUni.Models;

namespace SoftUni.Tests
{

    // ReSharper disable InconsistentNaming
    // ReSharper disable CheckNamespace

    [TestFixture]
    public class Test_012_001_001
    {
        [Test]
        public void ValidateOutput()
        {
            var services = new ServiceCollection()
                .AddDbContext<SoftUniContext>(b => b.UseInMemoryDatabase("SoftUni"));

            var serviceProvider = services.BuildServiceProvider();

            var context = serviceProvider.GetService<SoftUniContext>();

            Seed(context!);

            string expected = AssertMethod(context).Trim();

            string result = StartUp.IncreaseSalaries(context).Trim();

            Assert.AreEqual(expected, result, "Returned value is incorrect!");
        }

        private static void Seed(SoftUniContext context)
        {
            var departments = new List<Department>();

            var firstNames = new[]
            {
            "Svetlin", "Mariela", "Ani", "Slavi", "Pesho",
            "Kircata", "Gosheto", "OnziTam", "DamnGod", "Test123",
        };

            var lastNames = new[]
            {
            "Nakov", "Cureva", "Topova", "Ludakov", "Peshev",
            "Wakov", "Sliperov", "Topalov", "Hristov", "Petrov"
        };

            var middleNames = new[]
            {
            "Petrov", "Kirov", "Stoyanov", "Angelov", "Vladimirov",
            "Karlov", "Markov", "Kostov", "Pavlov", "Georgiev"
        };

            var jobTitles = new[]
            {
            "Bartender", "Waiter", "Artist", "Shefa", "Driver",
            "IT", "DevOps", "DataScientist", "BusDriver", "Trainer",
        };

            var salaries = new[]
            {
            642.12m, 5235, 4000.32m, 25432, 2535235.33m,
            52354.12m, 956, 658.32m, 95846, 75343215.33m,
        };

            var departmentNames = new[]
            {
            "Engineering","Tool Design","Marketing","Information Services",
            "ChillDepartment"
        };

            foreach (var departmentName in departmentNames)
            {
                var department = new Department
                {
                    Name = departmentName
                };

                int counter = 0;

                for (int i = 0; i < 2; i++)
                {
                    var employee = new Employee
                    {
                        FirstName = firstNames[counter],
                        LastName = lastNames[counter],
                        MiddleName = middleNames[counter],
                        JobTitle = jobTitles[counter],
                        Salary = salaries[counter],
                    };

                    counter++;
                    department.Employees.Add(employee);
                }

                departments.Add(department);
            }

            context.Departments.AddRange(departments);
            context.SaveChanges();
        }

        private static string AssertMethod(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var engineers = context.Employees
                .Where(employee =>
                    employee.Department.Name == "Engineering"
                    || employee.Department.Name == "Tool Design"
                    || employee.Department.Name == "Marketing"
                    || employee.Department.Name == "Information Services")
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            foreach (var e in engineers)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary * 1.12m:F2})");
            }

            return sb.ToString().TrimEnd();
        }
    }
}