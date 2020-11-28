using Data;
using System;
using System.Collections.Generic;
using System.Text;


namespace Services.Abstract
{
    public interface IBetPerository
    {
        public void AddBet(Bet bet);
        public void DeleteBet(Bet bet);
        public void DeleteBets(int? id);
    }
}
