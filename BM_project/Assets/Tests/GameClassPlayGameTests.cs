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
        private Team.TeamClass _russiaTestTeam;
        private Team.TeamClass _englandTestTeam;

        [SetUp]
        public void Setup()
        {
            InitializeTeams();
        }

        [Test]
        public void TeamPlayGame_CorrectTeamData_CheckMathGoalsNot0()
        {
            Team.Game gameclass = new Team.Game(_russiaTestTeam, _englandTestTeam);

            gameclass.PlayGame();

            Assert.AreNotEqual(0, gameclass.o1);
            Assert.AreNotEqual(0, gameclass.o2);
        }

        [Test]
        public void TeamPlayGame_FirstTeamWin_Gameo1MoreThanGameo2()
        {
            _russiaTestTeam.l = 50;
            _englandTestTeam.l = 15;
            Team.Game gameclass = new Team.Game(_russiaTestTeam, _englandTestTeam);

            gameclass.PlayGame();

            Assert.AreEqual(true, gameclass.o1>gameclass.o2);
        }

        [Test]
        public void TeamPlayGame_FirstTeamWin_Team1WinsInChampEqual1_Team2FailedInChampEquals1()
        {
            _russiaTestTeam.l = 50;
            _englandTestTeam.l = 4;
            Team.Game gameclass = new Team.Game(_russiaTestTeam, _englandTestTeam);

            gameclass.PlayGame();

            Assert.AreEqual(1, gameclass.team1.wins_in_champ);
            Assert.AreEqual(1, gameclass.team2.failed_in_champ);
        }

        [Test]
        public void TeamPlayGame_FirstTeamWin_Team1PointEqual2_Team2PointEqual1()
        {
            _russiaTestTeam.l = 50;
            _englandTestTeam.l = 4;
            Team.Game gameclass = new Team.Game(_russiaTestTeam, _englandTestTeam);

            gameclass.PlayGame();

            Assert.AreEqual(2, gameclass.team1.points);
            Assert.AreEqual(1, gameclass.team2.points);
        }

        [Test]
        public void TeamPlayGame_SecondTeamWin_Gameo2MoreThanGameo1()
        {
            _russiaTestTeam.l = 15;
            _englandTestTeam.l = 50;
            Team.Game gameclass = new Team.Game(_russiaTestTeam, _englandTestTeam);

            gameclass.PlayGame();

            Assert.AreEqual(true, gameclass.o1 < gameclass.o2);
        }

        [Test]
        public void TeamPlayGame_SecondTeamWin_Team2WinsInChampEqual1_Team1FailedInChampEquals1()
        {
            _russiaTestTeam.l = 15;
            _englandTestTeam.l = 50;
            Team.Game gameclass = new Team.Game(_russiaTestTeam, _englandTestTeam);

            gameclass.PlayGame();

            Assert.AreEqual(1, gameclass.team2.wins_in_champ);
            Assert.AreEqual(1, gameclass.team1.failed_in_champ);
        }

        [Test]
        public void TeamPlayGame_SecondTeamWin_Team2PointEqual2_Team1PointEqual1()
        {
            _russiaTestTeam.l = 15;
            _englandTestTeam.l = 50;
            Team.Game gameclass = new Team.Game(_russiaTestTeam, _englandTestTeam);

            gameclass.PlayGame();

            Assert.AreEqual(2, gameclass.team2.points);
            Assert.AreEqual(1, gameclass.team1.points);
        }

        [Test]
        public void TeamPlayGame_TieOfTeams_GoalsOneofTeamEqual20_AnotherTeamEqual0()
        {
            _russiaTestTeam.l = 0;
            _englandTestTeam.l = 0;
            Team.Game gameclass = new Team.Game(_russiaTestTeam, _englandTestTeam);

            gameclass.PlayGame();

            Assert.AreEqual(true, gameclass.o1 == 20 || gameclass.o2 == 20);
            Assert.AreEqual(true, gameclass.o1 == 0 || gameclass.o2 == 0);
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
