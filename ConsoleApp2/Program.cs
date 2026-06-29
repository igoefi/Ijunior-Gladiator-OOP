using System;

public static class Program
{
    public static void Main()
    {
        Aquarium aquarium = new Aquarium();
        aquarium.Live();
    }
}

public class FishFactory
{
    private const int MinFishAge = 5;
    private const int MaxFishAge = 15;

    public Fish Create() =>
        new(Utils.Random.Next(MinFishAge, MaxFishAge));
}

public class Aquarium
{
    const string AddFishChoise = "1";
    const string DeleteFishChoise = "2";
    const string ExitChoise = "0";

    FishFactory _factory;
    List<Fish> _fishList;

    public Aquarium()
    {
        _factory = new FishFactory();
        _fishList = new List<Fish>();
    }

    public void Live()
    {
        bool isWork = true;

        while (isWork)
        {
            Console.Clear();
            ShowFishes();
            string choise = Utils.GetUserInput($"{AddFishChoise} - Добавить случайную рыбу\n{DeleteFishChoise} - Удалить случайную рыбу\n" +
                $"{ExitChoise} - Выход\nЛюбая другая кнопка - пропустить ход\n");

            switch (choise)
            {
                case AddFishChoise:
                    AddFish();
                    break;

                case DeleteFishChoise:
                    DeleteFish();
                    break;

                case ExitChoise:
                    isWork = false;
                    break;

                default:
                    break;
            }
            AddFishAge();
            DeleteDeadFishes();
        }
    }

    private void AddFish() =>
        _fishList.Add(_factory.Create());

    private void DeleteFish()
    {
        if (_fishList.Count == 0)
            return;

        _fishList.Remove(_fishList[Utils.Random.Next(_fishList.Count)]);
    }

    private void DeleteDeadFishes()
    {
        List<Fish> fishList = _fishList.ToList();

        foreach (Fish fish in fishList)
            if(fish.IsLive == false)
                _fishList.Remove(fish);
    }

    private void ShowFishes()
    {
        Console.WriteLine("Список рыб:");

        for (int i = 0; i < _fishList.Count; i++)
        {
            Console.WriteLine($"{i}) {_fishList[i].ToString()}");
        }

        Console.WriteLine();
    }

    private void AddFishAge()
    {
        foreach(Fish fish in _fishList)
            fish.AddOneAge();
    }
}

public class Fish
{
    private int _age;
    private int _maxAge;

    public Fish(int maxAge)
    {
        _maxAge = maxAge;
        _age = 0;
    }

    public bool IsLive { get => _age <= _maxAge; }

    public void AddOneAge() =>
        _age++;

    public override string ToString() =>
        $"Возраст {_age}/{_maxAge}";
}

public static class Utils
{
    public static string GetUserInput(string text)
    {
        Console.WriteLine(text);
        return Console.ReadLine();
    }

    public static readonly Random Random = new();
}

