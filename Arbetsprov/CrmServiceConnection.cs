using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Net;

namespace Arbetsprov
{
    class CrmServiceConnection
    {
        /// <summary>
        /// Connect to CRM based on Client ID and Client Secret retrieved from Azure AD
        /// </summary>
        /// <returns></returns>
        public static void GetOrganizationServiceClientSecret()
        {
            //Gets all the config values
            string url = SiteGlobal.CRMUrl;
            string clientId = SiteGlobal.ClientID;
            string clientSecret = SiteGlobal.ClientSecret;

            //Connect to CRM using CrmServiceClient with CLient ID and Client Secret
            //and returns the OrganizationService object.
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var conn = new CrmServiceClient($@"AuthType=ClientSecret;url={url};ClientId={clientId};ClientSecret={clientSecret}");

                SiteGlobal.OrgService = conn.OrganizationWebProxyClient != null ? conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;
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
