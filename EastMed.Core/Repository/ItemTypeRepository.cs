using EastMed.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EastMed.Data.Model;
using System.Linq.Expressions;
using System.Data.Entity.Migrations;
// Repository for itemtype and with count delete get as select , get all select * , get by id as select * from where id = id , get getmany as a list , save as insert  and update  as update in sql command
namespace EastMed.Core.Repository
{
    public class ItemTypeRepository : IItemTypeRepository
    {
        private readonly EastMedDB _context = new EastMedDB();
        public int Count()
        {
            return _context.itemtype.Count();
        }

        public void Delete(int id)
        {
            var ITemType = GetById(id);
            if (ITemType != null)
            {
                _context.itemtype.Remove(ITemType);
            }
        }

        public itemtype Get(Expression<Func<itemtype, bool>> expression)
        {
            return _context.itemtype.FirstOrDefault(expression);
        }

        public IEnumerable<itemtype> GetAll()
        {
            return _context.itemtype.Select(x => x);
        }

        public itemtype GetById(int id)
        {
            return _context.itemtype.FirstOrDefault(x => x.ID == id);
        }

        public IQueryable<itemtype> GetMany(Expression<Func<itemtype, bool>> expression)
        {
            return _context.itemtype.Where(expression);
        }

        public void Insert(itemtype obj)
        {
            _context.itemtype.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(itemtype obj)
        {
            _context.itemtype.AddOrUpdate();
        }
    }
}
