using EastMed.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EastMed.Core.Infrastructure
{
    public interface IUserRepository: IRepository<user>
    {
        user UserFind(string UNIID);
    }
}
