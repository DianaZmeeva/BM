using System.Collections;
using System.Collections.Generic;
using Assets.Tests;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TeamPublicClassesTests
    {
        private SportManagerController.TeamClass _myRussiaTestTeam;

        [SetUp]
        public void Setup()
        {
            InitializeMyTeam();
        } 

        [Test]
        public void TeamClass_Initialize_CreateTeamClass()
        {
           SportManagerController.TeamClass testTeamClass = new SportManagerController.TeamClass(_myRussiaTestTeam.TeamName, _myRussiaTestTeam.TeamRating, _myRussiaTestTeam.TeamNumber);

            Assert.AreEqual(_myRussiaTestTeam.TeamName, testTeamClass.TeamName);
            Assert.AreEqual(_myRussiaTestTeam.TeamRating, testTeamClass.TeamRating);
            Assert.AreEqual(_myRussiaTestTeam.TeamNumber, testTeamClass.TeamNumber);
        }

        [Test]
        public void GameF_WithParamenter1_Return1()
        {
            SportManagerController.Game testGameClass= new SportManagerController.Game(_myRussiaTestTeam, _myRussiaTestTeam);

            var result = testGameClass.GetFactorial(1);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void GameF_WithParamenter0_Return1()
        {
            SportManagerController.Game testGameClass = new SportManagerController.Game(_myRussiaTestTeam, _myRussiaTestTeam);

            var result = testGameClass.GetFactorial(0);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void GameF_WithParameter5_Return120()
        {
            SportManagerController.Game testGameClass = new SportManagerController.Game(_myRussiaTestTeam, _myRussiaTestTeam);

            var result = testGameClass.GetFactorial(5);

            Assert.AreEqual(120, result);
        }

        [Test]
        public void TeamClass_ResetTeamStatisticFields_ReturnGoalsInChampEqualZero()
        {
            SportManagerController.TeamClass testTeamClass = new SportManagerController.TeamClass(_myRussiaTestTeam.TeamName, _myRussiaTestTeam.TeamRating, _myRussiaTestTeam.TeamNumber);
            testTeamClass.GoalsInChamp = 100;

            testTeamClass.ResetStatisticsFields();

            Assert.AreEqual(0, testTeamClass.GoalsInChamp);

        }

        [Test]
        public void TeamClass_ResetTeamStatisticFields_ReturnMissedGoalsInChampEqualZero()
        {
            SportManagerController.TeamClass testTeamClass = new SportManagerController.TeamClass(_myRussiaTestTeam.TeamName, _myRussiaTestTeam.TeamRating, _myRussiaTestTeam.TeamNumber);
            testTeamClass.MissedGoalsInChamp = 100;

            testTeamClass.ResetStatisticsFields();

            Assert.AreEqual(0, testTeamClass.MissedGoalsInChamp);

        }

        [Test]
        public void TeamClass_ResetTeamStatisticFields_ReturnPointsInChampEqualZero()
        {
            SportManagerController.TeamClass testTeamClass = new SportManagerController.TeamClass(_myRussiaTestTeam.TeamName, _myRussiaTestTeam.TeamRating, _myRussiaTestTeam.TeamNumber);
            testTeamClass.PointsInChamp = 6;

            testTeamClass.ResetStatisticsFields();

            Assert.AreEqual(0, testTeamClass.PointsInChamp);

        }

        [Test]
        public void TeamClass_ResetTeamStatisticFields_ReturnPlaceInChampEqualZero()
        {
            SportManagerController.TeamClass testTeamClass = new SportManagerController.TeamClass(_myRussiaTestTeam.TeamName, _myRussiaTestTeam.TeamRating, _myRussiaTestTeam.TeamNumber);
            testTeamClass.PlaceInChamp = 1;

            testTeamClass.ResetStatisticsFields();

            Assert.AreEqual(0, testTeamClass.PlaceInChamp);

        }

        [Test]
        public void TeamClass_ResetTeamStatisticFields_ReturnWinsInChampEqualZero()
        {
            SportManagerController.TeamClass testTeamClass = new SportManagerController.TeamClass(_myRussiaTestTeam.TeamName, _myRussiaTestTeam.TeamRating, _myRussiaTestTeam.TeamNumber);
            testTeamClass.WinsInChamp = 3;

            testTeamClass.ResetStatisticsFields();

            Assert.AreEqual(0, testTeamClass.WinsInChamp);

        }

        [Test]
        public void TeamClass_ResetTeamStatisticFields_ReturnDefeatsInChampEqualZero()
        {
            SportManagerController.TeamClass testTeamClass = new SportManagerController.TeamClass(_myRussiaTestTeam.TeamName, _myRussiaTestTeam.TeamRating, _myRussiaTestTeam.TeamNumber);
            testTeamClass.DefeatsInChamp = 1;

            testTeamClass.ResetStatisticsFields();

            Assert.AreEqual(0, testTeamClass.DefeatsInChamp);

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
