using Repo;
using System;
using System.Collections.Generic;
using System.Text;
using Data;
using Services.Abstract;
using System.Linq;

namespace Services.Entity
{
   public class LikesRepository : ILikesRepository
    {
        private ApplicationContext db;
        private IRepository<Likes> likeRep;
        private IRepository<Comments> commRep;
        public LikesRepository(ApplicationContext _context, IRepository<Likes> repository, IRepository<Comments> commRepepository)
        {
            db = _context;
            likeRep = repository;
            commRep = commRepepository;
        }

        public Likes FindLike(User user, Comments comm)
        {
           return db.Likes.Where(l => l.User == user && l.CommentsId == comm.Id).FirstOrDefault();
        }

        void ILikesRepository.AddLike(Likes like, Comments comm)
        {
            likeRep.Insert(like);
            commRep.Update(comm);
            db.SaveChanges();
        }

        void ILikesRepository.RemoveLike(Likes like, Comments comm)
        {
            likeRep.Delete(like);
            commRep.Update(comm);
            db.SaveChanges();
        }
    }
}
