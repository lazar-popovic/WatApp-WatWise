import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { DatePipe } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AppComponent } from './app.component';
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
import { AuthInterceptor } from './services/auth.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { ProfileIDComponent } from './components/profile-id/profile-id.component';
import { MapComponentComponent } from './components/map-component/map-component.component';
import { LeafletModule} from "@asymmetrik/ngx-leaflet";
import { DeviceDetailsComponent } from './components/device-details/device-details.component';
import { NgChartsModule } from 'ng2-charts';
import { EmployeeOverviewComponent } from './components/employee-overview/employee-overview.component';
import { AddEmployeeComponent } from './components/add-employee/add-employee.component';
import { DevicesInfoComponent } from './components/devices-info/devices-info.component';
import { DeviceInputComponent } from './components/device-input/device-input.component';
import { ProfileSettingsComponent } from './components/profile-settings/profile-settings.component';
import { ProfileViewSettingsComponent } from './components/profile-view-settings/profile-view-settings.component';
import { PasswordViewSettingsComponent } from './components/password-view-settings/password-view-settings.component';
import { OverviewDsoComponent } from './components/overview-dso/overview-dso.component';
import { UsersOverviewComponent } from './components/users-overview/users-overview.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatSlider, MatSliderModule } from '@angular/material/slider';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { EditDeviceComponent } from './components/edit-device/edit-device.component';
import { SliderComponent } from './components/slider/slider.component';
import { DeleteDeviceComponent } from './components/delete-device/delete-device.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { NgBusyModule } from 'ng-busy';
import { WeatherForecastComponent } from './components/weather-forecast/weather-forecast.component';
import { TooltipComponent } from './components/tooltip/tooltip.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import { LogoHolderComponent } from './components/logo-holder/logo-holder.component';
import { DeviceInfoCardComponent } from './components/device-info-card/device-info-card.component';
import { ImageCropperModule } from 'ngx-image-cropper';
import { UploadImageComponent } from './components/upload-image/upload-image.component';
import { DeviceJobFormComponent } from './components/device-job-form/device-job-form.component';
import { DeviceScheduleCardComponent } from './components/device-schedule-card/device-schedule-card.component';
import { ConfirmWindowComponent } from './components/confirm-window/confirm-window.component';
import { SchedulerComponent } from './components/scheduler/scheduler.component';
import { SchedulerJobsListComponent } from './components/scheduler-jobs-list/scheduler-jobs-list.component';
import { IdDeleteDialogComponent } from './components/id-delete-dialog/id-delete-dialog.component';
import { DeleteProfileComponent } from './components/delete-profile/delete-profile.component';
import { ResendVerificationMailDialogComponent } from './components/resend-verification-mail-dialog/resend-verification-mail-dialog.component';
import { PrimengTableComponent } from './components/primeng-table/primeng-table.component';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { ScrollTopModule } from 'primeng/scrolltop';
import { TooltipModule } from 'primeng/tooltip';
import { PrimengValueTableComponent } from './components/primeng-value-table/primeng-value-table.component';
import { ToggleButtonModule } from 'primeng/togglebutton';
import { PrimengEmployeeTableComponent } from './components/primeng-employee-table/primeng-employee-table.component';
import { ProfileCardComponent } from './components/profile-card/profile-card.component';
import { ProfileMapComponent } from './components/profile-map/profile-map.component';
import { UpdateProsumerComponent } from './components/update-prosumer/update-prosumer.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SidebarComponent,
    ProfileComponent,
    ProfileIDComponent,
    ProfileCardComponent,
    ProfileMapComponent,
    UpdateProsumerComponent,
    DevicesComponent,
    ConsumptionComponent,
    OverviewComponent,
    LogoHolderComponent,
    UsersComponent,
    MapComponent,
    EnergyUsageComponent,
    ProductionComponent,
    VerifyComponent,
    ChangePasswordComponent,
    ForgotPasswordComponent,
    PasswordInputComponent,
    MapComponentComponent,
    UsersOverviewComponent,
    DeviceDetailsComponent,
    DeviceInfoCardComponent,
    EmployeeOverviewComponent,
    AddEmployeeComponent,
    DevicesInfoComponent,
    DeviceInputComponent,
    MapComponentComponent,
    DeviceDetailsComponent,
    ProfileSettingsComponent,
    ProfileViewSettingsComponent,
    PasswordViewSettingsComponent,
    OverviewDsoComponent,
    EditDeviceComponent,
    SliderComponent,
    DeleteDeviceComponent,
    WeatherForecastComponent,
    TooltipComponent,
    UploadImageComponent,
    DeviceJobFormComponent,
    DeviceScheduleCardComponent,
    SchedulerComponent,
    SchedulerJobsListComponent,
    ConfirmWindowComponent,
    IdDeleteDialogComponent,
    DeleteProfileComponent,
    ResendVerificationMailDialogComponent,
    DeleteProfileComponent,
    PrimengTableComponent,
    PrimengValueTableComponent,
    PrimengEmployeeTableComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    CommonModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    LeafletModule,
    NgChartsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatFormFieldModule,
    MatSelectModule,
    MatSliderModule,
    MatCheckboxModule,
    NgxChartsModule,
    NgBusyModule,
    MatTooltipModule,
    ImageCropperModule,
    TableModule,
    ButtonModule,
    ScrollTopModule,
    TooltipModule,
    ToggleButtonModule
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor,
    multi: true
  },
  DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
