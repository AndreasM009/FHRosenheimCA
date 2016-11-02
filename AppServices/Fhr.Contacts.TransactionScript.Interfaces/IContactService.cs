//-------------------------------------------------------------------------------------
// Author:      amo
// Created:     8/2/2016 3:12:59 PM
// Copyright (c) white duck Gesellschaft für Softwareentwicklung mbH
//-------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fhr.Contacts.Dto;
using Fhr.Contacts.Models;

namespace Fhr.Contacts.TransactionScript.Interfaces
{
    public interface IContactService
    {
        Task<Contact> Add(Guid aUserId, Contact aContact, byte[] aImage);
        Task<Contact> Update(Guid aUserId, Contact aContact, byte[] aImage);
        Task Delete(Guid aUserId, Guid aId);
        Task<List<ContactDto>> GetAll(Guid aUserId);
        Task<ContactDto> GetById(Guid aUserId, Guid aId);
        Task SetImage(Guid aUserId, Guid aContactId, byte[] aImage);
        Task<Dictionary<Guid, ContactImage>> GetImages(IEnumerable<Guid> aContctIds);

        Task<ContactSettings> GetSettings(Guid aUserId);
        Task<ContactSettings> UpdateSettings(Guid aUserId, ContactSettings aSettings);
    }
}
