using System.Collections;
using System.Collections.Generic;
using Assets.Tests;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Internal.VR;
using UnityEngine.TestTools;

namespace Tests
{
    public class GameClassPlayGameTests
    {
        private SportManagerController.TeamClass _russiaTestTeam;
        private SportManagerController.TeamClass _englandTestTeam;

        [SetUp]
        public void Setup()
        {
            InitializeTeams();
        }

        [Test]
        public void TeamPlayGame_CorrectTeamData_CheckMathGoalsNot0()
        {
            SportManagerController.Game gameclass = new SportManagerController.Game(_russiaTestTeam, _englandTestTeam);

            gameclass.PlayGame();

            Assert.AreNotEqual(0, gameclass.FirstTeamPoints);
            Assert.AreNotEqual(0, gameclass.SecondTeamPoints);
        }

        [Test]
        public void TeamPlayGame_FirstTeamWin_Gameo1MoreThanGameo2()
        {
            _russiaTestTeam.TeamRating = 50;
            _englandTestTeam.TeamRating = 15;
            SportManagerController.Game gameclass = new SportManagerController.Game(_russiaTestTeam, _englandTestTeam);

            gameclass.PlayGame();

            Assert.AreEqual(true, gameclass.FirstTeamPoints>gameclass.SecondTeamPoints);
        }

        [Test]
        public void TeamPlayGame_FirstTeamWin_Team1WinsInChampEqual1_Team2FailedInChampEquals1()
        {
            _russiaTestTeam.TeamRating = 50;
            _englandTestTeam.TeamRating = 4;
            SportManagerController.Game gameclass = new SportManagerController.Game(_russiaTestTeam, _englandTestTeam);

            gameclass.PlayGame();

            Assert.AreEqual(1, gameclass.FirstTeamInGame.WinsInChamp);
            Assert.AreEqual(1, gameclass.SecondTeamInGame.DefeatsInChamp);
        }

        [Test]
        public void TeamPlayGame_FirstTeamWin_Team1PointEqual2_Team2PointEqual1()
        {
            _russiaTestTeam.TeamRating = 50;
            _englandTestTeam.TeamRating = 4;
            SportManagerController.Game gameclass = new SportManagerController.Game(_russiaTestTeam, _englandTestTeam);

            gameclass.PlayGame();

            Assert.AreEqual(2, gameclass.FirstTeamInGame.PointsInChamp);
            Assert.AreEqual(1, gameclass.SecondTeamInGame.PointsInChamp);
        }

        [Test]
        public void TeamPlayGame_SecondTeamWin_Gameo2MoreThanGameo1()
        {
            _russiaTestTeam.TeamRating = 15;
            _englandTestTeam.TeamRating = 50;
            SportManagerController.Game gameclass = new SportManagerController.Game(_russiaTestTeam, _englandTestTeam);

            gameclass.PlayGame();

            Assert.AreEqual(true, gameclass.FirstTeamPoints < gameclass.SecondTeamPoints);
        }

        [Test]
        public void TeamPlayGame_SecondTeamWin_Team2WinsInChampEqual1_Team1FailedInChampEquals1()
        {
            _russiaTestTeam.TeamRating = 15;
            _englandTestTeam.TeamRating = 50;
            SportManagerController.Game gameclass = new SportManagerController.Game(_russiaTestTeam, _englandTestTeam);

            gameclass.PlayGame();

            Assert.AreEqual(1, gameclass.SecondTeamInGame.WinsInChamp);
            Assert.AreEqual(1, gameclass.FirstTeamInGame.DefeatsInChamp);
        }

        [Test]
        public void TeamPlayGame_SecondTeamWin_Team2PointEqual2_Team1PointEqual1()
        {
            _russiaTestTeam.TeamRating = 15;
            _englandTestTeam.TeamRating = 50;
            SportManagerController.Game gameclass = new SportManagerController.Game(_russiaTestTeam, _englandTestTeam);

            gameclass.PlayGame();

            Assert.AreEqual(2, gameclass.SecondTeamInGame.PointsInChamp);
            Assert.AreEqual(1, gameclass.FirstTeamInGame.PointsInChamp);
        }

        [Test]
        public void TeamPlayGame_TieOfTeams_GoalsOneofTeamEqual20_AnotherTeamEqual0()
        {
            _russiaTestTeam.TeamRating = 0;
            _englandTestTeam.TeamRating = 0;
            SportManagerController.Game gameclass = new SportManagerController.Game(_russiaTestTeam, _englandTestTeam);

            gameclass.PlayGame();

            Assert.AreEqual(true, gameclass.FirstTeamPoints == 20 || gameclass.SecondTeamPoints == 20);
            Assert.AreEqual(true, gameclass.FirstTeamPoints == 0 || gameclass.SecondTeamPoints == 0);
        }

        private void InitializeTeams()
        {
            var teamStruct = new DataFotTests.CreateTeamStruct()
            {
                TeamName = "Russia",
                TeamRating = 35,
                TeamNumber = 1
            };
            _russiaTestTeam = DataFotTests.CreateTestTeam(teamStruct);

            teamStruct.TeamName = "England";
            _englandTestTeam = DataFotTests.CreateTestTeam(teamStruct);
        }
    }
}
