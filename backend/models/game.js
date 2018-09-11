module.exports = function (sequelize, DataTypes) {
    var tblGames = sequelize.define('tblGames', {
        idtblGames: { type: DataTypes.INTEGER, primaryKey: true },
        word: DataTypes.STRING,
        wordUniqueLetters: DataTypes.STRING,
        correctLettersGuessed: DataTypes.STRING,
        incorrectLettersGuessed: DataTypes.STRING,
        countIncorrectGuesses: DataTypes.INTEGER,
        result: DataTypes.INTEGER
    });

    return tblGames;
};
