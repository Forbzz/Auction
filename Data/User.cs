
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class User: IdentityUser
    {

        [Column("Registration")]
        public DateTime Registration { get; set; }


        public List<CarLot> CarLots { get; set; }


        public List<Bet> Bets { get; set; }
    }
}
