//-------------------------------------------------------------------------------------
// Author:      amo
// Created:     8/8/2016 2:56:06 PM
// Copyright (c) white duck Gesellschaft für Softwareentwicklung mbH
//-------------------------------------------------------------------------------------

using System;
using System.Text;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Fhr.Contacts.Repository.Implementation;
using Fhr.Contacts.Repository.Interfaces;
using Fhr.Contacts.TransactionScript.Implementation;
using Fhr.Contacts.TransactionScript.Interfaces;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using Fhr.Contacts.Models;

namespace Fhr.Contacts.UnitTests
{
    /// <summary>
    /// Summary description for ContectServiceTest
    /// </summary>
    [TestClass]
    public class ContectServiceTest
    {
        #region Fields

        private IContactService mContactService;
        private UnityContainer mUnity;
        private IImageBlobService mFakeImageBlobService;
        private DbConnection mDbConnection;

        #endregion

        #region Test Management        

        [TestInitialize()]
        public void TestInitialize()
        {
            mDbConnection = Effort.DbConnectionFactory.CreateTransient();
                
            mUnity = new UnityContainer();
            mUnity.RegisterType<ContactContext>(new TransientLifetimeManager(), new InjectionConstructor(mDbConnection));
            mUnity.RegisterType<IContactRepository, ContactRepository>(new ContainerControlledLifetimeManager());
            mUnity.RegisterType<IContactImageRepository, ContactImageRepository>(new ContainerControlledLifetimeManager());
            mUnity.RegisterType<IContactsSettingsRepository, ContactSettingsRepository>(new ContainerControlledLifetimeManager());            
            mUnity.RegisterType<IContactService, ContactService>(new ContainerControlledLifetimeManager());


            mFakeImageBlobService = A.Fake<IImageBlobService>();
            A.CallTo(() => mFakeImageBlobService.UploadImage(A<Guid>.Ignored, A<Guid>._, A<byte[]>.Ignored))
             .Invokes(context =>
             {
                 var contactId = (Guid)context.Arguments[1];
                 
                 using (var db = mUnity.Resolve<ContactContext>())
                 {
                     db.ContactsImages.Add(new ContactImage {ContactId = contactId, ImageUrl = "TestUrl"});
                     db.SaveChanges();
                 }
             })
             .Returns(Task.FromResult(true));

            mUnity.RegisterInstance<IImageBlobService>(mFakeImageBlobService);
            mContactService = mUnity.Resolve<IContactService>();
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            mUnity.Dispose();
            mDbConnection.Dispose();
            mUnity = null;
        }
        
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            var userId = Guid.NewGuid();

            var contact = mContactService.Add(userId, new Contact
                                                      {
                                                          Id = Guid.Empty,
                                                          Name = "TestName",
                                                          Surename = "TestSurename",
                                                          Birthdate = DateTime.Now,
                                                          OwnerUserId = userId,
                                                          Email = "test@test.de"
                                                      }, new byte[] {}).Result;

            Assert.IsFalse(Guid.Empty == contact.Id);

            // Call must have happened once
            A.CallTo(() => mFakeImageBlobService.UploadImage(A<Guid>.Ignored, A<Guid>._, A<byte[]>.Ignored)).MustHaveHappened();

            var dto = mContactService.GetById(userId, contact.Id).Result;
            Assert.IsFalse(string.IsNullOrEmpty(dto.ImageUrl));
        }
    }
}
