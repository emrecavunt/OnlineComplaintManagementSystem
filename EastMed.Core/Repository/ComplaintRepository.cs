using EastMed.Core.Infrastructure;
using EastMed.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity.Migrations;
// Repository for complaint and with count delete get as select , get all select * , get by id as select * from where id = id , get getmany as a list , save as insert  and update  as update in sql command
namespace EastMed.Core.Repository
{
    public class ComplaintRepository : IComplaintRepository
    {
        private readonly EastMedDB _context = new EastMedDB();
        public int Count()
        {
            return _context.complaint.Count();
        }

        public void Delete(int id)
        {
            var Complaint = GetById(id);
            if (Complaint != null)
            {
                _context.complaint.Remove(Complaint);
            }
        }

        public complaint Get(Expression<Func<complaint, bool>> expression)
        {
            return _context.complaint.FirstOrDefault(expression);
        }

        public IEnumerable<complaint> GetAll()
        {
            return _context.complaint.Select(x => x);
        }

        public complaint GetById(int id)
        {
            return _context.complaint.FirstOrDefault(x => x.ID == id);
        }

        public IQueryable<complaint> GetMany(Expression<Func<complaint, bool>> expression)
        {
            return _context.complaint.Where(expression);
        }

        public void Insert(complaint obj)
        {
            _context.complaint.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(complaint obj)
        {
            _context.complaint.AddOrUpdate();
        }
      
    }
}
