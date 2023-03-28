namespace TeisterMask.DataProcessor
{
    using System.Globalization;

    using Data;

    using Newtonsoft.Json;

    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ExportDto;
    using TeisterMask.Utilities;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            ExportProjectDto[] projectsDto = context.Projects
                    .Where(p => p.Tasks.Count > 0)
                    .Select(p => new ExportProjectDto()
                    {
                        ProjectName = p.Name,
                        HasEndDate = p.DueDate != null ? "Yes" : "No",
                        TasksCount = p.Tasks.Count,
                        Tasks = p.Tasks.Select(t => new ExportTaskDto()
                        {
                            Name = t.Name,
                            LabelType = t.LabelType.ToString()
                        })
                        .OrderBy(t => t.Name)
                        .ToArray()
                    })
                    .OrderByDescending(p => p.Tasks.Length)
                    .ThenBy(p => p.ProjectName)
                    .ToArray();

            string xmlResult = XmlHelper.Serialize(projectsDto, "Projects");

            return xmlResult;
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            DateTime parsedInputDate = DateTime.ParseExact(date.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var employees = context.Employees
                .ToArray()
                .Where(e => e.EmployeesTasks.Any(t => DateTime.ParseExact( t.Task.OpenDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture) >= parsedInputDate))
                .Select(e => new
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks
                        .Where(et => et.Task.OpenDate >= date)
                        .Select(et => new
                        {
                            TaskName = et.Task.Name,
                            OpenDate = et.Task.OpenDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                            DueDate = et.Task.DueDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                            LabelType = et.Task.LabelType.ToString(),
                            ExecutionType = et.Task.ExecutionType.ToString()
                        })
                        .OrderByDescending(t => DateTime.ParseExact(t.DueDate, "MM/dd/yyyy", CultureInfo.InvariantCulture))
                        .ThenBy(t => t.TaskName)
                        .ToArray()
                })
                .OrderByDescending(e => e.Tasks.Length)
                .ThenBy(e => e.Username)
                .Take(10)
                .ToArray();

            string jsonResult = JsonConvert.SerializeObject(employees, Formatting.Indented);

            return jsonResult;
        }
    }
}