import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { PlayerComponent } from './player/player.component';
import { ShowPlayerComponent } from './player/show-player/show-player.component';
import { AddEditPlayerComponent } from './player/add-edit-player/add-edit-player.component';

import { PlayersApiService } from './players-api.service';

@NgModule({
  declarations: [
    AppComponent,
    PlayerComponent,
    ShowPlayerComponent,
    AddEditPlayerComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [PlayersApiService],
  bootstrap: [AppComponent]
})
export class AppModule { }
