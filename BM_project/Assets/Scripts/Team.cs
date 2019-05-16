using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Team : MonoBehaviour
{
    public Button playChamp;


    public class TeamClass //класс команды 
    {
        public double l; //рейтинг команды (лямбда в распределении)
        public double attack;// коэффицент атаки
        public double defence; //  коэффицент защиты
        public int n; // количество матчей 
        public int goals; //  количество пропущенных мячей
        public int missed; // количество пропущенных мячей
        public string name; // Название команды
        public int money; //бюджет команды
        public int points; // количесвто очков в турнире


        public TeamClass(string n, double x)
        {
            name = n;
            l = x;
        }

    }

    public class Champ //класс турнира 
    {
        public int val_game;
        public List<Game> games = new List<Game>();
        public List<TeamClass> teams = new List<TeamClass>();
        public TeamClass myteam;

        public Champ(TeamClass t)
        {
            myteam = t;
            teams.Add(myteam);
        }

        public void GenerateTeams()
        {
            System.Random rnd = new System.Random();
            string[] country = { "Франция", "Германия", "США", "Италия", "Испания", "Бельгия", "Польша", "Китай", "Япония" }; ;
            for (int i = 0; i < 3; i++)
            {
                //TeamClass t = new TeamClass(country[rnd.Next(country.Length)], rnd.Next(30, 60));
                //t.name = country[rnd.Next(country.Length)];
                //t.l = rnd.Next(30, 60);
                teams.Add(new TeamClass(country[rnd.Next(country.Length)] + "_" + (i + 1), rnd.Next(30, 60)));
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

            for (int i = 0; i < 100; i++)
            {
                x1 = Puasson(team1.l, i);
                x2 = Puasson(team2.l, i);
                if (x1 > 0 && x1 <= 1)
                {
                    p1.Add(i, x1);
                }
                if (x2 > 0 && x2 <= 1)
                {
                    p2.Add(i, x2);
                }
            }

            o1 = Goal_Generate(p1);
            o2 = Goal_Generate(p2);
            if (o1 == 0)
                o2 = 20;
            if (o2 == 0)
                o1 = 20;
            if (o1 == o2)
            {
                System.Random rnd = new System.Random();
                x1 = rnd.NextDouble();
                if (x1 <= 0.5)
                    o1 += 2;
                else
                    o2 += 2;
            }
            if (o1 > o2)
            {
                team1.points += 2;
                team2.points += 1;

            }
            else
            {
                team2.points += 2;
                team1.points += 1;
            }
        }

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

        public double Puasson(double l, int m)
        {
            double x = (Math.Pow(l, m) / F(m)) * Math.Exp(-l);
            return x;
        }

        public double F(int z)
        {
            double s = 1;
            for (int i = z; i > 1; i--)
                s *= i;
            return s;
        }
    }

    TeamClass my;
    Champ n;
    public List<Button> points_buttons = new List<Button>();
    public List<Text> points_text = new List<Text>();
    public List<Text> oh = new List<Text>();
    public List<Text> team_text = new List<Text>();
    public List<Text> place_text = new List<Text>();
    public GameObject t1,t2;


    // Start is called before the first frame update
    void Start()
    {
        Change_canvas(true, false);
        my = new TeamClass("Россия", 30);
        //Champ n = new Champ(my);
        //n.GenerateTeams();
        //Game g = new Game(n.teams[2], n.teams[3]);
        //g.PlayGame();
        //n.games.Add(g);

        //DontDestroyOnLoad(this.gameObject);
        Button start = playChamp.GetComponent<Button>();
        start.onClick.AddListener(StartChamp);
        points_buttons[0].onClick.AddListener(() => play(0, 1, 0, 3, 0));
        points_buttons[1].onClick.AddListener(() => play(0, 2, 1, 6, 1));
        points_buttons[2].onClick.AddListener(() => play(0, 3, 2, 9, 2));
        points_buttons[3].onClick.AddListener(() => play(1, 2, 3, 7, 4));
        points_buttons[4].onClick.AddListener(() => play(1, 3, 4,10, 5));
        points_buttons[5].onClick.AddListener(() => play(2, 3, 5, 11, 8));
    }

    void StartChamp()
    {
        Change_canvas(false, true);
        n = new Champ(my);
        n.GenerateTeams();

        for (int i=0; i<n.teams.Count;i++)
        {
            team_text[i].text = n.teams[i].name;
        }

        //SceneManager.LoadScene("Champ");
    }

    void play(int i, int j, int b, int f, int s)
    {
        points_buttons[b].enabled = false;

        Game g = new Game(n.teams[i], n.teams[j]);
        g.PlayGame();
        n.games.Add(g);
        n.val_game++;


        points_text[s].text = g.o1 + ":" + g.o2;
        points_text[f].text = g.o2 + ":" + g.o1;
        oh[i].text = n.teams[i].points.ToString();
        oh[j].text = n.teams[j].points.ToString();
    }

    void Change_canvas(bool f1, bool f2)
    {
        GameObject temp = GameObject.Find("Canvas_team") as GameObject;
        t1.SetActive(f1);

        GameObject temp2 = GameObject.Find("Canvas_Champ") as GameObject;
        t2.SetActive(f2);
    }
}
