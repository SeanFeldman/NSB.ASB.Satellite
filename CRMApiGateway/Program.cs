using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

namespace CRMApiGatewayEndpoint
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            System.Console.Title = "CRM API Gateway Endpoint";

            var endpointConfiguration = new EndpointConfiguration("Samples.ServiceBus.CRMAPIGatewayEndpoint");
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.EnableInstallers();
            var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
            var connectionString = Environment.GetEnvironmentVariable("CRM.AzureServiceBus.ConnectionString");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("Could not read the 'CRM.AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
            }
            transport.ConnectionString(connectionString);
            transport.UseForwardingTopology();
            transport.BrokeredMessageBodyType(SupportedBrokeredMessageBodyTypes.Stream);
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.Recoverability().DisableLegacyRetriesSatellite();

            //Inject the helper class for calling the API
            endpointConfiguration.RegisterComponents(cc => cc.ConfigureComponent(b =>
            {
                  return new CRMApiManager();
            },
          
            DependencyLifecycle.SingleInstance));



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
