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
        private Team.TeamClass _myRussiaTestTeam;

        [SetUp]
        public void Setup()
        {
            InitializeMyTeam();
        } 

        [Test]
        public void TeamClass_Initialize_CreateTeamClass()
        {
           Team.TeamClass testTeamClass = new Team.TeamClass(_myRussiaTestTeam.TeamName, _myRussiaTestTeam.TeamRating, _myRussiaTestTeam.TeamNumber);

            Assert.AreEqual(_myRussiaTestTeam.TeamName, testTeamClass.TeamName);
            Assert.AreEqual(_myRussiaTestTeam.TeamRating, testTeamClass.TeamRating);
            Assert.AreEqual(_myRussiaTestTeam.TeamNumber, testTeamClass.TeamNumber);
        }

        [Test]
        public void GameF_WithParamenter1_Return1()
        {
            Team.Game testGameClass= new Team.Game(_myRussiaTestTeam, _myRussiaTestTeam);

            var result = testGameClass.F(1);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void GameF_WithParamenter0_Return1()
        {
            Team.Game testGameClass = new Team.Game(_myRussiaTestTeam, _myRussiaTestTeam);

            var result = testGameClass.F(0);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void GameF_WithParameter5_Return120()
        {
            Team.Game testGameClass = new Team.Game(_myRussiaTestTeam, _myRussiaTestTeam);

            var result = testGameClass.F(5);

            Assert.AreEqual(120, result);
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
