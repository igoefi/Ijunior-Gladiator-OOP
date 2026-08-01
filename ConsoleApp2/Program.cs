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

        var needHeight = 185;
        var needWeigth = 100;
        var needNationality = "Энчпочмак";

        var filtred = criiminals.Where(criminal => criminal.Nationality == needNationality && criminal.Height == needHeight
        && criminal.Weight == needHeight && criminal.IsOnTheArrest == false);
        Console.WriteLine("Все по запросу:");

        foreach (var criminal in filtred)
        {
            var arrestString = criminal.IsOnTheArrest ? "арестован" : "на свободе";
            Console.WriteLine($"{criminal.FullName}, {arrestString}, рост {criminal.Height}, вес {criminal.Weight}. " +
                $"Национальность {criminal.Nationality}");
        }
    }
}

public class Criminal(string fullName, bool isOnTheArrest, int height, float weight, string nationality)
{
    public string FullName { get; } = fullName;
    public bool IsOnTheArrest { get; private set; } = isOnTheArrest;
    public int Height { get; } = height;
    public float Weight { get; } = weight;
    public string Nationality { get; } = nationality;

    public void Arrest() =>
        IsOnTheArrest = true;
}
