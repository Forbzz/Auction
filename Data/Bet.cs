using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
   public class Bet : BaseEntity
    {

        [Column("NewPrice")]
        public uint NewPrice { get; set; }

        [Column("Time")]
        public DateTime Time { get; set; }

        [Column("CarLotId")]
        public CarLot CarLot { get; set; }

        [Column("UserId")]
        public User User { get; set; }
    }
}
