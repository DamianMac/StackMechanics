﻿using System;
using Autofac;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using StackCafe.Catalog.Cli;
using StackCafe.Catalog.Data;
using StackCafe.Catalog.Handlers;
using StackCafe.Catalog.InMemory;
using StackCafe.Catalog.Messages;
using StackCafe.Catalog.Messaging;
using StackCafe.Catalog.Model;

namespace Activity1
{
    class Program
    {
        static int Main()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            try
            {
                //var products = new InMemoryProductRepository();
                //var cli = new CommandLineCatalogApi(
                //    new InMemoryMessageBus(
                //        new AddProductCommandHandler(products),
                //        new LookupProductRequestHandler(products)));

                //cli.Start();
                //return 0;

                //var builder = new ContainerBuilder();

                //builder.RegisterType<InMemoryProductRepository>()
                //    .As<IProductRepository>()
                //    .SingleInstance();

                //builder.RegisterType<InMemoryMessageBus>().As<IBus>();

                //builder.RegisterType<CommandLineCatalogApi>();

                //builder.RegisterAssemblyTypes(typeof(Product).Assembly)
                //    .AsClosedTypesOf(typeof(IHandleRequest<,>));

                //builder.RegisterAssemblyTypes(typeof(Product).Assembly)
                //    .AsClosedTypesOf(typeof(IHandleCommand<>));

                //using (var container = builder.Build())
                //{
                //    var cli = container.Resolve<CommandLineCatalogApi>();
                //    cli.Start();
                //    return 0;
                //}

                var builder = new ContainerBuilder();
                builder.RegisterType<CommandLineCatalogApi>();
                builder.RegisterType<InMemoryMessageBus>().As<IBus>();
                builder.RegisterType<AddProductCommandHandler>();
                builder.RegisterType<LookupProductRequestHandler>();
                builder.RegisterType<InMemoryProductRepository>().As<IProductRepository>().SingleInstance();

                using (var container = builder.Build())
                {
                    var cli = container.Resolve<CommandLineCatalogApi>();
                    cli.Start();
                }

                return 0;

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The API terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
