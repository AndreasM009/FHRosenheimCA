//-------------------------------------------------------------------------------------
// Author:      amo
// Created:     8/2/2016 3:12:59 PM
// Copyright (c) white duck Gesellschaft für Softwareentwicklung mbH
//-------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fhr.Contacts.Models;

namespace Fhr.Contacts.Repository.Interfaces
{
    public interface IContactRepository
    {
        Task<Contact> Add(Guid aUserId, Contact aContact);
        Task<Contact> Update(Guid aUserId, Contact aContact);
        Task Delete(Guid aUserId, Guid aId);
        Task<List<Contact>> GetAll(Guid aUserId);
        Task<Contact> GetById(Guid aUserId, Guid aId);
    }
}
