using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankApplication.ViewModels
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "Ange ett användarnamn")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Ange ett lösenord")]
        public string Password { get; set; }

        public string CheckPassword { get; set; }

        public string RoleName { get; set; }
    }
}
