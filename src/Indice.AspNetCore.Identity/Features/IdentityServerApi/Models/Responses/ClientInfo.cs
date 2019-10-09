﻿namespace Indice.AspNetCore.Identity.Features
{
    /// <summary>
    /// Models a system client.
    /// </summary>
    public class ClientInfo
    {
        /// <summary>
        /// The unique identifier for this application.
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// Application name that will be seen on consent screens.
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// Application description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Determines whether this application is enabled or not.
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Specifies whether a consent screen is required.
        /// </summary>
        public bool RequireConsent { get; set; }
        /// <summary>
        /// Specifies whether consent screen is remembered after having been given.
        /// </summary>
        public bool AllowRememberConsent { get; set; }
        /// <summary>
        /// Application logo that will be seen on consent screens.
        /// </summary>
        public string LogoUri { get; set; }
        /// <summary>
        /// Application URL that will be seen on consent screens.
        /// </summary>
        public string ClientUri { get; set; }
    }

    /// <summary>
    /// Models a system client when API provides info for a single client.
    /// </summary>
    public class SingleClientInfo : ClientInfo
    {

    }
}
