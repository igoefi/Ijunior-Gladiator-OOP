public static class Program
{
    public static void Main()
    {
        //Странное название тушенки)
        List<Soldier> soldiers1 = new List<Soldier>()
        {
            new Soldier("Ефимов Игорь Денисович",  "Меч", "Гвардии рыцарь-полковник", 1005),
            new Soldier("Сакутин Роман Астольфович",  "Слово", "Гвардии рыцарь-генерал доты", 10000),
            new Soldier("Ефимов Денис Игоревич",  "Танк", "Гвардии рыцарь-генерал", 500),
            new Soldier("Бладимир Владимирович Владикавказович",  "Нож", "Гвардии младший рядовой", 2),
            new Soldier("Бан Асильник Вольфович",  "Автомат", "Гвардии рыцарь-полковник", 30),
        };
        List<Soldier> soldiers2 = new List<Soldier>();

        var selectedChar = 'Б';

        Console.WriteLine("\nПеревод:");
        var temp = soldiers1.Where(soldier => soldier.Name[0] == selectedChar).ToList(); ;
        soldiers1 = soldiers1.Except(soldiers2).ToList();
        soldiers2 = soldiers2.Union(temp).ToList();

        foreach (var soldier in soldiers2)
            Console.WriteLine($"{soldier.Name}, звание {soldier.Rank}");
    }
}

public class Soldier
{
    public Soldier(string name, string weapon, string rank, int serviceLife)
    {
        Name = name;
        Weapon = weapon;
        Rank = rank;
        ServiceLife = serviceLife;
    }

    public string Name { get; }
    public string Weapon { get; }
    public string Rank { get; }
    public int ServiceLife { get; }
}
