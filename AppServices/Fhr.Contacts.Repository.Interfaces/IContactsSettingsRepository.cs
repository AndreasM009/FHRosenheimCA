//-------------------------------------------------------------------------------------
// Author:      amo
// Created:     8/4/2016 1:08:09 PM
// Copyright (c) white duck Gesellschaft für Softwareentwicklung mbH
//-------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Fhr.Contacts.Models;

namespace Fhr.Contacts.Repository.Interfaces
{
    public interface IContactsSettingsRepository
    {
        Task<ContactSettings> Get(Guid aUserId);
        Task<ContactSettings> Update(Guid aUserId, ContactSettings aSettings);
    }
}
