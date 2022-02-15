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
        static public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the entity name with which the contact enity associates.
        /// </summary>
        static public string AssociateEntityName { get; set; }

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
        /// Gets or sets the associated field name between contact and account entities.
        /// </summary>
        static public string ParentCustomerId { get; set; }

        /// <summary>
        /// Gets or sets Modified On value of account entiy record.
        /// </summary>
        static public string ModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets account id of a record in account entity.
        /// </summary>
        static public string AccountId { get; set; }

        /// <summary>
        /// Gets or sets the file location.
        /// </summary>
        static public string FileLocation { get; set; }

        /// <summary>
        /// Gets or sets the delimiter in the CSV file.
        /// </summary>
        static public string Delimiter { get; set; }
        /// <summary>
        /// Gets or sets the size of records to be inserted into contact enity at once.
        /// </summary>
        static public int BatchSize { get; set; }
        static SiteGlobal()
        {

            CRMUrl = ConfigurationManager.AppSettings["crmUrl"];
            ClientID = ConfigurationManager.AppSettings["clientID"];
            ClientSecret = ConfigurationManager.AppSettings["clientSecret"];
            EntityName = ConfigurationManager.AppSettings["entityName"];
            AssociateEntityName =ConfigurationManager.AppSettings["associateEntityName"];
            FirstName = ConfigurationManager.AppSettings["firstname"];
            LastName = ConfigurationManager.AppSettings["lastname"];
            EmailAddress = ConfigurationManager.AppSettings["email"];
            AddressLine1 = ConfigurationManager.AppSettings["addressLine"];
            ParentCustomerId = ConfigurationManager.AppSettings["parentCustomerId"];
            ModifiedOn = ConfigurationManager.AppSettings["modifiedOn"];
            AccountId = ConfigurationManager.AppSettings["accountId"];
            FileLocation = ConfigurationManager.AppSettings["fileLocation"];
            Delimiter = ConfigurationManager.AppSettings["delimiter"];
            BatchSize = Convert.ToInt32(ConfigurationManager.AppSettings["batchSize"]);
        }
    }
}
