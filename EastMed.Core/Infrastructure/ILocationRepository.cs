﻿using EastMed.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EastMed.Core.Infrastructure
{
   public interface ILocationRepository : IRepository<location>
    {
        //IList<departmant> GetAllDepartments();
        //IList<location> GetAllLocationsByID(int DeptId);
    }
}
