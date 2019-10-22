import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { SharedModule } from 'src/app/shared/shared.module';
import { ResourcesRoutingModule } from './resources-routing.module';
import { IdentityResourcesComponent } from './identity/identity-resources.component';
import { IdentityResourceEditComponent } from './identity/edit/identity-resource-edit.component';
import { IdentityResourceDetailsComponent } from './identity/edit/details/identity-resource-details.component';
import { IdentityResourceClaimsComponent } from './identity/edit/claims/identity-resource-claims.component';
import { IdentityResourceAddComponent } from './identity/add/identity-resource-add.component';
import { ApiResourcesComponent } from './api/api-resources.component';
import { ApiResourceEditComponent } from './api/edit/api-resource-edit.component';
import { ApiResourceDetailsComponent } from './api/edit/details/api-resource-details.component';
import { ApiResourceScopesComponent } from './api/edit/scopes/api-resource-scopes.component';
import { ApiResourceScopeDetailsComponent } from './api/edit/scopes/details/api-resource-scope-details.component';
import { ApiResourceScopeClaimsComponent } from './api/edit/scopes/claims/api-resource-scope-claims.component';
import { ApiResourceAddComponent } from './api/add/api-resource-add.component';
import { BasicInfoStepComponent } from './api/add/wizard/steps/basic-info/basic-info-step.component';
import { UserClaimsStepComponent } from './api/add/wizard/steps/user-claims/user-claims-step.component';
import { IdentityResourcesStepComponent } from '../clients/add/wizard/steps/identity-resources/identity-resources-step.component';

@NgModule({
    declarations: [
        IdentityResourcesComponent,
        IdentityResourceEditComponent,
        IdentityResourceDetailsComponent,
        IdentityResourceClaimsComponent,
        IdentityResourceAddComponent,
        ApiResourcesComponent,
        ApiResourceEditComponent,
        ApiResourceDetailsComponent,
        ApiResourceScopesComponent,
        ApiResourceScopeDetailsComponent,
        ApiResourceScopeClaimsComponent,
        ApiResourceAddComponent,
        BasicInfoStepComponent,
        UserClaimsStepComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        ResourcesRoutingModule,
        SharedModule,
        SweetAlert2Module
    ],
    entryComponents: [
        BasicInfoStepComponent,
        UserClaimsStepComponent
    ]
})
export class ResourcesModule { }
