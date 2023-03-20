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
public class Test_012_000_001
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
    public void ImportCustomersZeroTest()
    {
        var context = this.serviceProvider.GetService<CarDealerContext>();

        var customersXml =
            "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><Customers><Customer><name>Emmitt Benally</name><birthDate>1993-11-20T00:00:00</birthDate><isYoungDriver>true</isYoungDriver></Customer><Customer><name>Natalie Poli</name><birthDate>1990-10-04T00:00:00</birthDate><isYoungDriver>false</isYoungDriver></Customer><Customer><name>Marcelle Griego</name><birthDate>1990-10-04T00:00:00</birthDate><isYoungDriver>true</isYoungDriver></Customer><Customer><name>Zada Attwood</name><birthDate>1982-10-01T00:00:00</birthDate><isYoungDriver>true</isYoungDriver></Customer><Customer><name>Hai Everton</name><birthDate>1985-10-01T00:00:00</birthDate><isYoungDriver>true</isYoungDriver></Customer><Customer><name>Lino Subia</name><birthDate>1985-12-21T00:00:00</birthDate><isYoungDriver>false</isYoungDriver></Customer><Customer><name>Rico Peer</name><birthDate>1984-10-01T00:00:00</birthDate><isYoungDriver>true</isYoungDriver></Customer><Customer><name>Teddy Hobby</name><birthDate>1975-10-01T00:00:00</birthDate><isYoungDriver>true</isYoungDriver></Customer><Customer><name>Johnette Derryberry</name><birthDate>1995-10-01T00:00:00</birthDate><isYoungDriver>true</isYoungDriver></Customer><Customer><name>Audrea Cardinal</name><birthDate>1976-10-01T00:00:00</birthDate><isYoungDriver>false</isYoungDriver></Customer><Customer><name>Francis Mckim</name><birthDate>1977-10-01T00:00:00</birthDate><isYoungDriver>true</isYoungDriver></Customer><Customer><name>Carole Witman</name><birthDate>1987-10-01T00:00:00</birthDate><isYoungDriver>false</isYoungDriver></Customer><Customer><name>Oren Perlman</name><birthDate>1988-10-01T00:00:00</birthDate><isYoungDriver>true</isYoungDriver></Customer><Customer><name>Brett Brickley</name><birthDate>1980-10-01T00:00:00</birthDate><isYoungDriver>true</isYoungDriver></Customer><Customer><name>Kristian Engberg</name><birthDate>1981-10-03T00:00:00</birthDate><isYoungDriver>false</isYoungDriver></Customer><Customer><name>Carri Knapik</name><birthDate>1972-02-02T00:00:00</birthDate><isYoungDriver>true</isYoungDriver></Customer><Customer><name>Rema Revelle</name><birthDate>1970-05-01T00:00:00</birthDate><isYoungDriver>true</isYoungDriver></Customer><Customer><name>Taina Achenbach</name><birthDate>1994-10-01T00:00:00</birthDate><isYoungDriver>false</isYoungDriver></Customer><Customer><name>Yvonne Mccalla</name><birthDate>1992-03-02T00:00:00</birthDate><isYoungDriver>true</isYoungDriver></Customer><Customer><name>Nisha Markwell</name><birthDate>1994-04-04T00:00:00</birthDate><isYoungDriver>false</isYoungDriver></Customer><Customer><name>Cinthia Lasala</name><birthDate>1992-11-01T00:00:00</birthDate><isYoungDriver>true</isYoungDriver></Customer><Customer><name>Daniele Zarate</name><birthDate>1995-10-05T00:00:00</birthDate><isYoungDriver>false</isYoungDriver></Customer><Customer><name>Faustina Burgher</name><birthDate>1994-06-01T00:00:00</birthDate><isYoungDriver>false</isYoungDriver></Customer><Customer><name>Jimmy Grossi</name><birthDate>1986-07-01T00:00:00</birthDate><isYoungDriver>true</isYoungDriver></Customer><Customer><name>Sylvie Mcelravy</name><birthDate>1983-10-01T00:00:00</birthDate><isYoungDriver>false</isYoungDriver></Customer><Customer><name>Hipolito Lamoreaux</name><birthDate>1982-08-01T00:00:00</birthDate><isYoungDriver>true</isYoungDriver></Customer><Customer><name>Garret Capron</name><birthDate>1975-07-01T00:00:00</birthDate><isYoungDriver>false</isYoungDriver></Customer><Customer><name>Donnetta Soliz</name><birthDate>1963-10-01T00:00:00</birthDate><isYoungDriver>true</isYoungDriver></Customer><Customer><name>Louann Holzworth</name><birthDate>1960-10-01T00:00:00</birthDate><isYoungDriver>false</isYoungDriver></Customer><Customer><name>Ann Mcenaney</name><birthDate>1992-03-02T00:00:00</birthDate><isYoungDriver>true</isYoungDriver></Customer></Customers>";

        var actualOutput =
            StartUp.ImportCustomers(context, customersXml);

        var expectedOutput = "Successfully imported 30";

        var assertContext = this.serviceProvider.GetService<CarDealerContext>();

        const int expectedCarsCount = 30;
        var actualCarsCount = assertContext.Customers.Count();

        Assert.That(actualCarsCount, Is.EqualTo(expectedCarsCount),
            $"Inserted {nameof(context.Customers)} count is incorrect!");

        Assert.That(actualOutput, Is.EqualTo(expectedOutput).NoClip,
            $"{nameof(StartUp.ImportCars)} output is incorrect!");
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