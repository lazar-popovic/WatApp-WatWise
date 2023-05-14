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
import { UploadImageComponent } from './components/upload-image/upload-image.component';
import { DeviceJobFormComponent } from './components/device-job-form/device-job-form.component';
import { ConfirmWindowComponent } from './components/confirm-window/confirm-window.component';
import { SchedulerComponent } from './components/scheduler/scheduler.component';
import { NotLoggedGuard } from './guards/not-logged.guard';
import { OnlyUserGuardGuard } from './guards/only-user-guard.guard';

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent},
  { path: 'verification', component : VerifyComponent, canActivate: [NotLoggedGuard]},
  { path: 'dso/overview',  component:  OverviewDsoComponent, canActivate: [EmployeeGuard]},
  { path: 'profile',  component:  ProfileComponent, canActivate: [UserGuard]},
  { path: 'profile/:id', component: ProfileIDComponent, canActivate: [EmployeeGuard]},
  { path: 'forgot-password', component: ForgotPasswordComponent},
  { path: 'prosumer/devices', component : DevicesComponent, canActivate: [UserGuard]},
  { path: 'prosumer/overview',  component:  OverviewComponent, canActivate: [OnlyUserGuardGuard]},
  { path: 'prosumer/consumption', component : ConsumptionComponent, canActivate: [OnlyUserGuardGuard]},
  { path: 'prosumer/production', component : ProductionComponent, canActivate: [OnlyUserGuardGuard]},
  { path: 'prosumer/scheduler', component: SchedulerComponent, canActivate: [OnlyUserGuardGuard]},
  { path: 'profile/change-password', component : ChangePasswordComponent, canActivate: [UserGuard]},
  { path: 'dso/prosumers', component : UsersOverviewComponent, canActivate: [EmployeeGuard]},
  { path: 'dso/map', component : MapComponent, canActivate: [EmployeeGuard]},
  { path: 'dso/energy-usage', component : EnergyUsageComponent, canActivate: [EmployeeGuard]},
  { path: 'prosumer/reset-password', component: PasswordInputComponent, canActivate: [NotLoggedGuard]},
  { path: 'prosumer/device/:id', component: DeviceDetailsComponent, canActivate: [UserGuard]},
  { path: 'dso/employees', component: EmployeeOverviewComponent, canActivate: [UserGuard]},
  { path: 'profile-settings', component: ProfileSettingsComponent, canActivate: [UserGuard]},
  { path: 'test-component', component: DeleteDeviceComponent},
  { path: 'test-geo', component: UsersComponent},
  { path: 'test-upload', component: UploadImageComponent},
  { path: 'test-confirm', component: ConfirmWindowComponent},
  { path: 'test-users', component: UsersComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
