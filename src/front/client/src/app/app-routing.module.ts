import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { LoginComponent } from './components/login/login.component';
import { ProfileComponent } from './components/profile/profile.component';
import { DevicesComponent } from './components/devices/devices.component';
import { ConsumptionComponent } from './components/consumption/consumption.component';
import { OverviewComponent } from './components/overview/overview.component';
import { UsersComponent } from './components/users/users.component';
import { MapComponent } from './components/map/map.component';
import { EnergyUsageComponent } from './components/energy-usage/energy-usage.component';
import { ProductionComponent } from './components/production/production.component';
import { VerifyComponent } from './components/verify/verify.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { PasswordInputComponent } from './components/password-input/password-input.component';
import { UserGuard } from './guards/user.guard';
import { EmployeeGuard } from './guards/employee.guard';
import { ProfileIDComponent } from './components/profile-id/profile-id.component';
import { LoggedGuard } from './guards/logged.guard';
import { UsersOverviewComponent } from './components/users-overview/users-overview.component';
import { DeviceDetailsComponent } from './components/device-details/device-details.component';
import { DevicesInfoComponent } from './components/devices-info/devices-info.component';
import { EmployeeOverviewComponent } from './components/employee-overview/employee-overview.component';
import { DeviceInputComponent } from './components/device-input/device-input.component';
import { OverviewDsoComponent } from './components/overview-dso/overview-dso.component';
import { ProfileSettingsComponent } from './components/profile-settings/profile-settings.component';
import { EditDeviceComponent } from './components/edit-device/edit-device.component';
import { SliderComponent } from './components/slider/slider.component';
import { DeleteDeviceComponent } from './components/delete-device/delete-device.component';

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent},
  { path: 'verification', component : VerifyComponent},
  { path: 'prosumer/overview',  component:  OverviewComponent, canActivate: [UserGuard]},
  { path: 'dso/overview',  component:  OverviewDsoComponent, canActivate: [EmployeeGuard]},
  { path: 'profile',  component:  ProfileComponent, canActivate: [UserGuard]},
  { path: 'profile/:id', component: ProfileIDComponent, canActivate: [EmployeeGuard]},
  { path: 'forgot-password', component: ForgotPasswordComponent},
  { path: 'prosumer/devices', component : DevicesComponent, canActivate: [UserGuard]},
  { path: 'prosumer/consumption', component : ConsumptionComponent, canActivate: [UserGuard]},
  { path: 'prosumer/production', component : ProductionComponent, canActivate: [UserGuard]},
  { path: 'profile/change-password', component : ChangePasswordComponent, canActivate: [UserGuard]},
  { path: 'dso/users', component : UsersOverviewComponent, canActivate: [EmployeeGuard]},
  { path: 'dso/map', component : MapComponent, canActivate: [EmployeeGuard]},
  { path: 'dso/consumption', component : EnergyUsageComponent, canActivate: [EmployeeGuard]},
  { path: 'prosumer/reset-password', component: PasswordInputComponent},
  { path: 'prosumer/device/:id', component: DeviceDetailsComponent},
  { path: 'profile-settings', component: ProfileSettingsComponent},
  { path: 'test-component', component: DeleteDeviceComponent},
  { path: 'test-geo', component: UsersComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
