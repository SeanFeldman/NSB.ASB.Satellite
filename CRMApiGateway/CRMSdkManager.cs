using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;

namespace CRMApiGateway
{
    class CRMSdkManager
    {
        private CrmServiceClient serviceclient { get; }
        public CRMSdkManager()
        {
            //Hook up the connection.  
            var connectionString = $"AuthType=Office365;" +  System.Environment.GetEnvironmentVariable("Dynamics365API.ConnectionString");
            serviceclient  = new CrmServiceClient(connectionString);

        }


        public  string  CreateTaskForContact(string ContactID, string Subject, string Description, DateTimeOffset Deadline)
        {

            // Create associated task (late bound)
            var task = new Entity("task") { Id = Guid.NewGuid() };
            task["regardingobjectid"] = new EntityReference("contact", new Guid(ContactID));
            task["subject"] =Subject;
            task["description"] = Description;
            task["scheduledend"] = Deadline;
            task["statecode"] = 0;
            task["statuscode"] = 3;

            serviceclient.Create(task);

            return task.Id.ToString();
            
        }






    }
}
