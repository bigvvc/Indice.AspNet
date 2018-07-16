﻿using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Options;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Indice.AspNetCore.Identity.Models;
using Indice.AspNetCore.Identity.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Indice.AspNetCore.Identity.Extensions
{

    /// <summary>
    /// Extension methods to configure the IdentityServer.
    /// </summary>
    public static class IdentityServerBuilderExtensions
    {
        /// <summary>
        /// Setup Users store.
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddIdentityUsersStore<TUser>(this IIdentityServerBuilder builder, IConfiguration configuration) where TUser : class, new() {
            builder.Services.AddTransient<IProfileService, ProfileService<User>>();
            builder.Services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator<User>>();

            return builder;
        }

        /// <summary>
        /// Setup Configuration store.
        /// </summary>
        /// <param name="options"></param>
        public static void SetupTables(this ConfigurationStoreOptions options) {
            options.DefaultSchema = "config";
            options.ClientClaim = new TableConfiguration(nameof(ClientClaim));
            options.ClientSecret = new TableConfiguration(nameof(ClientSecret));
            options.ClientScopes = new TableConfiguration(nameof(ClientScope));
            options.ClientPostLogoutRedirectUri = new TableConfiguration(nameof(ClientPostLogoutRedirectUri));
            options.ClientRedirectUri = new TableConfiguration(nameof(ClientRedirectUri));
            options.ClientGrantType = new TableConfiguration(nameof(ClientGrantType));
            options.Client = new TableConfiguration(nameof(Client));
            options.ApiScopeClaim = new TableConfiguration(nameof(ApiScopeClaim));
            options.ApiClaim = new TableConfiguration(nameof(options.ApiClaim));
            options.ApiScope = new TableConfiguration(nameof(ApiScope));
            options.ApiSecret = new TableConfiguration(nameof(ApiSecret));
            options.ApiResource = new TableConfiguration(nameof(ApiResource));
            options.IdentityClaim = new TableConfiguration(nameof(IdentityClaim));
            options.IdentityResource = new TableConfiguration(nameof(IdentityResource));
            options.ClientIdPRestriction = new TableConfiguration(nameof(ClientIdPRestriction));
            options.ClientCorsOrigin = new TableConfiguration(nameof(ClientCorsOrigin));
        }

        /// <summary>
        /// Setup Operational store.
        /// </summary>
        /// <param name="options"></param>
        public static void SetupTables(this OperationalStoreOptions options) {
            options.DefaultSchema = "auth";
            options.PersistedGrants = new TableConfiguration(nameof(PersistedGrant));
        }
    }
}
