using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbetsprov
{
    internal interface IContactEntity
    {
        void CreateSingleContact(string fName, string lName, string addr);
        void CreateMultipleContactsAssociatedWithAccount();

        void UploadFileToContactEntity();
    }
}
