import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { HangingManComponent } from './hanging-man/hanging-man.component';
import { WordDisplayComponent } from './word-display/word-display.component';
import { DataServiceService } from './data-service.service';
import { HttpClientModule } from '@angular/common/http';
import { HangmanInterfaceComponent } from './hangman-interface/hangman-interface.component';
import { StatsComponent } from './stats/stats.component';

const routes: Routes = [
  { path: 'play', component: HangmanInterfaceComponent },
  { path: '', redirectTo: 'play', pathMatch: 'full'}
]

@NgModule({
  declarations: [
    AppComponent,
    HangingManComponent,
    WordDisplayComponent,
    HangmanInterfaceComponent,
    StatsComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(routes),
    HttpClientModule,
    FormsModule
  ],
  providers: [DataServiceService],
  bootstrap: [AppComponent]
})
export class AppModule { }
