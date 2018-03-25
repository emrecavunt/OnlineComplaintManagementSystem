using EastMed.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;
using System.Data.Entity.Migrations;
using EastMed.Data.Model;
// Repository for role and with count delete get as select , get all select * , get by id as select * from where id = id , get getmany as a list , save as insert  and update  as update in sql command
namespace EastMed.Core.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly EastMedDB _context = new EastMedDB();
        public int Count()
        {
            return _context.privilege.Count();
        }

        public void Delete(int id)
        {
            var Privilege = GetById(id);
            if (Privilege != null)
            {
                _context.privilege.Remove(Privilege);
            }
        }

        public privilege Get(Expression<Func<privilege, bool>> expression)
        {
            return _context.privilege.FirstOrDefault(expression);
        }

        public IEnumerable<privilege> GetAll()
        {
            return _context.privilege.Select(x => x);
        }

        public privilege GetById(int id)
        {
            return _context.privilege.FirstOrDefault(x => x.ID == id);
        }

        public IQueryable<privilege> GetMany(Expression<Func<privilege, bool>> expression)
        {
            return _context.privilege.Where(expression);
        }

        public void Insert(privilege obj)
        {
            _context.privilege.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(privilege obj)
        {
            _context.privilege.AddOrUpdate();
        }
    }
}
