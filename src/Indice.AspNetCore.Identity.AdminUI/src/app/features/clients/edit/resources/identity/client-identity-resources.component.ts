import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { forkJoin, Subscription } from 'rxjs';
import { map } from 'rxjs/operators';
import { SingleClientInfo, IdentityResourceInfo } from 'src/app/core/services/identity-api.service';
import { ClientStore } from '../../client-store.service';

@Component({
    selector: 'app-client-identity-resources',
    templateUrl: './client-identity-resources.component.html'
})
export class ClientIdentityResourcesComponent implements OnInit, OnDestroy {
    private _getDataSubscription: Subscription;

    constructor(private _route: ActivatedRoute, private _clientStore: ClientStore) { }

    public clientId = '';
    public availableResources: IdentityResourceInfo[];
    public clientResources: IdentityResourceInfo[];

    public ngOnInit(): void {
        this.clientId = this._route.parent.parent.snapshot.params.id;
        const getClient = this._clientStore.getClient(this.clientId);
        const getIdentityResources = this._clientStore.getIdentityResources();
        this._getDataSubscription = forkJoin([getClient, getIdentityResources]).pipe(map((responses: [SingleClientInfo, IdentityResourceInfo[]]) => {
            return {
                client: responses[0],
                identityResources: responses[1]
            };
        })).subscribe((result: { client: SingleClientInfo, identityResources: IdentityResourceInfo[] }) => {
            const clientIdentityResources = result.client.identityResources;
            const allIdentityResources = result.identityResources;
            this.availableResources = allIdentityResources.filter(x => !clientIdentityResources.includes(x.name));
            this.clientResources = allIdentityResources.filter(x => clientIdentityResources.includes(x.name));
        });
    }

    public ngOnDestroy(): void {
        if (this._getDataSubscription) {
            this._getDataSubscription.unsubscribe();
        }
    }

    public addResource(resource: IdentityResourceInfo): void { }

    public removeResource(resource: IdentityResourceInfo): void { }
}