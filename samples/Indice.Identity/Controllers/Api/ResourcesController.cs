﻿using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Interfaces;
using Indice.Identity.Models;
using Indice.Identity.Security;
using Indice.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Indice.Identity.Controllers.Api
{
    /// <summary>
    /// Contains operations for managing application's identity and API resources.
    /// </summary>
    [Route("api/resources")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize(Policy = IdentityServerApi.SubScopes.Users)]
    public sealed class ResourcesController : ControllerBase
    {
        private readonly IConfigurationDbContext _configurationDbContext;

        /// <summary>
        /// Creates an instance of <see cref="ResourcesController"/>.
        /// </summary>
        /// <param name="configurationDbContext">Abstraction for the configuration context.</param>
        public ResourcesController(IConfigurationDbContext configurationDbContext) {
            _configurationDbContext = configurationDbContext ?? throw new ArgumentNullException(nameof(configurationDbContext));
        }

        /// <summary>
        /// Returns a list of <see cref="IdentityResourceInfo"/> objects containing the total number of identity resources in the database and the data filtered according to the provided <see cref="ListOptions"/>.
        /// </summary>
        /// <param name="options">List params used to navigate through collections. Contains parameters such as sort, search, page number and page size.</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("identity")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(ResultSet<IdentityResourceInfo>))]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ValidationProblemDetails))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(ProblemDetails))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(ProblemDetails))]
        public async Task<ActionResult<ResultSet<IdentityResourceInfo>>> GetIdentityResources([FromQuery]ListOptions options) {
            var query = _configurationDbContext.IdentityResources.Include(x => x.UserClaims).AsNoTracking();
            if (!string.IsNullOrEmpty(options.Search)) {
                var searchTerm = options.Search.ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(searchTerm) || x.Description.Contains(searchTerm));
            }
            var identityResources = await query.Select(resource => new IdentityResourceInfo {
                Id = resource.Id,
                Name = resource.Name,
                DisplayName = resource.DisplayName,
                Description = resource.Description,
                Enabled = resource.Enabled,
                Required = resource.Required,
                Emphasize = resource.Emphasize,
                NonEditable = resource.NonEditable,
                ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument,
                AllowedClaims = resource.UserClaims.Select(claim => claim.Type)
            }).ToResultSetAsync(options);
            return Ok(identityResources);
        }

        /// <summary>
        /// Gets an identity resource by it's unique id.
        /// </summary>
        /// <param name="id">The identifier of the identity resource.</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IdentityResourceInfo))]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ValidationProblemDetails))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(ProblemDetails))]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound, type: typeof(ProblemDetails))]
        [HttpGet("identity/{id:int}")]
        public async Task<ActionResult<IdentityResourceInfo>> GetIdentityResource([FromRoute]int id) {
            var identityResource = await _configurationDbContext.IdentityResources.Include(x => x.UserClaims).AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
            if (identityResource == null) {
                return NotFound();
            }
            return Ok(new IdentityResourceInfo {
                Id = identityResource.Id,
                Name = identityResource.Name,
                DisplayName = identityResource.DisplayName,
                Description = identityResource.Description,
                Enabled = identityResource.Enabled,
                Required = identityResource.Required,
                Emphasize = identityResource.Emphasize,
                NonEditable = identityResource.NonEditable,
                ShowInDiscoveryDocument = identityResource.ShowInDiscoveryDocument,
                AllowedClaims = identityResource.UserClaims.Select(claim => claim.Type)
            });
        }

        /// <summary>
        /// Permanently deletes an identity resource.
        /// </summary>
        /// <param name="id">The id of the identity resource to delete.</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("identity/{id:int}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ValidationProblemDetails))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(ProblemDetails))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(ProblemDetails))]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound, type: typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteIdentityResource([FromRoute]int id) {
            var resource = await _configurationDbContext.IdentityResources.SingleOrDefaultAsync(x => x.Id == id);
            if (resource == null) {
                return NotFound();
            }
            _configurationDbContext.IdentityResources.Remove(resource);
            await _configurationDbContext.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Returns a list of <see cref="ApiResourceInfo"/> objects containing the total number of API resources in the database and the data filtered according to the provided <see cref="ListOptions"/>.
        /// </summary>
        /// <param name="options">List params used to navigate through collections. Contains parameters such as sort, search, page number and page size.</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("protected")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(ResultSet<ApiResourceInfo>))]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ValidationProblemDetails))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(ProblemDetails))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(ProblemDetails))]
        public async Task<ActionResult<ResultSet<ApiResourceInfo>>> GetProtectedResources([FromQuery]ListOptions options) {
            var query = _configurationDbContext.ApiResources.Include(x => x.UserClaims).Include(x => x.Scopes).ThenInclude(x => x.UserClaims).AsNoTracking();
            if (!string.IsNullOrEmpty(options.Search)) {
                var searchTerm = options.Search.ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(searchTerm) || x.Description.ToLower().Contains(searchTerm));
            }
            var apiResources = await query.Select(resource => new ApiResourceInfo {
                Id = resource.Id,
                Name = resource.Name,
                DisplayName = resource.DisplayName,
                Description = resource.Description,
                Enabled = resource.Enabled,
                NonEditable = resource.NonEditable,
                AllowedClaims = resource.UserClaims.Select(e => e.Type),
                Scopes = resource.Scopes.Select(scope => new ScopeInfo {
                    Id = scope.Id,
                    Name = scope.Name,
                    DisplayName = scope.DisplayName,
                    Description = scope.Description,
                    Required = scope.Required,
                    Emphasize = scope.Emphasize,
                    ShowInDiscoveryDocument = scope.ShowInDiscoveryDocument,
                    UserClaims = scope.UserClaims.Select(scopeClaim => scopeClaim.Type)
                })
            })
            .ToResultSetAsync(options);
            return Ok(apiResources);
        }
    }
}