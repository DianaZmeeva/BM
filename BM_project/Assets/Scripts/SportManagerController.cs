using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class SportManagerController : MonoBehaviour
{

    public class TeamClass //класс команды 
    {
        public double TeamRating; //рейтинг команды (лямбда в распределении)
        public int PlaceInChamp; // место, занятое командой
        public int TeamNumber; // номер команды
        public int GoalsInChamp; //  количество забитых мячей
        public int MissedGoalsInChamp; // количество пропущенных мячей
        public string TeamName; // Название команды
        public int Budget=100; //бюджет команды
        public int PointsInChamp; // количество очков в турнире
        public int WinsInChamp; // количество побед в турнире
        public int DefeatsInChamp; //количесвто поражений в турнире
        public int NumberOfAllWins; //количество побед всего
        public int NumberOfAllDefeats; //количество поражений всего

        public TeamClass(string name, double rating, int number)
        {
            TeamName = name;
            TeamRating = rating;
            TeamNumber = number;
        }
    }

    public class Champ //класс турнира 
    {
        public int NumberOfPlayedGames; //количество игр в турнире 
        public int EarningsRatioForChamp;  //коэффицент заработка за турнир (используется при подсчете призовых денег за турнир)
        public List<Game> GamesInChamp = new List<Game>(); // игры турнира
        public List<TeamClass> TeamsInChamp = new List<TeamClass>(); //команды, участвующие в турнире
        public TeamClass PlayerTeam; //комнада игрока

        public Champ(TeamClass team)
        {
            PlayerTeam = team;
            TeamsInChamp.Add(PlayerTeam);
        }

        //функция создания команд-соперников на турнир (задание имени, рейтинга команды (в пределах min-max), номера команды)
        public void GenerateRivalTeams(int minRating, int maxRating)
        {
            System.Random rnd = new System.Random();
            string[] country = { "Франция", "Германия", "США", "Италия", "Испания", "Бельгия", "Польша", "Китай", "Япония" }; ;
            for (int i = 0; i < 3; i++)
            {
                //TeamClass t = new TeamClass(country[rnd.Next(country.Length)], rnd.Next(30, 60));
                //t.TeamName = country[rnd.Next(country.Length)];
                //t.TeamRating = rnd.Next(30, 60);
                TeamsInChamp.Add(new TeamClass(country[rnd.Next(country.Length)] + "_" + (i + 1), rnd.Next(minRating, maxRating), i+1));
            }
        }
    }

    public class Game // класс игры
    {
        public TeamClass FirstTeamInGame, SecondTeamInGame; // индексы команд
        public int FirstTeamPoints, SecondTeamPoints; //очки команд
        public Game(TeamClass team1, TeamClass team2)
        {
            FirstTeamInGame = team1;
            SecondTeamInGame = team2;
        }

        public void PlayGame()
        {
            double x1, x2;
            Dictionary<int, double> probabilityDictionaryForFirstTeam = new Dictionary<int, double>(); //словарь вероятностей для команды 1 
            Dictionary<int, double> probabilityDictionaryForSecondTeam = new Dictionary<int, double>(); //словарь вероятностей для команды 1 

            //расчет словаря вероятностей для обеих команд
            for (int i = 0; i < 100; i++)
            {
                x1 = GetProbabilityByPuassonDistribution(FirstTeamInGame.TeamRating, i);
                x2 = GetProbabilityByPuassonDistribution(SecondTeamInGame.TeamRating, i);
                if (x1 > 0 && x1 <= 1)
                {
                    probabilityDictionaryForFirstTeam.Add(i, x1);
                }
                if (x2 > 0 && x2 <= 1)
                {
                    probabilityDictionaryForSecondTeam.Add(i, x2);
                }
            }
            // определения количества очков, забитых командами (FirstTeamPoints, SecondTeamPoints для команд FirstTeamInGame, SecondTeamInGame соответсвенно)
            FirstTeamPoints = GoalGenerateByPuasson(probabilityDictionaryForFirstTeam);
            SecondTeamPoints = GoalGenerateByPuasson(probabilityDictionaryForSecondTeam);

            //если одна из коианд забила 0 очков - присваивается техническое поражение (счет 20:0)
            if (FirstTeamPoints == 0)
                SecondTeamPoints = 20;
            if (SecondTeamPoints == 0)
                FirstTeamPoints = 20;

            //при равенстве очков команда-победитель выбирается случайно (метод "побрасывание монетки")
            if (FirstTeamPoints == SecondTeamPoints)
            {
                System.Random rnd = new System.Random();
                x1 = rnd.NextDouble();
                if (x1 <= 0.5)
                    FirstTeamPoints += 2;
                else
                    SecondTeamPoints += 2;
            }
            
            // изменение показателей команд (очков за турнир, побед и поражений в турнире)
            if (FirstTeamPoints > SecondTeamPoints)
            {
                FirstTeamInGame.PointsInChamp += 2;
                FirstTeamInGame.WinsInChamp++;
                SecondTeamInGame.PointsInChamp += 1;
                SecondTeamInGame.DefeatsInChamp++;

            }
            else
            {
                SecondTeamInGame.PointsInChamp += 2;
                SecondTeamInGame.WinsInChamp++;
                FirstTeamInGame.PointsInChamp += 1;
                FirstTeamInGame.DefeatsInChamp++;
            }
        }

        //генератор Пуассоновского распределения
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
        
        //расчет вероятностей забития m очков командой с рейтингом TeamRating по Пуассоновскому распределению 
        public double GetProbabilityByPuassonDistribution(double rating, int numberOfPoints)
        {
            return (Math.Pow(rating, numberOfPoints) / GetFactorial(numberOfPoints)) * Math.Exp(-rating);
        }

        // функция рассчета факториала
        public double GetFactorial(int value)
        {
            double factorial = 1;
            for (int i = value; i > 1; i--)
                factorial *= i;
            return factorial;
        }
    }

    TeamClass _myGameTeam; // команда игрока
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


    // Start is called before the first frame update
    void Start()
    {
        System.Random rnd = new System.Random();
        Change_canvas(true, false, false, false);
       

        //создание команды игрока
        _myGameTeam = new TeamClass("Россия", rnd.Next(29,35), 0);
        Write_about_team();

        //Champ currentChamp = new Champ(myGameTeam);
        //currentChamp.GenerateRivalTeams();
        //Game g = new Game(currentChamp.TeamsInChamp[2], currentChamp.TeamsInChamp[3]);
        //g.PlayGame();
        //currentChamp.GamesInChamp.Add(g);
        //DontDestroyOnLoad(this.gameObject);

        //обработчики нажатия кнопок
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

    //функция прокачки команды  (i - служебная переменная, для определения места выхова функции)
    private void Up_team(int i)
    {

        if (_myGameTeam.TeamRating == 73)
        {
            money_text.text = "Вы достигли максимума!";
            money_text.color = Color.red;
            up.enabled = false;
        }
        else
        {
            if (_myGameTeam.TeamRating < 40)
            {
                p(10,i);
            }
           else if (_myGameTeam.TeamRating < 50)
            {
                p(100,i);
            }
            else if(_myGameTeam.TeamRating<60)
            {
                p(1000,i);
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

            if((_myGameTeam.TeamRating==40 || _myGameTeam.TeamRating==50 || _myGameTeam.TeamRating==60) && (i==1))
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

    // функция вывода статистики команд в турнире
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

    //функция возвращения обратно к таблице турнира
    private void Back_to_champ()
    {
        Change_canvas(false, true, true, false); //изменения отображения Canvas
    }

    //функция возвращения обратно к команде игрока
    void BackToGameTeam()
    {
        Up_team(0); //вызов функции (со значение 0) - для вывода стоимости, необходимой для прокачки команды
        Change_my_team(); //изменения значений комнады после турнира
        Write_about_team(); //изменение UI-элементов о команде
        Clear(); // очищение таблицы-турнира
        Change_canvas(true, false, false, false);//изменения отображения Canvas
    }

    //функция старта турнира
    void StartChamp()
    {
        _currentChamp = new Champ(_myGameTeam);

        int minRating=30, maxRating=65;

        //определения уровня турнира (рейтинг команд, изменение коэффицента заработка за турнир)
        if (choose.value > 0)
        {
            warning.text = "";

            if (choose.value == 1)
            {
                minRating = 30;
                maxRating = 45;
                _currentChamp.EarningsRatioForChamp = 1;

            }
            else if (choose.value == 2)
            {
                minRating = 40;
                maxRating = 55;
                _currentChamp.EarningsRatioForChamp = 10;
            }
            else if (choose.value == 3)
            {
                minRating = 50;
                maxRating = 73;
                _currentChamp.EarningsRatioForChamp = 100;
            }

            Change_canvas(false, true, false, false);//открытие таблицы-турнира
            _currentChamp.GenerateRivalTeams(minRating, maxRating); //генерация команд соперников

            for (int i = 0; i < _currentChamp.TeamsInChamp.Count; i++) 
            {
                team_text[i].text = _currentChamp.TeamsInChamp[i].TeamName;//вывод списка комнад участников турнира
            }
        }
        else //если уровень не выбран, показать сообщение об ошибке
        {
            warning.text = "Choose level!";
        }

    }

    //функция игры между 2 комадами
    void play(int numberOfFirstTeam, int numberOfSecondTeam, int numberOfButtonPlay, int numberOfFirstTeamPointsText, int numberOfSecondTeamPointsText)// i,j-номер команд; b-номер кнопки в массиве; f,s - номера тектовых полей для вывода счета
    {
        ListOfButtonGame[numberOfButtonPlay].enabled = false;

        //выяснение результатов игры 
        Game newGame = new Game(_currentChamp.TeamsInChamp[numberOfFirstTeam], _currentChamp.TeamsInChamp[numberOfSecondTeam]);
        newGame.PlayGame();
        _currentChamp.GamesInChamp.Add(newGame);

        _currentChamp.NumberOfPlayedGames++;

        //вывод текстовых значений об игре
        _currentChamp.TeamsInChamp[numberOfFirstTeam].GoalsInChamp += newGame.FirstTeamPoints;
        _currentChamp.TeamsInChamp[numberOfSecondTeam].GoalsInChamp += newGame.SecondTeamPoints;
        _currentChamp.TeamsInChamp[numberOfSecondTeam].MissedGoalsInChamp += newGame.FirstTeamPoints;
        _currentChamp.TeamsInChamp[numberOfFirstTeam].MissedGoalsInChamp += newGame.SecondTeamPoints;
        points_text[numberOfSecondTeamPointsText].text = newGame.FirstTeamPoints + ":" + newGame.SecondTeamPoints;
        points_text[numberOfFirstTeamPointsText].text = newGame.SecondTeamPoints + ":" + newGame.FirstTeamPoints;
        oh[numberOfFirstTeam].text = _currentChamp.TeamsInChamp[numberOfFirstTeam].PointsInChamp.ToString();
        oh[numberOfSecondTeam].text = _currentChamp.TeamsInChamp[numberOfSecondTeam].PointsInChamp.ToString();

        if (_currentChamp.NumberOfPlayedGames == 6) //проверка: является ли игра последней в турнире
        {
            var sortedTeams = _currentChamp.TeamsInChamp.OrderByDescending(u => u.PointsInChamp)
                .ThenByDescending(u => (u.GoalsInChamp - u.MissedGoalsInChamp)); //сортировка команд по местам (по количесвту очков; при равенстве таковых по разнице забитых-пропущенных)

            int teamPlace = 1;
            foreach (TeamClass team in sortedTeams) //отображение результатов турнира
            {
                _currentChamp.TeamsInChamp[team.TeamNumber].PlaceInChamp = teamPlace;
                place_text[team.TeamNumber].text = teamPlace.ToString();
                teamPlace++;
            }
            Change_canvas(false, true, true,false);//отображение PanelMenu
        }
    }

    //функция очистки турнира
    private void Clear()
    {
        //очистка текстовых поле таблици-турнира

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

        //очистка турнирных показателей команды игрока
        _myGameTeam.GoalsInChamp = 0;
        _myGameTeam.MissedGoalsInChamp = 0;
        _myGameTeam.PointsInChamp = 0;
        _myGameTeam.PlaceInChamp = 0;
        _myGameTeam.WinsInChamp = 0;
        _myGameTeam.DefeatsInChamp = 0;
    }

    //изменение отображения Canvas
    void Change_canvas(bool isFirstCanvasActive, bool isSecondCanvasActive, bool isThirdCanvasActive, bool isFourthCanvasActive)
    {
        //GameObject t1 = GameObject.Find("Canvas_team") as GameObject;
        t1.SetActive(isFirstCanvasActive);
        t2.SetActive(isSecondCanvasActive);
        t3.SetActive(isThirdCanvasActive);
        t4.SetActive(isFourthCanvasActive);
    }

    //функция изменения показателей команды игрока после турнира 
    private void Change_my_team()
    {
        //измененеие количества побед/поражений
        _myGameTeam.NumberOfAllWins += _myGameTeam.WinsInChamp;
        _myGameTeam.NumberOfAllDefeats += _myGameTeam.DefeatsInChamp;

        //измененение бюджета команды игрока при занятии призового места
        if (_myGameTeam.PlaceInChamp==3)
            _myGameTeam.Budget += 10 * _currentChamp.EarningsRatioForChamp;
        if (_myGameTeam.PlaceInChamp == 2)
            _myGameTeam.Budget += 20 * _currentChamp.EarningsRatioForChamp;
        if (_myGameTeam.PlaceInChamp == 1)
            _myGameTeam.Budget += 30 * _currentChamp.EarningsRatioForChamp;

    }

    //измененение тектовой информации о команде игрока  (на экране CanvasTeam)
    public void Write_about_team()
    {
        r.text= Math.Round(((_myGameTeam.TeamRating * 100) / 73), 1) + "%"; //подсчет рейтинга команды в процентах

        m.text = _myGameTeam.Budget.ToString();
        w.text = _myGameTeam.NumberOfAllWins.ToString();
        fa.text = _myGameTeam.NumberOfAllDefeats.ToString();
    }
} 
