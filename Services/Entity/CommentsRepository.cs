using System;
using System.Collections.Generic;
using System.Text;
using Data;
using Repo;
using Services.Abstract;

namespace Services.Entity
{
   public class CommentsRepository : ICommentsRepository
    {
        private ApplicationContext db;
        private IRepository<Comments> commRep;

        public CommentsRepository(ApplicationContext _context, IRepository<Comments> repository)
        {
            db = _context;
            commRep = repository;
        }
        void ICommentsRepository.AddComm(Comments comm)
        {
            commRep.Insert(comm);
        }

        void ICommentsRepository.RemoveComm(Comments comm)
        {
            commRep.Delete(comm);

        }
    }
}
