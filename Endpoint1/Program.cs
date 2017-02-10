using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

class Program
{
    internal static IMessageSession MessageSession;
    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        Console.Title = "Samples.Azure.ServiceBus.Endpoint1";

        var endpointConfiguration = new EndpointConfiguration("Samples.Azure.ServiceBus.Endpoint1");
        endpointConfiguration.OverrideLocalAddress("crmevents");
        endpointConfiguration.SendFailedMessagesTo("error");
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }
        transport.ConnectionString(connectionString);
        transport.UseForwardingTopology();
        transport.BrokeredMessageBodyType(SupportedBrokeredMessageBodyTypes.Stream);

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.Recoverability().DisableLegacyRetriesSatellite();

        transport.Routing().RouteToEndpoint(typeof(Message1), "Samples.Azure.ServiceBus.Endpoint2");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        // can be also injected into a container
        MessageSession = endpointInstance;

        try
        {
            Console.WriteLine("Press 'enter' to send a message");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }

                var message = new Message1
                {
                    Property = "Hello from Endpoint1"
                };
                await endpointInstance.Send(message)
                    .ConfigureAwait(false);
                Console.WriteLine("Message1 sent");
            }
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}