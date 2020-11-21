using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
    public class Comments: BaseEntity
    {

        public int CarLotId { get; set; }

        [Column("CarLotId")]
        public CarLot CarLot { get; set; }

        [Column("UserId")]
        public User User { get; set; }

        [Column("Content")]
        public string Content { get; set; }
        public IEnumerable<Likes> Likes { get; set; }
    }
}
