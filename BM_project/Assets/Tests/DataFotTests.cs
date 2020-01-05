using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Tests
{
    public class DataFotTests
    {
        public struct CreateTeamStruct
        {
            public string TeamName;
            public int TeamRating;
            public int TeamNumber;
        }
        public static Team.TeamClass CreateTestTeam(CreateTeamStruct team)
        {
            return new Team.TeamClass(team.TeamName, team.TeamRating, team.TeamNumber);
        }
    }
}
