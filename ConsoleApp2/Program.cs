public static class Program
{
    public static void Main()
    {
        List<Player> patients = new List<Player>()
        {
            new Player("Ефимов Игорь Денисович",  20, 5),
            new Player("Сакутин Роман Астольфович", 30, 10),
            new Player("Ефимов Денис Игоревич", 42, 45),
            new Player("Владимир Владимирович Владикавказович", 100, 72),
            new Player("Ян Асильник Вольфович", 13, 13),
            new Player("Яна Цист Игоревна", 14, 87),
            new Player("Влад Адо Недов", 30, 45),
            new Player("Гоша Рубчинский Оный", 32, 887),
            new Player("Владимир Люберц Григорьевич", 3, 21),
            new Player("Григорий Люберц Эльфович", 5, 3),
        };

        var topCount = 3;

        Console.WriteLine("Топ 3 по уровню");
        patients = patients.OrderByDescending(patient => patient.Level).ToList();

        for (int i = 0; i < topCount; i++)
            Console.WriteLine(patients[i]);

        Console.WriteLine("\nСортировка по силе");
        patients = patients.OrderByDescending(patient => patient.Strenght).ToList();

        for (int i = 0; i < topCount; i++)
            Console.WriteLine(patients[i]);
    }
}

public class Player(string fullName, int age, int disease)
{
    public string FullName { get; } = fullName;
    public int Level { get; } = age;
    public int Strenght { get; } = disease;

    public override string ToString() =>
        $"{FullName}, уровень {Level}, сила - {Strenght}";
}
