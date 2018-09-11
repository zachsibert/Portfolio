 
 module.exports = function (app, db, sequelize) {

    // Create a new game record in tblGames
    app.get('/api/startGame/', function (req, res) {
        db.sequelize.query('CALL sp_startNewGame();')
            .then(function(result) {
                res.json(result);
        });
    });

    // Guess a letter for specific game
    app.get('/api/guessLetter/:gameId/:letter/', function (req, res) {
        db.sequelize.query('CALL sp_guessLetter(' + req.params.letter + ', ' + req.params.gameId + ');')
            .then(function(result) {
                res.json(result);
            });
    });
    
    // Get the most recent game record from tblGames
    app.get('/api/gameRecord/', function (req, res) {
        db.sequelize.query('SELECT * FROM tblGames ORDER BY idtblGames DESC LIMIT 1;')
            .then(function(result) {
                res.json(result);
            });
    });

    // Get the current game's record by its id from tblGames
    app.get('/api/gameRecord/:gameId/', function (req, res) {
        db.sequelize.query('SELECT * FROM tblGames WHERE idtblGames = ' + req.params.gameId + ';')
            .then(function (result) {
                res.json(result);
        });
    });

    // Get wins from the database
    app.get('/api/wins/', function (req, res) {
        db.sequelize.query('SELECT COUNT(result) FROM tblGames WHERE result = 1')
            .then(function(result) {
                res.json(result);
            });
    });

    // Get losses from the database
    app.get('/api/losses/', function (req, res) {
        db.sequelize.query('SELECT COUNT(result) FROM tblGames WHERE result = 0')
            .then(function(result) {
                res.json(result);
            });
    });

    // Test if a game has been won, lost, or is still in progress
    app.get('/api/isGameOver/:gameId/', function (req, res) {
        db.sequelize.query('CALL sp_isGameOver(' + req.params.gameId + ');')
            .then(function(result) {
                res.json(result);
            });
    });

 }