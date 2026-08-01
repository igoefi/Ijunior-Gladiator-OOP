public static class Program
{
    public static void Main()
    {
        //Странное название тушенки)
        List<Stew> Stews = new List<Stew>()
        {
            new Stew("Ефимов Игорь Денисович",  2023, 5),
            new Stew("Сакутин Роман Астольфович", 2021, 10),
            new Stew("Ефимов Денис Игоревич", 2010, 45),
            new Stew("Владимир Владимирович Владикавказович", 1935, 72),
            new Stew("Ян Асильник Вольфович", 2010, 13),
            new Stew("Яна Цист Игоревна", 2026, 87),
            new Stew("Влад Адо Недов", 2000, 45),
            new Stew("Гоша Рубчинский Оный", 2011, 887),
            new Stew("Владимир Люберц Григорьевич", 2000, 21),
            new Stew("Григорий Люберц Эльфович", 2023, 3),
        };

        Console.WriteLine("\nВсё свежее:");
        Stews = Stews.Where(patient => patient.YearOfProduction + patient.ExpirationDate >= DateTime.Now.Year).ToList();

        foreach (var stew in Stews)
            Console.WriteLine(stew.ToString());
    }
}

public class Stew(string fullName, int yearOfProduction, int expirationDate)
{
    public string Name { get; } = fullName;
    public int YearOfProduction { get; } = yearOfProduction;
    public int ExpirationDate { get; } = expirationDate;

    public override string ToString() =>
        $"{Name}, год производства {YearOfProduction}, срок годности - {ExpirationDate}";
}
