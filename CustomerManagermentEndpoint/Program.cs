using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transports;
using NServiceBus.Transport.AzureServiceBus;
using CustomerManagementMessages;

namespace CustomerManagermentEndpoint
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            System.Console.Title = "Customer Management";

            var endpointConfiguration = new EndpointConfiguration("Samples.ServiceBus.CustomerManagementEndpoint");
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.EnableInstallers();
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
            endpointConfiguration.Recoverability().DisableLegacyRetriesSatellite();

            //Setup routing for our command;
            transport.Routing().RouteToEndpoint(
            messageType: typeof(CreateCustomerTaskRequest),
            destination: "Samples.ServiceBus.CRMAPIGatewayEndpoint");

            transport.Routing().RouteToEndpoint(
            messageType: typeof(UpdateTaskRequest),
            destination: "Samples.ServiceBus.CRMAPIGatewayEndpoint");


            var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

            try
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
            finally
            {
                await endpointInstance.Stop()
                    .ConfigureAwait(false);
            }

        }
    }

}
