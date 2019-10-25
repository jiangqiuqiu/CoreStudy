﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIDCServer.ViewModels
{
    public class ConsentViewModel
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientLogoUrl { get; set; }
        public bool AllowRemeberConsent { get; set; }
        public IEnumerable<ScopeViewModel> IdentityScopes { get; set; }
        public IEnumerable<ScopeViewModel> ResourceScopes { get; set; }
    }
}
