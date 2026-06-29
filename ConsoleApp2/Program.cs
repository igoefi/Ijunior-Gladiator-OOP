using System;

public static class Program
{
    public static void Main()
    {
        List<WarriorFactory> factories = [
            new WarriorFactory(),
            new LuckierFactory(),
            new BerserkFactory(),
            new AssassinFactory()
            ];

        WarriorsFactory warriorsFactory = new(factories);
        Arena arena = new(new(warriorsFactory.Create()), new(warriorsFactory.Create()));
        arena.Fight();
    }
}
public interface IWarriorFactory
{
    public Warrior Create();
}

public class WarriorsFactory
{
    private const int WarriorsCount = 100;

    List<WarriorFactory> _factoies;

    public WarriorsFactory(List<WarriorFactory> factories) =>
        _factoies = factories;


    public List<Warrior> Create()
    {
        List<Warrior> list = new();

        for (int i = 0; i < WarriorsCount; i++)
            list.Add(_factoies[Utils.Random.Next(_factoies.Count)].Create());

        return list;
    }

}

public class WarriorFactory : IWarriorFactory
{
    private const int MinWarriorsHealth = 50;
    private const int MaxWarriorsHealth = 150;
    private const int MinWarriorsDamage = 20;
    private const int MaxWarriorsDamage = 60;

    public virtual Warrior Create()
    {
        return new Warrior(GetRandomHealth(), GetRandomDamage());
    }

    protected int GetRandomHealth() =>
        Utils.Random.Next(MinWarriorsHealth, MaxWarriorsHealth + 1);

    protected int GetRandomDamage() =>
        Utils.Random.Next(MinWarriorsDamage, MaxWarriorsDamage + 1);
}

public class LuckierFactory : WarriorFactory
{
    private const float MinLuckierMultiplyer = 0.8f;
    private const float MaxLuckierMultiplyer = 2.5f;
    public override Warrior Create()
    {
        return new Luckier(GetRandomHealth(),
                    GetRandomDamage(),
                    Utils.Random.Next((int)(MinLuckierMultiplyer * 100), (int)(MaxLuckierMultiplyer * 100)) / 100);
    }
}

public class BerserkFactory : WarriorFactory
{
    protected const int MinBerserkDamageEnemiesPerAttack = 2;
    protected const int MaxBerserkDamageEnemiesPerAttack = 5;

    public override Warrior Create()
    {
        return new Berserk(GetRandomHealth(),
                    GetRandomDamage(),
                    Utils.Random.Next(MinBerserkDamageEnemiesPerAttack, MaxBerserkDamageEnemiesPerAttack));
    }
}

public class AssassinFactory : BerserkFactory
{
    public override Warrior Create()
    {
        return new Assassin(GetRandomHealth(),
                    GetRandomDamage(),
                    Utils.Random.Next(MinBerserkDamageEnemiesPerAttack, MaxBerserkDamageEnemiesPerAttack));
    }
}

public class Arena
{
    private Army _firstArmy;
    private Army _secondArmy;

    public Arena(Army firstArmy, Army secondArmy)
    {
        _firstArmy = firstArmy;
        _secondArmy = secondArmy;
    }

    public void Fight()
    {
        while (_firstArmy.IsLive && _secondArmy.IsLive)
        {
            Console.Clear();
            Console.WriteLine(_firstArmy.GetWarriorsInfo());
            Console.WriteLine();
            Console.WriteLine(_secondArmy.GetWarriorsInfo());

            _firstArmy.AttackEnemies(_secondArmy.Warriors);
            _secondArmy.AttackEnemies(_firstArmy.Warriors);
            _firstArmy.RemoveDeadWarriors();
            _secondArmy.RemoveDeadWarriors();
            Console.ReadKey();
        }
    }
}

public class Army
{
    private List<Warrior> _warriors;

    public Army(List<Warrior> warriors) =>
        _warriors = warriors;

    public bool IsLive { get => _warriors.Count > 0; }
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

    public void RemoveDeadWarriors()
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
    }

    public char Symbol { get; protected set; }
    public bool IsLive { get => Health > 0;}

    public void TakeDamage(int damage) =>
        Health -= damage;

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

        for (int i = 0; i < CountDamageEnemiesByAttack; i++)
        {
            if (enemiesList.Count > 0)
                break;

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

