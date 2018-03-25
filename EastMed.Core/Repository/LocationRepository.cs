using EastMed.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;
using System.Data.Entity.Migrations;
using EastMed.Data.Model;
// Repository for location and with count delete get as select , get all select * , get by id as select * from where id = id , get getmany as a list , save as insert  and update  as update in sql command
namespace EastMed.Core.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly EastMedDB _context = new EastMedDB();
        public int Count()
        {
            return _context.location.Count();
        }

        public void Delete(int id)
        {
            var Location = GetById(id);
            if (Location != null)
            {
                _context.location.Remove(Location);
            }
        }

        public location Get(Expression<Func<location, bool>> expression)
        {
            return _context.location.FirstOrDefault(expression);
        }

        public IEnumerable<location> GetAll()
        {
            return _context.location.Select(x => x);
        }
        

        public location GetById(int id)
        {
            return _context.location.FirstOrDefault(x => x.ID == id);
        }

        public IQueryable<location> GetMany(Expression<Func<location, bool>> expression)
        {
            return _context.location.Where(expression);
        }

        public void Insert(location obj)
        {
            _context.location.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(location obj)
        {
            _context.location.AddOrUpdate();
        }

    }
}
