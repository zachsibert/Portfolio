import { Component, OnInit, Input } from '@angular/core';

import { DataServiceService } from '../data-service.service';
import { HangmanInterfaceComponent } from '../hangman-interface/hangman-interface.component';

@Component({
  selector: 'app-word-display',
  templateUrl: './word-display.component.html',
  styleUrls: ['./word-display.component.css']
})
export class WordDisplayComponent implements OnInit {

  @Input() public gameRecord;
  public hiddenWord;
  public actualWord;
  
  constructor(private dataService: DataServiceService, private wrapperComponent: HangmanInterfaceComponent) { }

  ngOnInit() {    
    this.initializeWordArrays();
  }

  // Initialize the arrays used to show blanks on screen and manipulate them
  initializeWordArrays(): void {
    this.actualWord = this.gameRecord.word.toString().split('');
    this.hiddenWord = Array(this.gameRecord.word.length).fill("___");
  }

  // Guess letter from the input element in the format '(letter)'
  // The single quotes are needed for the http request's letter format
  guessLetter(letter): void {
    var newLetter = "'" + letter.toLowerCase() + "'"
    this.dataService.guessLetter(this.gameRecord.id, newLetter).subscribe(data => {
      console.log('User guessed letter: ' + letter);
      this.updateBlankWord(newLetter);
    });
  }

  // After a guess is made, update the hidden word to show correct guesses
  updateBlankWord(letter): void {
    for (var i = 0; i < this.actualWord.length; i++) {
      if (letter.charAt(1) === this.actualWord[i]) {
        this.hiddenWord[i] = " " + letter.charAt(1) + " ";
      }
    }
    this.wrapperComponent.getGameRecordById(this.gameRecord.id);
  }

}


