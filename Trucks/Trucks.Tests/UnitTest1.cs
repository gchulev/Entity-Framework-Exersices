//Resharper disable InconsistentNaming, CheckNamespace

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NUnit.Framework;

using Trucks;
using Trucks.Data;
using Trucks.DataProcessor;

[TestFixture]
public class Export_000_001
{
    private IServiceProvider serviceProvider;
    private static Assembly CurrentAssembly;

    [SetUp]
    public void Setup()
    {
        CurrentAssembly = typeof(StartUp).Assembly;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TrucksProfile>();
        });

        this.serviceProvider = ConfigureServices<TrucksContext>("Trucks");
    }

    [Test]
    public void ExportClientsWithMostTrucksZeroTest()
    {
        var context = serviceProvider.GetService<TrucksContext>();

        SeedDatabase(context);
        int capacity = 1000;
        var actualOutputValue = Serializer.ExportClientsWithMostTrucks(context, capacity);
        var actualOutput = JToken.Parse(actualOutputValue);

        var expectedOutputValue =
            "[{\"Name\":\"Gebr. Mayer GmbH & Co. KG\",\"Trucks\":[{\"TruckRegistrationNumber\":\"CT5206MM\",\"VinNumber\":\"WDB96341311261287\",\"TankCapacity\":1420,\"CargoCapacity\":28058,\"CategoryType\":\"Flatbed\",\"MakeType\":\"Daf\"},{\"TruckRegistrationNumber\":\"CT4453MP\",\"VinNumber\":\"WDB96341311269859\",\"TankCapacity\":1420,\"CargoCapacity\":28058,\"CategoryType\":\"Jumbo\",\"MakeType\":\"Man\"},{\"TruckRegistrationNumber\":\"CT6631TT\",\"VinNumber\":\"XLRTE47MS1G141929\",\"TankCapacity\":1200,\"CargoCapacity\":27303,\"CategoryType\":\"Refrigerated\",\"MakeType\":\"Scania\"},{\"TruckRegistrationNumber\":\"CT5204MM\",\"VinNumber\":\"WDB96341311261293\",\"TankCapacity\":1420,\"CargoCapacity\":28058,\"CategoryType\":\"Jumbo\",\"MakeType\":\"Volvo\"},{\"TruckRegistrationNumber\":\"CT2706TT\",\"VinNumber\":\"YS2R4X211D5333237\",\"TankCapacity\":1400,\"CargoCapacity\":27000,\"CategoryType\":\"Flatbed\",\"MakeType\":\"Volvo\"}]},{\"Name\":\"North Sea Express\",\"Trucks\":[{\"TruckRegistrationNumber\":\"CT3124BB\",\"VinNumber\":\"XLRTE47MS1G146169\",\"TankCapacity\":1200,\"CargoCapacity\":28295,\"CategoryType\":\"Jumbo\",\"MakeType\":\"Daf\"},{\"TruckRegistrationNumber\":\"CT1439BB\",\"VinNumber\":\"XLRTE47MS1G146275\",\"TankCapacity\":1200,\"CargoCapacity\":28295,\"CategoryType\":\"Refrigerated\",\"MakeType\":\"Mercedes\"},{\"TruckRegistrationNumber\":\"CT5208MM\",\"VinNumber\":\"WDB96341311261288\",\"TankCapacity\":1420,\"CargoCapacity\":28058,\"CategoryType\":\"Jumbo\",\"MakeType\":\"Mercedes\"},{\"TruckRegistrationNumber\":\"CT5203MM\",\"VinNumber\":\"WDB96341311261291\",\"TankCapacity\":1420,\"CargoCapacity\":28058,\"CategoryType\":\"Semi\",\"MakeType\":\"Mercedes\"},{\"TruckRegistrationNumber\":\"CT1231BB\",\"VinNumber\":\"XLRTE47MS1G146652\",\"TankCapacity\":1290,\"CargoCapacity\":28307,\"CategoryType\":\"Semi\",\"MakeType\":\"Scania\"}]},{\"Name\":\"Buchen UmweltService GmbH\",\"Trucks\":[{\"TruckRegistrationNumber\":\"CT3124BB\",\"VinNumber\":\"XLRTE47MS1G146169\",\"TankCapacity\":1200,\"CargoCapacity\":28295,\"CategoryType\":\"Jumbo\",\"MakeType\":\"Daf\"},{\"TruckRegistrationNumber\":\"CT5621PB\",\"VinNumber\":\"WDB96341311389279\",\"TankCapacity\":1420,\"CargoCapacity\":8306,\"CategoryType\":\"Refrigerated\",\"MakeType\":\"Daf\"},{\"TruckRegistrationNumber\":\"CT5208MM\",\"VinNumber\":\"WDB96341311261288\",\"TankCapacity\":1420,\"CargoCapacity\":28058,\"CategoryType\":\"Jumbo\",\"MakeType\":\"Mercedes\"},{\"TruckRegistrationNumber\":\"CT5205MM\",\"VinNumber\":\"WDB96341311261296\",\"TankCapacity\":1420,\"CargoCapacity\":28058,\"CategoryType\":\"Refrigerated\",\"MakeType\":\"Scania\"}]},{\"Name\":\"ELEKTON AO\",\"Trucks\":[{\"TruckRegistrationNumber\":\"CT4968PM\",\"VinNumber\":\"W1T96341311427813\",\"TankCapacity\":1420,\"CargoCapacity\":9196,\"CategoryType\":\"Refrigerated\",\"MakeType\":\"Man\"},{\"TruckRegistrationNumber\":\"CT5639PB\",\"VinNumber\":\"WDB96341311389271\",\"TankCapacity\":1420,\"CargoCapacity\":11073,\"CategoryType\":\"Jumbo\",\"MakeType\":\"Mercedes\"},{\"TruckRegistrationNumber\":\"CT5210MM\",\"VinNumber\":\"WDB96341311261289\",\"TankCapacity\":1420,\"CargoCapacity\":28058,\"CategoryType\":\"Flatbed\",\"MakeType\":\"Scania\"},{\"TruckRegistrationNumber\":\"CT6631TT\",\"VinNumber\":\"XLRTE47MS1G141929\",\"TankCapacity\":1200,\"CargoCapacity\":27303,\"CategoryType\":\"Refrigerated\",\"MakeType\":\"Scania\"}]},{\"Name\":\"Heinemann GmbH\",\"Trucks\":[{\"TruckRegistrationNumber\":\"CT3094CB\",\"VinNumber\":\"W1T96341311428767\",\"TankCapacity\":1420,\"CargoCapacity\":12817,\"CategoryType\":\"Semi\",\"MakeType\":\"Daf\"},{\"TruckRegistrationNumber\":\"CT5208MM\",\"VinNumber\":\"WDB96341311261288\",\"TankCapacity\":1420,\"CargoCapacity\":28058,\"CategoryType\":\"Jumbo\",\"MakeType\":\"Mercedes\"},{\"TruckRegistrationNumber\":\"CB0441XT\",\"VinNumber\":\"XLRTE47MS1G141844\",\"TankCapacity\":1290,\"CargoCapacity\":27317,\"CategoryType\":\"Refrigerated\",\"MakeType\":\"Mercedes\"},{\"TruckRegistrationNumber\":\"CT5647PB\",\"VinNumber\":\"WDB96341311389272\",\"TankCapacity\":1420,\"CargoCapacity\":8614,\"CategoryType\":\"Refrigerated\",\"MakeType\":\"Mercedes\"}]},{\"Name\":\"MCBURNEY HOLDINGS LIMITED\",\"Trucks\":[{\"TruckRegistrationNumber\":\"CT1157HH\",\"VinNumber\":\"WDB96341311356127\",\"TankCapacity\":1420,\"CargoCapacity\":28097,\"CategoryType\":\"Jumbo\",\"MakeType\":\"Man\"},{\"TruckRegistrationNumber\":\"CT4968PM\",\"VinNumber\":\"W1T96341311427813\",\"TankCapacity\":1420,\"CargoCapacity\":9196,\"CategoryType\":\"Refrigerated\",\"MakeType\":\"Man\"},{\"TruckRegistrationNumber\":\"CB0796TP\",\"VinNumber\":\"YS2R4X211D5318181\",\"TankCapacity\":1000,\"CargoCapacity\":23999,\"CategoryType\":\"Flatbed\",\"MakeType\":\"Scania\"},{\"TruckRegistrationNumber\":\"CT5617PB\",\"VinNumber\":\"WDB96341311389277\",\"TankCapacity\":1420,\"CargoCapacity\":6421,\"CategoryType\":\"Semi\",\"MakeType\":\"Volvo\"}]},{\"Name\":\"Meopta � optika s.r.o.\",\"Trucks\":[{\"TruckRegistrationNumber\":\"CT1230BB\",\"VinNumber\":\"XLRTE47MS1G146164\",\"TankCapacity\":1290,\"CargoCapacity\":28295,\"CategoryType\":\"Semi\",\"MakeType\":\"Daf\"},{\"TruckRegistrationNumber\":\"CT4453MP\",\"VinNumber\":\"WDB96341311269859\",\"TankCapacity\":1420,\"CargoCapacity\":28058,\"CategoryType\":\"Jumbo\",\"MakeType\":\"Man\"},{\"TruckRegistrationNumber\":\"CT1231BB\",\"VinNumber\":\"XLRTE47MS1G146652\",\"TankCapacity\":1290,\"CargoCapacity\":28307,\"CategoryType\":\"Semi\",\"MakeType\":\"Scania\"},{\"TruckRegistrationNumber\":\"CT2706TT\",\"VinNumber\":\"YS2R4X211D5333237\",\"TankCapacity\":1400,\"CargoCapacity\":27000,\"CategoryType\":\"Flatbed\",\"MakeType\":\"Volvo\"}]},{\"Name\":\"Schmidt Gastransporte GmbH & Co KG\",\"Trucks\":[{\"TruckRegistrationNumber\":\"CT5627PB\",\"VinNumber\":\"WDB96341311389285\",\"TankCapacity\":1420,\"CargoCapacity\":19156,\"CategoryType\":\"Refrigerated\",\"MakeType\":\"Daf\"},{\"TruckRegistrationNumber\":\"CT5629PB\",\"VinNumber\":\"WDB96341311388385\",\"TankCapacity\":1420,\"CargoCapacity\":8752,\"CategoryType\":\"Flatbed\",\"MakeType\":\"Man\"},{\"TruckRegistrationNumber\":\"CT5205MM\",\"VinNumber\":\"WDB96341311261296\",\"TankCapacity\":1420,\"CargoCapacity\":28058,\"CategoryType\":\"Refrigerated\",\"MakeType\":\"Scania\"},{\"TruckRegistrationNumber\":\"CT1145HH\",\"VinNumber\":\"WDB96341311356138\",\"TankCapacity\":1420,\"CargoCapacity\":28097,\"CategoryType\":\"Semi\",\"MakeType\":\"Volvo\"}]},{\"Name\":\"ABC-Logistik GmbH\",\"Trucks\":[{\"TruckRegistrationNumber\":\"CB6230XT\",\"VinNumber\":\"XLRTE47MS1G142311\",\"TankCapacity\":1290,\"CargoCapacity\":28315,\"CategoryType\":\"Flatbed\",\"MakeType\":\"Daf\"},{\"TruckRegistrationNumber\":\"CT6631TT\",\"VinNumber\":\"XLRTE47MS1G141929\",\"TankCapacity\":1200,\"CargoCapacity\":27303,\"CategoryType\":\"Refrigerated\",\"MakeType\":\"Scania\"},{\"TruckRegistrationNumber\":\"CT5625PB\",\"VinNumber\":\"WDB96341311389283\",\"TankCapacity\":1420,\"CargoCapacity\":5358,\"CategoryType\":\"Refrigerated\",\"MakeType\":\"Scania\"}]},{\"Name\":\"BILLY BOWIE SPECIAL PROJECTS LIMITED\",\"Trucks\":[{\"TruckRegistrationNumber\":\"CT5622PB\",\"VinNumber\":\"WDB96341311389281\",\"TankCapacity\":1420,\"CargoCapacity\":14970,\"CategoryType\":\"Flatbed\",\"MakeType\":\"Man\"},{\"TruckRegistrationNumber\":\"CT1293HH\",\"VinNumber\":\"XLRTE47MS1G141111\",\"TankCapacity\":1290,\"CargoCapacity\":27317,\"CategoryType\":\"Refrigerated\",\"MakeType\":\"Volvo\"},{\"TruckRegistrationNumber\":\"CT5633PB\",\"VinNumber\":\"WDB96341311388387\",\"TankCapacity\":1420,\"CargoCapacity\":15837,\"CategoryType\":\"Jumbo\",\"MakeType\":\"Volvo\"}]}]";
        var expectedOutput = JToken.Parse(expectedOutputValue);

        var expected = expectedOutput.ToString(Formatting.None);
        var actual = actualOutput.ToString(Formatting.None);
        ;
        Assert.That(actual, Is.EqualTo(expected).NoClip,
            $"{nameof(Serializer.ExportClientsWithMostTrucks)} output is incorrect!");
    }

    private static void SeedDatabase(TrucksContext context)
    {
        var datasetsJson = "{\"Despatcher\":[{\"Id\":1,\"Name\":\"Genadi Petrov\",\"Position\":\"Specialist\"},{\"Id\":2,\"Name\":\"Apostol Dobromirov\",\"Position\":\"Specialist\"},{\"Id\":3,\"Name\":\"Hristina Petrova\",\"Position\":\"Forwarder\"},{\"Id\":4,\"Name\":\"Ekaterina Hristova\",\"Position\":\"Trainee\"},{\"Id\":5,\"Name\":\"Petko Todorov\",\"Position\":\"Manager\"},{\"Id\":6,\"Name\":\"Todor Petrov\",\"Position\":\"Specialist\"},{\"Id\":7,\"Name\":\"Asen Hristov\",\"Position\":\"Trainee\"},{\"Id\":8,\"Name\":\"Georgi Atanasov\",\"Position\":\"Manager\"},{\"Id\":9,\"Name\":\"Vladislav Dobromirov\",\"Position\":\"Specialist\"},{\"Id\":10,\"Name\":\"Polina Kostadinova\",\"Position\":\"Forwarder\"},{\"Id\":11,\"Name\":\"Miahela Eneva\",\"Position\":\"Trainee\"},{\"Id\":12,\"Name\":\"Dobromir Plamenov\",\"Position\":\"Specialist\"},{\"Id\":13,\"Name\":\"Vladimir Hristov\",\"Position\":\"Forwarder\"},{\"Id\":14,\"Name\":\"Georgi Stefanov\",\"Position\":\"Trainee\"},{\"Id\":15,\"Name\":\"Atanas Petrov\",\"Position\":\"Manager\"},{\"Id\":16,\"Name\":\"Boris Todorov\",\"Position\":\"Trainee\"},{\"Id\":17,\"Name\":\"Ivan Borislavov\",\"Position\":\"Manager\"},{\"Id\":18,\"Name\":\"Borislav Petrov\",\"Position\":\"Specialist\"},{\"Id\":19,\"Name\":\"Ivan Petrov\",\"Position\":\"Forwarder\"},{\"Id\":20,\"Name\":\"Ivan Hristov\",\"Position\":\"Manager\"},{\"Id\":21,\"Name\":\"Hristo Ivanov\",\"Position\":\"Specialist\"},{\"Id\":22,\"Name\":\"Kalina Petrova\",\"Position\":\"Trainee\"},{\"Id\":23,\"Name\":\"Desislava Hristova\",\"Position\":\"Manager\"},{\"Id\":24,\"Name\":\"Ana Ivanova\",\"Position\":\"Forwarder\"},{\"Id\":25,\"Name\":\"Stela Stefanova\",\"Position\":\"Trainee\"},{\"Id\":26,\"Name\":\"Veselin Daskalov\",\"Position\":\"Manager\"},{\"Id\":27,\"Name\":\"Stefan Dragomirov\",\"Position\":\"Specialist\"},{\"Id\":28,\"Name\":\"Kosta Slavov\",\"Position\":\"Manager\"},{\"Id\":29,\"Name\":\"Grigor Petrov\",\"Position\":\"Manager\"},{\"Id\":30,\"Name\":\"Simeon Ivanov\",\"Position\":\"Trainee\"}],\"Truck\":[{\"Id\":1,\"RegistrationNumber\":\"CB0796TP\",\"VinNumber\":\"YS2R4X211D5318181\",\"TankCapacity\":1000,\"CargoCapacity\":23999,\"CategoryType\":0,\"MakeType\":3},{\"Id\":2,\"RegistrationNumber\":\"CT5209MM\",\"VinNumber\":\"WDB96341311261294\",\"TankCapacity\":1420,\"CargoCapacity\":28058,\"CategoryType\":2,\"MakeType\":4},{\"Id\":3,\"RegistrationNumber\":\"CT5210MM\",\"VinNumber\":\"WDB96341311261289\",\"TankCapacity\":1420,\"CargoCapacity\":28058,\"CategoryType\":0,\"MakeType\":3},{\"Id\":4,\"RegistrationNumber\":\"CT4453MP\",\"VinNumber\":\"WDB96341311269859\",\"TankCapacity\":1420,\"CargoCapacity\":28058,\"CategoryType\":1,\"MakeType\":1},{\"Id\":5,\"RegistrationNumber\":\"CT9054HM\",\"VinNumber\":\"WDB96341611356141\",\"TankCapacity\":990,\"CargoCapacity\":28399,\"CategoryType\":2,\"MakeType\":0},{\"Id\":6,\"RegistrationNumber\":\"CT9057HM\",\"VinNumber\":\"WDB96341611356142\",\"TankCapacity\":990,\"CargoCapacity\":28399,\"CategoryType\":0,\"MakeType\":1},{\"Id\":7,\"RegistrationNumber\":\"CT9049HM\",\"VinNumber\":\"WDB96341611356151\",\"TankCapacity\":990,\"CargoCapacity\":28399,\"CategoryType\":0,\"MakeType\":0},{\"Id\":8,\"RegistrationNumber\":\"CT1151HH\",\"VinNumber\":\"WDB96341611356438\",\"TankCapacity\":990,\"CargoCapacity\":28399,\"CategoryType\":2,\"MakeType\":4},{\"Id\":9,\"RegistrationNumber\":\"CT1160HH\",\"VinNumber\":\"WDB96341611356439\",\"TankCapacity\":990,\"CargoCapacity\":28399,\"CategoryType\":0,\"MakeType\":3},{\"Id\":10,\"RegistrationNumber\":\"CT1157HH\",\"VinNumber\":\"WDB96341311356127\",\"TankCapacity\":1420,\"CargoCapacity\":28097,\"CategoryType\":1,\"MakeType\":1},{\"Id\":11,\"RegistrationNumber\":\"CT1159HH\",\"VinNumber\":\"WDB96341611356143\",\"TankCapacity\":990,\"CargoCapacity\":28399,\"CategoryType\":1,\"MakeType\":0},{\"Id\":12,\"RegistrationNumber\":\"CT1143HH\",\"VinNumber\":\"WDB96341611356149\",\"TankCapacity\":991,\"CargoCapacity\":28399,\"CategoryType\":0,\"MakeType\":2},{\"Id\":13,\"RegistrationNumber\":\"CT1145HH\",\"VinNumber\":\"WDB96341311356138\",\"TankCapacity\":1420,\"CargoCapacity\":28097,\"CategoryType\":3,\"MakeType\":4},{\"Id\":14,\"RegistrationNumber\":\"CT5627PB\",\"VinNumber\":\"WDB96341311389285\",\"TankCapacity\":1420,\"CargoCapacity\":19156,\"CategoryType\":2,\"MakeType\":0},{\"Id\":15,\"RegistrationNumber\":\"CT5629PB\",\"VinNumber\":\"WDB96341311388385\",\"TankCapacity\":1420,\"CargoCapacity\":8752,\"CategoryType\":0,\"MakeType\":1},{\"Id\":16,\"RegistrationNumber\":\"CT5630PB\",\"VinNumber\":\"WDB96341311388386\",\"TankCapacity\":1420,\"CargoCapacity\":15083,\"CategoryType\":3,\"MakeType\":2},{\"Id\":17,\"RegistrationNumber\":\"CT5633PB\",\"VinNumber\":\"WDB96341311388387\",\"TankCapacity\":1420,\"CargoCapacity\":15837,\"CategoryType\":1,\"MakeType\":4},{\"Id\":18,\"RegistrationNumber\":\"CT5637PB\",\"VinNumber\":\"WDB96341311388389\",\"TankCapacity\":1420,\"CargoCapacity\":10623,\"CategoryType\":0,\"MakeType\":0},{\"Id\":19,\"RegistrationNumber\":\"CT5639PB\",\"VinNumber\":\"WDB96341311389271\",\"TankCapacity\":1420,\"CargoCapacity\":11073,\"CategoryType\":1,\"MakeType\":2},{\"Id\":20,\"RegistrationNumber\":\"CT5643PB\",\"VinNumber\":\"WDB96341311396613\",\"TankCapacity\":1420,\"CargoCapacity\":7108,\"CategoryType\":0,\"MakeType\":3},{\"Id\":21,\"RegistrationNumber\":\"CT5645PB\",\"VinNumber\":\"WDB96341311388186\",\"TankCapacity\":1420,\"CargoCapacity\":6036,\"CategoryType\":1,\"MakeType\":1},{\"Id\":22,\"RegistrationNumber\":\"CT5647PB\",\"VinNumber\":\"WDB96341311389272\",\"TankCapacity\":1420,\"CargoCapacity\":8614,\"CategoryType\":2,\"MakeType\":2},{\"Id\":23,\"RegistrationNumber\":\"CT5648PB\",\"VinNumber\":\"WDB96341311388187\",\"TankCapacity\":1420,\"CargoCapacity\":13039,\"CategoryType\":0,\"MakeType\":4},{\"Id\":24,\"RegistrationNumber\":\"CT5650PB\",\"VinNumber\":\"WDB96341311389276\",\"TankCapacity\":1420,\"CargoCapacity\":14798,\"CategoryType\":1,\"MakeType\":0},{\"Id\":25,\"RegistrationNumber\":\"CT5617PB\",\"VinNumber\":\"WDB96341311389277\",\"TankCapacity\":1420,\"CargoCapacity\":6421,\"CategoryType\":3,\"MakeType\":4},{\"Id\":26,\"RegistrationNumber\":\"CT5621PB\",\"VinNumber\":\"WDB96341311389279\",\"TankCapacity\":1420,\"CargoCapacity\":8306,\"CategoryType\":2,\"MakeType\":0},{\"Id\":27,\"RegistrationNumber\":\"CT5622PB\",\"VinNumber\":\"WDB96341311389281\",\"TankCapacity\":1420,\"CargoCapacity\":14970,\"CategoryType\":0,\"MakeType\":1},{\"Id\":28,\"RegistrationNumber\":\"CT5625PB\",\"VinNumber\":\"WDB96341311389283\",\"TankCapacity\":1420,\"CargoCapacity\":5358,\"CategoryType\":2,\"MakeType\":3},{\"Id\":29,\"RegistrationNumber\":\"CT3094CB\",\"VinNumber\":\"W1T96341311428767\",\"TankCapacity\":1420,\"CargoCapacity\":12817,\"CategoryType\":3,\"MakeType\":0},{\"Id\":30,\"RegistrationNumber\":\"CT6571CT\",\"VinNumber\":\"W1T96341311427814\",\"TankCapacity\":1420,\"CargoCapacity\":15795,\"CategoryType\":2,\"MakeType\":2},{\"Id\":31,\"RegistrationNumber\":\"CT5208MM\",\"VinNumber\":\"WDB96341311261288\",\"TankCapacity\":1420,\"CargoCapacity\":28058,\"CategoryType\":1,\"MakeType\":2},{\"Id\":32,\"RegistrationNumber\":\"CT4963PM\",\"VinNumber\":\"W1T96341311427817\",\"TankCapacity\":1420,\"CargoCapacity\":8168,\"CategoryType\":0,\"MakeType\":4},{\"Id\":33,\"RegistrationNumber\":\"CT5206MM\",\"VinNumber\":\"WDB96341311261287\",\"TankCapacity\":1420,\"CargoCapacity\":28058,\"CategoryType\":0,\"MakeType\":0},{\"Id\":34,\"RegistrationNumber\":\"CT5204MM\",\"VinNumber\":\"WDB96341311261293\",\"TankCapacity\":1420,\"CargoCapacity\":28058,\"CategoryType\":1,\"MakeType\":4},{\"Id\":35,\"RegistrationNumber\":\"CT2706TT\",\"VinNumber\":\"YS2R4X211D5333237\",\"TankCapacity\":1400,\"CargoCapacity\":27000,\"CategoryType\":0,\"MakeType\":4},{\"Id\":36,\"RegistrationNumber\":\"CB9655TX\",\"VinNumber\":\"YS2R4X211D5332926\",\"TankCapacity\":1400,\"CargoCapacity\":25000,\"CategoryType\":3,\"MakeType\":3},{\"Id\":37,\"RegistrationNumber\":\"CB8300XC\",\"VinNumber\":\"WDB9341621L884471\",\"TankCapacity\":990,\"CargoCapacity\":23738,\"CategoryType\":0,\"MakeType\":3},{\"Id\":38,\"RegistrationNumber\":\"CB0441XT\",\"VinNumber\":\"XLRTE47MS1G141844\",\"TankCapacity\":1290,\"CargoCapacity\":27317,\"CategoryType\":2,\"MakeType\":2},{\"Id\":39,\"RegistrationNumber\":\"CB2071XT\",\"VinNumber\":\"XLRTE47MS1G141194\",\"TankCapacity\":1290,\"CargoCapacity\":27317,\"CategoryType\":3,\"MakeType\":3},{\"Id\":40,\"RegistrationNumber\":\"CB2117XT\",\"VinNumber\":\"XLRTE47MS1G141136\",\"TankCapacity\":1290,\"CargoCapacity\":26317,\"CategoryType\":1,\"MakeType\":0},{\"Id\":41,\"RegistrationNumber\":\"CT6631TT\",\"VinNumber\":\"XLRTE47MS1G141929\",\"TankCapacity\":1200,\"CargoCapacity\":27303,\"CategoryType\":2,\"MakeType\":3},{\"Id\":42,\"RegistrationNumber\":\"CB6230XT\",\"VinNumber\":\"XLRTE47MS1G142311\",\"TankCapacity\":1290,\"CargoCapacity\":28315,\"CategoryType\":0,\"MakeType\":0},{\"Id\":43,\"RegistrationNumber\":\"CB7630XT\",\"VinNumber\":\"XLRTE47MS1G141917\",\"TankCapacity\":1200,\"CargoCapacity\":27303,\"CategoryType\":3,\"MakeType\":1},{\"Id\":44,\"RegistrationNumber\":\"CT1293HH\",\"VinNumber\":\"XLRTE47MS1G141111\",\"TankCapacity\":1290,\"CargoCapacity\":27317,\"CategoryType\":2,\"MakeType\":4},{\"Id\":45,\"RegistrationNumber\":\"CT1230BB\",\"VinNumber\":\"XLRTE47MS1G146164\",\"TankCapacity\":1290,\"CargoCapacity\":28295,\"CategoryType\":3,\"MakeType\":0},{\"Id\":46,\"RegistrationNumber\":\"CT1437BB\",\"VinNumber\":\"XLRTE47MS1G146235\",\"TankCapacity\":1290,\"CargoCapacity\":28295,\"CategoryType\":1,\"MakeType\":1},{\"Id\":47,\"RegistrationNumber\":\"CT1439BB\",\"VinNumber\":\"XLRTE47MS1G146275\",\"TankCapacity\":1200,\"CargoCapacity\":28295,\"CategoryType\":2,\"MakeType\":2},{\"Id\":48,\"RegistrationNumber\":\"CT1231BB\",\"VinNumber\":\"XLRTE47MS1G146652\",\"TankCapacity\":1290,\"CargoCapacity\":28307,\"CategoryType\":3,\"MakeType\":3},{\"Id\":49,\"RegistrationNumber\":\"CT3124BB\",\"VinNumber\":\"XLRTE47MS1G146169\",\"TankCapacity\":1200,\"CargoCapacity\":28295,\"CategoryType\":1,\"MakeType\":0},{\"Id\":50,\"RegistrationNumber\":\"CT3093BB\",\"VinNumber\":\"XLRTE47MS1G146691\",\"TankCapacity\":1290,\"CargoCapacity\":28307,\"CategoryType\":3,\"MakeType\":2},{\"Id\":51,\"RegistrationNumber\":\"CT2440BH\",\"VinNumber\":\"WMA13XZZ7FM681526\",\"TankCapacity\":960,\"CargoCapacity\":28416,\"CategoryType\":1,\"MakeType\":4},{\"Id\":52,\"RegistrationNumber\":\"CT8985BP\",\"VinNumber\":\"WMA13XZZ6GM685875\",\"TankCapacity\":960,\"CargoCapacity\":28431,\"CategoryType\":2,\"MakeType\":3},{\"Id\":53,\"RegistrationNumber\":\"CT8984BP\",\"VinNumber\":\"WMA13XZZ1GM685919\",\"TankCapacity\":960,\"CargoCapacity\":28441,\"CategoryType\":0,\"MakeType\":0},{\"Id\":54,\"RegistrationNumber\":\"CT4850CC\",\"VinNumber\":\"WMA13XZZ3GM685722\",\"TankCapacity\":960,\"CargoCapacity\":28441,\"CategoryType\":1,\"MakeType\":2},{\"Id\":55,\"RegistrationNumber\":\"CT7548TK\",\"VinNumber\":\"WMA13XZZ3GM685848\",\"TankCapacity\":960,\"CargoCapacity\":28451,\"CategoryType\":2,\"MakeType\":4},{\"Id\":56,\"RegistrationNumber\":\"CT8991BP\",\"VinNumber\":\"WMA13XZZ1GM685761\",\"TankCapacity\":960,\"CargoCapacity\":28441,\"CategoryType\":3,\"MakeType\":0},{\"Id\":57,\"RegistrationNumber\":\"CT2411BX\",\"VinNumber\":\"WMA13XZZ5GM689559\",\"TankCapacity\":960,\"CargoCapacity\":28431,\"CategoryType\":0,\"MakeType\":4},{\"Id\":58,\"RegistrationNumber\":\"CT0102MP\",\"VinNumber\":\"WMA13XZZ3GM689544\",\"TankCapacity\":960,\"CargoCapacity\":28441,\"CategoryType\":3,\"MakeType\":3},{\"Id\":59,\"RegistrationNumber\":\"CT4983HT\",\"VinNumber\":\"WMA13XZZ4GM689536\",\"TankCapacity\":960,\"CargoCapacity\":28446,\"CategoryType\":1,\"MakeType\":0},{\"Id\":60,\"RegistrationNumber\":\"CT7122KM\",\"VinNumber\":\"WMA13XZZ4GM689441\",\"TankCapacity\":960,\"CargoCapacity\":28441,\"CategoryType\":3,\"MakeType\":4},{\"Id\":61,\"RegistrationNumber\":\"CT2462BX\",\"VinNumber\":\"WMA13XZZ2GM689437\",\"TankCapacity\":960,\"CargoCapacity\":28441,\"CategoryType\":1,\"MakeType\":3},{\"Id\":62,\"RegistrationNumber\":\"CT2699CK\",\"VinNumber\":\"XLEG4X21115261281\",\"TankCapacity\":1235,\"CargoCapacity\":28921,\"CategoryType\":2,\"MakeType\":0},{\"Id\":63,\"RegistrationNumber\":\"CT5203MM\",\"VinNumber\":\"WDB96341311261291\",\"TankCapacity\":1420,\"CargoCapacity\":28058,\"CategoryType\":3,\"MakeType\":2},{\"Id\":64,\"RegistrationNumber\":\"CT5205MM\",\"VinNumber\":\"WDB96341311261296\",\"TankCapacity\":1420,\"CargoCapacity\":28058,\"CategoryType\":2,\"MakeType\":3},{\"Id\":65,\"RegistrationNumber\":\"CT4968PM\",\"VinNumber\":\"W1T96341311427813\",\"TankCapacity\":1420,\"CargoCapacity\":9196,\"CategoryType\":2,\"MakeType\":1}],\"Client\":[{\"Id\":1,\"Name\":\"Tekro spol. s r.o.\",\"Nationality\":\"Belgium\",\"Type\":\"platinum\"},{\"Id\":2,\"Name\":\"OLIVOMUNDO � SOCIEDADE AGRICOLA LDA\",\"Nationality\":\"Italy\",\"Type\":\"silver\"},{\"Id\":3,\"Name\":\"North Sea Express\",\"Nationality\":\"Belgium\",\"Type\":\"platinum\"},{\"Id\":4,\"Name\":\"PEK GROUP a.s.\",\"Nationality\":\"France\",\"Type\":\"silver\"},{\"Id\":5,\"Name\":\"ECCO RAIL SP Z O O\",\"Nationality\":\"Germany\",\"Type\":\"golden\"},{\"Id\":6,\"Name\":\"Felbermayr Deutschland GmbH\",\"Nationality\":\"The United Kingdom\",\"Type\":\"golden\"},{\"Id\":7,\"Name\":\"NAZAROVOAGROSNAB AO\",\"Nationality\":\"The Netherlands\",\"Type\":\"platinum\"},{\"Id\":8,\"Name\":\"KRIPTON GmbH\",\"Nationality\":\"Poland\",\"Type\":\"golden\"},{\"Id\":9,\"Name\":\"VYMPEL PLYUS GmbH\",\"Nationality\":\"Denmark\",\"Type\":\"silver\"},{\"Id\":10,\"Name\":\"DHL SERVICES LIMITED\",\"Nationality\":\"The United Kingdom\",\"Type\":\"golden\"},{\"Id\":11,\"Name\":\"SCHENKER SP ZOO\",\"Nationality\":\"France\",\"Type\":\"platinum\"},{\"Id\":12,\"Name\":\"Buchen UmweltService GmbH\",\"Nationality\":\"Belgium\",\"Type\":\"platinum\"},{\"Id\":13,\"Name\":\"SPETSENERGOTRANS AO\",\"Nationality\":\"Denmark\",\"Type\":\"platinum\"},{\"Id\":14,\"Name\":\"MCBURNEY HOLDINGS LIMITED\",\"Nationality\":\"Poland\",\"Type\":\"golden\"},{\"Id\":15,\"Name\":\"ZMK AO\",\"Nationality\":\"The Netherlands\",\"Type\":\"platinum\"},{\"Id\":16,\"Name\":\"Meopta � optika s.r.o.\",\"Nationality\":\"The United Kingdom\",\"Type\":\"platinum\"},{\"Id\":17,\"Name\":\"Srg Holding AS\",\"Nationality\":\"Germany\",\"Type\":\"platinum\"},{\"Id\":18,\"Name\":\"GEKSA-NETKANYE MATERIALY GmbH\",\"Nationality\":\"France\",\"Type\":\"silver\"},{\"Id\":19,\"Name\":\"SKA GmbH\",\"Nationality\":\"Denmark\",\"Type\":\"golden\"},{\"Id\":20,\"Name\":\"SAFPLAST GmbH\",\"Nationality\":\"The Netherlands\",\"Type\":\"golden\"},{\"Id\":21,\"Name\":\"PFEIFER & LANGEN ROMANIA S.R.L.\",\"Nationality\":\"The United Kingdom\",\"Type\":\"platinum\"},{\"Id\":22,\"Name\":\"KOMPANIYA BIO GmbH\",\"Nationality\":\"Germany\",\"Type\":\"silver\"},{\"Id\":23,\"Name\":\"ELEKTON AO\",\"Nationality\":\"France\",\"Type\":\"golden\"},{\"Id\":24,\"Name\":\"TK AGROGRUPP GmbH\",\"Nationality\":\"Belgium\",\"Type\":\"silver\"},{\"Id\":25,\"Name\":\"ABC-Logistik GmbH\",\"Nationality\":\"Denmark\",\"Type\":\"platinum\"},{\"Id\":26,\"Name\":\"Schmidt Gastransporte GmbH & Co KG\",\"Nationality\":\"Italy\",\"Type\":\"golden\"},{\"Id\":27,\"Name\":\"BILLY BOWIE SPECIAL PROJECTS LIMITED\",\"Nationality\":\"The Netherlands\",\"Type\":\"silver\"},{\"Id\":28,\"Name\":\"B.T. TRASPORTI SRL\",\"Nationality\":\"The United Kingdom\",\"Type\":\"golden\"},{\"Id\":29,\"Name\":\"Gebr. Mayer GmbH & Co. KG\",\"Nationality\":\"Germany\",\"Type\":\"golden\"},{\"Id\":30,\"Name\":\"DELTA GmbH\",\"Nationality\":\"France\",\"Type\":\"golden\"},{\"Id\":31,\"Name\":\"Heinemann GmbH\",\"Nationality\":\"The Netherlands\",\"Type\":\"platinum\"},{\"Id\":32,\"Name\":\"LOGISTICA AMBIENTALE SRL\",\"Nationality\":\"The United Kingdom\",\"Type\":\"silver\"}],\"ClientTruck\":[{\"Id\":1,\"ClientId\":14,\"TruckId\":1},{\"Id\":2,\"ClientId\":15,\"TruckId\":1},{\"Id\":3,\"ClientId\":32,\"TruckId\":1},{\"Id\":4,\"ClientId\":23,\"TruckId\":3},{\"Id\":5,\"ClientId\":4,\"TruckId\":4},{\"Id\":6,\"ClientId\":10,\"TruckId\":4},{\"Id\":7,\"ClientId\":11,\"TruckId\":4},{\"Id\":8,\"ClientId\":16,\"TruckId\":4},{\"Id\":9,\"ClientId\":24,\"TruckId\":4},{\"Id\":10,\"ClientId\":29,\"TruckId\":4},{\"Id\":11,\"ClientId\":21,\"TruckId\":5},{\"Id\":12,\"ClientId\":25,\"TruckId\":5},{\"Id\":13,\"ClientId\":2,\"TruckId\":6},{\"Id\":14,\"ClientId\":2,\"TruckId\":7},{\"Id\":15,\"ClientId\":2,\"TruckId\":8},{\"Id\":16,\"ClientId\":13,\"TruckId\":9},{\"Id\":17,\"ClientId\":14,\"TruckId\":10},{\"Id\":18,\"ClientId\":19,\"TruckId\":10},{\"Id\":19,\"ClientId\":19,\"TruckId\":11},{\"Id\":20,\"ClientId\":2,\"TruckId\":12},{\"Id\":21,\"ClientId\":30,\"TruckId\":12},{\"Id\":22,\"ClientId\":26,\"TruckId\":13},{\"Id\":23,\"ClientId\":26,\"TruckId\":14},{\"Id\":24,\"ClientId\":26,\"TruckId\":15},{\"Id\":25,\"ClientId\":18,\"TruckId\":16},{\"Id\":26,\"ClientId\":10,\"TruckId\":17},{\"Id\":27,\"ClientId\":22,\"TruckId\":17},{\"Id\":28,\"ClientId\":27,\"TruckId\":17},{\"Id\":29,\"ClientId\":20,\"TruckId\":18},{\"Id\":30,\"ClientId\":21,\"TruckId\":19},{\"Id\":31,\"ClientId\":23,\"TruckId\":19},{\"Id\":32,\"ClientId\":7,\"TruckId\":20},{\"Id\":33,\"ClientId\":22,\"TruckId\":20},{\"Id\":34,\"ClientId\":24,\"TruckId\":21},{\"Id\":35,\"ClientId\":4,\"TruckId\":22},{\"Id\":36,\"ClientId\":31,\"TruckId\":22},{\"Id\":37,\"ClientId\":24,\"TruckId\":23},{\"Id\":38,\"ClientId\":8,\"TruckId\":24},{\"Id\":39,\"ClientId\":9,\"TruckId\":24},{\"Id\":40,\"ClientId\":15,\"TruckId\":24},{\"Id\":41,\"ClientId\":14,\"TruckId\":25},{\"Id\":42,\"ClientId\":12,\"TruckId\":26},{\"Id\":43,\"ClientId\":20,\"TruckId\":27},{\"Id\":44,\"ClientId\":27,\"TruckId\":27},{\"Id\":45,\"ClientId\":21,\"TruckId\":28},{\"Id\":46,\"ClientId\":25,\"TruckId\":28},{\"Id\":47,\"ClientId\":31,\"TruckId\":29},{\"Id\":48,\"ClientId\":9,\"TruckId\":30},{\"Id\":49,\"ClientId\":3,\"TruckId\":31},{\"Id\":50,\"ClientId\":12,\"TruckId\":31},{\"Id\":51,\"ClientId\":22,\"TruckId\":31},{\"Id\":52,\"ClientId\":31,\"TruckId\":31},{\"Id\":53,\"ClientId\":15,\"TruckId\":32},{\"Id\":54,\"ClientId\":1,\"TruckId\":33},{\"Id\":55,\"ClientId\":29,\"TruckId\":33},{\"Id\":56,\"ClientId\":1,\"TruckId\":34},{\"Id\":57,\"ClientId\":29,\"TruckId\":34},{\"Id\":58,\"ClientId\":16,\"TruckId\":35},{\"Id\":59,\"ClientId\":29,\"TruckId\":35},{\"Id\":60,\"ClientId\":18,\"TruckId\":36},{\"Id\":61,\"ClientId\":31,\"TruckId\":38},{\"Id\":62,\"ClientId\":5,\"TruckId\":40},{\"Id\":63,\"ClientId\":6,\"TruckId\":41},{\"Id\":64,\"ClientId\":23,\"TruckId\":41},{\"Id\":65,\"ClientId\":25,\"TruckId\":41},{\"Id\":66,\"ClientId\":29,\"TruckId\":41},{\"Id\":67,\"ClientId\":25,\"TruckId\":42},{\"Id\":68,\"ClientId\":27,\"TruckId\":44},{\"Id\":69,\"ClientId\":5,\"TruckId\":45},{\"Id\":70,\"ClientId\":7,\"TruckId\":45},{\"Id\":71,\"ClientId\":16,\"TruckId\":45},{\"Id\":72,\"ClientId\":1,\"TruckId\":46},{\"Id\":73,\"ClientId\":32,\"TruckId\":46},{\"Id\":74,\"ClientId\":3,\"TruckId\":47},{\"Id\":75,\"ClientId\":3,\"TruckId\":48},{\"Id\":76,\"ClientId\":16,\"TruckId\":48},{\"Id\":77,\"ClientId\":3,\"TruckId\":49},{\"Id\":78,\"ClientId\":12,\"TruckId\":49},{\"Id\":79,\"ClientId\":32,\"TruckId\":50},{\"Id\":80,\"ClientId\":11,\"TruckId\":51},{\"Id\":81,\"ClientId\":15,\"TruckId\":51},{\"Id\":82,\"ClientId\":29,\"TruckId\":51},{\"Id\":83,\"ClientId\":32,\"TruckId\":51},{\"Id\":84,\"ClientId\":5,\"TruckId\":52},{\"Id\":85,\"ClientId\":32,\"TruckId\":52},{\"Id\":86,\"ClientId\":6,\"TruckId\":53},{\"Id\":87,\"ClientId\":9,\"TruckId\":53},{\"Id\":88,\"ClientId\":11,\"TruckId\":53},{\"Id\":89,\"ClientId\":16,\"TruckId\":53},{\"Id\":90,\"ClientId\":20,\"TruckId\":53},{\"Id\":91,\"ClientId\":12,\"TruckId\":54},{\"Id\":92,\"ClientId\":24,\"TruckId\":54},{\"Id\":93,\"ClientId\":28,\"TruckId\":54},{\"Id\":94,\"ClientId\":8,\"TruckId\":55},{\"Id\":95,\"ClientId\":11,\"TruckId\":56},{\"Id\":96,\"ClientId\":8,\"TruckId\":57},{\"Id\":97,\"ClientId\":28,\"TruckId\":57},{\"Id\":98,\"ClientId\":15,\"TruckId\":58},{\"Id\":99,\"ClientId\":4,\"TruckId\":59},{\"Id\":100,\"ClientId\":5,\"TruckId\":59},{\"Id\":101,\"ClientId\":8,\"TruckId\":59},{\"Id\":102,\"ClientId\":20,\"TruckId\":59},{\"Id\":103,\"ClientId\":9,\"TruckId\":60},{\"Id\":104,\"ClientId\":16,\"TruckId\":60},{\"Id\":105,\"ClientId\":28,\"TruckId\":61},{\"Id\":106,\"ClientId\":3,\"TruckId\":63},{\"Id\":107,\"ClientId\":17,\"TruckId\":63},{\"Id\":108,\"ClientId\":12,\"TruckId\":64},{\"Id\":109,\"ClientId\":26,\"TruckId\":64},{\"Id\":110,\"ClientId\":8,\"TruckId\":65},{\"Id\":111,\"ClientId\":14,\"TruckId\":65},{\"Id\":112,\"ClientId\":17,\"TruckId\":65},{\"Id\":113,\"ClientId\":23,\"TruckId\":65}]}";
        var datasets = JsonConvert.DeserializeObject<Dictionary<string, IEnumerable<JObject>>>(datasetsJson);

        foreach (var dataset in datasets)
        {
            var entityType = GetType(dataset.Key);
            var entities = dataset.Value
                .Select(j => j.ToObject(entityType))
                .ToArray();

            context.AddRange(entities!);
        }

        context.SaveChanges();
    }

    private static Type GetType(string modelName)
    {
        var modelType = CurrentAssembly
            .GetTypes()
            .FirstOrDefault(t => t.Name == modelName);

        Assert.IsNotNull(modelType, $"{modelName} model not found!");

        return modelType;
    }

    private static IServiceProvider ConfigureServices<TContext>(string databaseName)
        where TContext : DbContext
    {
        var services = ConfigureDbContext<TContext>(databaseName);

        var context = services.GetService<TContext>();

        try
        {
            context.Model.GetEntityTypes();
        }
        catch (InvalidOperationException ex) when (ex.Source == "Microsoft.EntityFrameworkCore.Proxies")
        {
            services = ConfigureDbContext<TContext>(databaseName, useLazyLoading: true);
        }

        return services;
    }

    private static IServiceProvider ConfigureDbContext<TContext>(string databaseName, bool useLazyLoading = false)
        where TContext : DbContext
    {
        var services = new ServiceCollection()
           .AddDbContext<TContext>(t => t
           .UseInMemoryDatabase(Guid.NewGuid().ToString())
           );

        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}