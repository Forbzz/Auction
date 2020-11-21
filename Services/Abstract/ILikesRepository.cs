using System;
using System.Collections.Generic;
using System.Text;
using Data;

namespace Services.Abstract
{
    public interface ILikesRepository
    {
        public void AddLike(Likes like, Comments comm);
        public void RemoveLike(Likes like, Comments comm);

        public Likes FindLike(User user, Comments comm);
    }
}
