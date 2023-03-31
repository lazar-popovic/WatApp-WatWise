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
import { UsersOverviewComponent } from './components/users-overview/users-overview.component';
import { DeviceDetailsComponent } from './components/device-details/device-details.component';
import { NgChartsModule } from 'ng2-charts';
import { EmployeeOverviewComponent } from './components/employee-overview/employee-overview.component';
import { AddEmployeeComponent } from './components/add-employee/add-employee.component';
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SidebarComponent,
    ProfileComponent,
    ProfileIDComponent,
    DevicesComponent,
    ConsumptionComponent,
    OverviewComponent,
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
    EmployeeOverviewComponent,
    AddEmployeeComponent,
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
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor,
    multi: true
  },
  DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
