using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class SportManagerController : MonoBehaviour
{
    public const int NumberOfTeams = 4;
    public const int NumberOfGamesInChamp = 6;

    private const int MaxProfessionalRatingValue = 73;
    private const int MinimalProfessionalRatingValue = 50;
    private const int MaxAmateurRatingValue = 45;
    private const int MinimalAmateurRatingValue = 35;
    private const int MaxSemiProfessionalRatingValue = 55;
    private const int MinimalSemiProfessionalRatingValue = 40;
    private const int MinimalRatingForUpTeamToProfessional = 60;

    private const int CostOfLowImprovement = 10;
    private const int CostOfSemiImprovement = 100;
    private const int CostOfHighImprovement = 1000;

    public class TeamClass
    {
        private const int StartBudget = 100;
        private const int WinPointsInChamp = 2;
        private const int LoosePointsInChamp = 1;

        public double TeamRating; 
        public int PlaceInChamp;
        public int TeamNumber; 
        public int GoalsInChamp; 
        public int MissedGoalsInChamp;
        public string TeamName; 
        public int Budget=StartBudget; 
        public int PointsInChamp; 
        public int WinsInChamp; 
        public int DefeatsInChamp; 
        public int NumberOfAllWins;
        public int NumberOfAllDefeats; 

        public TeamClass(string name, double rating, int number)
        {
            TeamName = name;
            TeamRating = rating;
            TeamNumber = number;
        }

        public void ChangeFieldsWithWin()
        {
            WinsInChamp++;
            PointsInChamp += WinPointsInChamp;
        }

        public void ChangeFieldsWithDefeat()
        {
            DefeatsInChamp++;
            PointsInChamp += LoosePointsInChamp;
        }

        public void ResetStatisticsFields()
        {
            GoalsInChamp = 0;
            MissedGoalsInChamp = 0;
            PointsInChamp = 0;
            PlaceInChamp = 0;
            WinsInChamp = 0;
            DefeatsInChamp = 0;
        }
    }

    public class Champ
    {
        public int NumberOfPlayedGames;
        public int EarningsRatioForChamp; 
        public List<Game> GamesInChamp = new List<Game>(); 
        public List<TeamClass> TeamsInChamp = new List<TeamClass>(); 
        public TeamClass PlayerTeam; 

        public Champ(TeamClass team)
        {
            PlayerTeam = team;
            TeamsInChamp.Add(PlayerTeam);
        }

        public void GenerateRivalTeams(int minRating, int maxRating)
        {
            System.Random rnd = new System.Random();
            string[] country = { "Франция", "Германия", "США", "Италия", "Испания", "Бельгия", "Польша", "Китай", "Япония" }; ;
            for (int i = 0; i < NumberOfTeams-1; i++)
            {
                TeamsInChamp.Add(new TeamClass(country[rnd.Next(country.Length)] + "_" + (i + 1), rnd.Next(minRating, maxRating), i+1));
            }
        }
    }

    public class Game 
    {
        public TeamClass FirstTeamInGame, SecondTeamInGame;
        public int FirstTeamPoints, SecondTeamPoints;
        private const int PointsWithTechnicalVictory = 20;
        private const int PointsWithTechnicalLoose = 0;
        public Game(TeamClass team1, TeamClass team2)
        {
            FirstTeamInGame = team1;
            SecondTeamInGame = team2;
        }

        public void PlayGame()
        {
            FirstTeamPoints = GoalGenerateByPuasson(CountProbabilityDictionary(FirstTeamInGame.TeamRating));
            SecondTeamPoints = GoalGenerateByPuasson(CountProbabilityDictionary(SecondTeamInGame.TeamRating));

            if (FirstTeamPoints == PointsWithTechnicalLoose)
                SecondTeamPoints = PointsWithTechnicalVictory;
            if (SecondTeamPoints == PointsWithTechnicalLoose)
                FirstTeamPoints = PointsWithTechnicalVictory;

            if (FirstTeamPoints == SecondTeamPoints)
                GenerateRandomVictory();

            if (FirstTeamPoints > SecondTeamPoints)
            {
                FirstTeamInGame.ChangeFieldsWithWin();
                SecondTeamInGame.ChangeFieldsWithDefeat();

            }
            else
            {
                SecondTeamInGame.ChangeFieldsWithWin();
                FirstTeamInGame.ChangeFieldsWithDefeat();
            }
        }

        private void GenerateRandomVictory()
        {
            System.Random rnd = new System.Random();
            var probability = rnd.NextDouble();
            if (probability <= 0.5)
                FirstTeamPoints += 2;
            else
                SecondTeamPoints += 2;
        }

        private Dictionary<int, double> CountProbabilityDictionary(double teamRating)
        {
            Dictionary<int, double> probabilityDictionary = new Dictionary<int, double>();
            for (int i = 0; i < 100; i++)
            {
                var probability = GetProbabilityByPuassonDistribution(teamRating, i);
                if (IsCorrectValueForProbability(probability))
                {
                    probabilityDictionary.Add(i, probability);
                }
            }

            return probabilityDictionary;
        }

        private static bool IsCorrectValueForProbability(double probability)
        {
            return probability > 0 && probability <= 1;
        }

        private int GoalGenerateByPuasson(Dictionary<int, double> probabilityDictionary)
        {
            System.Random rnd = new System.Random();
            double alpha = rnd.NextDouble();
            int value = -1;
            while (alpha > 0)
            {
                value++;
                if (probabilityDictionary.ContainsKey(value))
                    alpha -= probabilityDictionary[value];
            }
            return value;
        }
        
        public double GetProbabilityByPuassonDistribution(double rating, int numberOfPoints)
        {
            return (Math.Pow(rating, numberOfPoints) / GetFactorial(numberOfPoints)) * Math.Exp(-rating);
        }

        public double GetFactorial(int value)
        {
            double factorial = 1;
            for (int i = value; i > 1; i--)
                factorial *= i;
            return factorial;
        }
    }

    TeamClass _myGameTeam; 
    Champ _currentChamp;

    //UI-элементы для турнира (CanvasChamp)
    public List<Button> ListOfButtonGame = new List<Button>(); //кнопки - "Играть" для игр между командами
    public List<Text> points_text = new List<Text>();  //текстовые поля - очки за игру для игр для игр между командами
    public List<Text> oh = new List<Text>(); // текстовые поля - очки команд в турнире 
    public List<Text> team_text = new List<Text>(); // текстовые поля - названия команд
    public List<Text> place_text = new List<Text>(); // текстовые поля - места команд в тунире

    //общие UI-элементы 
    public GameObject t1,t2,t3,t4;//объекты Canvas (t1-CanvasTeam, t2-CanvasChamp, t3-PanelMenu, t4-CanvasStatistic)
    public Button playChamp, back_to_team, statistic, to_champ; //кнопки - переходы между Canvas

    //UI-элементы для вывода статистики команд (CanvasStatistic)
    public List<Text> t_text = new List<Text>();  // текстовые поля - названия команд
    public List<Text> pl_text = new List<Text>();// текстовые поля - места команд
    public List<Text> po_text = new List<Text>();// текстовые поля - количество очков в турнире
    public List<Text> w_text = new List<Text>();// текстовые поля - количество побед
    public List<Text> l_text = new List<Text>();// текстовые поля - количество поражений
    public List<Text> g_text = new List<Text>(); // текстовые поля - количество забитыхочков(мячей) 
    public List<Text> o_text = new List<Text>();// текстовые поля - количество пропущенных очков(мячей)

    //UI-элементы для команды игрока (CanvasTeam)
    public Text m, w, fa, r, money_text; // тектсовые поля - бюджет, победы, поражения, рейтинг, стоимость прокачки команды (соответсвенно)
    public Button up; // кнопка - увеличения рейтинга команды (прокачка команды)
    public Dropdown choose; //dropdown - выбор уровня турнира
    public Text warning;  //текстовое поле - ошибка (отсутвие выбранного уровня сложности)


    void Start()
    {
        System.Random rnd = new System.Random();
        Change_canvas(true, false, false, false);
       

        _myGameTeam = new TeamClass("Россия", rnd.Next(29,35), 0);
        Write_about_team();

        Button start = playChamp.GetComponent<Button>();
        start.onClick.AddListener(StartChamp);
        back_to_team.onClick.AddListener(BackToGameTeam);
        to_champ.onClick.AddListener(Back_to_champ);
        statistic.onClick.AddListener(Statistic);
        up.onClick.AddListener(() => Up_team(1));
        ListOfButtonGame[0].onClick.AddListener(() => play(0, 1, 0, 3, 0));
        ListOfButtonGame[1].onClick.AddListener(() => play(0, 2, 1, 6, 1));
        ListOfButtonGame[2].onClick.AddListener(() => play(0, 3, 2, 9, 2));
        ListOfButtonGame[3].onClick.AddListener(() => play(1, 2, 3, 7, 4));
        ListOfButtonGame[4].onClick.AddListener(() => play(1, 3, 4,10, 5));
        ListOfButtonGame[5].onClick.AddListener(() => play(2, 3, 5, 11, 8));
    }

    //i - служебная переменная, для определения места выхова функции
    private void Up_team(int i)
    {
        if (_myGameTeam.TeamRating == MaxProfessionalRatingValue)
        {
            money_text.text = "Вы достигли максимума!";
            money_text.color = Color.red;
            up.enabled = false;
        }
        else
        {
            if (_myGameTeam.TeamRating < MinimalSemiProfessionalRatingValue)
            {
                p(CostOfLowImprovement,i);
            }
           else if (_myGameTeam.TeamRating < MinimalProfessionalRatingValue)
            {
                p(CostOfSemiImprovement,i);
            }
            else if(_myGameTeam.TeamRating<MinimalRatingForUpTeamToProfessional)
            {
                p(CostOfHighImprovement,i);
            }
        }
        Write_about_team();
    }

    //функция изменения текстового поля money_text
    private void p(int costOfImprovement, int i)
    {
        if (_myGameTeam.Budget >= costOfImprovement || i==0)
        {
            _myGameTeam.Budget -= costOfImprovement;
            _myGameTeam.TeamRating += i;

            if((_myGameTeam.TeamRating==MinimalSemiProfessionalRatingValue || _myGameTeam.TeamRating==MinimalProfessionalRatingValue || _myGameTeam.TeamRating==MinimalRatingForUpTeamToProfessional) && (i==1))
            {
                money_text.text = "Стоимость: " +(costOfImprovement * 10);
            
            }
            else
            {
                money_text.text = "Стоимость: " + costOfImprovement;
            }
            money_text.color = Color.black;
            if (i == 0)
            {
                _myGameTeam.Budget += costOfImprovement;
            }
        }
        if (_myGameTeam.Budget < costOfImprovement && i==1)
        {
            money_text.text = "Недостаточно средств!";
            money_text.color = Color.red;
        }
    }

    private void Statistic()
    {
        Change_canvas(false, false, false, true);
        for (int i=0; i<_currentChamp.TeamsInChamp.Count; i++)
        {
            t_text[i].text = _currentChamp.TeamsInChamp[i].TeamName;
            pl_text[i].text = _currentChamp.TeamsInChamp[i].PlaceInChamp.ToString();
            po_text[i].text = _currentChamp.TeamsInChamp[i].PointsInChamp.ToString();
            w_text[i].text = _currentChamp.TeamsInChamp[i].WinsInChamp.ToString();
            l_text[i].text = _currentChamp.TeamsInChamp[i].DefeatsInChamp.ToString();
            g_text[i].text = _currentChamp.TeamsInChamp[i].GoalsInChamp.ToString();
            o_text[i].text = _currentChamp.TeamsInChamp[i].MissedGoalsInChamp.ToString();
        }
    }

   
    private void Back_to_champ()
    {
        Change_canvas(false, true, true, false); 
    }


    void BackToGameTeam()
    {
        Up_team(0); //вызов функции (со значение 0) - для вывода стоимости, необходимой для прокачки команды
        Change_my_team(); 
        Write_about_team(); 
        Clear(); 
        Change_canvas(true, false, false, false);
    }

   
    void StartChamp()
    {
        _currentChamp = new Champ(_myGameTeam);

        int minRating=MinimalAmateurRatingValue, maxRating=65;

        //определения уровня турнира (рейтинг команд, изменение коэффицента заработка за турнир)
        if (choose.value > 0)
        {
            warning.text = "";

            switch (choose.value)
            {
                case 1:
                    minRating = MinimalAmateurRatingValue;
                    maxRating = MaxAmateurRatingValue;
                    _currentChamp.EarningsRatioForChamp = 1;
                    break;
                case 2:
                    minRating = MinimalSemiProfessionalRatingValue;
                    maxRating = MaxSemiProfessionalRatingValue;
                    _currentChamp.EarningsRatioForChamp = 10;
                    break;
                case 3:
                    minRating = MinimalProfessionalRatingValue;
                    maxRating = MaxProfessionalRatingValue;
                    _currentChamp.EarningsRatioForChamp = 100;
                    break;
            }

            Change_canvas(false, true, false, false);
            _currentChamp.GenerateRivalTeams(minRating, maxRating); 

            for (int i = 0; i < _currentChamp.TeamsInChamp.Count; i++) 
            {
                team_text[i].text = _currentChamp.TeamsInChamp[i].TeamName;
            }
        }
        else 
        {
            warning.text = "Choose level!";
        }

    }

    
    void play(int numberOfFirstTeam, int numberOfSecondTeam, int numberOfButtonPlay, int numberOfFirstTeamPointsText, int numberOfSecondTeamPointsText)// i,j-номер команд; b-номер кнопки в массиве; f,s - номера тектовых полей для вывода счета
    {
        ListOfButtonGame[numberOfButtonPlay].enabled = false;

        Game newGame = new Game(_currentChamp.TeamsInChamp[numberOfFirstTeam], _currentChamp.TeamsInChamp[numberOfSecondTeam]);
        newGame.PlayGame();
        _currentChamp.GamesInChamp.Add(newGame);

        _currentChamp.NumberOfPlayedGames++;

        ChangeStringFieldsAfterGame(numberOfFirstTeam, numberOfSecondTeam, numberOfFirstTeamPointsText, numberOfSecondTeamPointsText, newGame);

        if (_currentChamp.NumberOfPlayedGames == NumberOfGamesInChamp)
        {
            var sortedTeams = _currentChamp.TeamsInChamp.OrderByDescending(u => u.PointsInChamp)
                .ThenByDescending(u => (u.GoalsInChamp - u.MissedGoalsInChamp)); 

            int teamPlace = 1;
            foreach (TeamClass team in sortedTeams)
            {
                _currentChamp.TeamsInChamp[team.TeamNumber].PlaceInChamp = teamPlace;
                place_text[team.TeamNumber].text = teamPlace.ToString();
                teamPlace++;
            }
            Change_canvas(false, true, true,false);
        }
    }

    private void ChangeStringFieldsAfterGame(int numberOfFirstTeam, int numberOfSecondTeam, int numberOfFirstTeamPointsText,
        int numberOfSecondTeamPointsText, Game newGame)
    {
        _currentChamp.TeamsInChamp[numberOfFirstTeam].GoalsInChamp += newGame.FirstTeamPoints;
        _currentChamp.TeamsInChamp[numberOfSecondTeam].GoalsInChamp += newGame.SecondTeamPoints;
        _currentChamp.TeamsInChamp[numberOfSecondTeam].MissedGoalsInChamp += newGame.FirstTeamPoints;
        _currentChamp.TeamsInChamp[numberOfFirstTeam].MissedGoalsInChamp += newGame.SecondTeamPoints;
        points_text[numberOfSecondTeamPointsText].text = newGame.FirstTeamPoints + ":" + newGame.SecondTeamPoints;
        points_text[numberOfFirstTeamPointsText].text = newGame.SecondTeamPoints + ":" + newGame.FirstTeamPoints;
        oh[numberOfFirstTeam].text = _currentChamp.TeamsInChamp[numberOfFirstTeam].PointsInChamp.ToString();
        oh[numberOfSecondTeam].text = _currentChamp.TeamsInChamp[numberOfSecondTeam].PointsInChamp.ToString();
    }

    private void Clear()
    {
        for (int i=0; i<ListOfButtonGame.Count; i++)
        {
            ListOfButtonGame[i].enabled = true;
        }

        for (int i = 0; i < points_text.Count; i++)
        {
            points_text[i].text = "";
        }

        for (int i = 0; i < oh.Count; i++)
        {
            oh[i].text = "";
            team_text[i].text = "";
            place_text[i].text = "";
        }

        _myGameTeam.ResetStatisticsFields();
    }

    void Change_canvas(bool isFirstCanvasActive, bool isSecondCanvasActive, bool isThirdCanvasActive, bool isFourthCanvasActive)
    {
        t1.SetActive(isFirstCanvasActive);
        t2.SetActive(isSecondCanvasActive);
        t3.SetActive(isThirdCanvasActive);
        t4.SetActive(isFourthCanvasActive);
    }

    private void Change_my_team()
    {
        
        _myGameTeam.NumberOfAllWins += _myGameTeam.WinsInChamp;
        _myGameTeam.NumberOfAllDefeats += _myGameTeam.DefeatsInChamp;

        
        if (_myGameTeam.PlaceInChamp==3)
            _myGameTeam.Budget += 10 * _currentChamp.EarningsRatioForChamp;
        if (_myGameTeam.PlaceInChamp == 2)
            _myGameTeam.Budget += 20 * _currentChamp.EarningsRatioForChamp;
        if (_myGameTeam.PlaceInChamp == 1)
            _myGameTeam.Budget += 30 * _currentChamp.EarningsRatioForChamp;

    }

    
    public void Write_about_team()
    {
        r.text= Math.Round(((_myGameTeam.TeamRating * 100) / MaxProfessionalRatingValue), 1) + "%"; //подсчет рейтинга команды в процентах

        m.text = _myGameTeam.Budget.ToString();
        w.text = _myGameTeam.NumberOfAllWins.ToString();
        fa.text = _myGameTeam.NumberOfAllDefeats.ToString();
    }
} 
