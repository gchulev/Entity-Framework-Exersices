//ReSharper disable InconsistentNaming, CheckNamespace

using System;
using System.Linq;
using System.Reflection;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

using ProductShop;
using ProductShop.Data;

[TestFixture]
public class Test_003_000_001
{
    private IServiceProvider serviceProvider;

    private static readonly Assembly CurrentAssembly = typeof(StartUp).Assembly;

    [SetUp]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductShopProfile>();
        });

        this.serviceProvider = ConfigureServices<ProductShopContext>(Guid.NewGuid().ToString());
    }

    [Test]
    public void ImportCategoriesZeroTests()
    {
        var context = this.serviceProvider.GetService<ProductShopContext>();

        var inputXml =
            "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><Categories><Category><name>Drugs</name></Category><Category><name>Adult</name></Category><Category><name>Electronics</name></Category><Category><name>Garden</name></Category><Category><name>Weapons</name></Category><Category><name>For Children</name></Category><Category><name>Sports</name></Category><Category><name>Fashion</name></Category><Category><name>Autoparts</name></Category><Category><name>Business</name></Category><Category><name>Other</name></Category></Categories>";

        var actualOutput =
            StartUp.ImportCategories(context, inputXml);

        var expectedOutput = $"Successfully imported 11";

        var assertContext = this.serviceProvider.GetService<ProductShopContext>();

        const int expectedCategoriesCount = 11;
        var actualGameCount = assertContext.Categories.Count();

        Assert.That(actualGameCount, Is.EqualTo(expectedCategoriesCount),
            $"Inserted {nameof(context.Categories)} count is incorrect!");

        Assert.That(actualOutput, Is.EqualTo(expectedOutput).NoClip,
            $"{nameof(StartUp.ImportCategories)} output is incorrect!");
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