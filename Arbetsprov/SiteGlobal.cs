using Microsoft.Xrm.Sdk;
using System;
using System.Configuration;

namespace Arbetsprov
{
    internal class SiteGlobal
    {
        /// <summary>
        /// Gets or sets the CRM Url.
        /// </summary>
        /// <value>CRM Instance Url.</value>
        static public string CRMUrl { get; set; }

        /// <summary>
        /// Gets or sets the client id.
        /// </summary>
        /// <value>Client ID.</value>
        static public string ClientID { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        /// <value>The content type header.</value>
        static public string ClientSecret { get; set; }        

        /// <summary>
        /// Gets or sets the entity name in which the records need to be inserted.
        /// </summary>
        static public string CreateRecordInEntity { get; set; }

        /// <summary>
        /// Gets or sets the organization service value.
        /// </summary>
        static public IOrganizationService OrgService { get; set; }

        /// <summary>
        /// Gets or sets the first name of a record in contact entity.
        /// </summary>
        static public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of a record in contact entity.
        /// </summary>
        static public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address of a record in contact entity.
        /// </summary>
        static public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the address line of a record in contact entity.
        /// </summary>
        static public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the file location.
        /// </summary>
        static public string FileLocation { get; set; }

        /// <summary>
        /// Gets or sets the delimiter in the CSV file.
        /// </summary>
        static public string Delimiter { get; set; }
        static SiteGlobal()
        {

            CRMUrl = ConfigurationManager.AppSettings["crmUrl"];
            ClientID = ConfigurationManager.AppSettings["clientID"];
            ClientSecret = ConfigurationManager.AppSettings["clientSecret"];
            CreateRecordInEntity = ConfigurationManager.AppSettings["createRecordInEntity"];
            FirstName = ConfigurationManager.AppSettings["firstname"];
            LastName = ConfigurationManager.AppSettings["lastname"];
            EmailAddress = ConfigurationManager.AppSettings["emailaddress1"];
            AddressLine1 = ConfigurationManager.AppSettings["address1_line1"];
            FileLocation = ConfigurationManager.AppSettings["fileLocation"];
            Delimiter = ConfigurationManager.AppSettings["delimiter"];
        }
    }
}
