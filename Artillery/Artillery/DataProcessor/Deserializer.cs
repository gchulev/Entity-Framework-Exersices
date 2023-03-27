namespace Artillery.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.Design;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Artillery.Utilities;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            ImportCountryDto[] countriesDto = XmlHelper.Deserialize<ImportCountryDto[]>(xmlString, "Countries");

            var sb = new StringBuilder();
            List<Country> countries = new List<Country>();

            foreach (ImportCountryDto country in countriesDto)
            {
                if (!IsValid(country))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                countries.Add(new Country()
                {
                    ArmySize = country.ArmySize,
                    CountryName = country.CountryName
                });

                sb.AppendLine(string.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
            }

            context.Countries.AddRange(countries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            ImportManufacturerDto[] manufacturersDto = XmlHelper.Deserialize<ImportManufacturerDto[]>(xmlString, "Manufacturers");

            var sb = new StringBuilder();
            var manufacturers = new List<Manufacturer>();

            foreach (ImportManufacturerDto mf in manufacturersDto)
            {
                if (!IsValid(mf) || manufacturers.Any(m => m.ManufacturerName == mf.ManufacturerName))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                manufacturers.Add(new Manufacturer()
                {
                    ManufacturerName = mf.ManufacturerName,
                    Founded = mf.Founded
                });

                string[] namesFound = NamesFound(mf.Founded);
                string town = namesFound[0];
                string country = namesFound[1];
                if (country == "Austria")
                {
                    country = " Austria";
                }
                string townCountryStr = $"{town}, {country}";

                sb.AppendLine(string.Format(SuccessfulImportManufacturer, mf.ManufacturerName, townCountryStr));
            }

            context.Manufacturers.AddRange(manufacturers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            ImportShellDto[] shellsDto = XmlHelper.Deserialize<ImportShellDto[]>(xmlString, "Shells");

            var sb = new StringBuilder();
            var shells = new List<Shell>();

            foreach (ImportShellDto s in shellsDto)
            {
                if (!IsValid(s))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                shells.Add(new Shell()
                {
                    ShellWeight = s.ShellWeight,
                    Caliber = s.Caliber
                });

                sb.AppendLine(string.Format(SuccessfulImportShell, s.Caliber, s.ShellWeight));
            }

            context.Shells.AddRange(shells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            var jsonSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
            };

            ImportGunDto[] gunsDto = JsonConvert.DeserializeObject<ImportGunDto[]>(jsonString, jsonSettings)!;

            var sb = new StringBuilder();
            var guns = new List<Gun>();

            foreach (ImportGunDto gn in gunsDto)
            {
                if (!IsValid(gn) || gn.GunType == null || !Enum.IsDefined(typeof(GunType), gn.GunType))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                guns.Add(new Gun()
                {
                    BarrelLength = gn.BarrelLength,
                    GunType = (GunType)Enum.Parse(typeof(GunType), gn.GunType!),
                    GunWeight = gn.GunWeight,
                    ManufacturerId = gn.ManufacturerId,
                    NumberBuild = gn.NumberBuild,
                    Range = gn.Range,
                    ShellId = gn.ShellId,
                    CountriesGuns = gn.Countries.Select(c => new CountryGun()
                    {
                        CountryId = c.Id
                    })
                    .ToArray()
                });

                sb.AppendLine(string.Format(SuccessfulImportGun, gn.GunType, gn.GunWeight, gn.BarrelLength));
            }

            context.Guns.AddRange(guns);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }

        private static string[] NamesFound(string inputText)
        {
            string pattern = @"\s*(?<town>[^,]+),\s*(?<country>[^,]+)$";

            var rgx = new Regex(pattern);

            MatchCollection foundMatches = rgx.Matches(inputText);
            var listValues = new List<string>();

            foreach (Match match in foundMatches)
            {
                if (!string.IsNullOrWhiteSpace(match.Groups["town"].Value.Trim()))
                {
                    listValues.Add(match.Groups["town"].Value.Trim());
                }

                if (!string.IsNullOrWhiteSpace(match.Groups["country"].Value.Trim()))
                {
                    listValues.Add(match.Groups["country"].Value.Trim());
                }
            }

            return listValues.ToArray();
        }
    }
}