using System;
using System.Data.Common;
using Fhr.Contacts.Repository.Implementation;
using Fhr.Contacts.Repository.Interfaces;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fhr.Contacts.UnitTests
{
    [TestClass]
    public class TestContactDb
    {
        #region Fields

        private UnityContainer mUnity;
        private IContactRepository mContactRepository;
        private DbConnection mDbConnection;

        #endregion

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext aContext)
        {
            Effort.Provider.EffortProviderConfiguration.RegisterProvider();
        }

        [TestInitialize]
        public void InitTest()
        {
            mDbConnection = Effort.DbConnectionFactory.CreateTransient();

            mUnity = new UnityContainer();
            mUnity.RegisterType<ContactContext>(new TransientLifetimeManager(), new InjectionConstructor(mDbConnection));
            mUnity.RegisterType<IContactRepository, ContactRepository>(new ContainerControlledLifetimeManager());
            mContactRepository = mUnity.Resolve<IContactRepository>();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            mUnity.Dispose();
            mDbConnection.Dispose();
            mUnity = null;
        }

        [TestMethod]
        public void TestEmptyDb()
        {            
            Assert.IsTrue(0 == mContactRepository.GetAll(Guid.NewGuid()).Result.Count);
            Assert.IsNull(mContactRepository.GetById(Guid.NewGuid(), Guid.NewGuid()).Result);
        }

        [TestMethod]
        public void TestAddAndGet()
        {
            var userId = Guid.NewGuid();

            var contact = new Models.Contact
                          {
                              Id = Guid.Empty,
                              Name = "TestName",
                              Surename = "TestSurename",
                              Birthdate = DateTime.Now,
                              Email = "test@test.de",
                              OwnerUserId = userId
                          };

            var result = mContactRepository.Add(userId, contact).Result;
            Assert.IsTrue(result.Id != Guid.Empty);

            var result2 = mContactRepository.GetById(userId, result.Id).Result;
            
            Assert.IsFalse(object.ReferenceEquals(result, result2));
            Assert.IsTrue(result2.Id == result.Id);


            Assert.IsTrue(result.Name == contact.Name);
            Assert.IsTrue(result.Surename == contact.Surename);
            Assert.IsTrue(result.Email == contact.Email);
            Assert.IsTrue(result.Birthdate == contact.Birthdate);
            Assert.IsTrue(result.OwnerUserId == userId);

            Assert.IsTrue(result2.Name == contact.Name);
            Assert.IsTrue(result2.Surename == contact.Surename);
            Assert.IsTrue(result2.Email == contact.Email);
            Assert.IsTrue(result2.Birthdate == contact.Birthdate);
            Assert.IsTrue(result2.OwnerUserId == userId);
        }

        [TestMethod]
        public void TestDelete()
        {
            var userId = Guid.NewGuid();

            var contact = new Models.Contact
            {
                Id = Guid.Empty,
                Name = "TestName",
                Surename = "TestSurename",
                Birthdate = DateTime.Now,
                Email = "test@test.de",
                OwnerUserId = userId
            };

            var result = mContactRepository.Add(userId, contact).Result;
            Assert.IsNotNull(mContactRepository.GetById(userId, result.Id).Result);

            mContactRepository.Delete(userId, result.Id).Wait();
            Assert.IsNull(mContactRepository.GetById(userId, result.Id).Result);
        }
    }
}
