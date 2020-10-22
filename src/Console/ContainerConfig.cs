using Autofac;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Domain.DomainObjects;
using Domain.Handlers;
using Domain.Mappers;
using Domain.OutputWriters;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Extensions.Autofac.DependencyInjection;
using System;
using System.Globalization;

namespace OrderParser
{
    public static class ContainerConfig
    {        
        public static IContainer Configure()
        {
            var services = ConfigureServices();
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterSerilog(new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File($"log/{nameof(OrderParser)}.log", rollOnFileSizeLimit: true, fileSizeLimitBytes: 10485760, retainedFileCountLimit: 5)
                .WriteTo.Console()
            );

            containerBuilder.RegisterType<CommandLineConfig>().AsSelf();

            containerBuilder.RegisterType<OrderHandler>().As<IOrderHandler>();
            containerBuilder.RegisterType<OrderMapper>().As<IOrderMapper>();
            containerBuilder.RegisterType<OrderOutputWriter>().As<IOutputWriter<Order>>();

            containerBuilder.RegisterType<ShippingLabelHandler>().As<IShippingLabelHandler>();
            containerBuilder.RegisterType<ShippingLabelMapper>().As<IShippingLabelMapper>();
            containerBuilder.RegisterType<ShippingLabelOutputWriter>().As<IOutputWriter<ShippingLabel>>();

            containerBuilder.RegisterType<Factory>().AsSelf();
            containerBuilder.Register<CsvConfiguration>(ctx =>
            {
                var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";",
                    ShouldQuote = (f, ctx) => true,
                };

                cfg.TypeConverterOptionsCache
                    .AddOptions<DateTime>(
                new TypeConverterOptions
                {
                    DateTimeStyle = DateTimeStyles.AdjustToUniversal,
                    Formats = new[] { "ddMMyyyy" }
                });

                return cfg;
            }).SingleInstance();

            return containerBuilder.Build();
        }

        private static object ConfigureServices()
        {
            var services = new ServiceCollection();

            return services;
        }
    }
}
