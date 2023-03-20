using AutoMapper;

using CarDealer.Data;

using Microsoft.Extensions.DependencyInjection;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            using (var context = new CarDealerContext())
            {
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddTransient<IMapper, Mapper>();
                var provider = serviceCollection.BuildServiceProvider();



            }

        }
        
    }
}