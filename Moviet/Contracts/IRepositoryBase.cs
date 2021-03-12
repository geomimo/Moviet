using System.Collections.Generic;

namespace Moviet.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        List<T> FindAll();
        T FindById(int id);
        bool Create(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        bool Save();
        void SetIdentityInsert(bool set);
        void Clear();
    }
}
