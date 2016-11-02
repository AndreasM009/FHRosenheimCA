//-------------------------------------------------------------------------------------
// Author:      amo
// Created:     8/8/2016 2:57:23 PM
// Copyright (c) white duck Gesellschaft für Softwareentwicklung mbH
//-------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace Fhr.Contacts.TransactionScript.Implementation
{
    public interface IImageBlobService
    {
        Task UploadImage(Guid aUserId, Guid aContactId, byte[] aImage);
    }
}
