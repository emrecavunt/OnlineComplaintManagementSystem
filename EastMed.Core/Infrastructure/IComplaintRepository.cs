using EastMed.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// interface from REpository class
namespace EastMed.Core.Infrastructure
{
   public interface IComplaintRepository : IRepository<complaint>
    {
    }
}
