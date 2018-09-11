import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';


import { GameRecord } from './gameRecord.model';
import { GameWins } from './gameWins.model';
import { GameLosses } from './gameLosses.model';

@Injectable({
  providedIn: 'root'
})
export class DataServiceService {

  uri = 'http://localhost:3000';
  tmp = '';

  constructor(private http: HttpClient) { }
  
  // Method to begin a new game
  beginGame() {
    //return this.http.post(`${this.uri}/api/startGame/`, this.tmp);
    return this.http.get(`${this.uri}/api/startGame/`);
  }

  // Return an observable GameRecord
  getGameRecord(): Observable<GameRecord> {
    return this.http.get<GameRecord>(`${this.uri}/api/gameRecord/`); 
  }

  // Method to get a game record by its id
  getGameById(gameId): Observable<GameRecord> {
    return this.http.get<GameRecord>(`${this.uri}/api/gameRecord/${gameId}`);
  }

  // Method to guess a letter
  guessLetter(gameId, letter) {
    return this.http.get(`${this.uri}/api/guessLetter/${gameId}/${letter}`);
  } 

  // Get wins from the database
  getWins(): Observable<GameWins> {
    return this.http.get<GameWins>(`${this.uri}/api/wins/`)
  }

  // Get losses from the database
  getLosses(): Observable<GameLosses> {
    return this.http.get<GameLosses>(`${this.uri}/api/losses/`)
  }
}
