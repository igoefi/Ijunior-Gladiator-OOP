public static class Program
{
    public static void Main()
    {
        List<Patient> patients = new List<Patient>()
        {
            new Patient("Ефимов Игорь Денисович",  20, "Рак"),
            new Patient("Сакутин Роман Астольфович", 30, "Большой член"),
            new Patient("Ефимов Денис Игоревич", 42, "Рак"),
            new Patient("Владимир Владимирович Владикавказович", 100, "Импотенция"),
            new Patient("Ян Асильник Вольфович", 13, "Сексофобия"),
            new Patient("Яна Цист Игоревна", 14, "Шизофриния"),
            new Patient("Влад Адо Недов", 30, "Импотенция"),
            new Patient("Гоша Рубчинский Оный", 32, "Большой член"),
            new Patient("Владимир Люберц Григорьевич", 3, "Импотенция"),
            new Patient("Григорий Люберц Эльфович", 5, "Большой член"),
        };

        var needDisease = "Большой член";

        Console.WriteLine("Сортировка по ФИО");
        patients = patients.OrderBy(patient => patient.FullName).ToList();

        foreach (var patient in patients)
            Console.WriteLine(patient.ToString());

        Console.WriteLine("\nСортировка по возрасту");
        patients = patients.OrderBy(patient => patient.Age).ToList();

        foreach (var patient in patients)
            Console.WriteLine(patient.ToString());

        Console.WriteLine($"\nСортировка по заболеванию {needDisease}");
        patients = patients.Where(patient => patient.Disease == needDisease).ToList();

        foreach (var patient in patients)
            Console.WriteLine(patient.ToString());
    }
}

public class Patient(string fullName, int age, string disease)
{
    public string FullName { get; } = fullName;
    public int Age { get; } = age;
    public string Disease { get; } = disease;

    public override string ToString() =>
        $"{FullName}, возраст {Age}, болезнь - {Disease}";
}
