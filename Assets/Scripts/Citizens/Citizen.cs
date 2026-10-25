public class Citizen
{
    public string Name { get; }
    public IResidence Home { get; set; }
    public IWorkplace Job { get; set; }
    public int Wallet { get; private set; }

    public bool HasJob => Job != null;

    public Citizen(string name)
    {
        Name = name;
    }

    public void Earn(int amount)
    {
        if (amount > 0) Wallet += amount;
    }

    public bool TrySpend(int amount)
    {
        if (amount <= 0 || Wallet < amount) return false;
        Wallet -= amount;
        return true;
    }
}