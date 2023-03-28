// ReSharper disable InconsistentNaming

namespace TeisterMask.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using TeisterMask.DataProcessor.ImportDto;
    using TeisterMask.Utilities;
    using System.Text;
    using TeisterMask.Data.Models;
    using System.Globalization;
    using TeisterMask.Data.Models.Enums;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            ImportProjectDto[] projectsDto = XmlHelper.Deserialize<ImportProjectDto[]>(xmlString, "Projects");

            var sb = new StringBuilder();
            var validProjects = new List<Project>();

            foreach (ImportProjectDto projectDto in projectsDto)
            {
                if (!IsValid(projectDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var validProject = new Project()
                {
                    Name = projectDto.Name,
                    OpenDate = DateTime.ParseExact(projectDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    DueDate = string.IsNullOrWhiteSpace(projectDto.DueDate) ? null : DateTime.ParseExact(projectDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                };

                foreach (ImportTaskDto taskDto in projectDto.Tasks)
                {
                    if (!IsValid(taskDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(taskDto.DueDate))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(taskDto.OpenDate))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime taskOpenDate = DateTime.ParseExact(taskDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime projectOpenDate = DateTime.ParseExact(projectDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (taskOpenDate < projectOpenDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime taskDueDate = DateTime.ParseExact(taskDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime? projectDueDate = string.IsNullOrWhiteSpace(projectDto.DueDate) ? null : DateTime.ParseExact(projectDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (taskDueDate > projectDueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var validTask = new Task()
                    {
                        Name = taskDto.Name,
                        OpenDate = DateTime.ParseExact(taskDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        DueDate = DateTime.ParseExact(taskDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ExecutionType = (ExecutionType)taskDto.ExecutionType,
                        LabelType = (LabelType)taskDto.LabelType
                    };

                    validProject.Tasks.Add(validTask);
                }
                validProjects.Add(validProject);
                sb.AppendLine(string.Format(SuccessfullyImportedProject, validProject.Name, validProject.Tasks.Count));
            }

            context.Projects.AddRange(validProjects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            ImportEmployeeDto[] employeesDto = JsonConvert.DeserializeObject<ImportEmployeeDto[]>(jsonString)!;

            var sb = new StringBuilder();
            var validEmployees = new List<Employee>();

            foreach (ImportEmployeeDto employeeDto in employeesDto)
            {
                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var validEmployee = new Employee()
                {
                    Username = employeeDto.Username,
                    Email = employeeDto.Email,
                    Phone = employeeDto.Phone,
                };

                int[] allTaskIds = context.Tasks.Select(t => t.Id).ToArray();
                int[] uniqueTaskIds = employeeDto.Tasks.Distinct().ToArray();

                foreach (int taskIdDto in uniqueTaskIds)
                {
                    if (!allTaskIds.Any(id => id == taskIdDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    validEmployee.EmployeesTasks.Add(new EmployeeTask()
                    {
                        TaskId = taskIdDto
                    });
                }

                validEmployees.Add(validEmployee);
                sb.AppendLine(string.Format(SuccessfullyImportedEmployee, validEmployee.Username, validEmployee.EmployeesTasks.Count));
            }

            context.Employees.AddRange(validEmployees);
            context.SaveChanges();
            int tasksCount = validEmployees.SelectMany(e => e.EmployeesTasks.Select(t => t.TaskId)).Count();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}