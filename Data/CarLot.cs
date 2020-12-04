using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
    public class CarLot: BaseEntity
    {
        [Column("Name")]
        public string Name { get; set; }

        [Column("StartPrice")]
        public uint StartPrice { get; set; }

        [Column("Price")]
        public uint Price { get; set; }

        [Column("CarId")]
        public Car Car { get; set; }

        [Column("WinnerName")]
        public string WinnerName { get; set; }

        [Column("UserId")]
        public User User { get; set; }

        [Column("Exposing")]
        public DateTime Exposing { get; set; }

        [Column("Ending")]
        public DateTime Ending { get; set; }

        [Column("Bets")]
        public List<Bet> Bets { get; set; }

        [Column("Applyed")]
        public bool Applyed { get; set; } = false;

        public IEnumerable<Comments> Comments { get; set; }
        public bool IsActual()
        {
            if (DateTime.Now < Ending)
                return true;
            else return false;
        }

        [Column("Ended")]
        public bool Ended { get; set; }
    }
}
