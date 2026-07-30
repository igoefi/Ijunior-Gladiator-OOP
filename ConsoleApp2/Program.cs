public static class Program
{
    public static void Main()
    {
        const int CarsCount = 10;
        const int CarServiceMoney = 20000;

        var details = new List<Detail>()
        {
            new Detail("Подшибник", 20, false),
            new Detail("Датчик масла", 700, false),
            new Detail("Бензонасос", 5000, false),
            new Detail("Коленвал", 3000, false),
            new Detail("Ремень ГРВ", 1200, false),
            new Detail("Генератор", 9000, false),
            new Detail("Аккумулятор", 2500, false),
            new Detail("Блок управления", 7000, false),
            new Detail("Блок ABS", 4500, false),
            new Detail("Дроссельная заслонка", 800, false)
        };

        var cars = new CarsFactory().CreateCars(CarsCount, details);
        var servise = new CarService(cars, CarServiceMoney,
            new DetailsFactory().CreateDetailsForCarService(details));
        servise.Work();
    }
}

public class CarService
{
    const string LookStorageChoise = "1";
    const string RepairChoise = "2";
    const string DenyRepairChoise = "3";
    const int DenyRepairCost = 500;

    private List<Car> _cars;
    private int _money;
    private List<Detail> _storageDetails;

    public CarService(List<Car> cars, int money, List<Detail> details)
    {
        _cars = cars;
        _money = money;
        _storageDetails = details;
    }

    public void Work()
    {
        while (_cars.Count > 0)
        {
            Console.Clear();
            var car = _cars.First();

            Console.WriteLine($"Твой автосервис:\nДеньги: {_money}\n" +
                $"Стоимость отказа от машины:{DenyRepairCost}\nСледующая машина:");
            Console.WriteLine(car.ToString(false));


            string userChoise = Utils.GetUserInput($"\n{LookStorageChoise}) Посмотреть склад\n" +
                $"{RepairChoise}) Начать починку машины\n{DenyRepairChoise}) Отказаться от починки ({DenyRepairCost} р.)\n");

            switch (userChoise)
            {
                case LookStorageChoise:
                    ShowStorage();
                    break;

                case RepairChoise:
                    Repair(car);
                    break;

                case DenyRepairChoise:
                    DenyRepair(car, false);
                    break;
            }
        }

        Console.WriteLine("Машины закончились");
    }

    private void ShowStorage()
    {
        Console.Clear();

        foreach (var detail in _storageDetails)
            Console.WriteLine(detail.ToString());

        Console.ReadKey();
    }

    private void Repair(Car car)
    {
        Console.Clear();
        var brokenDetails = car.GetBrokenDetails();
        var repairDetailsReward = GetDetailsSumPrice(brokenDetails);

        Console.WriteLine($"Плата за починку машины - {car.RepairReward} + стоимость деталей ({repairDetailsReward})");
        while (brokenDetails != null)
        {
            Console.WriteLine(car.ToString(true)); 
            var denyRepairCost = GetDetailsSumPrice(brokenDetails);
            string userChoise = Utils.GetUserInput($"\n{LookStorageChoise}) Посмотреть склад\n" +
                $"{RepairChoise}) Починить деталь\n{DenyRepairChoise}) Отказаться от починки ({denyRepairCost} р.)\n");

            switch (userChoise)
            {
                case LookStorageChoise:
                    ShowStorage();
                    break;

                case RepairChoise:
                    if (RepairDetail(brokenDetails.First()))
                        Console.WriteLine("Успешно");
                    else
                        Console.WriteLine("Нет такой детали(");
                    break;

                case DenyRepairChoise:
                    DenyRepair(car, true);
                    return;
            }

            brokenDetails = car.GetBrokenDetails();
            if (brokenDetails.Count == 0)
                brokenDetails = null;
        }

        _cars.Remove(car);
        _money += repairDetailsReward + car.RepairReward;
        Console.WriteLine("Машина починена");
    }

    private bool RepairDetail(Detail carDetail)
    {
        foreach (var detail in _storageDetails)
            if (detail.Equals(carDetail))
            {
                carDetail.Repair();
                _storageDetails.Remove(detail);
                return true;
            }

        return false;
    }

    private int GetDetailsSumPrice(List<Detail> details)
    {
        var sum = 0;

        foreach (var detail in details)
            sum += detail.Cost;

        return sum;
    }

    private void DenyRepair(Car car, bool isCarInWork)
    {
        if (isCarInWork)
        {
            _money -= DenyRepairCost;
            _money -= GetDetailsSumPrice(car.GetBrokenDetails());
            _cars.Remove(car);
        }
        else
        {
            _money -= DenyRepairCost;
            _cars.Remove(car);
        }
    }
}

public class CarsFactory
{
    const int MaxCarRepairReward = 10000;

    public List<Car> CreateCars(int count, List<Detail> details)
    {
        var detailsFactory = new DetailsFactory();
        var cars = new List<Car>();

        for (int i = 0; i < count; i++)
        {
            cars.Add(new Car(detailsFactory.CreateDetailsForCars(details), Utils.Random.Next(MaxCarRepairReward)));
        }

        return cars;
    }
}

public class DetailsFactory()
{
    const double BrokeChanse = .5;
    const int MaxDetailsCount = 15;

    public List<Detail> CreateDetailsForCars(List<Detail> details)
    {
        var list = details.ToList();

        for (int i = 0; i < list.Count; i++)
        {
            int j = Utils.Random.Next(i + 1);
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;

            if (i == 0)
                list[i] = list[i].Clone(true);
            else
                if (Utils.Random.NextDouble() < BrokeChanse)
                    list[i] = list[i].Clone(true);
        }

        return list;
    }

    public List<Detail> CreateDetailsForCarService(List<Detail> details)
    {
        var list = new List<Detail>();

        foreach (var detail in details)
        {
            var count = Utils.Random.Next(MaxDetailsCount);

            for (int i = 0; i < count; i++)
                list.Add(detail.Clone(false));
        }

        return list;
    }


}

public class Car
{
    private List<Detail> _details;

    public Car(List<Detail> details, int repairReward)
    {
        _details = details;
        RepairReward = repairReward;
    }

    public int RepairReward { get; private set; }

    public List<Detail> GetBrokenDetails()
    {
        var list = new List<Detail>();

        foreach (Detail detail in _details)
            if (detail.IsBroke)
                list.Add(detail);

        return list;
    }

    public string ToString(bool onlyBrokenDetails)
    {
        var detailsInfo = "";

        var details = onlyBrokenDetails ? GetBrokenDetails() : _details;

        foreach (var detail in details)
            detailsInfo += detail.ToString() + "\n";

        return $"Награда за ремонт(не включая детали) - {RepairReward}.\nДетали:\n{detailsInfo}";
    }
}

public class Detail
{
    private string _name;
    private int _cost;

    public Detail(string name, int cost, bool isBroke)
    {
        _name = name;
        _cost = cost;
        IsBroke = isBroke;
    }
    public bool IsBroke { get; private set; }
    public int Cost { get { return _cost; } }

    public void Repair() =>
        IsBroke = false;

    public override string ToString()
    {
        string state = IsBroke ? "сломано" : "нормальное";
        return $"{_name}, состояние - {state}. Работающая стоит {_cost}";
    }

    public Detail Clone(bool isBroke) =>
        new Detail(_name, _cost, isBroke);

    public bool Equals(Detail detail) =>
        detail._name == _name && detail._cost == _cost;
}

public class Client
{
    private Car Car { get; set; }
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

