import { Injectable } from '@angular/core';

import { AsyncSubject, Observable } from 'rxjs';
import { IdentityApiService, SingleClientInfo, IdentityResourceInfoResultSet, IdentityResourceInfo, ApiResourceInfo, ApiResourceInfoResultSet } from 'src/app/core/services/identity-api.service';
import { UrlType } from './urls/models/urlType';

@Injectable()
export class ClientStore {
    private _client: AsyncSubject<SingleClientInfo>;
    private _identityResources: AsyncSubject<IdentityResourceInfo[]>;
    private _apiResources: AsyncSubject<IdentityResourceInfo[]>;

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

    public getApiResources(): Observable<ApiResourceInfo[]> {
        if (!this._apiResources) {
            this._apiResources = new AsyncSubject<ApiResourceInfo[]>();
            this._api.getProtectedResources(1, 2147483647, 'name+', undefined).subscribe((response: ApiResourceInfoResultSet) => {
                this._apiResources.next(response.items);
                this._apiResources.complete();
            });
        }
        return this._apiResources;
    }
}