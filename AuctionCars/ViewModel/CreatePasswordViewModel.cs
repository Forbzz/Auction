using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCars.ViewModel
{
    public class CreatePasswordViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "NewPassword")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "ConfirmPassword")]
        [DataType(DataType.Password)]
        public string NewPasswordConfirm { get; set; }
    }
}
