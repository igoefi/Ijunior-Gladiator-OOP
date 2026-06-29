public static class Program
{
    public static void Main()
    {
        Administrator dispatcher = new();
        dispatcher.Work();
    }
}

public class ClientFactory
{
    public Stack<Client> Create(int count, int maxProductsCount, int maxMoney, List<Product> products)
    {
        Stack<Client> clientsStack = new Stack<Client>();

        for (int i = 0; i < count; i++)
        {
            List<Product> addProducts = new List<Product>();

            for (int j = 0; j < Utils.Random.Next(maxProductsCount); j++)
                addProducts.Add(products[Utils.Random.Next(1, products.Count)]);

            clientsStack.Push(new Client(addProducts, Utils.Random.Next(maxMoney + 1)));
        }

        return clientsStack;
    }
}

public class ProductFactory
{
    public List<Product> Create()
    {
        List<Product> products =
        [
            new Product("Жевачка", 10),
            new Product("Молоко", 100),
            new Product("Хлопья", 80),
            new Product("Сыр", 150),
            new Product("Пепси-кола", 199),
            new Product("Вода", 60),
            new Product("Чипсики", 120),
            new Product("Салфетки", 70),
        ];

        return products;
    }
}


public class Administrator
{
    private const int MaxClientsCount = 10;
    private const int MaxClientsProductsCount = 15;
    private const int MaxClientsMoney = 2000;

    private Stack<Client> _clients;
    private List<Product> _products;
    private int _money;

    public Administrator()
    {
        _products = new ProductFactory().Create();
        _clients = new ClientFactory().Create(MaxClientsCount, MaxClientsProductsCount, MaxClientsMoney, _products);
        _money = 0;
    }

    public void Work()
    {
        while(_clients.Count > 0)
        {
            Console.ReadKey();
            Client client = _clients.Pop();
            int income = client.BuyProducts();

            Console.WriteLine($"Клиент купил на сумму {income}. Вот какие товары купил:");

            foreach(Product product in client.ProductsInBag)
                Console.WriteLine($"{product.Name} за {product.Cost} деняг");

            Console.WriteLine();
            _money += income;
        }

        Console.WriteLine($"Клиенты закончились. На счету магазина {_money} денег");
    }
}

public class Client
{
    private List<Product> _productsInBasket;
    private List<Product> _productsInBag;
    private int _money;

    public Client(List<Product> productsInBasket, int money)
    {
        _productsInBasket = productsInBasket;
        _money = money;
    }

    public List<Product> ProductsInBag { get =>  _productsInBasket.ToList(); }

    public int BuyProducts()
    {
        while(GetProductsCost() > _money)
            DeleteRandomProductInBasket();

        int cost = GetProductsCost();
        _money -= cost;
        _productsInBag = _productsInBasket.ToList();
        _productsInBasket.Clear();
        return cost;
    }

    private void DeleteRandomProductInBasket() =>
        _productsInBasket.Remove
            (_productsInBasket[Utils.Random.Next(_productsInBasket.Count)]);

    private int GetProductsCost()
    {
        int cost = 0;

        foreach(Product product in _productsInBasket)
            cost += product.Cost;

        return cost;
    }
}

public struct Product(string name, int cost)
{
    public string Name { get; private set; } = name;
    public int Cost { get; private set; } = cost;
}

public static class Utils
{
    public static readonly Random Random = new();

    public static string GetUserInput(string text)
    {
        Console.WriteLine(text);
        return Console.ReadLine();
    }

    public static bool GetNumFromUser(out int num)
    {
        if (int.TryParse(Console.ReadLine(), out num))
            return true;

        Console.WriteLine("Некоректный ввод");
        return false;
    }
}

