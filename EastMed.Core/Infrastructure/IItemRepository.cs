using EastMed.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EastMed.Core.Infrastructure
{
    public interface IItemRepository : IRepository<item>
    {
        //IQueryable<item> Etiketler(string[] items);

        //void AddItem(int LocationID, string item);

        //void AddLocationItem(int LocationID, string[] items);
    }
}
