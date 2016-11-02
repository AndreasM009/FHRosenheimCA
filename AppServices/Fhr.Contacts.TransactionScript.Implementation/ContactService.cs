using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Fhr.Contacts.Common;
using Fhr.Contacts.Dto;
using Fhr.Contacts.Models;
using Fhr.Contacts.Repository.Interfaces;
using Fhr.Contacts.TransactionScript.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace Fhr.Contacts.TransactionScript.Implementation
{
    public class ContactService : IContactService
    {

        #region Fields

        private readonly IContactRepository mContactRepository;
        private readonly IContactImageRepository mContactImageRepository;
        private readonly IContactsSettingsRepository mContactsSettingsRepository;
        private readonly IImageBlobService mImageBlobService;

        public ContactService(IContactRepository aContactRepository, IContactImageRepository aContactImageRepository, IContactsSettingsRepository aContactsSettingsRepository, IImageBlobService aImageBlobService)
        {
            mContactRepository = aContactRepository;
            mContactImageRepository = aContactImageRepository;
            mContactsSettingsRepository = aContactsSettingsRepository;
            mImageBlobService = aImageBlobService;
        }

        #endregion

        #region Implementation of IContactService

        async Task<Contacts.Models.Contact> IContactService.Add(Guid aUserId, Contacts.Models.Contact aContact, byte[] aImage)
        {
            try
            {                
                var result = await mContactRepository.Add(aUserId, aContact);
                await mImageBlobService.UploadImage(aUserId, result.Id, aImage);
                return result;
            }
            catch (Exception)
            {                
                throw;
            }
        }

        async Task<Contacts.Models.Contact> IContactService.Update(Guid aUserId, Contacts.Models.Contact aContact, byte[] aImage)
        {
            try
            {
                var result = await mContactRepository.Update(aUserId, aContact);
                await mImageBlobService.UploadImage(aUserId, result.Id, aImage);
                return result;
            }
            catch (Exception)
            {                
                throw;
            }
        }

        async Task IContactService.Delete(Guid aUserId, Guid aId)
        {
            try
            {
                await mContactRepository.Delete(aUserId, aId);
            }
            catch (Exception)
            {                
                throw;
            }
        }

        async Task<List<ContactDto>> IContactService.GetAll(Guid aUserId)
        {
            try
            {
                var contacts = await mContactRepository.GetAll(aUserId);
                var images = await mContactImageRepository.GetImages(contacts.Select(c => c.Id));

                return contacts.Select(c => new ContactDto
                                            {
                                                Id = c.Id,
                                                OwnerUserId = c.OwnerUserId,
                                                ImageUrl = images.ContainsKey(c.Id) ? images[c.Id].ImageUrl : string.Empty,
                                                Email = c.Email,
                                                Surename = c.Surename,
                                                Birthdate = c.Birthdate,
                                                Name = c.Name
                                            }).ToList();
            }
            catch (Exception)
            {                
                throw;
            }
        }

        async Task<ContactDto> IContactService.GetById(Guid aUserId, Guid aId)
        {
            try
            {
                var contact =  await mContactRepository.GetById(aUserId, aId);
                if (null == contact)
                    return null;

                var images = await mContactImageRepository.GetImages(new[] {contact.Id});

                return new ContactDto
                       {
                           Id = contact.Id,
                           OwnerUserId = contact.OwnerUserId,
                           ImageUrl = images.ContainsKey(contact.Id) ? images[contact.Id].ImageUrl : string.Empty,
                           Email = contact.Email,
                           Surename = contact.Surename,
                           Birthdate = contact.Birthdate,
                           Name = contact.Name
                       };
            }
            catch (Exception)
            {                
                throw;
            }
        }

        async Task IContactService.SetImage(Guid aUserId, Guid aContactId, byte[] aImage)
        {
            await mImageBlobService.UploadImage(aUserId, aContactId, aImage);
        }

        async Task<Dictionary<Guid, ContactImage>> IContactService.GetImages(IEnumerable<Guid> aContctIds)
        {
            return await mContactImageRepository.GetImages(aContctIds);
        }

        async Task<ContactSettings> IContactService.GetSettings(Guid aUserId)
        {
            try
            {
                return await mContactsSettingsRepository.Get(aUserId);
            }
            catch (Exception)
            {                
                throw;
            }
        }

        async Task<ContactSettings> IContactService.UpdateSettings(Guid aUserId, ContactSettings aSettings)
        {
            try
            {
                return await mContactsSettingsRepository.Update(aUserId, aSettings);
            }
            catch (Exception)
            {                
                throw;
            }
        }

        #endregion        
    }
}

