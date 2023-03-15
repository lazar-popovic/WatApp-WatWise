import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { DatePipe } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';
import { LoginComponent } from './login/login.component';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './register/register.component';
import { ProfileComponent } from './profile/profile.component';  
import { HttpClientModule } from '@angular/common/http';
import { FormsModule} from '@angular/forms';
import { LoginDSOComponent } from './login-dso/login-dso.component';
import { RegisterDSOComponent } from './register-dso/register-dso.component';
import { DevicesComponent } from './devices/devices.component';
import { ConsumptionComponent } from './consumption/consumption.component';
import { OverviewComponent } from './overview/overview.component';  
import { CommonModule } from '@angular/common';
import { SidebarDsoComponent } from './sidebar-dso/sidebar-dso.component';
import { UsersComponent } from './users/users.component';
import { MapComponent } from './map/map.component';
import { EnergyUsageComponent } from './energy-usage/energy-usage.component';
import { ProductionComponent } from './production/production.component';
import { VerifyComponent } from './verify/verify.component';
import { VerifyDsoComponent } from './verify-dso/verify-dso.component';

const routes: Routes = [
  { path: '', redirectTo: 'pronsumer/login', pathMatch: 'full' },
  { path: 'pronsumer/login', component: LoginComponent },
  { path: 'pronsumer/register', component: RegisterComponent },
  { path: 'dso/login', component: LoginDSOComponent },
  { path: 'dso/register', component: RegisterDSOComponent },
  
  { path: 'dso/verification', component : VerifyDsoComponent},
  { path: 'prosumer/verification', component : VerifyComponent},

  { path: 'overview',  component:  OverviewComponent },
  { path: 'profile',  component:  ProfileComponent },
  
  { path: 'prosumer/devices', component : DevicesComponent},
  { path: 'prosumer/consumption', component : ConsumptionComponent},
  { path: 'prosumer/production', component : ProductionComponent},

  { path: 'dso/users', component : UsersComponent},
  { path: 'dso/map', component : MapComponent},
  { path: 'dso/energy-usage', component : EnergyUsageComponent}

];

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    LoginDSOComponent,
    RegisterComponent,
    SidebarComponent,
    ProfileComponent,
    RegisterDSOComponent,
    DevicesComponent,
    ConsumptionComponent,
    OverviewComponent,
    SidebarDsoComponent,
    UsersComponent,
    MapComponent,
    EnergyUsageComponent,
    ProductionComponent,
    VerifyComponent,
    VerifyDsoComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    RouterModule.forRoot(routes),
    HttpClientModule,
    FormsModule,
    CommonModule
  ],
  providers: [DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
