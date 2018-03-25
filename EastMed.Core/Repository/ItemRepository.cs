using EastMed.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EastMed.Data.Model;
using System.Linq.Expressions;
using System.Data.Entity.Migrations;
// Repository for item and with count delete get as select , get all select * , get by id as select * from where id = id , get getmany as a list , save as insert  and update  as update in sql command
namespace EastMed.Core.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly EastMedDB _context = new EastMedDB();
        public int Count()
        {
            return _context.item.Count();
        }

        public void Delete(int id)
        {
            var Item = GetById(id);
            if (Item != null)
            {
                _context.item.Remove(Item);
            }
        }

        public item Get(Expression<Func<item, bool>> expression)
        {
            return _context.item.FirstOrDefault(expression);
        }

        public IEnumerable<item> GetAll()
        {
            return _context.item.Select(x => x);
        }

        public item GetById(int id)
        {
            return _context.item.FirstOrDefault(x => x.ID == id);
        }

        public IQueryable<item> GetMany(Expression<Func<item, bool>> expression)
        {
            return _context.item.Where(expression);
        }

        public void Insert(item obj)
        {
            _context.item.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(item obj)
        {
            _context.item.AddOrUpdate();
        }
        //public IQueryable<Data.Model.item> Items(string[] items)
        //{
        //    return _context.item.Where(x => items.Contains(x.ITEM_NAME));
        //}

        //public void AddItem(int LocationID, string Item)
        //{

        //    if (Item != null && Item != "")
        //    {
        //        //string[] Etikets = Etiket.Split(',');
        //        foreach (var tag in Item)
        //        {
        //            item item = this.Get(x => x.ITEM_NAME.ToLower() == tag.ToLower().Trim());
        //            if (item == null)
        //            {
        //                item = new item();
        //                item.ITEM_NAME = tag;
        //                this.Insert(item);
        //                this.Save();
        //            }

        //        }
        //        this.HaberEtiketEkle(HaberID, Etikets);
        //    }

        //}


        //public void HaberEtiketEkle(int HaberID, string[] etiketler)
        //{
        //    var Haber = _context.Haber.FirstOrDefault(x => x.ID == HaberID);
        //    var gelenEtiket = this.Etiketler(etiketler);

        //    Haber.Etiket.Clear();
        //    gelenEtiket.ToList().ForEach(etiket => Haber.Etiket.Add(etiket));
        //    _context.SaveChanges();
        //}
    }
}
