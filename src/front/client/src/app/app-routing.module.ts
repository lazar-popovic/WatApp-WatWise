import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { ProfileComponent } from './components/profile/profile.component';  
import { LoginDSOComponent } from './components/login-dso/login-dso.component';
import { RegisterDSOComponent } from './components/register-dso/register-dso.component';
import { DevicesComponent } from './components/devices/devices.component';
import { ConsumptionComponent } from './components/consumption/consumption.component';
import { OverviewComponent } from './components/overview/overview.component';  
import { SidebarDsoComponent } from './components/sidebar-dso/sidebar-dso.component';
import { UsersComponent } from './components/users/users.component';
import { MapComponent } from './components/map/map.component';
import { EnergyUsageComponent } from './components/energy-usage/energy-usage.component';
import { ProductionComponent } from './components/production/production.component';
import { VerifyComponent } from './components/verify/verify.component';
import { VerifyDsoComponent } from './components/verify-dso/verify-dso.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { PasswordInputComponent } from './components/password-input/password-input.component';

const routes: Routes = [
  { path: '', redirectTo: 'pronsumer/login', pathMatch: 'full' },
  { path: 'pronsumer/login', component: LoginComponent },
  { path: 'pronsumer/register', component: RegisterComponent },
  { path: 'dso/login', component: LoginDSOComponent },
  { path: 'dso/register', component: RegisterDSOComponent },
  { path: 'dso/verification', component : VerifyDsoComponent},
  { path: 'prosumer/verification', component : VerifyComponent},
  { path: 'prosumer/overview',  component:  OverviewComponent },
  { path: 'dso/overview',  component:  OverviewComponent },
  { path: 'profile',  component:  ProfileComponent },
  { path: 'test/forgot', component: ForgotPasswordComponent},
  { path: 'prosumer/devices', component : DevicesComponent},
  { path: 'prosumer/consumption', component : ConsumptionComponent},
  { path: 'prosumer/production', component : ProductionComponent},
  { path: 'test/component', component : ChangePasswordComponent},
  { path: 'dso/users', component : UsersComponent},
  { path: 'dso/map', component : MapComponent},
  { path: 'dso/consumption', component : EnergyUsageComponent},
  { path: 'prosumer/reset-password', component: PasswordInputComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
