//-------------------------------------------------------------------------------------
// Author:      amo
// Created:     8/4/2016 1:10:44 PM
// Copyright (c) white duck Gesellschaft für Softwareentwicklung mbH
//-------------------------------------------------------------------------------------

using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Fhr.Contacts.Models;
using Fhr.Contacts.Repository.Interfaces;

namespace Fhr.Contacts.Repository.Implementation
{
    public class ContactSettingsRepository : IContactsSettingsRepository
    {
        private readonly Func<ContactContext> mDbContext;

        public ContactSettingsRepository(Func<ContactContext> aDbContext)
        {
            mDbContext = aDbContext;
        }

        #region Implementation of IContactsSettingsService

        async Task<ContactSettings> IContactsSettingsRepository.Get(Guid aUserId)
        {
            try
            {
                using (var db = mDbContext())
                {
                    var settings = await db.ContactsSettings.Where(s => s.UserId == aUserId).FirstOrDefaultAsync();
                    if (null == settings)
                    {
                        return new ContactSettings
                               {
                                   UserId = aUserId,
                                   SortBySurename = true
                               };
                    }

                    return settings;
                }
            }
            catch (Exception)
            {                
                throw;
            }
        }

        async Task<ContactSettings> IContactsSettingsRepository.Update(Guid aUserId, ContactSettings aSettings)
        {
            try
            {
                using (var db = mDbContext())
                {
                    var settings = await db.ContactsSettings.Where(s => s.UserId == aUserId).FirstOrDefaultAsync();
                    if (null == settings)
                    {
                        settings = new ContactSettings
                                   {
                                       UserId = aUserId,
                                       SortBySurename = aSettings.SortBySurename
                                   };

                        db.ContactsSettings.Add(settings);
                    }
                    else
                    {
                        db.Entry(settings).State = EntityState.Modified;
                        settings.UserId = aUserId;
                        settings.SortBySurename = aSettings.SortBySurename;
                        settings.DisplaySurenameFirst = aSettings.DisplaySurenameFirst;
                    }

                    await db.SaveChangesAsync();
                    return settings;
                }
            }
            catch (Exception)
            {                
                throw;
            }
        }

        #endregion
    }
}
