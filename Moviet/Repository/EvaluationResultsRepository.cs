using Moviet.Contracts;
using Moviet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Repository
{
    public class EvaluationResultsRepository : IEvaluationResultsRepository
    {
        private readonly ApplicationDbContext _db;

        public EvaluationResultsRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Create(EvaluationResults entity)
        {
            _db.EvaluationResults.Add(entity);
            return Save();
        }

        public bool Delete(EvaluationResults entity)
        {
            throw new NotImplementedException();
        }

        public List<EvaluationResults> FindAll()
        {
            return _db.EvaluationResults.ToList();
        }

        public EvaluationResults FindById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0;
        }

        public void SetIdentityInsert(bool set)
        {
            throw new NotImplementedException();
        }

        public bool Update(EvaluationResults entity)
        {
            throw new NotImplementedException();
        }
    }
}
