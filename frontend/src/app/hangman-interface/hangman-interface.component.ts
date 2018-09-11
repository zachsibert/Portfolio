import { Component, OnInit, Output, Injectable } from '@angular/core';
import { DataServiceService } from '../data-service.service';
import { GameRecord } from '../gameRecord.model';
import { WordDisplayComponent } from '../word-display/word-display.component';
import { GameWins } from '../gameWins.model';
import { GameLosses } from '../gameLosses.model';

@Component({
  selector: 'app-hangman-interface',
  templateUrl: './hangman-interface.component.html',
  styleUrls: ['./hangman-interface.component.css']
})

export class HangmanInterfaceComponent implements OnInit {

  @Output() 
  public gameRecord: GameRecord;
  public gameWins: GameWins;
  public gameLosses: GameLosses;

  constructor(private dataService: DataServiceService) { }

  ngOnInit() {
    this.getGameRecord();
    this.getWins();
    this.getLosses();
  }

  // Begin a new game of hangman, create a new record in the database
  startGame(): void {
    this.dataService.beginGame().subscribe((gameRecord) => {
      console.log("A new game has started");
      this.getGameRecord();
    });
  }

  // Retrieve the most recently made hangman game in the database => the one the user just created
  getGameRecord(): void {
    this.dataService.getGameRecord().subscribe(data => this.gameRecord = {
      id: data[0][0]['idtblGames'],
      word: data[0][0]['word'],
      wordUniqueLetters: data[0][0]['wordUniqueLetters'],
      correctLettersGuessed: data[0][0]['correctLettersGuessed'],
      incorrectLettersGuessed: data[0][0]['incorrectLettersGuessed'],
      countIncorrectGuesses: data[0][0]['countIncorrectGuesses'],
      result: data[0][0]['result']
    });
    
  }

  // Retrieve a game record by its id
  getGameRecordById(gameId): void {
    this.dataService.getGameById(gameId).subscribe(data => this.gameRecord = {
      id: data[0][0]['idtblGames'],
      word: data[0][0]['word'],
      wordUniqueLetters: data[0][0]['wordUniqueLetters'],
      correctLettersGuessed: data[0][0]['correctLettersGuessed'],
      incorrectLettersGuessed: data[0][0]['incorrectLettersGuessed'],
      countIncorrectGuesses: data[0][0]['countIncorrectGuesses'],
      result: data[0][0]['result']
    });
  }

  // Retrieve the amount of wins
  getWins(): void {
    this.dataService.getWins().subscribe(data => this.gameWins = {
      wins: data[0][0]['COUNT(result)']
    })
  }
  
  // Retrieve the amount of losses
  getLosses(): void {
    this.dataService.getLosses().subscribe(data => this.gameLosses = {
      losses: data[0][0]['COUNT(result)']
    })
  }
}
