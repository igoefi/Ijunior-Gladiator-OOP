public static class Program
{
    public static void Main()
    {
        List<Criminal> criiminals = new List<Criminal>()
        {
            new Criminal("Ефимов Игорь Денисович", false, 185, 100, "Энчпочмак"),
            new Criminal("Сакутин Роман Астольфович", false, 356, 230, "Протеанин"),
            new Criminal("Ефимов Денис Игоревич", false, 185, 100, "Энчпочмак"),
            new Criminal("Владимир Владимирович Владикавказович", true, 165, 100, "Русский")
        };

        var filtred = from Criminal criminal in criiminals
                      where criminal.Nationality == "Энчпочмак" &&
                      criminal.Height == 185 && criminal.Weight == 100
                      select criminal;
        Console.WriteLine("Все по запросу:");

        foreach (var criminal in filtred)
            Console.WriteLine($"{criminal.FullName}, {(criminal.IsOnTheArrest ? "арестован" : "на свободе")}, рост {criminal.Height}, вес {criminal.Weight}. " +
                $"Национальность {criminal.Nationality}");
    }
}

public struct Criminal(string fullName, bool isOnTheArrest, int height, float weight, string nationality)
{
    public readonly string FullName { get; } = fullName;
    public bool IsOnTheArrest { get; private set; } = isOnTheArrest;
    public readonly int Height { get; } = height;
    public readonly float Weight { get; } = weight;
    public readonly string Nationality { get; } = nationality;

    public void Arrest() =>
        IsOnTheArrest = true;
}
