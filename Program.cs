using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        // Количество команд в турнире
        int numTeams = 10;

        // Количество матчей для моделирования
        int numMatches = 1000;

        // Среднее количество голов, забитых одной командой за матч
        double lambda = 2.5;

        // Создаем генератор случайных чисел
        Random rand = new Random();

        // Список команд с их очками
        Dictionary<string, int> teams = new Dictionary<string, int>();
        for (int i = 0; i < numTeams; i++)
        {
            teams.Add($"Team {i + 1}", 0);
        }

        // Моделируем результаты матчей и обновляем турнирные таблицы
        for (int i = 0; i < numMatches; i++)
        {
            string homeTeam = $"Team {rand.Next(1, numTeams + 1)}";
            string awayTeam;
            do
            {
                awayTeam = $"Team {rand.Next(1, numTeams + 1)}";
            }
            while (homeTeam == awayTeam);

            int homeGoals = GetPoisson(lambda, rand);
            int awayGoals = GetPoisson(lambda, rand);

            // Обновляем очки команд по результатам матча
            if (homeGoals > awayGoals)
            {
                teams[homeTeam] += 3; // Победа домашней команды
            }
            else if (homeGoals < awayGoals)
            {
                teams[awayTeam] += 3; // Победа гостевой команды
            }
            else
            {
                teams[homeTeam] += 1; // Ничья
                teams[awayTeam] += 1;
            }
        }

        // Выводим турнирную таблицу
        Console.WriteLine("Турнирная таблица:");
        Console.WriteLine("=================");
        Console.WriteLine("Команда\t\tОчки");
        Console.WriteLine("=================");
        foreach (var team in teams)
        {
            Console.WriteLine($"{team.Key}\t\t{team.Value}");
        }
        Console.WriteLine("=================");

        // Определяем чемпиона
        int maxPoints = 0;
        string champion = "";
        foreach (var team in teams)
        {
            if (team.Value > maxPoints)
            {
                maxPoints = team.Value;
                champion = team.Key;
            }
        }

        Console.WriteLine($"Чемпион: {champion}");
    }

    // Функция для генерации количества голов с использованием распределения Пуассона
    static int GetPoisson(double lambda, Random rand)
    {
        double L = Math.Exp(-lambda);
        double p = 1.0;
        int k = 0;

        do
        {
            k++;
            p *= rand.NextDouble();
        }
        while (p > L);

        return k - 1;
    }
}
