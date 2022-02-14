using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System.ServiceModel.Channels;
using System.Net;

using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Client;

namespace CRMConnection
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IOrganizationService orgService = GetOrganizationServiceClientSecret(
                        "0d01af43-7760-4408-9e1b-366d8826c22c",
                        "c887Q~hh3y0nliEMjFGFcGkLQMpI-vmlgKDRQ",
                        "https://org01828d4a.api.crm4.dynamics.com");
                   
            Guid userid = ((WhoAmIResponse)orgService.Execute(new WhoAmIRequest())).UserId;
            if (userid != Guid.Empty)
            {
                Console.WriteLine("Connection Successful!...");
            }
            //Console.WriteLine(response);
            Console.ReadKey();
        }
        public static IOrganizationService GetOrganizationServiceClientSecret(string clientId, string clientSecret, string organizationUri)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var conn = new CrmServiceClient($@"AuthType=ClientSecret;url={organizationUri};ClientId={clientId};ClientSecret={clientSecret}");

                return conn.OrganizationWebProxyClient != null ? conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while connecting to CRM " + ex.Message);
                Console.ReadKey();
                return null;
            }
        }
    }
}
