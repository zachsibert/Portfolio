CREATE DEFINER=`sibertzt35`@`%` PROCEDURE `sp_guessLetter`(IN in_letter varchar(255), IN in_gameID int(11))
BEGIN
	# boolean flags: 0 = false, 1 = true
    SET @correctGuess = 0;
    SET @hasBeenGuessed = 0;
    
    # If the guess is correct, set the correct guess to true
	IF EXISTS (SELECT * FROM tblGames 
			   WHERE idtblGames = in_gameID AND word LIKE CONCAT(CONCAT('%', in_letter), '%'))
		THEN SET @correctGuess = 1; 
        END IF;
    
    # If the letter is found to have been guessed already (correct), set hasBeenGuessed flag to true
	IF EXISTS (SELECT * FROM tblGames
			   WHERE idtblGames = in_gameID AND correctLettersGuessed LIKE CONCAT(CONCAT('%', in_letter), '%'))
		THEN SET @hasBeenGuessed = 1;
		END IF;
     
    # If the letter is found to have been guessed already (incorrect), set hasBeenGuessed flag to true
	IF EXISTS (SELECT * FROM tblGames
			   WHERE idtblGames = in_gameID AND incorrectLettersGuessed LIKE CONCAT(CONCAT('%', in_letter), '%'))
		THEN SET @hasBeenGuessed = 1;
		END IF;
    
    # If the guess is correct and it the letter hasn't been guessed yet, add it to the correctLettersGuessed column
    IF (@correctGuess = 1 AND @hasBeenGuessed = 0) 
		THEN
			UPDATE	tblGames
            SET		correctLettersGuessed = concat(correctLettersGuessed, in_letter)
            WHERE	idtblGames = in_gameID;
		END IF;
	
    # If the guess is incorrect and it the letter hasn't been guessed yet, add it to the incorrectLettersGuessed column
    IF (@correctGuess = 0 AND @hasBeenGuessed = 0) 
		THEN
			UPDATE	tblGames 
            SET		incorrectLettersGuessed = concat(incorrectLettersGuessed, in_letter), countIncorrectGuesses =  countIncorrectGuesses + 1
            WHERE	idtblGames = in_gameID;
		END IF;
    
    # Test if the letter guess wins or loses the game by executing this stored procedure
	CALL sp_isGameOver(in_gameID);
END