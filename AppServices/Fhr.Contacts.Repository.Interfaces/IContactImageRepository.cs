//-------------------------------------------------------------------------------------
// Author:      amo
// Created:     8/5/2016 3:43:35 PM
// Copyright (c) white duck Gesellschaft für Softwareentwicklung mbH
//-------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fhr.Contacts.Models;

namespace Fhr.Contacts.Repository.Interfaces
{
    public interface IContactImageRepository
    {
        Task<Dictionary<Guid, ContactImage>> GetImages(IEnumerable<Guid> aContctIds);
    }
}
