using DbDataAccess;
using LightDriverService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System;
using System.Threading.Tasks;
using TestApplication.Repositories;

namespace TestApplication.Tests
{
    [TestClass]
    public class BulbFaultBehaviourTests
    {
        Guid _bulbId = Guid.NewGuid();
        IDataService _testService;
        ILightDriverService _lightDriver;
        IStreetlightRepository _repository;

        [TestInitialize]
        public void SetupTests()
        {
            _lightDriver = MockRepository.GenerateMock<ILightDriverService>();
            _testService = MockRepository.GenerateMock<IDataService>();

            _repository = new StreetlightRepository(_testService
                , _lightDriver);
        }

        private Task<bool> GetBooleanTask(bool value)
        {
            var task = new Task<bool>(() =>
            {
                return value;
            });
            task.Start();
            return task;
        }
    }
}
