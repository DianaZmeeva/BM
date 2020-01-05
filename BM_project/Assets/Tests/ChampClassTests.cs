using System.Collections;
using System.Collections.Generic;
using Assets.Tests;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ChampClassTests
    {
        private Team.TeamClass _myRussiaTestTeam;

        [SetUp]
        public void Setup()
        {
            InitializeMyTeam();
        }

        [Test]
        public void ChampClass_CreateClass_AddingMyTeamToClass()
        {
            Team.Champ testChamp = new Team.Champ(_myRussiaTestTeam);

            Assert.AreNotEqual(null, testChamp.myteam);
            Assert.AreEqual(1, testChamp.teams.Count);
        }

        [Test]
        public void ChampClass_CreateClass_Equal_myRussiaTestTeam()
        {
            Team.Champ testChamp = new Team.Champ(_myRussiaTestTeam);

            Assert.AreEqual(_myRussiaTestTeam, testChamp.myteam);
        }

        [Test]
        public void ChampClass_GenerateTeams_TeamsCountEqual4()
        {
            Team.Champ testChamp = new Team.Champ(_myRussiaTestTeam);

            testChamp.GenerateTeams(30, 55);

            Assert.AreEqual(4, testChamp.teams.Count);
        }

        [Test]
        public void ChampClass_GenerateTeams30Min55Max_AllRivalTeamsRatingBetweenMinAndMax()
        {
            int minRating = 30;
            int maxRating = 55;
            Team.Champ testChamp = new Team.Champ(_myRussiaTestTeam);

            testChamp.GenerateTeams(minRating, maxRating);

            Assert.AreEqual(true, CheckTeamRating(testChamp.teams[1].l, minRating, maxRating));
            Assert.AreEqual(true, CheckTeamRating(testChamp.teams[2].l, minRating, maxRating));
            Assert.AreEqual(true, CheckTeamRating(testChamp.teams[3].l, minRating, maxRating));
        }

        private bool CheckTeamRating(double teamRating, int minRating, int maxRating)
        {
            return teamRating >= minRating && teamRating <= maxRating;
        }


        private void InitializeMyTeam()
        {
            var teamStruct = new DataFotTests.CreateTeamStruct()
            {
                TeamName = "Russia",
                TeamRating = 35,
                TeamNumber = 1
            };
            _myRussiaTestTeam = DataFotTests.CreateTestTeam(teamStruct);
        }
    }
}
