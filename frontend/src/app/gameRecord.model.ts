export interface GameRecord {
    id: number;
    word: String;
    wordUniqueLetters: String;
    correctLettersGuessed: String;
    incorrectLettersGuessed: String;
    countIncorrectGuesses: number;
    result: number;
  }
