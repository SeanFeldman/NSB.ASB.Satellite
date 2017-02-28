namespace CRMApiGatewayEndpoint
{
    using Microsoft.Crm.Sdk.Samples.HelperCode;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Text;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Linq;
    using System.Net;


    public class CRMApiManager : IDisposable
    {
        private HttpClient httpClient { get; }

        public CRMApiManager()
        {
            Configuration config = new FileConfiguration(null);

            Authentication auth = new Authentication(config);
            //Next use a HttpClient object to connect to specified CRM Web service.
            httpClient = new HttpClient(auth.ClientHandler, true);
            //Define the Web API base address, the max period of execute time, the
            // default OData version, and the default response payload format.
            httpClient.BaseAddress = new Uri(config.ServiceUrl + "api/data/v8.1/");
            httpClient.Timeout = new TimeSpan(0, 2, 0);
            httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<string> CreateTaskForContact(Guid ContactID, string Subject, string Description, DateTimeOffset Deadline)
        {
            string newtaskuri;

            //Get the URI from the connectionstring and build the proper Customer URI
            //var connection = System.Configuration.ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            // var connection = System.Environment.GetEnvironmentVariable("Dynamics365API.ConnectionString");
            //var crmcontactURI = connection.Substring(connection.IndexOf('=') + 1, (connection.IndexOf(';')) - (connection.IndexOf('=') + 1)) + $"api/data/v8.1/contacts({ContactID})" ;

            var crmcontactURI = httpClient.BaseAddress.OriginalString + $"contacts({ContactID})";
            JObject task = new JObject();
            task.Add("subject", Subject);
            task.Add("description", Description);
            task.Add("scheduledend", Deadline);
            task.Add("statecode", 0);
            task.Add("statuscode", 3);
            task.Add("regardingobjectid_contact@odata.bind", crmcontactURI);

            HttpRequestMessage createRequest =
               new HttpRequestMessage(HttpMethod.Post, "tasks");
            createRequest.Content = new StringContent(task.ToString(),
                Encoding.UTF8, "application/json");
            HttpResponseMessage createResponse =
                await httpClient.SendAsync(createRequest);
            if (createResponse.StatusCode == HttpStatusCode.NoContent)
            {
                newtaskuri = createResponse.Headers.GetValues("OData-EntityId").
                    FirstOrDefault();

                Console.WriteLine("Task '{0}' created. {1}", task.GetValue("subject"), newtaskuri);
                return newtaskuri;
            }
            else
            {
                Console.WriteLine("Failed to create task for reason: {0}.", createResponse.ReasonPhrase);
                throw new CrmHttpResponseException(createResponse.Content);
            }
        }

        public async Task<Guid> UpdateTask(Guid TaskId, bool MarkComplete, Guid AssignTo, string Description)
        {
            //Get the URI from the connectionstring and build the proper Customer URI
            Console.WriteLine($"Updating Task {TaskId} with Mark complete = {MarkComplete}");
            var taskUri = httpClient.BaseAddress.OriginalString + $"tasks({TaskId})?$select=regardingobjectid_contact";


            JObject taskAdd = new JObject();

            if (MarkComplete)
            {
                taskAdd.Add("statuscode", 5);
                taskAdd.Add("statecode", 1);
                taskAdd.Add("percentcomplete", 100);
            }
            else
            {
                taskAdd.Add("statuscode", 3);
                taskAdd.Add("statecode", 0);

            }
            taskAdd.Add("description", Description);
            // taskAdd.Add("owninguser", AssignTo);

            HttpRequestMessage updateRequest1 = new HttpRequestMessage(
                new HttpMethod("PATCH"), taskUri);
            updateRequest1.Content = new StringContent(taskAdd.ToString(),
                Encoding.UTF8, "application/json");
            HttpResponseMessage updateResponse1 =
                await httpClient.SendAsync(updateRequest1);
            if (updateResponse1.StatusCode == HttpStatusCode.NoContent) //204
            {
                // var ContactID = updateResponse1.Headers.GetValues("OData-")
                Console.WriteLine($"Task {TaskId} has been updated");
                return new Guid();// updateResponse1.Content.Headers.GetValues("regardingobjectid_contact"));
            }
            else
            {
                //Console.WriteLine("Failed to update contact for reason: {0}",
                //  updateResponse1.ReasonPhrase);
                throw new CrmHttpResponseException(updateResponse1.Content);
            }


        }

        public void Dispose()
        {
            if (this.httpClient != null)
            {
                httpClient.CancelPendingRequests();
                httpClient.Dispose();

            }
        }
    }
}
