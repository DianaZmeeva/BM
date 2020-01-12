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
        public void TeamPlayGame_OneOfTeamsWin_ItPointsMore()
        {
            _russiaTestTeam.TeamRating = 35;
            _englandTestTeam.TeamRating = 35;
            SportManagerController.Game gameclass = new SportManagerController.Game(_russiaTestTeam, _englandTestTeam);

            gameclass.PlayGame();

            bool result = (gameclass.FirstTeamPoints > gameclass.SecondTeamPoints) ||
                          (gameclass.FirstTeamPoints < gameclass.SecondTeamPoints);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void TeamPlayGame_OneTeamWins_ItWinsInChampEqual1_OtherTeamFailedInChampEquals1()
        {
            _russiaTestTeam.TeamRating = 35;
            _englandTestTeam.TeamRating = 35;
            SportManagerController.Game gameclass = new SportManagerController.Game(_russiaTestTeam, _englandTestTeam);

            gameclass.PlayGame();

            if (gameclass.FirstTeamPoints > gameclass.SecondTeamPoints)
            {
                Assert.AreEqual(1, gameclass.FirstTeamInGame.WinsInChamp);
                Assert.AreEqual(1, gameclass.SecondTeamInGame.DefeatsInChamp);
            }
            else
            {
                Assert.AreEqual(1, gameclass.SecondTeamInGame.WinsInChamp);
                Assert.AreEqual(1, gameclass.FirstTeamInGame.DefeatsInChamp);
            }
        }

        [Test]
        public void TeamPlayGame_OneTeamWin_ItPointEqual2_OtherTeamPointEqual1()
        {
            _russiaTestTeam.TeamRating = 50;
            _englandTestTeam.TeamRating = 4;
            SportManagerController.Game gameclass = new SportManagerController.Game(_russiaTestTeam, _englandTestTeam);

            gameclass.PlayGame();

            if (gameclass.FirstTeamPoints > gameclass.SecondTeamPoints)
            {
                Assert.AreEqual(2, gameclass.FirstTeamInGame.PointsInChamp);
                Assert.AreEqual(1, gameclass.SecondTeamInGame.PointsInChamp);
            }
            else
            {
                Assert.AreEqual(1, gameclass.FirstTeamInGame.PointsInChamp);
                Assert.AreEqual(2, gameclass.SecondTeamInGame.PointsInChamp);
            }

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
