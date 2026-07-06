public static class Program
{
    public static void Main()
    {
        List<AnimalBase> animalsList = [
            new Lion(),
            new Parrot(),
            new Turtle(),
            new Infocigan()
            ];
        AviariesFactory aviariesFactory = new AviariesFactory();


        Zoopark zoopark = new Zoopark(aviariesFactory.Create(animalsList));
        zoopark.Work();
    }
}

public class AnimalFactory
{
    public List<AnimalBase> CreateRandomCount(AnimalBase animalType, int randomMin, int randomMax)
    {
        var list = new List<AnimalBase>();
        int count = Utils.Random.Next(randomMin, randomMax);

        for (int i = 0; i < count; i++)
            list.Add(animalType.Multiply());

        return list;
    }
}

public class AviariesFactory
{
    private const int MinAnimalsCount = 3;
    private const int MaxAnimalsCount = 8;

    public List<Aviary> Create(List<AnimalBase> animals)
    {
        var animalsFactory = new AnimalFactory();
        var aviares = new List<Aviary>();

        foreach (var animal in animals)
        {
            aviares.Add(new(animalsFactory.CreateRandomCount(animal, MinAnimalsCount, MaxAnimalsCount)));
        }

        return aviares;
    }
}

public class Aviary
{
    private List<AnimalBase> _animals;

    public Aviary(List<AnimalBase> animals) =>
        _animals = animals;

    public override string ToString()
    {
        if (_animals == null || _animals.Count == 0)
            return $"В этом вальере никого нет";

        var animalRoster = "";

        foreach (var animal in _animals)
            animalRoster += animal.ToString() + "\n";

        return $"Здесь обитают животные вида: {_animals[0].Name}. Вот их список:\n{animalRoster}";
    }
}

public class Zoopark
{
    private List<Aviary> _aviaries;

    public Zoopark(List<Aviary> aviaries) =>
        _aviaries = aviaries;

    public void Work()
    {
        foreach (var aviary in _aviaries)
            Console.WriteLine(aviary.ToString());
    }
}

public class AnimalBase
{
    private string _name;
    private string _sex;
    private string _sound;

    protected AnimalBase(string sound, string name)
    {
        _sex = Utils.Random.NextDouble() > 0.5 ? "мужской" : "женский";
        _sound = sound;
        _name = name;
    }

    public string Name { get => _name; }

    public override string ToString() =>
        $"Пол - {_sex}, звук - {_sound}";

    public AnimalBase Multiply() =>
        new(_sound, _name);
}

public class Lion : AnimalBase
{
    public Lion() : base("Арррррр", "Лев") { }
}

public class Parrot : AnimalBase
{
    public Parrot() : base("Ку-ку ки-ки", "Попугай") { }
}

public class Turtle : AnimalBase
{
    public Turtle() : base("Кхххх", "Черепаха") { }
}

public class Infocigan : AnimalBase
{
    public Infocigan() : base("Ку-ку-купи курсы", "Инфоцыгане") { }
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

