using CsvHelper.Configuration.Attributes;

namespace Arbetsprov
{
    internal class ContactClass
    {       
        /// <summary>
        /// Gets or sets the first name of contact entity.
        /// </summary>
        [Name("firstname")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of contact entity.
        /// </summary>
        [Name("lastname")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email of contact entity.
        /// </summary>
        [Name("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the address of contact entity.
        /// </summary>
        [Name("address1_line1")]
        public string Address1_Line1 { get; set; }              
    }
}
