//Resharper disable InconsistentNaming, CheckNamespace

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Theatre;
using Theatre.Data;
using Theatre.Data.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[TestFixture]
public class Import_002
{
    private IServiceProvider serviceProvider;

    private static readonly Assembly CurrentAssembly = typeof(StartUp).Assembly;

    [SetUp]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TheatreProfile>();
        });

        this.serviceProvider = ConfigureServices<TheatreContext>("Theatre");
    }

    [Test]
    public void ImportCastsTest()
    {
        var context = this.serviceProvider.GetService<TheatreContext>();

        var inputXml =
            @"<?xml version='1.0' encoding='UTF-8'?>
<Casts>
  <Cast>
    <FullName>Van Tyson</FullName>
    <IsMainCharacter>false</IsMainCharacter>
    <PhoneNumber>+44-35-745-2774</PhoneNumber>
    <PlayId>1</PlayId>
  </Cast>
  <Cast>
    <FullName>Carlina Desporte</FullName>
    <IsMainCharacter>false</IsMainCharacter>
    <PhoneNumber>+44-00-715-9959</PhoneNumber>
    <PlayId>2</PlayId>
  </Cast>
  <Cast>
    <FullName>Elke Kavanagh</FullName>
    <IsMainCharacter>true</IsMainCharacter>
    <PhoneNumber>+44-53-468-3479</PhoneNumber>
    <PlayId>3</PlayId>
  </Cast>
  <Cast>
    <FullName>Lorry Ferreo</FullName>
    <IsMainCharacter>false</IsMainCharacter>
    <PhoneNumber>+44-03-229-7456</PhoneNumber>
    <PlayId>4</PlayId>
  </Cast>
  <Cast>
    <FullName>Vonny Henlon</FullName>
    <IsMainCharacter>true</IsMainCharacter>
    <PhoneNumber>+44-29-590-5125</PhoneNumber>
    <PlayId>5</PlayId>
  </Cast>
  <Cast>
    <FullName>Brock Palle</FullName>
    <IsMainCharacter>true</IsMainCharacter>
    <PhoneNumber>+44-31-458-2012</PhoneNumber>
    <PlayId>6</PlayId>
  </Cast>
  <Cast>
    <FullName>Jefferson Chell</FullName>
    <IsMainCharacter>true</IsMainCharacter>
    <PhoneNumber>+44-02-183-3699</PhoneNumber>
    <PlayId>1</PlayId>
  </Cast>
  <Cast>
    <FullName>Estelle Haycox</FullName>
    <IsMainCharacter>true</IsMainCharacter>
    <PhoneNumber>+44-22-799-4279</PhoneNumber>
    <PlayId>7</PlayId>
  </Cast>
  <Cast>
    <FullName>Torrin Darke</FullName>
    <IsMainCharacter>true</IsMainCharacter>
    <PhoneNumber>+44-23-069-4342</PhoneNumber>
    <PlayId>1</PlayId>
  </Cast>
  <Cast>
    <FullName>Andie Greatham</FullName>
    <IsMainCharacter>true</IsMainCharacter>
    <PhoneNumber>+44-87-646-3735</PhoneNumber>
    <PlayId>2</PlayId>
  </Cast>
  <Cast>
    <FullName>Galvin Iggulden</FullName>
    <IsMainCharacter>false</IsMainCharacter>
    <PhoneNumber>+44-26-507-6901</PhoneNumber>
    <PlayId>3</PlayId>
  </Cast>
  <Cast>
    <FullName>Currey Le Frank</FullName>
    <IsMainCharacter>true</IsMainCharacter>
    <PhoneNumber>+44-81-800-1289</PhoneNumber>
    <PlayId>1</PlayId>
  </Cast>
  <Cast>
    <FullName>Ernaline Gayforth</FullName>
    <IsMainCharacter>true</IsMainCharacter>
    <PhoneNumber>+44-01-748-4377</PhoneNumber>
    <PlayId>4</PlayId>
  </Cast>
  <Cast>
    <FullName>Devy Everest</FullName>
    <IsMainCharacter>true</IsMainCharacter>
    <PhoneNumber>+44-31-665-5842</PhoneNumber>
    <PlayId>5</PlayId>
  </Cast>
  <Cast>
    <FullName>Ashly Manchett</FullName>
    <IsMainCharacter>true</IsMainCharacter>
    <PhoneNumber>+44-96-124-0972</PhoneNumber>
    <PlayId>6</PlayId>
  </Cast>
  <Cast>
    <FullName>Donnie Stonard</FullName>
    <IsMainCharacter>true</IsMainCharacter>
    <PhoneNumber>+44-95-484-0739</PhoneNumber>
    <PlayId>7</PlayId>
  </Cast>
  <Cast>
    <FullName/>
    <IsMainCharacter>true</IsMainCharacter>
    <PhoneNumber>+44-38-594-0623</PhoneNumber>
    <PlayId>1</PlayId>
  </Cast>
  <Cast>
    <FullName>Emmott Ditts</FullName>
    <IsMainCharacter>false</IsMainCharacter>
    <PhoneNumber>+44-35-497-0732</PhoneNumber>
    <PlayId>2</PlayId>
  </Cast>
  <Cast>
    <FullName>Hatty Friary</FullName>
    <IsMainCharacter>true</IsMainCharacter>
    <PhoneNumber>+44-20-352-2388</PhoneNumber>
    <PlayId>3</PlayId>
  </Cast>
  <Cast>
    <FullName>Zonnya Miner</FullName>
    <IsMainCharacter>false</IsMainCharacter>
    <PhoneNumber>+44-87-503-2640</PhoneNumber>
    <PlayId>4</PlayId>
  </Cast>
  <Cast>
    <FullName/>
    <IsMainCharacter>true</IsMainCharacter>
    <PhoneNumber>+44-67-792-9706</PhoneNumber>
    <PlayId>5</PlayId>
  </Cast>
  <Cast>
    <FullName>Adelaida Hadlow</FullName>
    <IsMainCharacter>false</IsMainCharacter>
    <PhoneNumber>+44-35-737-4249</PhoneNumber>
    <PlayId>6</PlayId>
  </Cast>
  <Cast>
    <FullName>Tarrah Scouler</FullName>
    <IsMainCharacter>false</IsMainCharacter>
    <PhoneNumber>+44-80-377-0406</PhoneNumber>
    <PlayId>7</PlayId>
  </Cast>
  <Cast>
    <FullName>Morganica Irons</FullName>
    <IsMainCharacter>false</IsMainCharacter>
    <PhoneNumber>+44-91-230-8245654620</PhoneNumber>
    <PlayId>1</PlayId>
  </Cast>
  <Cast>
    <FullName>Whitney Standering</FullName>
    <IsMainCharacter>true</IsMainCharacter>
    <PhoneNumber>+44-16-828-7732</PhoneNumber>
    <PlayId>3</PlayId>
  </Cast>
</Casts>";
        ;
        var actualOutput =
            Theatre.DataProcessor.Deserializer.ImportCasts(context, inputXml).TrimEnd();
        ;
        var expectedOutput =
            "Successfully imported actor Van Tyson as a lesser character!\r\nSuccessfully imported actor Carlina Desporte as a lesser character!\r\nSuccessfully imported actor Elke Kavanagh as a main character!\r\nSuccessfully imported actor Lorry Ferreo as a lesser character!\r\nSuccessfully imported actor Vonny Henlon as a main character!\r\nSuccessfully imported actor Brock Palle as a main character!\r\nSuccessfully imported actor Jefferson Chell as a main character!\r\nSuccessfully imported actor Estelle Haycox as a main character!\r\nSuccessfully imported actor Torrin Darke as a main character!\r\nSuccessfully imported actor Andie Greatham as a main character!\r\nSuccessfully imported actor Galvin Iggulden as a lesser character!\r\nSuccessfully imported actor Currey Le Frank as a main character!\r\nSuccessfully imported actor Ernaline Gayforth as a main character!\r\nSuccessfully imported actor Devy Everest as a main character!\r\nSuccessfully imported actor Ashly Manchett as a main character!\r\nSuccessfully imported actor Donnie Stonard as a main character!\r\nInvalid data!\r\nSuccessfully imported actor Emmott Ditts as a lesser character!\r\nSuccessfully imported actor Hatty Friary as a main character!\r\nSuccessfully imported actor Zonnya Miner as a lesser character!\r\nInvalid data!\r\nSuccessfully imported actor Adelaida Hadlow as a lesser character!\r\nSuccessfully imported actor Tarrah Scouler as a lesser character!\r\nInvalid data!\r\nSuccessfully imported actor Whitney Standering as a main character!";

        var assertContext = this.serviceProvider.GetService<TheatreContext>();

        const int expectedProjectionCount = 22;
        var actualProjectionCount = assertContext.Casts.Count();

        Assert.That(actualProjectionCount, Is.EqualTo(expectedProjectionCount),
            $"Inserted {nameof(context.Casts)} count is incorrect!");

        Assert.That(actualOutput, Is.EqualTo(expectedOutput).NoClip,
            $"{nameof(Theatre.DataProcessor.Deserializer.ImportCasts)} output is incorrect!");
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