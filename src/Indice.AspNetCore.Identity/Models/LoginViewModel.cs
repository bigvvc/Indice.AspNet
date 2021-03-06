// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;

namespace Indice.AspNetCore.Identity.Models
{
    /// <summary>
    /// Login view model
    /// </summary>
    public class LoginViewModel : LoginInputModel
    {
        /// <summary>
        /// Allow remember login
        /// </summary>
        public bool AllowRememberLogin { get; set; } = true;
        /// <summary>
        /// Enables local logins (if false onlu external provider list will be available)
        /// </summary>
        public bool EnableLocalLogin { get; set; } = true;
        /// <summary>
        /// List of external providers
        /// </summary>
        public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();
        /// <summary>
        /// The visible external providers are those form the <see cref="ExternalProviders"/> list that have a display name
        /// </summary>
        public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !String.IsNullOrWhiteSpace(x.DisplayName));
        /// <summary>
        /// Use this flag to hide the local login form
        /// </summary>
        public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
        /// <summary>
        /// The scheme to use for external login cookie.
        /// </summary>
        public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;
    }
}