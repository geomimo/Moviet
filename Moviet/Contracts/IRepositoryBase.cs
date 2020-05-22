using Moviet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        List<T> FindAll();
        T FindById();
        bool Create(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        bool Save();
    }
}
