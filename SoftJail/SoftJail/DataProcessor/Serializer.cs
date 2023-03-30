namespace SoftJail.DataProcessor
{
    using System.Globalization;
    using System.Text;

    using Data;

    using Newtonsoft.Json;

    using SoftJail.DataProcessor.ExportDto;
    using SoftJail.Utilities;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .ToArray()
                .Where(p => ids.Contains(p.Id))
                .Select(p => new
                {
                    p.Id,
                    Name = p.FullName,
                    p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers.Select(po => new
                    {
                        OfficerName = po.Officer.FullName,
                        Department = po.Officer.Department.Name
                    })
                    .OrderBy(po => po.OfficerName),
                    TotalOfficerSalary = Math.Round(p.PrisonerOfficers.Sum(po => po.Officer.Salary), 2)
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();

            string jsonResult = JsonConvert.SerializeObject(prisoners, Formatting.Indented);

            return jsonResult;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            string[] prisonerNamesArray = prisonersNames.Split(",");

            ExportPrisonerDto[] prisoners = context.Prisoners
                .Where(p => prisonerNamesArray.Contains(p.FullName))
                .Select(p => new ExportPrisonerDto()
                {
                    Id = p.Id,
                    Name = p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    EncryptedMessages = p.Mails.Select(m => new ExportMessageDto()
                    {
                        Description = new string(m.Description.ToCharArray().Reverse().ToArray()),
                    })
                    .ToArray()
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();

            string xmlResult = XmlHelper.Serialize(prisoners, "Prisoners");

            return xmlResult;
        }
    }
}