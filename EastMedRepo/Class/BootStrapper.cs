using Autofac;
using Autofac.Integration.Mvc;
using EastMed.Core.Infrastructure;
using EastMed.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EastMedRepo.Class
{
   
    public class BootStrapper
    {
        // Boot Aşamasında çalışacak
        public static void RunConfig()
        {
            BuildAutoFac();
        }
        private static void BuildAutoFac()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<RoleRepository>().As<IRoleRepository>();
            builder.RegisterType<LocationRepository>().As<ILocationRepository>();
            builder.RegisterType<DepartmantRepository>().As<IDepartmentRepository>();
            builder.RegisterType<ItemTypeRepository>().As<IItemTypeRepository>();
            builder.RegisterType<ComplaintRepository>().As<IComplaintRepository>();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}