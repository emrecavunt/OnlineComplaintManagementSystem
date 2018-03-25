using EastMed.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;
using System.Data.Entity.Migrations;
using EastMed.Data.Model;
// Repository for short cut the sql commands
//// Repository for user and with count delete get as select , get all select * , get by id as select * from where id = id , get getmany as a list , save as insert  and update  as update in sql command
namespace EastMed.Core.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly EastMedDB _context = new EastMedDB();
        public int Count()
        {
            return _context.user.Count();
        }

        public void Delete(int id)
        {
            var User = GetById(id);
            if(User != null)
            {
                _context.user.Remove(User);
            }
        }

        public user Get(Expression<Func<user, bool>> expression)
        {
            return _context.user.FirstOrDefault(expression);
        }

        public IEnumerable<user> GetAll()
        {
            return _context.user.Select(x => x); 
        }

        public user GetById(int id)
        {
            return _context.user.FirstOrDefault(x => x.ID == id);
        }

        public IQueryable<user> GetMany(Expression<Func<user, bool>> expression)
        {
            return _context.user.Where(expression);
        }

        public void Insert(user obj)
        {
            _context.user.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(user obj)
        {
            _context.user.AddOrUpdate();
        }
        public Data.Model.user UserFind(string UNIID)
        {
            return _context.user.SingleOrDefault(x => x.UNI_ID == UNIID);
        }
    }
}
