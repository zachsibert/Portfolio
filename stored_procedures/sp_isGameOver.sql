CREATE DEFINER=`sibertzt35`@`%` PROCEDURE `sp_isGameOver`(IN in_gameID int(11))
BEGIN
    
    # If the count of incorrect guesses made is 10 (or somehow greater), set the game result to '0': loss
    IF ((SELECT countIncorrectGuesses FROM tblGames WHERE idtblGames = in_gameID) >= 10) 
		THEN 
			UPDATE tblGames 
            SET result = 0
            WHERE idtblGames = in_gameID;
		END IF;
    
    # If the length of the correctLettersGuessed column is the same length as the wordUniqueLetters column,
    # set the game result to '1': win. A simple length comparison is made instead of comparing exact strings.
    # Logic supporting this method is in sp_guessLetter
	IF ( (SELECT CHAR_LENGTH(wordUniqueLetters) FROM tblGames WHERE idtblGames = in_gameID) 
				= (SELECT CHAR_LENGTH(correctLettersGuessed) FROM tblGames WHERE idtblGames = in_gameID) )
		THEN 
			UPDATE tblGames 
            SET result = 1
            WHERE idtblGames = in_gameID;
        END IF;
        
END