using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class Team : MonoBehaviour
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
        public int val_game; //количество игр в турнире 
        public int y;  //коэффицент заработка за турнир (используется при подсчете призовых денег за турнир)
        public List<Game> games = new List<Game>(); // игры турнира
        public List<TeamClass> teams = new List<TeamClass>(); //команды, участвующие в турнире
        public TeamClass myteam; //комнада игрока

        public Champ(TeamClass t)
        {
            myteam = t;
            teams.Add(myteam);
        }

        //функция создания команд-соперников на турнир (задание имени, рейтинга команды (в пределах min-max), номера команды)
        public void GenerateTeams(int min, int max)
        {
            System.Random rnd = new System.Random();
            string[] country = { "Франция", "Германия", "США", "Италия", "Испания", "Бельгия", "Польша", "Китай", "Япония" }; ;
            for (int i = 0; i < 3; i++)
            {
                //TeamClass t = new TeamClass(country[rnd.Next(country.Length)], rnd.Next(30, 60));
                //t.TeamName = country[rnd.Next(country.Length)];
                //t.TeamRating = rnd.Next(30, 60);
                teams.Add(new TeamClass(country[rnd.Next(country.Length)] + "_" + (i + 1), rnd.Next(min, max), i+1));
            }
        }
    }

    public class Game // класс игры
    {
        double x1, x2; 
        public TeamClass team1, team2; // индексы команд
        public int o1, o2; //очки команд
        public Game(TeamClass i, TeamClass j)
        {
            team1 = i;
            team2 = j;
        }

        public void PlayGame()
        {
            Dictionary<int, double> p1 = new Dictionary<int, double>(); //словарь вероятностей для команды 1 
            Dictionary<int, double> p2 = new Dictionary<int, double>(); //словарь вероятностей для команды 1 

            //расчет словаря вероятностей для обеих команд
            for (int i = 0; i < 100; i++)
            {
                x1 = Puasson(team1.TeamRating, i); 
                x2 = Puasson(team2.TeamRating, i);
                if (x1 > 0 && x1 <= 1)
                {
                    p1.Add(i, x1);
                }
                if (x2 > 0 && x2 <= 1)
                {
                    p2.Add(i, x2);
                }
            }
            // определения количества очков, забитых командами (o1, o2 для команд team1, team2 соответсвенно)
            o1 = Goal_Generate(p1);
            o2 = Goal_Generate(p2);

            //если одна из коианд забила 0 очков - присваивается техническое поражение (счет 20:0)
            if (o1 == 0)
                o2 = 20;
            if (o2 == 0)
                o1 = 20;

            //при равенстве очков команда-победитель выбирается случайно (метод "побрасывание монетки")
            if (o1 == o2)
            {
                System.Random rnd = new System.Random();
                x1 = rnd.NextDouble();
                if (x1 <= 0.5)
                    o1 += 2;
                else
                    o2 += 2;
            }
            
            // изменение показателей команд (очков за турнир, побед и поражений в турнире)
            if (o1 > o2)
            {
                team1.PointsInChamp += 2;
                team1.WinsInChamp++;
                team2.PointsInChamp += 1;
                team2.DefeatsInChamp++;

            }
            else
            {
                team2.PointsInChamp += 2;
                team2.WinsInChamp++;
                team1.PointsInChamp += 1;
                team1.DefeatsInChamp++;
            }
        }

        //генератор Пуассоновского распределения
        private int Goal_Generate(Dictionary<int, double> p1)
        {
            System.Random rnd = new System.Random();
            double x1 = rnd.NextDouble();
            int i = -1;
            while (x1 > 0)
            {
                i++;
                if (p1.ContainsKey(i))
                    x1 -= p1[i];
            }
            return i;
        }
        
        //расчет вероятностей забития m очков командой с рейтингом TeamRating по Пуассоновскому распределению 
        public double Puasson(double l, int m)
        {
            double x = (Math.Pow(l, m) / F(m)) * Math.Exp(-l);
            return x;
        }

        // функция рассчета факториала
        public double F(int z)
        {
            double s = 1;
            for (int i = z; i > 1; i--)
                s *= i;
            return s;
        }
    }

    TeamClass my; // команда игрока
    Champ n;

    //UI-элементы для турнира (CanvasChamp)
    public List<Button> points_buttons = new List<Button>(); //кнопки - "Играть" для игр между командами
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
        my = new TeamClass("Россия", rnd.Next(29,35), 0);
        Write_about_team();

        //Champ n = new Champ(my);
        //n.GenerateTeams();
        //Game g = new Game(n.teams[2], n.teams[3]);
        //g.PlayGame();
        //n.games.Add(g);
        //DontDestroyOnLoad(this.gameObject);

        //обработчики нажатия кнопок
        Button start = playChamp.GetComponent<Button>();
        start.onClick.AddListener(StartChamp);
        back_to_team.onClick.AddListener(Back);
        to_champ.onClick.AddListener(Back_to_champ);
        statistic.onClick.AddListener(Statistic);
        up.onClick.AddListener(() => Up_team(1));
        points_buttons[0].onClick.AddListener(() => play(0, 1, 0, 3, 0));
        points_buttons[1].onClick.AddListener(() => play(0, 2, 1, 6, 1));
        points_buttons[2].onClick.AddListener(() => play(0, 3, 2, 9, 2));
        points_buttons[3].onClick.AddListener(() => play(1, 2, 3, 7, 4));
        points_buttons[4].onClick.AddListener(() => play(1, 3, 4,10, 5));
        points_buttons[5].onClick.AddListener(() => play(2, 3, 5, 11, 8));
    }

    //функция прокачки команды  (i - служебная переменная, для определения места выхова функции)
    private void Up_team(int i)
    {

        if (my.TeamRating == 73)
        {
            money_text.text = "Вы достигли максимума!";
            money_text.color = Color.red;
            up.enabled = false;
        }
        else
        {
            if (my.TeamRating < 40)
            {
                p(10,i);
            }
           else if (my.TeamRating < 50)
            {
                p(100,i);
            }
            else if(my.TeamRating<60)
            {
                p(1000,i);
            }
        }
        Write_about_team();
    }

    //функция изменения текстового поля money_text
    private void p(int v, int i)
    {
        if (my.Budget >= v || i==0)
        {
            my.Budget -= v;
            my.TeamRating += i;

            if((my.TeamRating==40 || my.TeamRating==50 || my.TeamRating==60) && (i==1))
            {
                money_text.text = "Стоимость: " +(v*10);
            
            }
            else
            {
                money_text.text = "Стоимость: " + v;
            }
            money_text.color = Color.black;
            if (i == 0)
            {
                my.Budget += v;
            }
        }
        if (my.Budget < v && i==1)
        {
            money_text.text = "Недостаточно средств!";
            money_text.color = Color.red;
        }
    }

    // функция вывода статистики команд в турнире
    private void Statistic()
    {
        Change_canvas(false, false, false, true);
        for (int i=0; i<n.teams.Count; i++)
        {
            t_text[i].text = n.teams[i].TeamName;
            pl_text[i].text = n.teams[i].PlaceInChamp.ToString();
            po_text[i].text = n.teams[i].PointsInChamp.ToString();
            w_text[i].text = n.teams[i].WinsInChamp.ToString();
            l_text[i].text = n.teams[i].DefeatsInChamp.ToString();
            g_text[i].text = n.teams[i].GoalsInChamp.ToString();
            o_text[i].text = n.teams[i].MissedGoalsInChamp.ToString();
        }
    }

    //функция возвращения обратно к таблице турнира
    private void Back_to_champ()
    {
        Change_canvas(false, true, true, false); //изменения отображения Canvas
    }

    //функция возвращения обратно к команде игрока
    void Back()
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
        n = new Champ(my);

        int min=30, max=65;

        //определения уровня турнира (рейтинг команд, изменение коэффицента заработка за турнир)
        if (choose.value > 0)
        {
            warning.text = "";

            if (choose.value == 1)
            {
                min = 30;
                max = 45;
                n.y = 1;

            }
            else if (choose.value == 2)
            {
                min = 40;
                max = 55;
                n.y = 10;
            }
            else if (choose.value == 3)
            {
                min = 50;
                max = 73;
                n.y = 100;
            }

            Change_canvas(false, true, false, false);//открытие таблицы-турнира
            n.GenerateTeams(min, max); //генерация команд соперников

            for (int i = 0; i < n.teams.Count; i++) 
            {
                team_text[i].text = n.teams[i].TeamName;//вывод списка комнад участников турнира
            }
        }
        else //если уровень не выбран, показать сообщение об ошибке
        {
            warning.text = "Choose level!";
        }

    }

    //функция игры между 2 комадами
    void play(int i, int j, int b, int f, int s)// i,j-номер команд; b-номер кнопки в массиве; f,s - номера тектовых полей для вывода счета
    {
        points_buttons[b].enabled = false;

        //выяснение результатов игры 
        Game g = new Game(n.teams[i], n.teams[j]);
        g.PlayGame();
        n.games.Add(g);

        n.val_game++;

        //вывод текстовых значений об игре
        n.teams[i].GoalsInChamp += g.o1;
        n.teams[j].GoalsInChamp += g.o2;
        n.teams[j].MissedGoalsInChamp += g.o1;
        n.teams[i].MissedGoalsInChamp += g.o2;
        points_text[s].text = g.o1 + ":" + g.o2;
        points_text[f].text = g.o2 + ":" + g.o1;
        oh[i].text = n.teams[i].PointsInChamp.ToString();
        oh[j].text = n.teams[j].PointsInChamp.ToString();

        if (n.val_game == 6) //проверка: является ли игра последней в турнире
        {
            var t = n.teams.OrderByDescending(u => u.PointsInChamp).ThenByDescending(u => (u.GoalsInChamp - u.MissedGoalsInChamp)); //сортировка команд по местам (по количесвту очков; при равенстве таковых по разнице забитых-пропущенных)

            int o = 1;
            foreach (TeamClass u in t) //отображение результатов турнира
            {
                n.teams[u.TeamNumber].PlaceInChamp = o;
                place_text[u.TeamNumber].text = o.ToString();
                o++;
            }
            Change_canvas(false, true, true,false);//отображение PanelMenu
        }
    }

    //функция очистки турнира
    private void Clear()
    {
        //очистка текстовых поле таблици-турнира

        for (int i=0; i<points_buttons.Count; i++)
        {
            points_buttons[i].enabled = true;
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
        my.GoalsInChamp = 0;
        my.MissedGoalsInChamp = 0;
        my.PointsInChamp = 0;
        my.PlaceInChamp = 0;
        my.WinsInChamp = 0;
        my.DefeatsInChamp = 0;
    }

    //изменение отображения Canvas
    void Change_canvas(bool f1, bool f2, bool f3, bool f4)
    {
        //GameObject t1 = GameObject.Find("Canvas_team") as GameObject;
        t1.SetActive(f1);
        t2.SetActive(f2);
        t3.SetActive(f3);
        t4.SetActive(f4);
    }

    //функция изменения показателей команды игрока после турнира 
    private void Change_my_team()
    {
        //измененеие количества побед/поражений
        my.NumberOfAllWins += my.WinsInChamp;
        my.NumberOfAllDefeats += my.DefeatsInChamp;

        //измененение бюджета команды игрока при занятии призового места
        if (my.PlaceInChamp==3)
            my.Budget += 10 * n.y;
        if (my.PlaceInChamp == 2)
            my.Budget += 20 * n.y;
        if (my.PlaceInChamp == 1)
            my.Budget += 30 * n.y;

    }

    //измененение тектовой информации о команде игрока  (на экране CanvasTeam)
    public void Write_about_team()
    {
        r.text= Math.Round(((my.TeamRating * 100) / 73), 1) + "%"; //подсчет рейтинга команды в процентах

        m.text = my.Budget.ToString();
        w.text = my.NumberOfAllWins.ToString();
        fa.text = my.NumberOfAllDefeats.ToString();
    }
} 
