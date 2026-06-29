public static class Program
{
    public static void Main()
    {
        Administrator dispatcher = new();

        while (true) 
        { 
            dispatcher.Work();
        }
    }
}


public class WarriorsFactory
{
    private const int WarriorsCount = 100;
    private const int MinWarriorsHealth = 50;
    private const int MaxWarriorsHealth = 150;
    private const int MinWarriorsDamage = 20;
    private const int MaxWarriorsDamage = 60;
    private const float MinLuckierMultiplyer = 0.8f;
    private const float MaxLuckierMultiplyer = 2.5f;
    private const int MinBerserkDamageEnemiesPerAttack = 2;
    private const int MaxBerserkDamageEnemiesPerAttack = 5;


    public List<Warrior> Create()
    {
        List<Warrior> list = new();

        for (int i = 0; i < WarriorsCount; i++)
            list.Add(CreateRandom());

        return list;
    }

    private Warrior CreateRandom()
    {
        int type = Utils.Random.Next(Enum.GetValues(typeof(WarriorType)).Length);
        Warrior warrior;
        Random random = Utils.Random;

        switch (type)
        {
            case (int)WarriorType.Warrior:
                return new Warrior(random.Next(MinWarriorsHealth, MaxWarriorsHealth + 1), 
                    random.Next(MinWarriorsDamage, MaxWarriorsDamage));

            case (int)WarriorType.Luckier:
                return new Luckier(random.Next(MinWarriorsHealth, MaxWarriorsHealth + 1),
                    random.Next(MinWarriorsDamage, MaxWarriorsDamage), 
                    random.Next((int)(MinLuckierMultiplyer * 100), (int)(MaxLuckierMultiplyer * 100)) / 100);

            case (int)WarriorType.Berserk:
                return new Berserk(random.Next(MinWarriorsHealth, MaxWarriorsHealth + 1),
                    random.Next(MinWarriorsDamage, MaxWarriorsDamage),
                    random.Next(MinBerserkDamageEnemiesPerAttack, MaxBerserkDamageEnemiesPerAttack));

            case (int)WarriorType.Assassin:
                return new Assassin(random.Next(MinWarriorsHealth, MaxWarriorsHealth + 1),
                    random.Next(MinWarriorsDamage, MaxWarriorsDamage),
                    random.Next(MinBerserkDamageEnemiesPerAttack, MaxBerserkDamageEnemiesPerAttack));
        }

        return null;
    }

    private enum WarriorType
    {
        Warrior = 0,
        Luckier = 1,
        Berserk = 2,
        Assassin = 3
    }
}


public class Administrator
{
    private Army _firstArmy;
    private Army _secondArmy;

    public void Work()
    {
        _firstArmy = new(new WarriorsFactory().Create());
        _secondArmy = new(new WarriorsFactory().Create());

        while (_firstArmy.IsLive && _secondArmy.IsLive)
        {
            Console.Clear();
            Console.WriteLine(_firstArmy.GetWarriorsInfo());
            Console.WriteLine();
            Console.WriteLine(_secondArmy.GetWarriorsInfo());

            _firstArmy.AttackEnemies(_secondArmy.Warriors);
            _secondArmy.AttackEnemies(_firstArmy.Warriors);
            _firstArmy.CheckLivedWarriors();
            _secondArmy.CheckLivedWarriors();
            Console.ReadKey();
        }
    }
}

public class Army
{
    private List<Warrior> _warriors;

    public Army(List<Warrior> warriors) =>
        _warriors = warriors;

    public bool IsLive { get => (_warriors.Count > 0); }
    public List<IDamagable> Warriors { get => _warriors.ToList<IDamagable>(); }


    public string GetWarriorsInfo()
    {
        string info = "";

        foreach (Warrior warrior in _warriors)
            info += warrior.Symbol;

        return info;
    }

    public void AttackEnemies(List<IDamagable> enemies)
    {
        foreach(Warrior warrior in _warriors)
            warrior.Attack(enemies);
    }

    public void CheckLivedWarriors()
    {
        List<Warrior> warriors = _warriors.ToList();

        foreach (Warrior warrior in warriors)
            if(warrior.IsLive == false)
                _warriors.Remove(warrior);
    }
}

public interface IDamagable
{
    public void TakeDamage(int damage);
}


public class Warrior : IDamagable
{
    protected int Health;
    protected int Damage;

    public Warrior(int health, int damage)
    {
        Health = health;
        Damage = damage;

        Symbol = 'W';
        IsLive = true;
    }

    public char Symbol { get; protected set; }
    public bool IsLive { get; private set; }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if(Health <= 0)
            IsLive = false;
    }

    public virtual void Attack(List<IDamagable> enemies) =>
        enemies[Utils.Random.Next(enemies.Count)].TakeDamage(Damage);
}

public class Luckier : Warrior
{
    private float _damageMultiplyer;

    public Luckier(int health, int damage, float damageMultiplyer) : base(health, damage)
    {
        _damageMultiplyer = damageMultiplyer;
        Symbol = 'L';
    }

    public override void Attack(List<IDamagable> enemies) =>
        enemies[Utils.Random.Next(enemies.Count)].TakeDamage((int)(Damage * _damageMultiplyer));
}

public class Berserk : Warrior
{
    protected int CountDamageEnemiesByAttack;

    public Berserk(int health, int damage, int countDamagableEnemiesByAttack) : base(health, damage)
    {
        CountDamageEnemiesByAttack = countDamagableEnemiesByAttack;
        Symbol = 'B';
    }

    public override void Attack(List<IDamagable> enemies)
    {
        List<IDamagable> enemiesList = enemies.ToList();

        for (int i = 0; i < CountDamageEnemiesByAttack && enemiesList.Count > 0; i++)
        {
            IDamagable enemy = enemiesList[Utils.Random.Next(enemiesList.Count)];
            enemy.TakeDamage(Damage);
            enemiesList.Remove(enemy);
        }
    }
}

public class Assassin : Berserk
{
    public Assassin(int health, int damage, int countDamagableEnemiesByAttack)
        : base(health, damage, countDamagableEnemiesByAttack)
    {
        Symbol = 'A';
    }

    public override void Attack(List<IDamagable> enemies)
    {
        for (int i = 0; i < CountDamageEnemiesByAttack; i++)
        {
            IDamagable enemy = enemies[Utils.Random.Next(enemies.Count)];
            enemy.TakeDamage(Damage);
        }
    }
}

public static class Utils
{
    public static readonly Random Random = new();
}

