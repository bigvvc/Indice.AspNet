﻿using Indice.AspNetCore.Features;
using Indice.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Adds feature extensions to the <see cref="IMvcBuilder"/>.
    /// </summary>
    public static class AvatarFeatureExtensions
    {
        /// <summary>
        /// Add the Avatar feature to MVC.
        /// </summary>
        /// <param name="mvcBuilder">An interface for configuring MVC services.</param>
        public static IMvcBuilder AddAvatars(this IMvcBuilder mvcBuilder) {
            mvcBuilder.ConfigureApplicationPartManager(apm => apm.FeatureProviders.Add(new AvatarFeatureProvider()));
            mvcBuilder.Services.AddResponseCaching();
            mvcBuilder.Services.AddSingleton(sp => new AvatarGenerator());
            return mvcBuilder;
        }
    }
}
