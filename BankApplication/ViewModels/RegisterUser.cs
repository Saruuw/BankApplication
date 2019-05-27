using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BankApplication.ViewModels
{
    public class RegisterUser
    {
        [DisplayName("Ange ett användarnamn")]
        public string UserName { get; set; }

        [DisplayName("Ange ett lösenord")]
        public string Password { get; set; }

        public string RoleName { get; set; }
    }
}
