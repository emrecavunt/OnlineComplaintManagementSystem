using EastMed.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;
using System.Data.Entity.Migrations;
using EastMed.Data.Model;
// Repository for department and with count delete get as select , get all select * , get by id as select * from where id = id , get getmany as a list , save as insert  and update  as update in sql command
namespace EastMed.Core.Repository
{
    public class DepartmantRepository : IDepartmentRepository
    {
        private readonly EastMedDB _context = new EastMedDB();
        public int Count()
        {
            return _context.departmant.Count();
        }

        public void Delete(int id)
        {
            var Department = GetById(id);
            if (Department != null)
            {
                _context.departmant.Remove(Department);
            }
        }

        public departmant Get(Expression<Func<departmant, bool>> expression)
        {
            return _context.departmant.FirstOrDefault(expression);
        }

        public IEnumerable<departmant> GetAll()
        {
            return _context.departmant.Select(x => x);
        }

        public departmant GetById(int id)
        {
            return _context.departmant.FirstOrDefault(x => x.ID == id);
        }

        public IQueryable<departmant> GetMany(Expression<Func<departmant, bool>> expression)
        {
            return _context.departmant.Where(expression);
        }

        public void Insert(departmant obj)
        {
            _context.departmant.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(departmant obj)
        {
            _context.departmant.AddOrUpdate();
        }
    }
}
