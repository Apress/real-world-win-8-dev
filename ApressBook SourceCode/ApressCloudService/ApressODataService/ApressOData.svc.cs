//------------------------------------------------------------------------------
// <copyright file="WebDataService.svc.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;

namespace ApressODataService
{
    public class ApressOData : DataService<ApressBookDBEntities>
    {
        public static void InitializeService(DataServiceConfiguration config)
        {
            // Set rules to indicate which entity sets and service operations are visible, updatable, etc.
            config.SetEntitySetAccessRule("ApressBooks", EntitySetRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
        }
    }
}
