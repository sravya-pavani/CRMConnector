using CsvHelper;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;

namespace Arbetsprov
{
    internal class ContactEntityCRUD : IContactEntity
    {
        #region Global Variables
        //declare all global variables
        private string entityName;
        private string associateEntityName;
        private IOrganizationService orgService;
        private string firstName;
        private string lastName;
        private string emailAddress;
        private string addressLine;
        private string parentCustId;
        private string modifiedOn;
        private string accountId;
        private string fileLocation;
        private char delimiter;
        private int batchSize;
        #endregion

        public ContactEntityCRUD()
        {
            SetConfigValues();
        }
        public void PerformCRUD()
        {
            //CreateSingleContact("Robert", "Testsson", "Klarabergsviadukten 70", Guid.Empty);
            //CreateRecordAssociateWithAccount("Alex", "John", "Klarabergsviadukten 70");
            //RetrieveMultipleRecords("Alex");
            UploadFileToContactEntity();
        }

        /// <summary>
        /// Sets all the config values.
        /// </summary>
        public void SetConfigValues()
        {
            //assign all global variables from config file.
            entityName = SiteGlobal.EntityName;
            associateEntityName =SiteGlobal.AssociateEntityName;
            orgService = SiteGlobal.OrgService;
            firstName = SiteGlobal.FirstName;
            lastName = SiteGlobal.LastName;
            emailAddress = SiteGlobal.EmailAddress;
            addressLine = SiteGlobal.AddressLine1;
            parentCustId = SiteGlobal.ParentCustomerId;
            modifiedOn = SiteGlobal.ModifiedOn;
            accountId = SiteGlobal.AccountId;
            fileLocation = SiteGlobal.FileLocation;
            delimiter = char.Parse(SiteGlobal.Delimiter);
            batchSize = SiteGlobal.BatchSize;
        }

        /// <summary>
        /// Creates a new record in contact entity.
        /// </summary>
        /// <param name="fName"></param>
        /// <param name="lName"></param>
        /// <param name="addr"></param>
        /// <param name="acctId"></param>
        public void CreateSingleContact(string fName, string lName, string addr, Guid acctId)
        {
            try
            {
                var newContact = new Entity(entityName);
                newContact.Attributes[firstName] = fName;
                newContact.Attributes[lastName] = lName;
                newContact.Attributes[addressLine] = addr;

                if (acctId != Guid.Empty)
                {
                    newContact.Attributes.Add(parentCustId, new EntityReference(associateEntityName, acctId));
                }

                Guid RecordID = orgService.Create(newContact);
                Console.WriteLine("Contact create with ID - " + RecordID);
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                Console.WriteLine("The application terminated with an error.");
                Console.WriteLine("Timestamp: {0}", ex.Detail.Timestamp);
                Console.WriteLine("Code: {0}", ex.Detail.ErrorCode);
                Console.WriteLine("Message: {0}", ex.Detail.Message);
                Console.WriteLine("Plugin Trace: {0}", ex.Detail.TraceText);
                Console.WriteLine("Inner Fault: {0}",
                    null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateRecordAssociateWithAccount(string fName, string lName, string addr)
        {
            try
            {
                //In order to associate contact enity records with account entity on field
                //parentcustomerid on the last saved record in account entity,
                //retrieve the last saved record from account entity and based on the account id of 
                //the record, create a record in contact entity associated with it.
                QueryExpression qeAccount = new QueryExpression(associateEntityName);
                FilterExpression fe = new FilterExpression();
                qeAccount.ColumnSet = new ColumnSet(true);
                qeAccount.Orders.Add(new OrderExpression(modifiedOn, OrderType.Descending));
                EntityCollection ecAccount = orgService.RetrieveMultiple(qeAccount);
                Entity firstRecord = ecAccount.Entities.First();
                
                Guid guidAccount = (Guid)firstRecord[accountId];

                //calling the function to create a record with guid id to associate it with account entity.
                CreateSingleContact(fName, lName, addr, guidAccount);               
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                Console.WriteLine("The application terminated with an error.");
                Console.WriteLine("Timestamp: {0}", ex.Detail.Timestamp);
                Console.WriteLine("Code: {0}", ex.Detail.ErrorCode);
                Console.WriteLine("Message: {0}", ex.Detail.Message);
                Console.WriteLine("Plugin Trace: {0}", ex.Detail.TraceText);
                Console.WriteLine("Inner Fault: {0}",
                    null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
            }
        }
       /// <summary>
       /// Uploads records from a CSV file to contact entity.
       /// </summary>
        public void UploadFileToContactEntity()
        {
            //create request collection for bulk upload
            var request = new ExecuteMultipleRequest()
            {
                Requests = new OrganizationRequestCollection(),
                Settings = new ExecuteMultipleSettings
                {
                    ContinueOnError = false,
                    ReturnResponses = true
                }
            };
            try
            {
                //read the records from the file.
                using (StreamReader reader = new StreamReader(@fileLocation))
                {
                    //skip the first line as it contains the logical names of fields.
                    string headerLine = reader.ReadLine();
                    string line;
                    int counter = 0;
                    while ((line = reader.ReadLine()) != null && line != string.Empty)
                    {
                        counter++;
                        var values = line.Split(delimiter);

                        //create a record in contact entity with each line of data from the file
                        Entity customEntity = new Entity(entityName);
                        customEntity[firstName] = values[0];
                        customEntity[lastName] = values[1];
                        customEntity[emailAddress] = values[2];
                        customEntity[addressLine] = values[3];

                        //create a request with the record and bulk upload with specified amount of records.
                        var createRequest = new CreateRequest()
                        {
                            Target = customEntity
                        };
                        request.Requests.Add(createRequest);

                        //checks if all the records specified in batch size are added to request
                        //if true, bulk upload all the records to contact enity in CRM.              
                        if (counter == batchSize)
                        {
                            var response = (ExecuteMultipleResponse)orgService.Execute(request);
                            counter = 0;
                            request.Requests.Clear();
                        }
                    }
                    if (request.Requests.Count > 0)
                    {
                        var response = (ExecuteMultipleResponse)orgService.Execute(request);
                        request.Requests.Clear();
                    }
                    Console.WriteLine("Successfully uploaded all the records from CSV file to CRM.");
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                Console.WriteLine("The application terminated with an error.");
                Console.WriteLine("Timestamp: {0}", ex.Detail.Timestamp);
                Console.WriteLine("Code: {0}", ex.Detail.ErrorCode);
                Console.WriteLine("Message: {0}", ex.Detail.Message);
                Console.WriteLine("Plugin Trace: {0}", ex.Detail.TraceText);
                Console.WriteLine("Inner Fault: {0}",
                    null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
            }
        }

        /// <summary>
        /// Retrieves multiple records whose first name matches with search string.
        /// </summary>
        /// <param name="searchStr"></param>
        public void RetrieveMultipleRecords(string searchStr)
        {
            //Retrieve Multiple Record
            QueryExpression qe = new QueryExpression(entityName);
            qe.ColumnSet = new ColumnSet(firstName);
            EntityCollection ec = orgService.RetrieveMultiple(qe);

            for (int i = 0; i < ec.Entities.Count; i++)
            {
                if (ec.Entities[i].Attributes.ContainsKey(firstName) && ec.Entities[i].Attributes[firstName].ToString() == searchStr)
                {
                    Console.WriteLine(ec.Entities[i].Attributes[firstName]);
                }
            }
            Console.WriteLine("Retrieved all Contacts with first name : " + searchStr);
        }
    }
}
