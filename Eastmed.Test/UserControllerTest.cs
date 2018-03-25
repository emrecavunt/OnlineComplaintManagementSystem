using EastMed.Core.Infrastructure;
using EastMedRepo.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eastmed.Test
{
    [TestClass]
    public class UserControllerTest
    {
        private readonly IUserRepository _userRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IRoleRepository _roleRepository;
        public UserControllerTest(IRoleRepository roleRepository , IUserRepository userRepository,IDepartmentRepository departmentRepository, ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _departmentRepository = departmentRepository;
        }
        /// <summary>
        /// You Have to ovverride this method!
        /// </summary>
        [TestMethod]
        public  void TestDetailsViewData()
        {
           
            //var usercontroller = new UserController(_userRepository,_roleRepository,_locationRepository,_departmentRepository);
            //// Act
            //var result = usercontroller.Details(1)w as ViewResult;
            //// Assert
            //Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }
    }
}
