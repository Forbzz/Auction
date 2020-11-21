using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCars.ViewModel
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "NameError")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 20 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "EmailError")]
        [EmailAddress(ErrorMessage = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "PasswordRepeat")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "RepeatError")]
        public string PassowrdConfirm { get; set; }
    }
}
