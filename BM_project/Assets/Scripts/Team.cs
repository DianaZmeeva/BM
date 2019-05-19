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
        public double l; //рейтинг команды (лямбда в распределении)
        public int place; // место, занятое командой
        public int number; // номер команды
        public int goals; //  количество забитых мячей
        public int missed; // количество пропущенных мячей
        public string name; // Название команды
        public int money=100; //бюджет команды
        public int points; // количество очков в турнире
        public int wins_in_champ;
        public int failed_in_champ;
        public int wins;
        public int failed;


        public TeamClass(string n, double x, int num)
        {
            name = n;
            l = x;
            number = num;
        }
    }

    public class Champ //класс турнира 
    {
        public int val_game;
        public int y; 
        public List<Game> games = new List<Game>();
        public List<TeamClass> teams = new List<TeamClass>();
        public TeamClass myteam;

        public Champ(TeamClass t)
        {
            myteam = t;
            teams.Add(myteam);
        }

        public void GenerateTeams(int min, int max)
        {
            System.Random rnd = new System.Random();
            string[] country = { "Франция", "Германия", "США", "Италия", "Испания", "Бельгия", "Польша", "Китай", "Япония" }; ;
            for (int i = 0; i < 3; i++)
            {
                //TeamClass t = new TeamClass(country[rnd.Next(country.Length)], rnd.Next(30, 60));
                //t.name = country[rnd.Next(country.Length)];
                //t.l = rnd.Next(30, 60);
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
                team1.wins_in_champ++;
                team2.points += 1;
                team2.failed_in_champ++;

            }
            else
            {
                team2.points += 2;
                team2.wins_in_champ++;
                team1.points += 1;
                team1.failed_in_champ++;
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
    public GameObject t1,t2,t3,t4;
    public Button playChamp, back_to_team, statistic, to_champ, up;
    public Dropdown choose;
    public Text warning;

    public List<Text> t_text = new List<Text>();
    public List<Text> pl_text = new List<Text>();
    public List<Text> po_text = new List<Text>();
    public List<Text> w_text = new List<Text>();
    public List<Text> l_text = new List<Text>();
    public List<Text> g_text = new List<Text>();
    public List<Text> o_text = new List<Text>();

    public Text m, w, fa, r, money_text;


    // Start is called before the first frame update
    void Start()
    {
        System.Random rnd = new System.Random();
        Change_canvas(true, false, false, false);
        my = new TeamClass("Россия", rnd.Next(29,35), 0);
        Write_about_team();

        //Champ n = new Champ(my);
        //n.GenerateTeams();
        //Game g = new Game(n.teams[2], n.teams[3]);
        //g.PlayGame();
        //n.games.Add(g);

        //DontDestroyOnLoad(this.gameObject);
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

    private void Up_team(int i)
    {

        if (my.l == 73)
        {
            money_text.text = "Вы достигли максимума!";
            money_text.color = Color.red;
            up.enabled = false;
        }
        else
        {
            if (my.l < 40)
            {
                p(10,i);
            }
           else if (my.l < 50)
            {
                p(100,i);
            }
            else if(my.l<60)
            {
                p(1000,i);
            }
        }
        Write_about_team();
    }

    private void p(int v, int i)
    {
        if (my.money >= v || i==0)
        {
            my.money -= v;
            my.l += i;

            if((my.l==40 || my.l==50 || my.l==60) && (i==1))
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
                my.money += v;
            }
        }
        if (my.money < v && i==1)
        {
            money_text.text = "Недостаточно средств!";
            money_text.color = Color.red;
        }
    }

    private void Statistic()
    {
        Change_canvas(false, false, false, true);
        for (int i=0; i<n.teams.Count; i++)
        {
            t_text[i].text = n.teams[i].name;
            pl_text[i].text = n.teams[i].place.ToString();
            po_text[i].text = n.teams[i].points.ToString();
            w_text[i].text = n.teams[i].wins_in_champ.ToString();
            l_text[i].text = n.teams[i].failed_in_champ.ToString();
            g_text[i].text = n.teams[i].goals.ToString();
            o_text[i].text = n.teams[i].missed.ToString();
        }
    }

    private void Back_to_champ()
    {
        Change_canvas(false, true, true, false);
    }

    void Back()
    {
        Up_team(0);
        Change_my_team();
        Write_about_team();
        Clear();
        Change_canvas(true, false, false, false);
    }

    void StartChamp()
    {
        n = new Champ(my);

        int min=30, max=65;
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

            Change_canvas(false, true, false, false);
            n.GenerateTeams(min, max);

            for (int i = 0; i < n.teams.Count; i++)
            {
                team_text[i].text = n.teams[i].name;
            }
        }
        else
        {
            warning.text = "Choose level!";
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

        n.teams[i].goals += g.o1;
        n.teams[j].goals += g.o2;

        n.teams[j].missed += g.o1;
        n.teams[i].missed += g.o2;

        points_text[s].text = g.o1 + ":" + g.o2;
        points_text[f].text = g.o2 + ":" + g.o1;
        oh[i].text = n.teams[i].points.ToString();
        oh[j].text = n.teams[j].points.ToString();



        if (n.val_game == 6)
        {
            var t = n.teams.OrderByDescending(u => u.points).ThenByDescending(u => (u.goals - u.missed));

            int o = 1;
            foreach (TeamClass u in t)
            {
                n.teams[u.number].place = o;
                place_text[u.number].text = o.ToString();
                o++;
            }
            Change_canvas(false, true, true,false);
        }
    }

    private void Clear()
    {
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

        my.goals = 0;
        my.missed = 0;
        my.points = 0;
        my.place = 0;
        my.wins_in_champ = 0;
        my.failed_in_champ = 0;
    }

    void Change_canvas(bool f1, bool f2, bool f3, bool f4)
    {
        //GameObject t1 = GameObject.Find("Canvas_team") as GameObject;
        t1.SetActive(f1);
        t2.SetActive(f2);
        t3.SetActive(f3);
        t4.SetActive(f4);
    }

    private void Change_my_team()
    {
        my.wins += my.wins_in_champ;
        my.failed += my.failed_in_champ;

        if(my.place==3)
            my.money += 10 * n.y;
        if (my.place == 2)
            my.money += 20 * n.y;
        if (my.place == 1)
            my.money += 30 * n.y;

    }

     public void Write_about_team()
    {
        r.text= Math.Round(((my.l * 100) / 73), 1) + "%";
        m.text = my.money.ToString();
        w.text = my.wins.ToString();
        fa.text = my.failed.ToString();
    }
} 
