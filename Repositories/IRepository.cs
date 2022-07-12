using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Repositories
{
    internal interface IRepository<T> where T: class
    {
        List<T> GetAll();

        List<T> GetFiltered(ExpressionStarter<T> condition);

        T GetById(int id);

        void Create(T entity);

        void Update(T entity);

        void Delete(int id);
    }
}
