//ReSharper disable InconsistentNaming, CheckNamespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using CarDealer;
using CarDealer.Data;
using CarDealer.Models;
using Newtonsoft.Json;

[TestFixture]
public class Test_011_000_001
{
    private IServiceProvider serviceProvider;

    private static readonly Assembly CurrentAssembly = typeof(StartUp).Assembly;

    [SetUp]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CarDealerProfile>();
        });

        this.serviceProvider = ConfigureServices<CarDealerContext>(Guid.NewGuid().ToString());
    }

    [Test]
    public void ImportPartCarsZeroTest()
    {
        ;
        var context = this.serviceProvider.GetService<CarDealerContext>();

        SeedDatabase(context);

        var carsXml =
                "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><Cars><Car><make>Opel</make><model>Omega</model><TraveledDistance>176664996</TraveledDistance><parts><partId id=\"38\"/><partId id=\"102\"/><partId id=\"23\"/><partId id=\"116\"/><partId id=\"46\"/><partId id=\"68\"/><partId id=\"88\"/><partId id=\"104\"/><partId id=\"71\"/><partId id=\"32\"/><partId id=\"114\"/></parts></Car><Car><make>Opel</make><model>Astra</model><TraveledDistance>516628215</TraveledDistance><parts><partId id=\"39\"/><partId id=\"62\"/><partId id=\"72\"/></parts></Car><Car><make>Opel</make><model>Astra</model><TraveledDistance>156191509</TraveledDistance><parts><partId id=\"48\"/><partId id=\"44\"/><partId id=\"112\"/></parts></Car><Car><make>Opel</make><model>Corsa</model><TraveledDistance>347259126</TraveledDistance><parts><partId id=\"36\"/><partId id=\"114\"/><partId id=\"88\"/><partId id=\"115\"/><partId id=\"72\"/><partId id=\"11\"/><partId id=\"50\"/><partId id=\"75\"/><partId id=\"20\"/><partId id=\"54\"/><partId id=\"8\"/></parts></Car><Car><make>Opel</make><model>Kadet</model><TraveledDistance>31737446</TraveledDistance><parts><partId id=\"65\"/><partId id=\"95\"/><partId id=\"90\"/></parts></Car><Car><make>Opel</make><model>Vectra</model><TraveledDistance>238042093</TraveledDistance><parts><partId id=\"40\"/><partId id=\"29\"/><partId id=\"102\"/><partId id=\"73\"/><partId id=\"68\"/><partId id=\"7\"/></parts></Car><Car><make>Opel</make><model>Insignia</model><TraveledDistance>225253817</TraveledDistance><parts><partId id=\"108\"/><partId id=\"8\"/><partId id=\"102\"/><partId id=\"57\"/><partId id=\"23\"/><partId id=\"79\"/><partId id=\"103\"/><partId id=\"70\"/><partId id=\"67\"/><partId id=\"71\"/><partId id=\"46\"/></parts></Car><Car><make>Opel</make><model>Astra</model><TraveledDistance>31468479</TraveledDistance><parts><partId id=\"102\"/><partId id=\"54\"/><partId id=\"15\"/></parts></Car><Car><make>Opel</make><model>Corsa</model><TraveledDistance>282499542</TraveledDistance><parts><partId id=\"46\"/><partId id=\"58\"/><partId id=\"12\"/></parts></Car><Car><make>Opel</make><model>Omega</model><TraveledDistance>111313670</TraveledDistance><parts><partId id=\"22\"/><partId id=\"52\"/><partId id=\"59\"/><partId id=\"59\"/><partId id=\"10\"/><partId id=\"112\"/><partId id=\"99\"/><partId id=\"48\"/><partId id=\"8\"/><partId id=\"62\"/><partId id=\"27\"/></parts></Car><Car><make>Opel</make><model>Insignia</model><TraveledDistance>168539389</TraveledDistance><parts><partId id=\"42\"/><partId id=\"96\"/><partId id=\"45\"/><partId id=\"84\"/><partId id=\"66\"/><partId id=\"6\"/></parts></Car><Car><make>Opel</make><model>Vectra</model><TraveledDistance>433351992</TraveledDistance><parts><partId id=\"90\"/><partId id=\"113\"/><partId id=\"98\"/></parts></Car><Car><make>Opel</make><model>Astra</model><TraveledDistance>349837452</TraveledDistance><parts><partId id=\"117\"/><partId id=\"61\"/><partId id=\"78\"/><partId id=\"111\"/><partId id=\"65\"/><partId id=\"93\"/><partId id=\"34\"/><partId id=\"93\"/><partId id=\"108\"/><partId id=\"96\"/><partId id=\"48\"/></parts></Car><Car><make>Opel</make><model>Omega</model><TraveledDistance>109910837</TraveledDistance><parts><partId id=\"12\"/><partId id=\"68\"/><partId id=\"103\"/></parts></Car><Car><make>Opel</make><model>Corsa</model><TraveledDistance>241891505</TraveledDistance><parts><partId id=\"49\"/><partId id=\"107\"/><partId id=\"94\"/></parts></Car><Car><make>Opel</make><model>Insignia</model><TraveledDistance>339785118</TraveledDistance><parts><partId id=\"34\"/><partId id=\"67\"/><partId id=\"99\"/><partId id=\"88\"/><partId id=\"94\"/><partId id=\"100\"/><partId id=\"103\"/><partId id=\"44\"/><partId id=\"113\"/><partId id=\"57\"/><partId id=\"111\"/></parts></Car><Car><make>Opel</make><model>Omega</model><TraveledDistance>254808828</TraveledDistance><parts><partId id=\"47\"/><partId id=\"5\"/><partId id=\"17\"/></parts></Car><Car><make>Opel</make><model>Corsa</model><TraveledDistance>267253567</TraveledDistance><parts><partId id=\"10\"/><partId id=\"19\"/><partId id=\"46\"/></parts></Car><Car><make>Opel</make><model>Omega</model><TraveledDistance>277250812</TraveledDistance><parts><partId id=\"97\"/><partId id=\"46\"/><partId id=\"59\"/><partId id=\"34\"/><partId id=\"79\"/><partId id=\"2\"/><partId id=\"47\"/><partId id=\"91\"/><partId id=\"25\"/><partId id=\"115\"/><partId id=\"99\"/></parts></Car><Car><make>BMW</make><model>F20</model><TraveledDistance>401291910</TraveledDistance><parts><partId id=\"100\"/><partId id=\"60\"/><partId id=\"78\"/></parts></Car><Car><make>BMW</make><model>F25</model><TraveledDistance>476132712</TraveledDistance><parts><partId id=\"82\"/><partId id=\"63\"/><partId id=\"57\"/><partId id=\"58\"/><partId id=\"108\"/><partId id=\"100\"/></parts></Car><Car><make>BMW</make><model>M5 F10</model><TraveledDistance>140799461</TraveledDistance><parts><partId id=\"96\"/><partId id=\"109\"/><partId id=\"46\"/><partId id=\"61\"/><partId id=\"35\"/><partId id=\"37\"/><partId id=\"4\"/><partId id=\"8\"/><partId id=\"15\"/><partId id=\"72\"/><partId id=\"48\"/></parts></Car><Car><make>BMW</make><model>F04</model><TraveledDistance>418839575</TraveledDistance><parts><partId id=\"6\"/><partId id=\"79\"/><partId id=\"107\"/></parts></Car><Car><make>BMW</make><model>E88</model><TraveledDistance>27453411</TraveledDistance><parts><partId id=\"91\"/><partId id=\"88\"/><partId id=\"16\"/></parts></Car><Car><make>BMW</make><model>M Roadster E85</model><TraveledDistance>475685374</TraveledDistance><parts><partId id=\"28\"/><partId id=\"40\"/><partId id=\"33\"/><partId id=\"20\"/><partId id=\"82\"/><partId id=\"37\"/><partId id=\"71\"/><partId id=\"89\"/><partId id=\"30\"/><partId id=\"22\"/><partId id=\"65\"/></parts></Car><Car><make>BMW</make><model>1M Coupe</model><TraveledDistance>39826890</TraveledDistance><parts><partId id=\"115\"/><partId id=\"69\"/><partId id=\"33\"/><partId id=\"44\"/><partId id=\"104\"/><partId id=\"4\"/></parts></Car><Car><make>BMW</make><model>X6 M</model><TraveledDistance>322292247</TraveledDistance><parts><partId id=\"36\"/><partId id=\"74\"/><partId id=\"85\"/></parts></Car><Car><make>BMW</make><model>E67</model><TraveledDistance>476830509</TraveledDistance><parts><partId id=\"84\"/><partId id=\"84\"/><partId id=\"10\"/><partId id=\"83\"/><partId id=\"7\"/><partId id=\"45\"/><partId id=\"39\"/><partId id=\"35\"/><partId id=\"61\"/><partId id=\"106\"/><partId id=\"38\"/></parts></Car><Car><make>BMW</make><model>M6 E63</model><TraveledDistance>57189891</TraveledDistance><parts><partId id=\"72\"/><partId id=\"79\"/><partId id=\"15\"/></parts></Car><Car><make>BMW</make><model>M6 E63</model><TraveledDistance>69863204</TraveledDistance><parts><partId id=\"114\"/><partId id=\"103\"/><partId id=\"44\"/></parts></Car><Car><make>BMW</make><model>F20</model><TraveledDistance>487897422</TraveledDistance><parts><partId id=\"48\"/><partId id=\"99\"/><partId id=\"41\"/><partId id=\"61\"/><partId id=\"57\"/><partId id=\"91\"/><partId id=\"101\"/><partId id=\"25\"/><partId id=\"95\"/><partId id=\"31\"/><partId id=\"7\"/></parts></Car><Car><make>BMW</make><model>F04</model><TraveledDistance>443756363</TraveledDistance><parts><partId id=\"89\"/><partId id=\"104\"/><partId id=\"43\"/></parts></Car><Car><make>BMW</make><model>M5 F10</model><TraveledDistance>435603343</TraveledDistance><parts><partId id=\"40\"/><partId id=\"103\"/><partId id=\"89\"/></parts></Car><Car><make>BMW</make><model>X6 M</model><TraveledDistance>227443571</TraveledDistance><parts><partId id=\"18\"/><partId id=\"29\"/><partId id=\"116\"/><partId id=\"36\"/><partId id=\"98\"/><partId id=\"5\"/><partId id=\"83\"/><partId id=\"4\"/><partId id=\"114\"/><partId id=\"9\"/><partId id=\"69\"/></parts></Car><Car><make>BMW</make><model>F20</model><TraveledDistance>284809463</TraveledDistance><parts><partId id=\"78\"/><partId id=\"5\"/><partId id=\"23\"/></parts></Car><Car><make>BMW</make><model>X6 M</model><TraveledDistance>183346013</TraveledDistance><parts><partId id=\"5\"/><partId id=\"27\"/><partId id=\"29\"/><partId id=\"42\"/><partId id=\"22\"/><partId id=\"2\"/></parts></Car></Cars>";

        var actualOutput =
            StartUp.ImportCars(context, carsXml);

        var expectedOutput = "Successfully imported 36";

        var assertContext = this.serviceProvider.GetService<CarDealerContext>();

        const int expectedCarsCount = 36;
        var actualCarsCount = assertContext.Cars.Count();

        const int expectedPartCarsCount = 216;
        var actualPartCarsCount = assertContext.PartsCars.Count();

        Assert.That(actualPartCarsCount, Is.EqualTo(expectedPartCarsCount),
            $"Inserted {nameof(context.PartsCars)} count is incorrect!");

        Assert.That(actualCarsCount, Is.EqualTo(expectedCarsCount),
            $"Inserted {nameof(context.Cars)} count is incorrect!");

        Assert.That(actualOutput, Is.EqualTo(expectedOutput).NoClip,
            $"{nameof(StartUp.ImportCars)} output is incorrect!");
    }

    private void SeedDatabase(CarDealerContext context)
    {
        var suppliersJson =
            @"[{""name"":""3M Company"",""isImporter"":true},{""name"":""Agway Inc."",""isImporter"":false},{""name"":""Anthem, Inc."",""isImporter"":true},{""name"":""Airgas, Inc."",""isImporter"":false},{""name"":""Atmel Corporation"",""isImporter"":true},{""name"":""Big Lots, Inc."",""isImporter"":true},{""name"":""Caterpillar Inc."",""isImporter"":false},{""name"":""Casey's General Stores Inc."",""isImporter"":true},{""name"":""Cintas Corp."",""isImporter"":false},{""name"":""Chubb Corp"",""isImporter"":true},{""name"":""Cintas Corp."",""isImporter"":false},{""name"":""CNF Inc."",""isImporter"":true},{""name"":""CMGI Inc."",""isImporter"":true},{""name"":""The Clorox Co."",""isImporter"":false},{""name"":""Danaher Corporation"",""isImporter"":true},{""name"":""E.I. Du Pont de Nemours and Company"",""isImporter"":false},{""name"":""E*Trade Group, Inc."",""isImporter"":true},{""name"":""Emcor Group Inc."",""isImporter"":true},{""name"":""GenCorp Inc."",""isImporter"":false},{""name"":""IDT Corporation"",""isImporter"":true},{""name"":""Level 3 Communications Inc."",""isImporter"":false},{""name"":""Merck & Co., Inc."",""isImporter"":true},{""name"":""Nicor Inc"",""isImporter"":false},{""name"":""Olin Corp."",""isImporter"":true},{""name"":""Paychex Inc"",""isImporter"":true},{""name"":""Saks Inc"",""isImporter"":false},{""name"":""Sunoco Inc."",""isImporter"":true},{""name"":""Textron Inc"",""isImporter"":true},{""name"":""VF Corporation"",""isImporter"":false},{""name"":""Wyeth"",""isImporter"":true},{""name"":""Zale"",""isImporter"":false}]";

        var partsJson =
            "[{\"name\":\"Bonnet/hood\",\"price\":1001.34,\"quantity\":10,\"supplierId\":17},{\"name\":\"Unexposed bumper\",\"price\":1003.34,\"quantity\":10,\"supplierId\":12},{\"name\":\"Exposed bumper\",\"price\":1400.34,\"quantity\":10,\"supplierId\":13},{\"name\":\"Cowl screen\",\"price\":1500.34,\"quantity\":10,\"supplierId\":22},{\"name\":\"Decklid\",\"price\":1060.34,\"quantity\":11,\"supplierId\":19},{\"name\":\"Fascia\",\"price\":100.34,\"quantity\":10,\"supplierId\":18},{\"name\":\"Fender\",\"price\":10.34,\"quantity\":10,\"supplierId\":16},{\"name\":\"Front clip\",\"price\":100,\"quantity\":10,\"supplierId\":11},{\"name\":\"Front fascia\",\"price\":11.99,\"quantity\":10,\"supplierId\":11},{\"name\":\"Grille\",\"price\":144.99,\"quantity\":10,\"supplierId\":12},{\"name\":\"Pillar\",\"price\":100.99,\"quantity\":10,\"supplierId\":32},{\"name\":\"Quarter panel\",\"price\":100.99,\"quantity\":200,\"supplierId\":12},{\"name\":\"Radiator \",\"price\":100.99,\"quantity\":10,\"supplierId\":56},{\"name\":\"Rocker\",\"price\":100.99,\"quantity\":10,\"supplierId\":41},{\"name\":\"Roof rack\",\"price\":100.99,\"quantity\":10,\"supplierId\":1},{\"name\":\"Spoiler\",\"price\":3000.99,\"quantity\":10,\"supplierId\":12},{\"name\":\"Rims\",\"price\":4020.99,\"quantity\":10,\"supplierId\":31},{\"name\":\"Trim package\",\"price\":1900.99,\"quantity\":10,\"supplierId\":65},{\"name\":\"Trunk/boot/hatch\",\"price\":2200.99,\"quantity\":300,\"supplierId\":32},{\"name\":\"Valance\",\"price\":1002.99,\"quantity\":10,\"supplierId\":22},{\"name\":\"Welded assembly\",\"price\":1020.99,\"quantity\":10,\"supplierId\":13},{\"name\":\"Front Right Outer door handle\",\"price\":999.99,\"quantity\":345,\"supplierId\":12},{\"name\":\"Front Left Side Outer door handle\",\"price\":999.99,\"quantity\":10,\"supplierId\":12},{\"name\":\"Rear Right Side Outer door handle\",\"price\":80.99,\"quantity\":10,\"supplierId\":13},{\"name\":\"Rear Left Side Outer door handle\",\"price\":120.99,\"quantity\":10,\"supplierId\":12},{\"name\":\"Front Right Side Inner door handle\",\"price\":100.99,\"quantity\":10,\"supplierId\":13},{\"name\":\"Front Left Side Inner door handle\",\"price\":77.99,\"quantity\":1032,\"supplierId\":13},{\"name\":\"Rear Right Side Inner door handle\",\"price\":79.99,\"quantity\":10,\"supplierId\":19},{\"name\":\"Rear Left Side Inner door handle\",\"price\":93.99,\"quantity\":234,\"supplierId\":19},{\"name\":\"Back Door Outer Door Handle\",\"price\":43.99,\"quantity\":10,\"supplierId\":15},{\"name\":\"Front Right Side Window motor\",\"price\":44.99,\"quantity\":10,\"supplierId\":13},{\"name\":\"Front Left Side Window motor\",\"price\":56.99,\"quantity\":10,\"supplierId\":17},{\"name\":\"Rear Right Side Window motor\",\"price\":22.99,\"quantity\":10,\"supplierId\":19},{\"name\":\"Rear Left Side Window motor\",\"price\":144.99,\"quantity\":10,\"supplierId\":19},{\"name\":\"Door control module\",\"price\":421.99,\"quantity\":10,\"supplierId\":17},{\"name\":\"Door seal\",\"price\":444.99,\"quantity\":10,\"supplierId\":16},{\"name\":\"Door water-shield\",\"price\":123.99,\"quantity\":10,\"supplierId\":15},{\"name\":\"Hinge\",\"price\":11.99,\"quantity\":10,\"supplierId\":14},{\"name\":\"Door latch\",\"price\":2331.99,\"quantity\":10,\"supplierId\":13},{\"name\":\"Door lock and power door locks\",\"price\":120.99,\"quantity\":10,\"supplierId\":12},{\"name\":\"Center-locking\",\"price\":313.99,\"quantity\":10,\"supplierId\":12},{\"name\":\"Fuel tank\",\"price\":100.98,\"quantity\":10,\"supplierId\":13},{\"name\":\"Front Right Side Door Glass\",\"price\":100.91,\"quantity\":10,\"supplierId\":16},{\"name\":\"Front Left Side Door Glass\",\"price\":100.92,\"quantity\":10,\"supplierId\":19},{\"name\":\"Rear Right Side Door Glass\",\"price\":100.9,\"quantity\":10,\"supplierId\":16},{\"name\":\"Rear Left Side Door Glass\",\"price\":100.66,\"quantity\":10,\"supplierId\":15},{\"name\":\"Rear Right Quarter Glass\",\"price\":100.66,\"quantity\":10,\"supplierId\":12},{\"name\":\"Rear Left Quarter Glass\",\"price\":100.55,\"quantity\":10,\"supplierId\":51},{\"name\":\"Sunroof\",\"price\":103.43,\"quantity\":10,\"supplierId\":12},{\"name\":\"Sunroof Rail\",\"price\":100.25,\"quantity\":10,\"supplierId\":31},{\"name\":\"Sunroof Glass\",\"price\":205.24,\"quantity\":10,\"supplierId\":22},{\"name\":\"Window motor\",\"price\":123.49,\"quantity\":10,\"supplierId\":13},{\"name\":\"Window regulator\",\"price\":100.99,\"quantity\":10,\"supplierId\":61},{\"name\":\"Windshield\",\"price\":100.99,\"quantity\":10,\"supplierId\":14},{\"name\":\"Window seal\",\"price\":100.99,\"quantity\":10,\"supplierId\":31},{\"name\":\"Engine\",\"price\":100.99,\"quantity\":10,\"supplierId\":12},{\"name\":\"Camshaft\",\"price\":100.98,\"quantity\":10,\"supplierId\":12},{\"name\":\"Crank case\",\"price\":2.93,\"quantity\":10,\"supplierId\":11},{\"name\":\"Crank pulley\",\"price\":2.8,\"quantity\":10,\"supplierId\":9},{\"name\":\"Crankshaft\",\"price\":4.99,\"quantity\":10,\"supplierId\":6},{\"name\":\"Cylinder head\",\"price\":6.99,\"quantity\":10,\"supplierId\":5},{\"name\":\"Distributor\",\"price\":7.99,\"quantity\":10,\"supplierId\":4},{\"name\":\"Distributor cap\",\"price\":9.99,\"quantity\":10,\"supplierId\":1},{\"name\":\"Drive belt\",\"price\":3.99,\"quantity\":10,\"supplierId\":7},{\"name\":\"Engine block\",\"price\":40.99,\"quantity\":10,\"supplierId\":1},{\"name\":\"Engine cradle\",\"price\":39.99,\"quantity\":10,\"supplierId\":11},{\"name\":\"Engine shake damper and vibration absorber\",\"price\":100.99,\"quantity\":10,\"supplierId\":11},{\"name\":\"Engine valve\",\"price\":49.99,\"quantity\":10,\"supplierId\":11},{\"name\":\"Fan belt\",\"price\":10.99,\"quantity\":10,\"supplierId\":11},{\"name\":\"Gudgeon pin\",\"price\":44.99,\"quantity\":10,\"supplierId\":16},{\"name\":\"Harmonic balancer\",\"price\":67.99,\"quantity\":10,\"supplierId\":12},{\"name\":\"Heater\",\"price\":82.99,\"quantity\":10,\"supplierId\":11},{\"name\":\"Piston\",\"price\":94.99,\"quantity\":10,\"supplierId\":15},{\"name\":\"Poppet valve\",\"price\":88.99,\"quantity\":10,\"supplierId\":11},{\"name\":\"Rocker arm\",\"price\":98.99,\"quantity\":10,\"supplierId\":11},{\"name\":\"Rocker cover\",\"price\":10.99,\"quantity\":10,\"supplierId\":13},{\"name\":\"Starter motor\",\"price\":0.99,\"quantity\":10,\"supplierId\":13},{\"name\":\"Turbocharger\",\"price\":0.4,\"quantity\":10,\"supplierId\":2},{\"name\":\"Tappet\",\"price\":300.29,\"quantity\":10,\"supplierId\":19},{\"name\":\"Timing tape\",\"price\":40.9,\"quantity\":10,\"supplierId\":14},{\"name\":\"Valve cover\",\"price\":130.99,\"quantity\":10,\"supplierId\":13},{\"name\":\"Valve housing\",\"price\":400.99,\"quantity\":10,\"supplierId\":31},{\"name\":\"Valve spring\",\"price\":104.99,\"quantity\":10,\"supplierId\":51},{\"name\":\"Valve stem seal\",\"price\":120.99,\"quantity\":10,\"supplierId\":13},{\"name\":\"Water pump pulley\",\"price\":150.99,\"quantity\":10,\"supplierId\":11},{\"name\":\"Air blower\",\"price\":140.99,\"quantity\":10,\"supplierId\":11},{\"name\":\"Coolant hose \",\"price\":150.99,\"quantity\":10,\"supplierId\":12},{\"name\":\"Cooling fan\",\"price\":120.99,\"quantity\":10,\"supplierId\":12},{\"name\":\"Fan blade\",\"price\":150.99,\"quantity\":10,\"supplierId\":1},{\"name\":\"Fan clutch\",\"price\":120.99,\"quantity\":10,\"supplierId\":34},{\"name\":\"Radiator bolt\",\"price\":160.99,\"quantity\":10,\"supplierId\":33},{\"name\":\"Radiator (fan) shroud\",\"price\":300.99,\"quantity\":10,\"supplierId\":32},{\"name\":\"Radiator gasket\",\"price\":140.99,\"quantity\":10,\"supplierId\":31},{\"name\":\"Radiator pressure cap\",\"price\":600.99,\"quantity\":10,\"supplierId\":30},{\"name\":\"Overflow tank\",\"price\":1200.99,\"quantity\":10,\"supplierId\":29},{\"name\":\"Thermostat\",\"price\":1050.99,\"quantity\":10,\"supplierId\":28},{\"name\":\"Water neck\",\"price\":900.99,\"quantity\":10,\"supplierId\":27},{\"name\":\"Water neck o-ring\",\"price\":43.99,\"quantity\":10,\"supplierId\":26},{\"name\":\"Water pipe\",\"price\":44.94,\"quantity\":10,\"supplierId\":25},{\"name\":\"Water pump\",\"price\":44.93,\"quantity\":10,\"supplierId\":24},{\"name\":\"Water pump gasket\",\"price\":120.29,\"quantity\":10,\"supplierId\":23},{\"name\":\"Water tank\",\"price\":100.99,\"quantity\":10,\"supplierId\":22},{\"name\":\"Oil filter\",\"price\":200.99,\"quantity\":10,\"supplierId\":21},{\"name\":\"Oil gasket\",\"price\":130.99,\"quantity\":10,\"supplierId\":20},{\"name\":\"Oil pan\",\"price\":140.99,\"quantity\":10,\"supplierId\":19},{\"name\":\"Oil pipe\",\"price\":100.29,\"quantity\":10,\"supplierId\":18},{\"name\":\"Oil pump\",\"price\":100.19,\"quantity\":10,\"supplierId\":17},{\"name\":\"Oil strainer\",\"price\":45.29,\"quantity\":10,\"supplierId\":16},{\"name\":\"Catalytic converter\",\"price\":540.99,\"quantity\":10,\"supplierId\":15},{\"name\":\"Exhaust clamp and bracket\",\"price\":140.99,\"quantity\":10,\"supplierId\":14},{\"name\":\"Exhaust flange gasket\",\"price\":130.99,\"quantity\":10,\"supplierId\":13},{\"name\":\"Resonator\",\"price\":106.99,\"quantity\":10,\"supplierId\":12},{\"name\":\"Muffler\",\"price\":106.9,\"quantity\":10,\"supplierId\":12},{\"name\":\"Exhaust pipe\",\"price\":130.99,\"quantity\":10,\"supplierId\":10},{\"name\":\"Heat shield\",\"price\":10.99,\"quantity\":10,\"supplierId\":9},{\"name\":\"Air filter\",\"price\":200.99,\"quantity\":10,\"supplierId\":8},{\"name\":\"Choke cable\",\"price\":130.99,\"quantity\":10,\"supplierId\":7},{\"name\":\"Fuel cooler\",\"price\":140.99,\"quantity\":10,\"supplierId\":6},{\"name\":\"Fuel filter\",\"price\":162.99,\"quantity\":10,\"supplierId\":5},{\"name\":\"Fuel rail\",\"price\":107.99,\"quantity\":10,\"supplierId\":4},{\"name\":\"Fuel pressure regulator\",\"price\":100.99,\"quantity\":10,\"supplierId\":3},{\"name\":\"Steering arm\",\"price\":104.99,\"quantity\":10,\"supplierId\":2},{\"name\":\"Steering box\",\"price\":103.99,\"quantity\":10,\"supplierId\":1},{\"name\":\"Clutch assembly\",\"price\":120.99,\"quantity\":10,\"supplierId\":19},{\"name\":\"Master cylinder\",\"price\":130.99,\"quantity\":10,\"supplierId\":54},{\"name\":\"Speed reducer\",\"price\":14.99,\"quantity\":10,\"supplierId\":13},{\"name\":\"Transmission gear\",\"price\":15.99,\"quantity\":10,\"supplierId\":43},{\"name\":\"Transmission pan\",\"price\":106.99,\"quantity\":10,\"supplierId\":12},{\"name\":\"Differential seal\",\"price\":109.99,\"quantity\":10,\"supplierId\":3},{\"name\":\"Differential clutch\",\"price\":120.99,\"quantity\":10,\"supplierId\":3},{\"name\":\"Clutch shoe\",\"price\":10.49,\"quantity\":10,\"supplierId\":2}]";

        var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(suppliersJson);
        var parts = JsonConvert.DeserializeObject<List<Part>>(partsJson);

        context.Suppliers.AddRange(suppliers);
        context.Parts.AddRange(parts);
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
        var services = new ServiceCollection();

        services
            .AddDbContext<TContext>(
                options => options
                    .UseInMemoryDatabase(databaseName)
                    .UseLazyLoadingProxies(useLazyLoading)
            );

        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}