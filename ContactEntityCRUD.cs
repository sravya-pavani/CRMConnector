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
    internal class ContactEntityCRUD
    {
        //declare all global variables
        private string entityName;
        private IOrganizationService orgService;
        private string firstName;
        private string lastName;
        private string emailAddress;
        private string addressLine;
        private string fileLocation;
        private char delimiter;

        public void PerformCRUD(string guid)
        {
            SetConfigValues();
            /*QueryExpression qeAccount = new QueryExpression("account");
            FilterExpression fe = new FilterExpression();
            qeAccount.ColumnSet = new ColumnSet(true);
            qeAccount.Orders.Add(new OrderExpression("modifiedon", OrderType.Descending));
            EntityCollection ecAccount = orgService.RetrieveMultiple(qeAccount);
           Entity firstRecord= ecAccount.Entities.First();

            Console.WriteLine(firstRecord["modifiedon"] + "name = " + firstRecord["name"]
                + "Accountid = " + firstRecord["accountid"]);

            Guid guidAccount = (Guid)firstRecord["accountid"];

            var myContact = new Entity("contact");
            myContact.Attributes["lastname"] = "John";
            myContact.Attributes["firstname"] = "Alex";
            myContact.Attributes["address1_line1"] = "Klarabergsviadukten 70";

            //Entity Contact_To_Be_Created = new Entity("contact");
            myContact.Attributes.Add("parentcustomerid", new EntityReference("account", guidAccount));
            Guid RecordID = orgService.Create(myContact);
            Console.WriteLine("Contact create with ID - " + RecordID);
            
            */

            // ImportCSVToContactEntity();
            UploadFileToContactEntity();

        }

        /// <summary>
        /// Sets all the config values.
        /// </summary>
        public void SetConfigValues()
        {
            //assign all global variables from config file.
            entityName = SiteGlobal.CreateRecordInEntity;
            orgService = SiteGlobal.OrgService;
            firstName = SiteGlobal.FirstName;
            lastName = SiteGlobal.LastName;
            emailAddress = SiteGlobal.EmailAddress;
            addressLine = SiteGlobal.AddressLine1;
            fileLocation = SiteGlobal.FileLocation;
            delimiter = char.Parse(SiteGlobal.Delimiter);
        }

        /// <summary>
        /// Creates a new record in contact entity.
        /// </summary>
        /// <param name="fName"></param>
        /// <param name="lName"></param>
        /// <param name="addr"></param>
        public void CreateSingleContact(string fName, string lName, string addr)
        {
            try
            {
                var newContact = new Entity(entityName);
                newContact.Attributes[firstName] = fName;
                newContact.Attributes[lastName] = lName;
                newContact.Attributes[addressLine] = addr;
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

        //check once
        public void CreateMultipleContactsAssociatedWithAccount()
        {
            //Execute Multiple
            var request = new ExecuteMultipleRequest()
            {
                Requests = new OrganizationRequestCollection(),
                Settings = new ExecuteMultipleSettings
                {
                    ContinueOnError = false,
                    ReturnResponses = true
                }
            };

            Entity acc1 = new Entity("account");
            acc1["name"] = "Soft1";
            Entity acc2 = new Entity("account");
            acc2["name"] = "Soft2";

            var createRequest1 = new CreateRequest()
            {
                Target = acc1
            };
            var createRequest2 = new CreateRequest()
            {
                Target = acc2
            };

            request.Requests.Add(createRequest1);
            request.Requests.Add(createRequest2);
            var response = (ExecuteMultipleResponse)orgService.Execute(request);
        }

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
                using (StreamReader reader = new StreamReader(@fileLocation))
                {
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

                        //create a request with the record and bulk upload with 500 records 
                        var createRequest = new CreateRequest()
                        {
                            Target = customEntity
                        };
                        request.Requests.Add(createRequest);

                        //checks if 500 records are added to request
                        //if true, bulk upload all the records to contact enity in cRM                
                        if (counter == 500)
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

        public void RetrieveMultipleRecords()
        {
            //Retrieve Multiple Record
            QueryExpression qe = new QueryExpression(entityName);
            qe.ColumnSet = new ColumnSet(firstName);
            EntityCollection ec = orgService.RetrieveMultiple(qe);

            for (int i = 0; i < ec.Entities.Count; i++)
            {
                if (ec.Entities[i].Attributes.ContainsKey(firstName) && ec.Entities[i].Attributes["firstname"].ToString() == "Alex")
                {
                    Console.WriteLine(ec.Entities[i].Attributes[firstName]);
                }
            }
            Console.WriteLine("Retrieved all Contacts...");
        }
    }
}
