using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCars.ViewModel
{
    public class LoginViewModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email")]
        public string Email { get; set; }
        
        [Required(ErrorMessage ="Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool Remember { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExtrernalLogins { get; set; }
    }
}
