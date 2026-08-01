public static class Program
{
    public static void Main()
    {
        //Странное название тушенки)
        List<Soldier> soldiers = new List<Soldier>()
        {
            new Soldier("Ефимов Игорь Денисович",  "Меч", "Гвардии рыцарь-полковник", 1005),
            new Soldier("Сакутин Роман Астольфович",  "Слово", "Гвардии рыцарь-генерал доты", 10000),
            new Soldier("Ефимов Денис Игоревич",  "Танк", "Гвардии рыцарь-генерал", 500),
            new Soldier("Владимир Владимирович Владикавказович",  "Нож", "Гвардии младший рядовой", 2),
            new Soldier("Ян Асильник Вольфович",  "Автомат", "Гвардии рыцарь-полковник", 30),
        };

        Console.WriteLine("\nТолько имена и звания:");
        var selected = soldiers.Select(soldier => (soldier.Name, soldier.Rank)).ToList(); ;

        foreach (var soldier in selected)
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
