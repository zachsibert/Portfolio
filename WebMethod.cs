using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using System.IO;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;
using Newtonsoft.Json;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class ws : System.Web.Services.WebService {

    #region ################################################################################################################# Wrapper Methods [DON'T MODIFY]
    private Helper helper = new Helper();

    // Database
    private void addParam(string name, object value) { helper.addParam(name, value); }
    private DataSet sqlExecDataSet(string sql) { return helper.sqlExecDataSet(sql); }
    private DataTable sqlExecDataTable(string sql) { return helper.sqlExecDataTable(sql); }
    private DataTable sqlExec(string sql) { return helper.sqlExec(sql); }
    private DataTable sqlExec(string sql, DataTable dt, string udtblParam) { return helper.sqlExec(sql, dt, udtblParam); }
    private DataTable sqlExecQuery(string sql) { return helper.sqlExecQuery(sql); }

    // Serializer
    private void streamJson(string jsonString) { helper.streamJson(jsonString); }
    private void serialize(Object obj) { helper.serialize(obj); }
    private void serializeSingleDataTableRow(DataTable dt) { helper.serializeSingleDataTableRow(dt); }
    private void serializeDataTable(DataTable dt) { helper.serializeDataTable(dt); }
    private void serializeDataSet(DataSet ds) { helper.serializeDataSet(ds); }
    private void serializeXML<T>(T value) { helper.serializeXML(value); }
    private void serializeDictionary(Dictionary<object, object> dic) { helper.serializeDictionary(dic); }
    private void serializeObject(Object obj) { helper.serializeObject(obj); }
    #endregion

    /*
            Once you complete this block of steps you can delete it...

            1) Click the "View" option on the menu bar and select "SQL Server Object Explorer
            2) Double-click the "create_ap.sql" file in the Solution Explorer panel (probably on right-hand of screen)
            3) When the script opens up run it by clicking the little green arrow that is on the tab - not the one on the menu bar
            4) A window will pop up.  Expand "Local" and then select "MSSQLLocalDB" and click the Connect button
            5) In the SQL Server Object Explorer panel click the refresh button
            6) Expand the (localdb)\MSSQLLocalDB and then Databases
            7) You should see the AP database the script created.  If you don't then let me know.

            If all worked then you can delete this comment block if you like.
     
     */


    // Keys are hidden in GitHub for privacy
    static string consumerKey = "12345";
    static string consumerKeySecret = "12345";
    static string accessToken = "909114325-12345";
    static string accessTokenSecret = "12345";

    static Twitter bot = new Twitter(consumerKey, consumerKeySecret, accessToken, accessTokenSecret);

    
     
    [WebMethod]
    public void getTweets(string screenName, int count)
    {
        string tweetData = "";
        int UserID = -1;
        try
        {
            tweetData = bot.GetTweets(screenName);
            addParam("@UserHandle", screenName);
            UserID = Convert.ToInt32(sqlExec("spGetUserID").Rows[0]["UserID"]);
        }
        catch (Exception e)
        {
            streamJson("{User Not Found}");
            return;
        }
        storeTweets(UserID, tweetData);
        streamJson(tweetData);
    }

    private void storeTweets(int UserID, string tweetData)
    {
        dynamic tweetDataJSON = JsonConvert.DeserializeObject(tweetData);

        foreach (var tweet in tweetDataJSON)
        {
            string ts = tweet.text;
            int wc = 0;
            int lc = tweet.favorite_count;
            int rc = tweet.retweet_count;
            string t = tweet.created_at;

            addParam("@UserID", UserID);
            addParam("@TweetString", ts);
            addParam("@WordCount", wc);
            addParam("@LikeCount", lc);
            addParam("@RetweetCount", rc);
            sqlExec("spAddTweet");
        }
    }


    [WebMethod]
    public void getFollowings(string screenName)
    {
        int UserID = -1;
        string followingData = "";

        try
        {
            followingData = bot.GetFollowings(screenName);
            addParam("@UserHandle", screenName);
            UserID = Convert.ToInt32(sqlExec("spGetUserID").Rows[0]["UserID"]);

        }
        catch (Exception e)
        {
            streamJson("{User Not Found}");
            return;
        }

        storeFollowings(UserID, screenName, followingData);
        streamJson(followingData);
    }

    private void storeFollowings(int UserID, string screenName, string followingData)
    {
        dynamic followingDataJSON = JsonConvert.DeserializeObject(followingData);
        // Gets corrsesponding UserID from table

        //Add each following
        foreach (var user in followingDataJSON.users)
        {
            string fhandle = user.screen_name;
            addParam("@UserID", UserID);
            addParam("@FollowingHandle", fhandle);
            sqlExec("spAddFollowing");
        }
    }

    // Gets and stores various user data
    [WebMethod]
    public void getUserData(string screenName)
    {
        string UserData = "";

        try
        {
            UserData = bot.GetUserData(screenName);
        }
        catch (Exception e)
        {
            streamJson("Not found Error");
            return;
        } 
        storeUser(UserData);
        streamJson(UserData);
    }

    // Stores a user
    private void storeUser(string UserData)
    {
        dynamic UserDataJSON = JsonConvert.DeserializeObject(UserData);
        string name = UserDataJSON.name;
        string handle = UserDataJSON.screen_name;
        int cnt = UserDataJSON.statuses_count;

        addParam("@UserName", name);
        addParam("@UserHandle", handle);
        addParam("@UserTweetCount", cnt);
        sqlExec("spAddUser");
    }

    [WebMethod]
    public void calculateBias(string screenName)
    {
        int UserID = -1;

        //store user
        string UserData = bot.GetUserData(screenName);
        storeUser(UserData);

        //check for user error
        try
        {
            //check that user is in db, get userID
            addParam("@UserHandle", screenName);
            UserID = Convert.ToInt32(sqlExec("spGetUserID").Rows[0]["UserID"]);
        }
        catch (Exception e)
        {
            streamJson("Not found Error");
            return;// -1;
        }

        //store followings
        string followingData = bot.GetFollowings(screenName);
        storeFollowings(UserID, screenName, followingData);

        //calc
        addParam("@UserID", UserID);
        double bias = Convert.ToDouble(sqlExec("spCalculateBias").Rows[0]["UserBias"]);
        streamJson("" + bias);
    }

    [WebMethod]
    public void calculatePoliticalEngagement(string screenName)
    {

        int UserID = -1;

        //store user
        string UserData = bot.GetUserData(screenName);
        storeUser(UserData);

        //check for user error
        try
        {
            //check that user is in db, get userID
            addParam("@UserHandle", screenName);
            UserID = Convert.ToInt32(sqlExec("spGetUserID").Rows[0]["UserID"]);
        }
        catch (Exception e)
        {
            streamJson("Not found Error");
            return;// -1;
        }

        //store tweets
        string TweetData = bot.GetTweets(screenName);
        storeTweets(UserID, TweetData);

        //calc
        addParam("@UserID", UserID);
        double engagement = Convert.ToDouble(sqlExec("spCalculatePoliticalEngagementV2").Rows[0]["UserPoliticalEngagement"]);
        streamJson("" + engagement);
    }


}

// Followed the below tutorial to implement twitter api oAuth validation
// Implementation: https://www.codeproject.com/Articles/1200390/Taming-the-Twitter-API-in-Csharp
// Author: John Newcombe, 7 Aug 2017

public class Twitter
    {
        public const string OauthVersion = "1.0";
        public const string OauthSignatureMethod = "HMAC-SHA1";

        public Twitter
          (string consumerKey, string consumerKeySecret, string accessToken, string accessTokenSecret)
        {
            this.ConsumerKey = consumerKey;
            this.ConsumerKeySecret = consumerKeySecret;
            this.AccessToken = accessToken;
            this.AccessTokenSecret = accessTokenSecret;
        }

        public string ConsumerKey { set; get; }
        public string ConsumerKeySecret { set; get; }
        public string AccessToken { set; get; }
        public string AccessTokenSecret { set; get; }

        public string GetFollowings(string screenName) // *** Use this for an api call *** //
        {
            string resourceUrl =
                string.Format("https://api.twitter.com/1.1/friends/list.json");

            var requestParameters = new SortedDictionary<string, string>();
            requestParameters.Add("screen_name", screenName);
        requestParameters.Add("count", 200.ToString());

        var response = GetResponse(resourceUrl, Method.GET, requestParameters);

            return response;
        }

        public string GetTweets(string screenName) // *** Use this for an api call *** //
    {
            string resourceUrl =
                string.Format("https://api.twitter.com/1.1/statuses/user_timeline.json");

        var requestParameters = new SortedDictionary<string, string>();
            requestParameters.Add("count", 200.ToString());
            requestParameters.Add("screen_name", screenName);

            var response = GetResponse(resourceUrl, Method.GET, requestParameters);

            return response;
        }

    public string GetUserData(string screenName) // *** Use this for an api call *** //
    {
        string resourceUrl =
            string.Format("https://api.twitter.com/1.1/users/show.json");

        var requestParameters = new SortedDictionary<string, string>();
        requestParameters.Add("screen_name", screenName);

        var response = GetResponse(resourceUrl, Method.GET, requestParameters);

        return response;
    }

    private string GetResponse
        (string resourceUrl, Method method, SortedDictionary<string, string> requestParameters)
        {
            ServicePointManager.Expect100Continue = false;
            WebRequest request = null;
            string resultString = string.Empty;

            if (method == Method.POST)
            {
                var postBody = requestParameters.ToWebString();

                request = (HttpWebRequest)WebRequest.Create(resourceUrl);
                request.Method = method.ToString();
                request.ContentType = "application/x-www-form-urlencoded";

                using (var stream = request.GetRequestStream())
                {
                    byte[] content = Encoding.ASCII.GetBytes(postBody);
                    stream.Write(content, 0, content.Length);
                }
            }
            else if (method == Method.GET)
            {
                request = (HttpWebRequest)WebRequest.Create(resourceUrl + "?"
                    + requestParameters.ToWebString());
                request.Method = method.ToString();
            }
            else
            {
                //other verbs can be addressed here...
            }

            if (request != null)
            {
                var authHeader = CreateHeader(resourceUrl, method, requestParameters);
                request.Headers.Add("Authorization", authHeader);
                var response = request.GetResponse(); //Errors here...

                using (var sd = new StreamReader(response.GetResponseStream()))
                {
                    resultString = sd.ReadToEnd();
                    response.Close();
                }
            }

            return resultString;
        }

        private string CreateOauthNonce()
        {
            return Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
        }

        private string CreateHeader(string resourceUrl, Method method,
                                    SortedDictionary<string, string> requestParameters)
        {
            var oauthNonce = CreateOauthNonce();
            // Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            var oauthTimestamp = CreateOAuthTimestamp();
            var oauthSignature = CreateOauthSignature
            (resourceUrl, method, oauthNonce, oauthTimestamp, requestParameters);

            //The oAuth signature is then used to generate the Authentication header. 
            const string headerFormat = "OAuth oauth_nonce=\"{0}\",oauth_signature_method =\"{1}\", " +
                                         "oauth_timestamp=\"{2}\", oauth_consumer_key =\"{3}\", " +
                                         "oauth_token=\"{4}\", oauth_signature =\"{5}\", " +
                                         "oauth_version=\"{6}\"";

            var authHeader = string.Format(headerFormat,
                                           Uri.EscapeDataString(oauthNonce),
                                           Uri.EscapeDataString(OauthSignatureMethod),
                                           Uri.EscapeDataString(oauthTimestamp),
                                           Uri.EscapeDataString(ConsumerKey),
                                           Uri.EscapeDataString(AccessToken),
                                           Uri.EscapeDataString(oauthSignature),
                                           Uri.EscapeDataString(OauthVersion)
                );

            return authHeader;
        }

        private string CreateOauthSignature
        (string resourceUrl, Method method, string oauthNonce, string oauthTimestamp,
                                            SortedDictionary<string, string> requestParameters)
        {
            //firstly we need to add the standard oauth parameters to the sorted list
            requestParameters.Add("oauth_consumer_key", ConsumerKey);
            requestParameters.Add("oauth_nonce", oauthNonce);
            requestParameters.Add("oauth_signature_method", OauthSignatureMethod);
            requestParameters.Add("oauth_timestamp", oauthTimestamp);
            requestParameters.Add("oauth_token", AccessToken);
            requestParameters.Add("oauth_version", OauthVersion);

            var sigBaseString = requestParameters.ToWebString();

            var signatureBaseString = string.Concat
            (method.ToString(), "&", Uri.EscapeDataString(resourceUrl), "&",
                                Uri.EscapeDataString(sigBaseString.ToString()));

            //Using this base string, we then encrypt the data using a composite of the 
            //secret keys and the HMAC-SHA1 algorithm.
            var compositeKey = string.Concat(Uri.EscapeDataString(ConsumerKeySecret), "&",
                                             Uri.EscapeDataString(AccessTokenSecret));

            string oauthSignature;
            using (var hasher = new HMACSHA1(Encoding.ASCII.GetBytes(compositeKey)))
            {
                oauthSignature = Convert.ToBase64String(
                    hasher.ComputeHash(Encoding.ASCII.GetBytes(signatureBaseString)));
            }

            return oauthSignature;
        }

        private static string CreateOAuthTimestamp()
        {

            var nowUtc = DateTime.UtcNow;
            var timeSpan = nowUtc - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            return timestamp;
        }
    }

    public enum Method
    {
        POST,
        GET
    }

    public static class Extensions
    {
        public static string ToWebString(this SortedDictionary<string, string> source)
        {
            var body = new StringBuilder();

            foreach (var requestParameter in source)
            {
                body.Append(requestParameter.Key);
                body.Append("=");
                body.Append(Uri.EscapeDataString(requestParameter.Value));
                body.Append("&");
            }
            //remove trailing '&'
            body.Remove(body.Length - 1, 1);

            return body.ToString();
        }
    }








<!DOCTYPE html>
<html>
<head>
    <title>Political Bias Calculator</title>
    <meta charset="utf-8" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="https://code.jquery.com/jquery-3.3.1.min.js"
            integrity="sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8="
            crossorigin="anonymous">
    </script>
    <!-- Latest compiled JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            //var bias = document.getElementById('bias');
            var biasLabel = $('#bias');
            var engagementLabel = $('#engagement');
            //var ulList = $('#ulList');

            //================================================================== Button click Listeners
            $('#btnCalc').click(function () {
                ajax("calculateBias", { "screenName": $('#userIn').val() }, bias);
                ajax("calculatePoliticalEngagement", { "screenName": $('#userIn').val() }, engagement);
            });

            //================================================================== Call-back method
            function bias(data) {

                var num = parseFloat(data);
                //var num = 0.43;
                if (num == -1) {
                    biasLabel.css("color", "gray")
                } else if (num < 0.45) {
                    biasLabel.css("color", "blue");
                } else if (num >= 0.45 && num < 0.55) {
                    biasLabel.css("color", "purple");
                } else {
                    biasLabel.css("color", "red");
                }

                biasLabel.text('Bias: ');
                biasLabel.append('<i><b>' + data + '<i><b>');
            }

            function engagement(data) {

                var num = parseFloat(data);

                if (num == -1) {
                    engagementLabel.css("color", "gray");
                }
                else if (num < 0.25) {
                    engagementLabel.css("color", "red");
                } else if (num >= 0.25 && num < 0.75) {
                    engagementLabel.css("color", "yellow");
                } else {
                    engagementLabel.css("color", "green")
                }

                engagementLabel.text('Engagement: ');
                engagementLabel.append('<i><b>' + data + '<i><b>');
            }

            function tweets(data) {

            }

            //================================================================== Main AJAX method
            function ajax(method, data, fn) {
                $.ajax({
                    type: 'GET',
                    url: 'http://localhost:30382/ws.asmx/' + method,
                    dataType: 'json',
                    data: data,
                    success: fn,
                    error: function (data) { debugger; alert('User does not exist'); }
                    //data.responseText
                });
            }
        });
    </script>

</head>

<body style="background-color:lightblue">


    <br><br>
    <h1 align="center">Political Bias Calculator</h1>
    <br><br><br><br>

    <form>
        <div align="center" class="form-group">
            <label>Enter a Twitter Handle without an '@'</label>
        </div>
        <div align="center" class="form-group">
            <input type="text" id="userIn" />
        </div>
        <div align="center" class="form-group">
            <input type="button" id="btnCalc" value="Calculate" class="btn btn-primary" />
        </div>
    </form>

    <form>
        <div class="form-inline">
            <label style="font-size:200%; color:black" id="bias">Bias</label>
            <p><i>Based 200 most recent accounts you followed</i></p>
            <p><i>(-1 means a calculation could not be made due to a private a account or no political data. <br>
                0 is more left, 1 is more right)
            </i></p>

        </div>

        <br><br>

        <div class="form-inline">
            <label style="font-size:200%; color:black" id="engagement">Engagement </label>
            <p><i>Based your 200 most recent tweets</i></p>
            <p><i>(-1 means a calculation could not be made due to a private a account or no political data. <br>
                0 is less political, 1 is more politcial):</i></p>
        </div>
    </form>


    <table class="table table-dark">
        <thead>
            <tr>
                <th scope="col">Tweet</th>
                <th scope="col">Likes</th>
                <th scope="col">Retweets</th>
            </tr>
        </thead>
        <tbody id="tweetBody">
            <tr>
                <th scope="row">1</th>
                <td>Mark</td>
                <td>Otto</td>
                <td>@mdo</td>
            </tr>
        </tbody>
    </table>

</body>
</html>














