using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCars.ViewModel
{
    public class ProfileViewModel
    {
        public User user { get; set; }
        public bool isMe { get; set; }
    }
}
