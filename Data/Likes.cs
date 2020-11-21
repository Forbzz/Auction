using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
    public class Likes: BaseEntity
    {

        public int CommentsId { get; set; }

        [Column("UserId")]
        public User User { get; set; }

        [Column("CommentsId")]
        public Comments Comments { get; set; }
    }
}
