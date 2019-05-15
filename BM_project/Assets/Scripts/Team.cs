using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Team : MonoBehaviour
{
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


        public TeamClass(string n, double x)
        {
            name = n;
            l = x;
        }
    }

    public class Champ //класс турнира 
    {
        public List<Game> games =new List<Game>();
        public List<TeamClass> teams =new List<TeamClass>();
        public TeamClass myteam;

        public Champ(TeamClass t)
        {
            myteam = t;
            teams.Add(myteam);
        }

        public void GenerateTeams()
        {
            string[] country = {"Франция","Германия", "США", "Италия", "Испания", "Бельгия", "Польша", "Китай", "Япония" }; ;
            for (int i=0; i < 3; i++)
            {
                System.Random rnd = new System.Random();
                //TeamClass t = new TeamClass(country[rnd.Next(country.Length)], rnd.Next(30, 60));
                //t.name = country[rnd.Next(country.Length)];
                //t.l = rnd.Next(30, 60);
                teams.Add(new TeamClass(country[rnd.Next(country.Length)], rnd.Next(30, 60)));
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

            for (int i=0; i<100; i++)
            {
                x1 = Puasson(team1.l, i);
                x2 = Puasson(team2.l, i);
                if (x1 > 0 && x1<=1)
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
        }

        private int Goal_Generate(Dictionary<int, double> p1)
        {
            System.Random rnd = new System.Random();
            double x1 = rnd.NextDouble();
            int i = -1;
            while (x1 > 0)
            {
                i++;
                if ( p1.ContainsKey(i))
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


    // Start is called before the first frame update
    void Start()
    {
        TeamClass my = new TeamClass("Россия", 60);
        Champ n = new Champ(my);
        n.GenerateTeams();
        Game g = new Game(n.teams[2], n.teams[3]);
        g.PlayGame();
        n.games.Add(g);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
