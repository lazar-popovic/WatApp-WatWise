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

const routes: Routes = [
  { path: '', redirectTo: 'pronsumer/login', pathMatch: 'full' },
  { path: 'pronsumer/login', component: LoginComponent },
  { path: 'pronsumer/register', component: RegisterComponent },
  { path: 'dso/login', component: LoginDSOComponent },
  { path: 'dso/register', component: RegisterDSOComponent },
  { path: 'overview',  component:  OverviewComponent },
  { path: 'pronsumer/profile',  component:  ProfileComponent },
  { path: 'pronsumer/devices', component : DevicesComponent},
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
