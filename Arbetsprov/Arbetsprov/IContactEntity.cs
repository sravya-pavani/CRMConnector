using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbetsprov
{
    internal interface IContactEntity
    {
        /// <summary>
        /// creates a record in contact entity.
        /// </summary>
        /// <param name="fName"></param>
        /// <param name="lName"></param>
        /// <param name="addr"></param>
        /// <param name="acctId"></param>
        void CreateSingleContact(string fName, string lName, string addr, Guid acctId);

        /// <summary>
        /// creates a record in contact entity associated with a record in account enity.
        /// </summary>
        /// <param name="fName"></param>
        /// <param name="lName"></param>
        /// <param name="addr"></param>
        void CreateRecordAssociateWithAccount(string fName, string lName, string addr);

        /// <summary>
        /// uploads records in CSV file into CRM.
        /// </summary>
        void UploadFileToContactEntity();

        /// <summary>
        /// retrieves multiple records with search string.
        /// </summary>
        /// <param name="searchStr"></param>
        void RetrieveMultipleRecords(string searchStr);
    }
}
