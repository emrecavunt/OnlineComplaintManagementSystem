using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EastMed.Core.Infrastructure
{
    public interface IRepository<T> where T: class
    {
        // Will take all data
        IEnumerable<T> GetAll();

        // will return only one value
        T GetById(int id);

        T Get(Expression<Func<T, bool>> expression);
        // will return many with expression as sql command where
        IQueryable<T> GetMany(Expression<Func<T, bool>> expression);
        // will insert the data
        void Insert(T obj);
        // will update database
        void Update(T obj);
        // will delete data
        void Delete(int id);
        // will count the table
        int Count();
        // will save the data
        void Save();
    }
}
