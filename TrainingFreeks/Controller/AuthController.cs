using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;

namespace TrainingFreeks
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        const string clientId = "19051";
        const string clientSecret = "6a5f8d363152c4646211c140b2c67c5169cacbd4";
        public static string bearer;

        public AuthController()
        {
        }

        [HttpGet]
        public async Task<IActionResult> TokenExchange([FromQuery]string state, [FromQuery]string code)
        {
            HttpClient req = new HttpClient();
            var exchangeContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret",clientSecret),
                new KeyValuePair<string, string>("code",code),
            });
            HttpResponseMessage response = await req.PostAsync("https://www.strava.com/oauth/token", exchangeContent);

            HttpContent responseContent = response.Content;
            var data = responseContent.ReadAsStringAsync();
            var json = data.Result;
            var modelData = JsonConvert.DeserializeObject<Rootobject>(json);
            bearer = modelData.access_token;
            
            return Redirect(Url.Action("Index", "Strava"));
        }

        public class Rootobject
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public Athlete athlete { get; set; }
        }

        public class Athlete
        {
            public int id { get; set; }
            public string username { get; set; }
            public int resource_state { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string country { get; set; }
            public string sex { get; set; }
            public bool premium { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public int badge_type_id { get; set; }
            public string profile_medium { get; set; }
            public string profile { get; set; }
            public object friend { get; set; }
            public object follower { get; set; }
            public string email { get; set; }
        }
    }
}


