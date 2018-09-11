var express = require('express');
var app = express();
var bodyParser = require('body-parser');
var db = require('./models');
var apiRoutes = require('./app/routes/apiRoutes.js');
var cors = require('cors');
//const port = process.env.PORT || 3000; // listen for environment port, if n/a then use 3000

// Set up the express app for data parsing
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.text());
app.use(bodyParser.json( { type: 'application/vnd.api+json' }));

// static directory
app.use(express.static('app/public'));

app.use(cors());

apiRoutes(app, db);


db.sequelize.sync().then(function() {
    app.listen(3000, function () {
        console.log("Listening on PORT 3000");
    })
});