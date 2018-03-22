using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Strava.Authentication;
using Strava.Clients;
using Strava.Activities;
using PolylineEncoder.Net.Utility;
using Newtonsoft.Json;

namespace TrainingFreeks
{
    [Route("/Strava")]
    public class StravaController : Controller
    {
        public StaticAuthentication auth;
        public StravaClient client;
        DateTime value = new DateTime(2017, 10, 30);

        public StravaController()
        {
            auth = new StaticAuthentication(AuthController.bearer);
            client = new StravaClient(auth);
        }
        
        [HttpGet("Index")]
        public IActionResult Index()
        {

            List<ActivitySummary> allData = client.Activities.GetActivitiesAfter(value);
            Summary weekSum = client.Activities.GetWeeklyProgress();
            Strava.Athletes.Athlete ath = client.Athletes.GetAthlete();
            ViewData["info"] = allData;
            ViewData["date"] = value.ToShortDateString();
            ViewData["weekSum"] = weekSum;
            ViewData["ath"] = ath;
            return View();
        }
        [HttpGet("ridedata")]
        public PartialViewResult RideData([FromQuery]string Id)
        {
            var utility = new PolylineUtility();
            Strava.Activities.Activity ride = client.Activities.GetActivity(Id, true);
            List<Strava.Streams.ActivityStream> watts = client.Streams.GetActivityStream(Id, Strava.Streams.StreamType.Watts, Strava.Streams.StreamResolution.High);
            var stream = watts;
            var decodedPoints = utility.Decode(ride.Map.Polyline);
            var polyArray = JsonConvert.SerializeObject(decodedPoints);

            var test = client.Streams.GetActivityStream(Id, Strava.Streams.StreamType.Watts);

            ViewData["watts"] = stream[1];
            ViewData["rideData"] = ride;
            ViewData["poly"] = polyArray;
            return PartialView("~/Views/Strava/RideData.cshtml", ride);
        }


    }
}