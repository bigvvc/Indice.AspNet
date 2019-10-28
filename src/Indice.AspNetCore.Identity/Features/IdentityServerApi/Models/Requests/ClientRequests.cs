﻿using System.Collections.Generic;
using IdentityServer4.Models;

namespace Indice.AspNetCore.Identity.Features
{
    /// <summary>
    /// Models a client that will be created on the server.
    /// </summary>
    public class CreateClientRequest : BaseClientRequest
    {
        /// <summary>
        /// Describes the type of the client.
        /// </summary>
        public ClientType ClientType { get; set; }
        /// <summary>
        /// The unique identifier for this application.
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// Allowed URL to return after logging in.
        /// </summary>
        public string RedirectUri { get; set; }
        /// <summary>
        /// Allowed URL to return after logout.
        /// </summary>
        public string PostLogoutRedirectUri { get; set; }
        /// <summary>
        /// The client secrets.
        /// </summary>
        public List<ClientSecretRequest> Secrets { get; set; } = new List<ClientSecretRequest>();
        /// <summary>
        /// The list of identity resources allowed by the client.
        /// </summary>
        public IEnumerable<string> IdentityResources { get; set; } = new List<string>();
        /// <summary>
        /// The list of API resources allowed by the client.
        /// </summary>
        public IEnumerable<string> ApiResources { get; set; } = new List<string>();
    }

    /// <summary>
    /// Models a client that will be updated on the server.
    /// </summary>
    public class UpdateClientRequest : BaseClientRequest
    {
        /// <summary>
        /// Lifetime of identity token in seconds.
        /// </summary>
        public int IdentityTokenLifetime { get; set; }
        /// <summary>
        /// Lifetime of access token in seconds
        /// </summary>
        public int AccessTokenLifetime { get; set; }
        /// <summary>
        /// Lifetime of a user consent in seconds.
        /// </summary>
        public int ConsentLifetime { get; set; }
        /// <summary>
        /// The maximum duration (in seconds) since the last time the user authenticated.
        /// </summary>
        public int UserSsoLifetime { get; set; }
        /// <summary>
        /// Specifies logout URI at client for HTTP front-channel based logout.
        /// </summary>
        public string FrontChannelLogoutUri { get; set; }
        /// <summary>
        /// Gets or sets a salt value used in pair-wise subjectId generation for users of this client.
        /// </summary>
        public string PairWiseSubjectSalt { get; set; }
        /// <summary>
        /// Specifies whether the access token is a reference token or a self contained JWT token.
        /// </summary>
        public AccessTokenType? AccessTokenType { get; set; }
        /// <summary>
        /// Specifies is the user's session id should be sent to the FrontChannelLogoutUri.
        /// </summary>
        public bool FrontChannelLogoutSessionRequired { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether JWT access tokens should include an identifier.
        /// </summary>
        public bool IncludeJwtId { get; set; }
        /// <summary>
        /// Controls whether access tokens are transmitted via the browser for this client. This can prevent accidental leakage of access tokens when multiple response types are allowed.
        /// </summary>
        public bool AllowAccessTokensViaBrowser { get; set; }
        /// <summary>
        /// When requesting both an id token and access token, should the user claims always be added to the id token instead of requring the client to use the userinfo endpoint.
        /// </summary>
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether client claims should be always included in the access tokens - or only for client credentials flow.
        /// </summary>
        public bool AlwaysSendClientClaims { get; set; }
        /// <summary>
        /// Lifetime of authorization code in seconds.
        /// </summary>
        public int AuthorizationCodeLifetime { get; set; }
        /// <summary>
        /// Specifies whether a proof key is required for authorization code based token requests.
        /// </summary>
        public bool RequirePkce { get; set; }
        /// <summary>
        /// Specifies whether a proof key can be sent using plain method.
        /// </summary>
        public bool AllowPlainTextPkce { get; set; }
        /// <summary>
        /// Gets or sets a value to prefix it on client claim types.
        /// </summary>
        public string ClientClaimsPrefix { get; set; }
        /// <summary>
        /// Specifies whether consent screen is remembered after having been given.
        /// </summary>
        public bool AllowRememberConsent { get; set; }
    }

    /// <summary>
    /// Models a client request.
    /// </summary>
    public class BaseClientRequest
    {
        /// <summary>
        /// Application name that will be seen on consent screens.
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// Application URL that will be seen on consent screens.
        /// </summary>
        public string ClientUri { get; set; }
        /// <summary>
        /// Application logo that will be seen on consent screens.
        /// </summary>
        public string LogoUri { get; set; }
        /// <summary>
        /// Application description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Specifies whether a consent screen is required.
        /// </summary>
        public bool RequireConsent { get; set; }
    }

    /// <summary>
    /// Models an OAuth client type.
    /// </summary>
    public enum ClientType
    {
        /// <summary>
        /// Single page application supporting authorization code.
        /// </summary>
        SPA,
        /// <summary>
        /// Classic web application.
        /// </summary>
        WebApp,
        /// <summary>
        /// A desktop or mobile application running on a user's device.
        /// </summary>
        Native,
        /// <summary>
        /// A server to server application.
        /// </summary>
        Machine,
        /// <summary>
        /// IoT application or otherwise browserless or input constrained device.
        /// </summary>
        Device,
        /// <summary>
        /// Single page application supporting implicit flow.
        /// </summary>
        SPALegacy
    }
}
