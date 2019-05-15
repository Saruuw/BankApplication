using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankApplication.Models
{
    public partial class Customers
    {
        public Customers()
        {
            Dispositions = new HashSet<Dispositions>();
        }

        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Ange kön")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Ange förnamn")]
        public string Givenname { get; set; }

        [Required(ErrorMessage = "Ange efternamn")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Ange adress")]
        public string Streetaddress { get; set; }

        [Required(ErrorMessage = "Ange stad")]
        public string City { get; set; }

        [Required(ErrorMessage = "Ange postnummer")]
        public string Zipcode { get; set; }

        [Required(ErrorMessage = "Ange land")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Ange landskod")]
        public string CountryCode { get; set; }

        [Required(ErrorMessage = "Ange födelsedag")]
        public DateTime? Birthday { get; set; }

        public string NationalId { get; set; }

        [Required(ErrorMessage = "Ange riktnummer")]
        public string Telephonecountrycode { get; set; }

        [Required(ErrorMessage = "Ange telefonnummer")]
        public string Telephonenumber { get; set; }

        [Required(ErrorMessage = "Ange emailadress")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Ogiltig emailadress")]
        public string Emailaddress { get; set; }

        public virtual ICollection<Dispositions> Dispositions { get; set; }
    }
}
