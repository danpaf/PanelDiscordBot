using MainBot;


IHost host = Host.CreateDefaultBuilder(args)
    
    .ConfigureServices(
        (hostContext, services) =>
        {

            services.AddHostedService<Bot>();
        })
    .Build();

await host.RunAsync();

