CREATE DEFINER=`sibertzt35`@`%` PROCEDURE `sp_startNewGame`()
BEGIN
	SET @randomNumber = (FLOOR(RAND() * 10) + 1); # randomize a number between 1-10
    SET @randomWord = (SELECT word FROM tblWords WHERE idTblWords = @randomNumber); # select a word corresponding to that random number
    SET @randomWordUniqueLetters = (SELECT wordUniqueLetters FROM tblWords WHERE idTblWords = @randomNumber); # select corresponding unique letters
    
    # Create a new game record with a randomized word
    INSERT INTO tblGames (word, wordUniqueLetters, createdAt, updatedAt, result)
					VALUES (@randomWord, @randomWordUniqueLetters, NOW(), NOW(), -1); 
END