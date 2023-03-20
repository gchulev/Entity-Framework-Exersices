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
public class Test_010_000_001
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
    public void ImportPartsZeroTest()
    {
        var context = this.serviceProvider.GetService<CarDealerContext>();

        SeedDatabase(context);

        var partsXml =
            "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><Parts><Part><name>Bonnet/hood</name><price>1001.34</price><quantity>10</quantity><supplierId>17</supplierId></Part><Part><name>Unexposed bumper</name><price>1003.34</price><quantity>10</quantity><supplierId>12</supplierId></Part><Part><name>Exposed bumper</name><price>1400.34</price><quantity>10</quantity><supplierId>13</supplierId></Part><Part><name>Cowl screen</name><price>1500.34</price><quantity>10</quantity><supplierId>22</supplierId></Part><Part><name>Decklid</name><price>1060.34</price><quantity>11</quantity><supplierId>19</supplierId></Part><Part><name>Fascia</name><price>100.34</price><quantity>10</quantity><supplierId>18</supplierId></Part><Part><name>Fender</name><price>10.34</price><quantity>10</quantity><supplierId>16</supplierId></Part><Part><name>Front clip</name><price>100</price><quantity>10</quantity><supplierId>11</supplierId></Part><Part><name>Front fascia</name><price>11.99</price><quantity>10</quantity><supplierId>11</supplierId></Part><Part><name>Grille</name><price>144.99</price><quantity>10</quantity><supplierId>12</supplierId></Part><Part><name>Pillar</name><price>100.99</price><quantity>10</quantity><supplierId>32</supplierId></Part><Part><name>Quarter panel</name><price>100.99</price><quantity>200</quantity><supplierId>12</supplierId></Part><Part><name>Radiator </name><price>100.99</price><quantity>10</quantity><supplierId>56</supplierId></Part><Part><name>Rocker</name><price>100.99</price><quantity>10</quantity><supplierId>41</supplierId></Part><Part><name>Roof rack</name><price>100.99</price><quantity>10</quantity><supplierId>1</supplierId></Part><Part><name>Spoiler</name><price>3000.99</price><quantity>10</quantity><supplierId>12</supplierId></Part><Part><name>Rims</name><price>4020.99</price><quantity>10</quantity><supplierId>31</supplierId></Part><Part><name>Trim package</name><price>1900.99</price><quantity>10</quantity><supplierId>65</supplierId></Part><Part><name>Trunk/boot/hatch</name><price>2200.99</price><quantity>300</quantity><supplierId>32</supplierId></Part><Part><name>Valance</name><price>1002.99</price><quantity>10</quantity><supplierId>22</supplierId></Part><Part><name>Welded assembly</name><price>1020.99</price><quantity>10</quantity><supplierId>13</supplierId></Part><Part><name>Front Right Outer door handle</name><price>999.99</price><quantity>345</quantity><supplierId>12</supplierId></Part><Part><name>Front Left Side Outer door handle</name><price>999.99</price><quantity>10</quantity><supplierId>12</supplierId></Part><Part><name>Rear Right Side Outer door handle</name><price>80.99</price><quantity>10</quantity><supplierId>13</supplierId></Part><Part><name>Rear Left Side Outer door handle</name><price>120.99</price><quantity>10</quantity><supplierId>12</supplierId></Part><Part><name>Front Right Side Inner door handle</name><price>100.99</price><quantity>10</quantity><supplierId>13</supplierId></Part><Part><name>Front Left Side Inner door handle</name><price>77.99</price><quantity>1032</quantity><supplierId>13</supplierId></Part><Part><name>Rear Right Side Inner door handle</name><price>79.99</price><quantity>10</quantity><supplierId>19</supplierId></Part><Part><name>Rear Left Side Inner door handle</name><price>93.99</price><quantity>234</quantity><supplierId>19</supplierId></Part><Part><name>Back Door Outer Door Handle</name><price>43.99</price><quantity>10</quantity><supplierId>15</supplierId></Part><Part><name>Front Right Side Window motor</name><price>44.99</price><quantity>10</quantity><supplierId>13</supplierId></Part><Part><name>Front Left Side Window motor</name><price>56.99</price><quantity>10</quantity><supplierId>17</supplierId></Part><Part><name>Rear Right Side Window motor</name><price>22.99</price><quantity>10</quantity><supplierId>19</supplierId></Part><Part><name>Rear Left Side Window motor</name><price>144.99</price><quantity>10</quantity><supplierId>19</supplierId></Part><Part><name>Door control module</name><price>421.99</price><quantity>10</quantity><supplierId>17</supplierId></Part><Part><name>Door seal</name><price>444.99</price><quantity>10</quantity><supplierId>16</supplierId></Part><Part><name>Door water-shield</name><price>123.99</price><quantity>10</quantity><supplierId>15</supplierId></Part><Part><name>Hinge</name><price>11.99</price><quantity>10</quantity><supplierId>14</supplierId></Part><Part><name>Door latch</name><price>2331.99</price><quantity>10</quantity><supplierId>13</supplierId></Part><Part><name>Door lock and power door locks</name><price>120.99</price><quantity>10</quantity><supplierId>12</supplierId></Part><Part><name>Center-locking</name><price>313.99</price><quantity>10</quantity><supplierId>12</supplierId></Part><Part><name>Fuel tank</name><price>100.98</price><quantity>10</quantity><supplierId>13</supplierId></Part><Part><name>Front Right Side Door Glass</name><price>100.91</price><quantity>10</quantity><supplierId>16</supplierId></Part><Part><name>Front Left Side Door Glass</name><price>100.92</price><quantity>10</quantity><supplierId>19</supplierId></Part><Part><name>Rear Right Side Door Glass</name><price>100.9</price><quantity>10</quantity><supplierId>16</supplierId></Part><Part><name>Rear Left Side Door Glass</name><price>100.66</price><quantity>10</quantity><supplierId>15</supplierId></Part><Part><name>Rear Right Quarter Glass</name><price>100.66</price><quantity>10</quantity><supplierId>12</supplierId></Part><Part><name>Rear Left Quarter Glass</name><price>100.55</price><quantity>10</quantity><supplierId>51</supplierId></Part><Part><name>Sunroof</name><price>103.43</price><quantity>10</quantity><supplierId>12</supplierId></Part><Part><name>Sunroof Rail</name><price>100.25</price><quantity>10</quantity><supplierId>31</supplierId></Part><Part><name>Sunroof Glass</name><price>205.24</price><quantity>10</quantity><supplierId>22</supplierId></Part><Part><name>Window motor</name><price>123.49</price><quantity>10</quantity><supplierId>13</supplierId></Part><Part><name>Window regulator</name><price>100.99</price><quantity>10</quantity><supplierId>61</supplierId></Part><Part><name>Windshield</name><price>100.99</price><quantity>10</quantity><supplierId>14</supplierId></Part><Part><name>Window seal</name><price>100.99</price><quantity>10</quantity><supplierId>31</supplierId></Part><Part><name>Engine</name><price>100.99</price><quantity>10</quantity><supplierId>12</supplierId></Part><Part><name>Camshaft</name><price>100.98</price><quantity>10</quantity><supplierId>12</supplierId></Part><Part><name>Crank case</name><price>2.93</price><quantity>10</quantity><supplierId>11</supplierId></Part><Part><name>Crank pulley</name><price>2.8</price><quantity>10</quantity><supplierId>9</supplierId></Part><Part><name>Crankshaft</name><price>4.99</price><quantity>10</quantity><supplierId>6</supplierId></Part><Part><name>Cylinder head</name><price>6.99</price><quantity>10</quantity><supplierId>5</supplierId></Part><Part><name>Distributor</name><price>7.99</price><quantity>10</quantity><supplierId>4</supplierId></Part><Part><name>Distributor cap</name><price>9.99</price><quantity>10</quantity><supplierId>1</supplierId></Part><Part><name>Drive belt</name><price>3.99</price><quantity>10</quantity><supplierId>7</supplierId></Part><Part><name>Engine block</name><price>40.99</price><quantity>10</quantity><supplierId>1</supplierId></Part><Part><name>Engine cradle</name><price>39.99</price><quantity>10</quantity><supplierId>11</supplierId></Part><Part><name>Engine shake damper and vibration absorber</name><price>100.99</price><quantity>10</quantity><supplierId>11</supplierId></Part><Part><name>Engine valve</name><price>49.99</price><quantity>10</quantity><supplierId>11</supplierId></Part><Part><name>Fan belt</name><price>10.99</price><quantity>10</quantity><supplierId>11</supplierId></Part><Part><name>Gudgeon pin</name><price>44.99</price><quantity>10</quantity><supplierId>16</supplierId></Part><Part><name>Harmonic balancer</name><price>67.99</price><quantity>10</quantity><supplierId>12</supplierId></Part><Part><name>Heater</name><price>82.99</price><quantity>10</quantity><supplierId>11</supplierId></Part><Part><name>Piston</name><price>94.99</price><quantity>10</quantity><supplierId>15</supplierId></Part><Part><name>Poppet valve</name><price>88.99</price><quantity>10</quantity><supplierId>11</supplierId></Part><Part><name>Rocker arm</name><price>98.99</price><quantity>10</quantity><supplierId>11</supplierId></Part><Part><name>Rocker cover</name><price>10.99</price><quantity>10</quantity><supplierId>13</supplierId></Part><Part><name>Starter motor</name><price>0.99</price><quantity>10</quantity><supplierId>13</supplierId></Part><Part><name>Turbocharger</name><price>0.4</price><quantity>10</quantity><supplierId>2</supplierId></Part><Part><name>Tappet</name><price>300.29</price><quantity>10</quantity><supplierId>19</supplierId></Part><Part><name>Timing tape</name><price>40.9</price><quantity>10</quantity><supplierId>14</supplierId></Part><Part><name>Valve cover</name><price>130.99</price><quantity>10</quantity><supplierId>13</supplierId></Part><Part><name>Valve housing</name><price>400.99</price><quantity>10</quantity><supplierId>31</supplierId></Part><Part><name>Valve spring</name><price>104.99</price><quantity>10</quantity><supplierId>51</supplierId></Part><Part><name>Valve stem seal</name><price>120.99</price><quantity>10</quantity><supplierId>13</supplierId></Part><Part><name>Water pump pulley</name><price>150.99</price><quantity>10</quantity><supplierId>11</supplierId></Part><Part><name>Air blower</name><price>140.99</price><quantity>10</quantity><supplierId>11</supplierId></Part><Part><name>Coolant hose </name><price>150.99</price><quantity>10</quantity><supplierId>12</supplierId></Part><Part><name>Cooling fan</name><price>120.99</price><quantity>10</quantity><supplierId>12</supplierId></Part><Part><name>Fan blade</name><price>150.99</price><quantity>10</quantity><supplierId>1</supplierId></Part><Part><name>Fan clutch</name><price>120.99</price><quantity>10</quantity><supplierId>34</supplierId></Part><Part><name>Radiator bolt</name><price>160.99</price><quantity>10</quantity><supplierId>33</supplierId></Part><Part><name>Radiator (fan) shroud</name><price>300.99</price><quantity>10</quantity><supplierId>32</supplierId></Part><Part><name>Radiator gasket</name><price>140.99</price><quantity>10</quantity><supplierId>31</supplierId></Part><Part><name>Radiator pressure cap</name><price>600.99</price><quantity>10</quantity><supplierId>30</supplierId></Part><Part><name>Overflow tank</name><price>1200.99</price><quantity>10</quantity><supplierId>29</supplierId></Part><Part><name>Thermostat</name><price>1050.99</price><quantity>10</quantity><supplierId>28</supplierId></Part><Part><name>Water neck</name><price>900.99</price><quantity>10</quantity><supplierId>27</supplierId></Part><Part><name>Water neck o-ring</name><price>43.99</price><quantity>10</quantity><supplierId>26</supplierId></Part><Part><name>Water pipe</name><price>44.94</price><quantity>10</quantity><supplierId>25</supplierId></Part><Part><name>Water pump</name><price>44.93</price><quantity>10</quantity><supplierId>24</supplierId></Part><Part><name>Water pump gasket</name><price>120.29</price><quantity>10</quantity><supplierId>23</supplierId></Part><Part><name>Water tank</name><price>100.99</price><quantity>10</quantity><supplierId>22</supplierId></Part><Part><name>Oil filter</name><price>200.99</price><quantity>10</quantity><supplierId>21</supplierId></Part><Part><name>Oil gasket</name><price>130.99</price><quantity>10</quantity><supplierId>20</supplierId></Part><Part><name>Oil pan</name><price>140.99</price><quantity>10</quantity><supplierId>19</supplierId></Part><Part><name>Oil pipe</name><price>100.29</price><quantity>10</quantity><supplierId>18</supplierId></Part><Part><name>Oil pump</name><price>100.19</price><quantity>10</quantity><supplierId>17</supplierId></Part><Part><name>Oil strainer</name><price>45.29</price><quantity>10</quantity><supplierId>16</supplierId></Part><Part><name>Catalytic converter</name><price>540.99</price><quantity>10</quantity><supplierId>15</supplierId></Part><Part><name>Exhaust clamp and bracket</name><price>140.99</price><quantity>10</quantity><supplierId>14</supplierId></Part><Part><name>Exhaust flange gasket</name><price>130.99</price><quantity>10</quantity><supplierId>13</supplierId></Part><Part><name>Resonator</name><price>106.99</price><quantity>10</quantity><supplierId>12</supplierId></Part><Part><name>Muffler</name><price>106.9</price><quantity>10</quantity><supplierId>12</supplierId></Part><Part><name>Exhaust pipe</name><price>130.99</price><quantity>10</quantity><supplierId>10</supplierId></Part><Part><name>Heat shield</name><price>10.99</price><quantity>10</quantity><supplierId>9</supplierId></Part><Part><name>Air filter</name><price>200.99</price><quantity>10</quantity><supplierId>8</supplierId></Part><Part><name>Choke cable</name><price>130.99</price><quantity>10</quantity><supplierId>7</supplierId></Part><Part><name>Fuel cooler</name><price>140.99</price><quantity>10</quantity><supplierId>6</supplierId></Part><Part><name>Fuel filter</name><price>162.99</price><quantity>10</quantity><supplierId>5</supplierId></Part><Part><name>Fuel rail</name><price>107.99</price><quantity>10</quantity><supplierId>4</supplierId></Part><Part><name>Fuel pressure regulator</name><price>100.99</price><quantity>10</quantity><supplierId>3</supplierId></Part><Part><name>Steering arm</name><price>104.99</price><quantity>10</quantity><supplierId>2</supplierId></Part><Part><name>Steering box</name><price>103.99</price><quantity>10</quantity><supplierId>1</supplierId></Part><Part><name>Clutch assembly</name><price>120.99</price><quantity>10</quantity><supplierId>19</supplierId></Part><Part><name>Master cylinder</name><price>130.99</price><quantity>10</quantity><supplierId>54</supplierId></Part><Part><name>Speed reducer</name><price>14.99</price><quantity>10</quantity><supplierId>13</supplierId></Part><Part><name>Transmission gear</name><price>15.99</price><quantity>10</quantity><supplierId>43</supplierId></Part><Part><name>Transmission pan</name><price>106.99</price><quantity>10</quantity><supplierId>12</supplierId></Part><Part><name>Differential seal</name><price>109.99</price><quantity>10</quantity><supplierId>3</supplierId></Part><Part><name>Differential clutch</name><price>120.99</price><quantity>10</quantity><supplierId>3</supplierId></Part><Part><name>Clutch shoe</name><price>10.49</price><quantity>10</quantity><supplierId>2</supplierId></Part></Parts>";

        var actualOutput =
            StartUp.ImportParts(context, partsXml);

        var expectedOutput = "Successfully imported 118";

        var assertContext = this.serviceProvider.GetService<CarDealerContext>();

        const int expectedPartsCount = 118;
        var actualGameCount = assertContext.Parts.Count();

        Assert.That(actualGameCount, Is.EqualTo(expectedPartsCount),
            $"Inserted {nameof(context.Parts)} count is incorrect!");

        Assert.That(actualOutput, Is.EqualTo(expectedOutput).NoClip,
            $"{nameof(StartUp.ImportParts)} output is incorrect!");
    }

    private void SeedDatabase(CarDealerContext context)
    {
        var suppliersJson =
            @"[{""name"":""3M Company"",""isImporter"":true},{""name"":""Agway Inc."",""isImporter"":false},{""name"":""Anthem, Inc."",""isImporter"":true},{""name"":""Airgas, Inc."",""isImporter"":false},{""name"":""Atmel Corporation"",""isImporter"":true},{""name"":""Big Lots, Inc."",""isImporter"":true},{""name"":""Caterpillar Inc."",""isImporter"":false},{""name"":""Casey's General Stores Inc."",""isImporter"":true},{""name"":""Cintas Corp."",""isImporter"":false},{""name"":""Chubb Corp"",""isImporter"":true},{""name"":""Cintas Corp."",""isImporter"":false},{""name"":""CNF Inc."",""isImporter"":true},{""name"":""CMGI Inc."",""isImporter"":true},{""name"":""The Clorox Co."",""isImporter"":false},{""name"":""Danaher Corporation"",""isImporter"":true},{""name"":""E.I. Du Pont de Nemours and Company"",""isImporter"":false},{""name"":""E*Trade Group, Inc."",""isImporter"":true},{""name"":""Emcor Group Inc."",""isImporter"":true},{""name"":""GenCorp Inc."",""isImporter"":false},{""name"":""IDT Corporation"",""isImporter"":true},{""name"":""Level 3 Communications Inc."",""isImporter"":false},{""name"":""Merck & Co., Inc."",""isImporter"":true},{""name"":""Nicor Inc"",""isImporter"":false},{""name"":""Olin Corp."",""isImporter"":true},{""name"":""Paychex Inc"",""isImporter"":true},{""name"":""Saks Inc"",""isImporter"":false},{""name"":""Sunoco Inc."",""isImporter"":true},{""name"":""Textron Inc"",""isImporter"":true},{""name"":""VF Corporation"",""isImporter"":false},{""name"":""Wyeth"",""isImporter"":true},{""name"":""Zale"",""isImporter"":false}]";

        var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(suppliersJson);

        context.Suppliers.AddRange(suppliers);
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