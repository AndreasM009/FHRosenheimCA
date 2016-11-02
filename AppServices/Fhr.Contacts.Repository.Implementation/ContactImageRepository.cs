//-------------------------------------------------------------------------------------
// Author:      amo
// Created:     8/5/2016 3:44:44 PM
// Copyright (c) white duck Gesellschaft für Softwareentwicklung mbH
//-------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Fhr.Contacts.Models;
using Fhr.Contacts.Repository.Interfaces;

namespace Fhr.Contacts.Repository.Implementation
{
    public class ContactImageRepository : IContactImageRepository
    {
        private readonly Func<ContactContext> mDbContext;

        public ContactImageRepository(Func<ContactContext> aDbContext)
        {
            mDbContext = aDbContext;
        }

        public async Task<Dictionary<Guid, ContactImage>> GetImages(IEnumerable<Guid> aContctIds)
        {
            var ids = aContctIds.ToList();
            using (var db = mDbContext())
            {
                var images = await db.ContactsImages.Where(img => ids.Contains(img.ContactId)).ToDictionaryAsync(img => img.ContactId);
                return images;
            }
        }        
    }
}
