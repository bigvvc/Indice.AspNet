import { Injectable } from '@angular/core';

import { AsyncSubject, Observable, forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';
import {
    IdentityApiService, SingleClientInfo, IdentityResourceInfoResultSet, IdentityResourceInfo, ApiResourceInfo, CreateClaimRequest, ClaimInfo, UpdateClientRequest, IUpdateClientRequest,
    ScopeInfo, ScopeInfoResultSet, GrantTypeInfo, UpdateClientUrls, CreateSecretRequest, SecretInfo, ApiSecretInfo, ClientSecretInfo
} from 'src/app/core/services/identity-api.service';
import { UrlType } from './urls/models/urlType';

@Injectable()
export class ClientStore {
    private _client: AsyncSubject<SingleClientInfo>;
    private _identityResources: AsyncSubject<IdentityResourceInfo[]>;
    private _apiScopes: AsyncSubject<ScopeInfo[]>;

    constructor(private _api: IdentityApiService) { }

    public getClient(clientId: string): Observable<SingleClientInfo> {
        if (!this._client) {
            this._client = new AsyncSubject<SingleClientInfo>();
            this._api.getClient(clientId).subscribe((client: SingleClientInfo) => {
                this._client.next(client);
                this._client.complete();
            });
        }
        return this._client;
    }

    public updateClient(client: SingleClientInfo): Observable<void> {
        return this._api.updateClient(client.clientId, new UpdateClientRequest({
            accessTokenLifetime: client.accessTokenLifetime,
            absoluteRefreshTokenLifetime: client.absoluteRefreshTokenLifetime,
            accessTokenType: client.accessTokenType,
            allowAccessTokensViaBrowser: client.allowAccessTokensViaBrowser,
            allowPlainTextPkce: client.allowPlainTextPkce,
            refreshTokenUsage: client.refreshTokenUsage,
            refreshTokenExpiration: client.refreshTokenExpiration,
            allowOfflineAccess: client.allowOfflineAccess,
            updateAccessTokenClaimsOnRefresh: client.updateAccessTokenClaimsOnRefresh,
            allowRememberConsent: client.allowRememberConsent,
            alwaysIncludeUserClaimsInIdToken: client.alwaysIncludeUserClaimsInIdToken,
            alwaysSendClientClaims: client.alwaysSendClientClaims,
            authorizationCodeLifetime: client.authorizationCodeLifetime,
            clientClaimsPrefix: client.clientClaimsPrefix,
            clientName: client.clientName,
            clientUri: client.clientUri,
            consentLifetime: client.consentLifetime,
            description: client.description,
            frontChannelLogoutSessionRequired: client.frontChannelLogoutSessionRequired,
            frontChannelLogoutUri: client.frontChannelLogoutUri,
            identityTokenLifetime: client.identityTokenLifetime,
            includeJwtId: client.includeJwtId,
            logoUri: client.logoUri,
            pairWiseSubjectSalt: client.pairWiseSubjectSalt,
            requireConsent: client.requireConsent,
            requirePkce: client.requirePkce,
            userSsoLifetime: client.userSsoLifetime,
            backChannelLogoutUri: client.backChannelLogoutUri,
            backChannelLogoutSessionRequired: client.backChannelLogoutSessionRequired,
            deviceCodeLifetime: client.deviceCodeLifetime,
            userCodeType: client.userCodeType,
            enabled: client.enabled,
            slidingRefreshTokenLifetime: client.slidingRefreshTokenLifetime
        } as IUpdateClientRequest)).pipe(map(_ => {
            this._client.next(client);
            this._client.complete();
        }));
    }

    public updateClientUrl(clientId: string, url: string, added: boolean, urlType: UrlType): void {
        this.getClient(clientId).subscribe((client: SingleClientInfo) => {
            switch (urlType) {
                case UrlType.Redirect:
                    if (added) {
                        client.redirectUris.push(url);
                    } else {
                        const index = client.redirectUris.findIndex(x => x === url);
                        if (index > -1) {
                            client.redirectUris.splice(index, 1);
                        }
                    }
                    break;
                case UrlType.Cors:
                    if (added) {
                        client.allowedCorsOrigins.push(url);
                    } else {
                        const index = client.allowedCorsOrigins.findIndex(x => x === url);
                        if (index > -1) {
                            client.allowedCorsOrigins.splice(index, 1);
                        }
                    }
                    break;
                case UrlType.PostLogoutRedirect:
                    if (added) {
                        client.postLogoutRedirectUris.push(url);
                    } else {
                        const index = client.postLogoutRedirectUris.findIndex(x => x === url);
                        if (index > -1) {
                            client.postLogoutRedirectUris.splice(index, 1);
                        }
                    }
                    break;
                default:
                    break;
            }
            this._client.next(client);
            this._client.complete();
        });
    }

    public sendUpdateClientUrls(clientId: string, allowedCorsOrigins: string[], postLogoutRedirectUris: string[], redirectUris: string[]): Observable<void> {
        return this._api.updateClientUrls(clientId, {
            allowedCorsOrigins,
            postLogoutRedirectUris,
            redirectUris
        } as UpdateClientUrls);
    }

    public getIdentityResources(): Observable<IdentityResourceInfo[]> {
        if (!this._identityResources) {
            this._identityResources = new AsyncSubject<IdentityResourceInfo[]>();
            this._api.getIdentityResources(1, 2147483647, 'name+', undefined).subscribe((response: IdentityResourceInfoResultSet) => {
                this._identityResources.next(response.items);
                this._identityResources.complete();
            });
        }
        return this._identityResources;
    }

    public getApiScopes(): Observable<ScopeInfo[]> {
        if (!this._apiScopes) {
            this._apiScopes = new AsyncSubject<ApiResourceInfo[]>();
            this._api.getApiScopes(1, 2147483647, 'name+', undefined).subscribe((response: ScopeInfoResultSet) => {
                this._apiScopes.next(response.items);
                this._apiScopes.complete();
            });
        }
        return this._apiScopes;
    }

    public addClaim(clientId: string, claim: ClaimInfo): Observable<void> {
        return this._api.addClientClaim(clientId, {
            type: claim.type,
            value: claim.value
        } as CreateClaimRequest).pipe(map((createdClaim: ClaimInfo) => {
            this.getClient(clientId).subscribe((client: SingleClientInfo) => {
                client.claims.push(createdClaim);
                this._client.next(client);
                this._client.complete();
            });
        }));
    }

    public deleteClaim(clientId: string, claim: ClaimInfo): Observable<void> {
        this.getClient(clientId).subscribe((client: SingleClientInfo) => {
            const index = client.claims.findIndex(x => x.id === claim.id);
            if (index > -1) {
                client.claims.splice(index, 1);
            }
            this._client.next(client);
            this._client.complete();
        });
        return this._api.deleteClientClaim(clientId, claim.id);
    }

    public addApiResource(clientId: string, resource: ApiResourceInfo): Observable<void> {
        this.getClient(clientId).subscribe((client: SingleClientInfo) => {
            client.apiResources.push(resource.name);
            this._client.next(client);
            this._client.complete();
        });
        return this._api.addClientResources(clientId, [resource.name]);
    }

    public deleteApiResource(clientId: string, resource: ApiResourceInfo): Observable<void> {
        this.getClient(clientId).subscribe((client: SingleClientInfo) => {
            const index = client.apiResources.findIndex(x => x === resource.name);
            if (index > -1) {
                client.apiResources.splice(index, 1);
            }
            this._client.next(client);
            this._client.complete();
        });
        return this._api.deleteClientResource(clientId, [resource.name]);
    }

    public addIdentityResource(clientId: string, resource: IdentityResourceInfo): Observable<void> {
        this.getClient(clientId).subscribe((client: SingleClientInfo) => {
            client.identityResources.push(resource.name);
            this._client.next(client);
            this._client.complete();
        });
        return this._api.addClientResources(clientId, [resource.name]);
    }

    public deleteIdentityResource(clientId: string, resource: IdentityResourceInfo): Observable<void> {
        this.getClient(clientId).subscribe((client: SingleClientInfo) => {
            const index = client.identityResources.findIndex(x => x === resource.name);
            if (index > -1) {
                client.identityResources.splice(index, 1);
            }
            this._client.next(client);
            this._client.complete();
        });
        return this._api.deleteClientResource(clientId, [resource.name]);
    }

    public addGrantType(clientId: string, grantType: string): Observable<void> {
        return this._api.addClientGrantType(clientId, grantType).pipe(map((createdGrantType: GrantTypeInfo) => {
            this.getClient(clientId).subscribe((client: SingleClientInfo) => {
                client.grantTypes.push(createdGrantType.name);
                this._client.next(client);
                this._client.complete();
            });
        }));
    }

    public deleteGrantType(clientId: string, grantType: string): Observable<void> {
        this.getClient(clientId).subscribe((client: SingleClientInfo) => {
            const index = client.grantTypes.findIndex(x => x === grantType);
            if (index > -1) {
                client.grantTypes.splice(index, 1);
            }
            this._client.next(client);
            this._client.complete();
        });
        return this._api.deleteClientGrantType(clientId, grantType);
    }

    public addSecret(clientId: string, secret: CreateSecretRequest): Observable<void> {
        const getClient = this.getClient(clientId);
        const addSecret = this._api.addClientSecret(clientId, secret);
        return forkJoin([getClient, addSecret]).pipe(map((responses: [SingleClientInfo, SecretInfo]) => {
            return {
                client: responses[0],
                addedSecret: responses[1]
            };
        })).pipe(map((result: { client: SingleClientInfo, addedSecret: ClientSecretInfo }) => {
            (result.addedSecret as any).valueText = 'Value is hidden';
            result.client.secrets.push(result.addedSecret);
            this._client.next(result.client);
            this._client.complete();
        }));
    }

    public deleteClientSecret(clientId: string, secret: ClientSecretInfo): Observable<void> {
        this.getClient(clientId).subscribe((client: SingleClientInfo) => {
            const index = client.secrets.indexOf(secret);
            if (index > -1) {
                client.secrets.splice(index, 1);
            }
            this._client.next(client);
            this._client.complete();
        });
        return this._api.deleteClientSecret(clientId, secret.id);
    }

    public deleteClient(clientId: string): Observable<void> {
        return this._api.deleteClient(clientId);
    }
}
