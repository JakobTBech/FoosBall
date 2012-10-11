﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoosBall.Models;
using FoosBall.Main;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace FoosBall.Controllers
{
    public class MatchController : Controller
    {       
        private readonly MongoDatabase _dbh;
        public MatchController()
        {
            _dbh = Db.GetDataBaseHandle();
        }

        //
        // GET: /Match/

        public ActionResult Index()
        {
            // Fetch all players to display in a <select>
            var playerCollection = _dbh.GetCollection<Player>("Players").FindAll().ToList();
            // Fetch all FoosBall fights, finished and unfinished. 
            var matchCollection = _dbh.GetCollection<Match>("Matches").FindAll().ToList();
            var playedMatches = new List<Match>();
            var pendingMatches = new List<Match>();
            
            foreach (var match in matchCollection)
            {
                if (match.GameOverTime != BsonDateTime.Create(DateTime.MinValue))
                {
                    playedMatches.Add(match);
                } 
                else
                {
                    pendingMatches.Add(match);
                }
            }

            var selectItems = playerCollection
                .Select(team => new SelectListItem() { Selected = false, Text = team.Name, Value = team.Id.ToString() })
                .ToList();
            return View(model: new MatchModel { PlayedMatches = playedMatches, PendingMatches = pendingMatches, SelectPlayers = selectItems });
            
        }
        
        //
        // POST: /Match/Create/{FormCollection}
        [HttpPost]
        public ActionResult Create(FormCollection formValues)
        {
            // var teamRedId = BsonObjectId.Create(formValues.GetValue("TeamRed").AttemptedValue);
            // var teamBlueId = BsonObjectId.Create(formValues.GetValue("TeamBlue").AttemptedValue);
            // 
            // // only try to create a match if properties are set correctly
            // if (teamRedId.IsBsonNull == false && teamBlueId.IsBsonNull == false)
            // {
            //     var dbh = Db.GetDataBaseHandle();
            // 
            //     var matchCollection = dbh.GetCollection<Match>("Matches");
            //     var teamCollection = dbh.GetCollection<Team>("Teams");
            //     
            //     var teamRed = teamCollection.FindOne(Query.EQ("_id", teamRedId));
            //     var teamBlue = teamCollection.FindOne(Query.EQ("_id", teamBlueId));
            // 
            //     var newMatch = new Match()
            //                         {
            //                             TeamRed = teamRed,
            //                             TeamBlue = teamBlue,
            //                             TeamRedScore = 0,
            //                             TeamBlueScore = 0,
            //                             CreationTime = new BsonDateTime(DateTime.Now),
            //                             GameOverTime = new BsonDateTime(DateTime.MinValue),
            //                         };
            // 
            //     matchCollection.Save(newMatch);
            // }
            // 
            return RedirectToAction("Index");
        }
        
        // 
        // GET: /Match/SaveMatchResult/{id}
        [HttpGet]
        public ActionResult SaveMatchResult(string id)
        {
            var matchCollection = _dbh.GetCollection<Match>("Matches");
            var query = Query.EQ("_id", BsonObjectId.Create(id));
            var match = matchCollection.FindOne(query);

            return View("SaveMatchResult", match);
        }

        // 
        // POST: /Match/SaveMatchResult/
        [HttpPost]
        public ActionResult SaveMatchResult(FormCollection formValues)
        {
            // var redScore = formValues.GetValue("team-red-score").AttemptedValue;
            // var blueScore = formValues.GetValue("team-blue-score").AttemptedValue;
            // 
            // if (String.IsNullOrEmpty(redScore) == false && String.IsNullOrEmpty(blueScore) == false)
            // {
            //     var dbh = Db.GetDataBaseHandle();
            //     var matchCollection = dbh.GetCollection<Match>("Matches");
            //     var teamCollection = dbh.GetCollection<Team>("Teams");
            //     var playerCollection = dbh.GetCollection<Player>("Players");
            // 
            //     var query = Query.EQ("_id", ObjectId.Parse(formValues.GetValue("match-id").AttemptedValue));
            //     var match = matchCollection.FindOne(query);
            //     
            //     // Fetch the team collections of the two opposing teams 
            //     var teamRed = teamCollection.FindOne(Query.EQ("_id", match.TeamRed.Id));
            //     var teamBlue = teamCollection.FindOne(Query.EQ("_id", match.TeamBlue.Id));
            // 
            //     var intRedScore = System.Int32.Parse(redScore, NumberStyles.Float);
            //     var intBlueScore = System.Int32.Parse(blueScore, NumberStyles.Float);
            // 
            //     // Determine the winner and the loser 
            //     Team winner;
            //     Team loser;
            //     if (intRedScore > intBlueScore)
            //     {
            //         winner = teamRed;
            //         match.TeamRed = winner;
            //         loser = teamBlue;
            //         match.TeamBlue = loser;
            //     } else
            //     {
            //         winner = teamBlue;
            //         match.TeamBlue = winner;
            //         loser = teamRed;
            //         match.TeamRed = loser;
            //     }
            // 
            //     // Get the new team/member ratings
            //     var ratingModifier = Rating.GetRatingModifier(winner.GetTeamRating(), loser.GetTeamRating());
            //     
            //     // Propagate the rating and stats to the team members of both teams
            //     foreach (var member in winner.Members)
            //     {
            //         member.Rating += ratingModifier;
            //         member.Won++;
            //         member.Played++;
            //         playerCollection.Save(member);
            //     }
            // 
            //     foreach (var member in loser.Members)
            //     {
            //         member.Rating -= ratingModifier;
            //         member.Lost++;
            //         member.Played++;
            //         playerCollection.Save(member);
            //     }
            // 
            //     // Update match score/time stats
            //     match.TeamRedScore = System.Convert.ToInt32(formValues.GetValue("team-red-score").AttemptedValue, 10);
            //     match.TeamBlueScore = System.Convert.ToInt32(formValues.GetValue("team-blue-score").AttemptedValue, 10);
            //     match.GameOverTime = new BsonDateTime(DateTime.Now);
            //     
            //     // Update teams won/lost/played stats
            //     winner.Won++; 
            //     winner.Played++;
            //     loser.Lost++;
            //     loser.Played++;
            // 
            //     // Save the data to Db
            //     matchCollection.Save(match);
            //     teamCollection.Save(winner);
            //     teamCollection.Save(loser);
            // }
            // 
            return RedirectToAction("Index");
        }
        
        //
        // POST: /Match/Delete/5
        [HttpGet]
        public ActionResult Delete(string id)
        {
            var matchCollection = _dbh.GetCollection<Match>("Matches");

            var query = Query.EQ("_id", BsonObjectId.Create(id));
            matchCollection.Remove(query);
    
            return RedirectToAction("Index");
        }

    }
}
