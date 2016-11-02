//-------------------------------------------------------------------------------------
// Author:      amo
// Created:     8/4/2016 1:07:03 PM
// Copyright (c) white duck Gesellschaft für Softwareentwicklung mbH
//-------------------------------------------------------------------------------------

using System;

namespace Fhr.Contacts.Models
{
    public class ContactSettings
    {
        public Guid UserId { get; set; }
        public bool SortBySurename { get; set; } = true;
        public bool DisplaySurenameFirst { get; set; } = true;

    }
}
