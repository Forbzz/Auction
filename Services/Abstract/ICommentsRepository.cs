using System;
using System.Collections.Generic;
using System.Text;
using Data;

namespace Services.Abstract
{
    public interface ICommentsRepository
    {
        public void AddComm(Comments comm);
        public void RemoveComm(Comments comm);
    }
}
