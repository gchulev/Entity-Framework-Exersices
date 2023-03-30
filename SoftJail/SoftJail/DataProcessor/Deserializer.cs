namespace SoftJail.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    using Data;

    using Newtonsoft.Json;

    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using SoftJail.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";

        private const string SuccessfullyImportedDepartment = "Imported {0} with {1} cells";

        private const string SuccessfullyImportedPrisoner = "Imported {0} {1} years old";

        private const string SuccessfullyImportedOfficer = "Imported {0} ({1} prisoners)";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            ImportDepatmentDto[] departmentsDto = JsonConvert.DeserializeObject<ImportDepatmentDto[]>(jsonString)!;

            var sb = new StringBuilder();
            var validDepartments = new List<Department>();

            foreach (ImportDepatmentDto departmentDto in departmentsDto)
            {
                if (!IsValid(departmentDto) || departmentDto.Cells == null || departmentDto.Cells.Length == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var validDepartment = new Department()
                {
                    Name = departmentDto.Name,
                };

                bool isValidDepartment = true;

                foreach (ImportCellDto cellDto in departmentDto.Cells)
                {
                    if (!IsValid(cellDto))
                    {
                        isValidDepartment = false;
                        sb.AppendLine(ErrorMessage);
                        break;
                    }

                    Cell validCell = new Cell()
                    {
                        CellNumber = cellDto.CellNumber,
                        HasWindow = cellDto.HasWindow
                    };
                    validDepartment.Cells.Add(validCell);
                }

                if (isValidDepartment)
                {
                    validDepartments.Add(validDepartment);
                    sb.AppendLine(string.Format(SuccessfullyImportedDepartment, validDepartment.Name, validDepartment.Cells.Count));
                }
            }

            context.Departments.AddRange(validDepartments);
            context.SaveChanges();

            int cellsCount = validDepartments.SelectMany(d => d.Cells).Count();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            ImportPrisonerDto[] prisonersDto = JsonConvert.DeserializeObject<ImportPrisonerDto[]>(jsonString)!;

            var sb = new StringBuilder();
            var validPrisoners = new List<Prisoner>();

            foreach (ImportPrisonerDto prisonerDto in prisonersDto)
            {
                if (!IsValid(prisonerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var validPrisoner = new Prisoner()
                {
                    FullName = prisonerDto.FullName,
                    Nickname = prisonerDto.Nickname,
                    Age = prisonerDto.Age,
                    IncarcerationDate = DateTime.ParseExact(prisonerDto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ReleaseDate = prisonerDto.ReleaseDate == null ? null : DateTime.ParseExact(prisonerDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Bail = prisonerDto.Bail,
                    CellId = prisonerDto.CellId
                };

                bool isValidPrisoner = true;

                foreach (ImportMailDto mailDto in prisonerDto.Mails)
                {
                    if (!IsValid(mailDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        isValidPrisoner = false;
                        break;
                    }
                    Mail validMail = new Mail()
                    {
                        Description = mailDto.Description,
                        Sender = mailDto.Sender,
                        Address = mailDto.Address
                    };
                    validPrisoner.Mails.Add(validMail);
                }

                if (isValidPrisoner)
                {
                    validPrisoners.Add(validPrisoner);
                    sb.AppendLine(string.Format(SuccessfullyImportedPrisoner, validPrisoner.FullName, validPrisoner.Age));
                }
            }
            int mailCount = validPrisoners.SelectMany(vp => vp.Mails).Count();
            context.Prisoners.AddRange(validPrisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            ImportOfficerDto[] officersDto = XmlHelper.Deserialize<ImportOfficerDto[]>(xmlString, "Officers");

            var sb = new StringBuilder();
            var validOfficers = new List<Officer>();

            foreach (ImportOfficerDto officerDto in officersDto)
            {
                if (!IsValid(officerDto) || !Enum.IsDefined(typeof(Position), officerDto.Position) || !Enum.IsDefined(typeof(Weapon), officerDto.Weapon))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var validOfficer = new Officer()
                {
                    FullName = officerDto.FullName,
                    Salary = officerDto.Salary,
                    Position = (Position)Enum.Parse(typeof(Position), officerDto.Position),
                    Weapon = (Weapon)Enum.Parse(typeof(Weapon), officerDto.Weapon),
                    DepartmentId = officerDto.DepartmentId,
                    OfficerPrisoners = officerDto.Prisoners.Select(p => new OfficerPrisoner()
                    {
                        PrisonerId = p.Id
                    })
                    .ToArray()
                };

                validOfficers.Add(validOfficer);
                sb.AppendLine(string.Format(SuccessfullyImportedOfficer, validOfficer.FullName, validOfficer.OfficerPrisoners.Count));
            }

            int prisonersCount = validOfficers.SelectMany(vo => vo.OfficerPrisoners).Count();

            context.Officers.AddRange(validOfficers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}