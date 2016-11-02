using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Fhr.Contacts.Repository.Interfaces;

namespace Fhr.Contacts.Repository.Implementation
{
    public class ContactRepository : IContactRepository
    {
        #region Fields

        private readonly Func<ContactContext> mDbContextFactory;

        public ContactRepository(Func<ContactContext> aDbContextFactory)
        {
            mDbContextFactory = aDbContextFactory;
        }

        #endregion

        #region Implementation of IContactService

        async Task<Contacts.Models.Contact> IContactRepository.Add(Guid aUserId, Contacts.Models.Contact aContact)
        {
            try
            {
                using (var db = mDbContextFactory())
                {
                    aContact.Id = Guid.NewGuid();
                    aContact.OwnerUserId = aUserId;
                    db.Contacts.Add(aContact);
                    await db.SaveChangesAsync();
                    return aContact;
                }
            }
            catch (Exception)
            {                
                throw;
            }
        }

        async Task<Contacts.Models.Contact> IContactRepository.Update(Guid aUserId, Contacts.Models.Contact aContact)
        {
            try
            {
                using (var db = mDbContextFactory())
                {
                    var contact = await db.Contacts.FirstOrDefaultAsync(c => c.Id == aContact.Id);

                    if (null == contact)
                        return null;

                    contact.OwnerUserId = aUserId;
                    contact.Name = aContact.Name;
                    contact.Surename = aContact.Surename;
                    contact.Birthdate = aContact.Birthdate;

                    db.Entry(contact).State = EntityState.Modified;

                    await db.SaveChangesAsync();

                    return contact;
                }
            }
            catch (Exception)
            {                
                throw;
            }
        }

        async Task IContactRepository.Delete(Guid aUserId, Guid aId)
        {
            try
            {
                using (var db = mDbContextFactory())
                {
                    var contact = await db.Contacts.FirstOrDefaultAsync(c => c.Id == aId && c.OwnerUserId == aUserId);

                    if (null == contact)
                        return;

                    db.Contacts.Remove(contact);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception)
            {                
                throw;
            }
        }

        async Task<List<Contacts.Models.Contact>> IContactRepository.GetAll(Guid aUserId)
        {
            try
            {
                using (var db = mDbContextFactory())
                {
                    return await db.Contacts.Where(u => u.OwnerUserId == aUserId).ToListAsync();
                }
            }
            catch (Exception)
            {                
                throw;
            }
        }

        async Task<Contacts.Models.Contact> IContactRepository.GetById(Guid aUserId, Guid aId)
        {
            try
            {
                using (var db = mDbContextFactory())
                {
                    return await db.Contacts.FirstOrDefaultAsync(c => c.Id == aId && c.OwnerUserId == aUserId);
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
