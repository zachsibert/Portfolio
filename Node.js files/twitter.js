console.log('Starting...');

var Twitter = require('twitter');

var client = new Twitter( {
  consumer_key: 'QicQmvc0dhor0scuqCuzt160X',
  consumer_secret: 'tTrxFWqOT3PrZRD6jSj1BeZ8VSC78kfBBWagxDfKmqYvg2KgYR',
  access_token_key: '909114325-BmJULIAjCf13x0XVmMWJKPnOznr8sBh7iFRRqYl4',
  access_token_secret: 'iNBl96FSx6z6fO6zyg7ZOYPD5xwpicGXOTSKnruV32gUw'
});


// ************************************************************************** //


//Path is the what api call you want to make

//Takes a search query and count of desired tweets
function searchTweets(query, tweetCount) {
  var path = 'search/tweets';
  var params = {
    q: query,
    count: tweetCount
  };

  client.get(path, params, function(error, data, response) {
    if (!error) {
      var tweets = data.statuses;
      for(var i = 0; i < tweets.length; i++){
        console.log(tweets[i].created_at);
        console.log(tweets[i].text);
      }
    }

    else{
      console.log('Error when searching!!!');
    }
  });
}

//Takes a screen name (dont include @)
function getUserData(screenName) {
  var path = 'users/show';
  var params = {
    screen_name: screenName
  };

  client.get(path, params, function(error, data, response) {
    if (!error) {
      console.log(data);
    }
    else{
      console.log('Error when finding user!!!');
    }
  });
}

//Takes a screen name (dont include @) and count of desired tweets
function getUserFollowings(screenName, cnt) {
  var path = 'friends/list';
  var params = {
    screen_name: screenName,
    count: cnt
  };

  client.get(path, params, function(error, data, response) {
    if (!error) {
      for (var i = 0; i < data.users.length; i++) {
      console.log(data.users[i].name);
      }
    }
    else{
      console.log('Error when finding user\'s followers!!!');
    }
  });
}

// Other twitter api functions (get, post, stream)
// client.post(path, params, function() {});
// client.stream(path, params, function() {});

console.log('atttempting to search...\n');
//These are functions i've made that wraps a twitter api function and error checking.
//Run in console to see results

searchTweets('Donald Trump',3);
//getUserData('therealbearb');
//getUserFollowings('therealbearb',5);
