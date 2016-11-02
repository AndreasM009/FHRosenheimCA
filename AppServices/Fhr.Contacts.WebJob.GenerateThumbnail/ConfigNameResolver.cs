//-------------------------------------------------------------------------------------
// Author:      amo
// Created:     8/9/2016 10:36:02 AM
// Copyright (c) white duck Gesellschaft für Softwareentwicklung mbH
//-------------------------------------------------------------------------------------

using System.Configuration;
using Microsoft.Azure.WebJobs;

namespace Fhr.Contacts.WebJob.GenerateThumbnail
{
    internal class ConfigNameResolver : INameResolver
    {        
        #region Implementation of INameResolver

        public string Resolve(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }

        #endregion
    }
}
