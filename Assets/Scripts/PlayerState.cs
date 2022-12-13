public class PlayerState
{
    public int Funds { get; private set; }
    public int Score { get; private set; }

    public PlayerState()
    {
        Funds = 0;
        Score = 0;
    } 

    public void AddScore(int amount)
    {
        Score += amount;
    }

    public void AddFunds(int amount)
    {
        Funds += amount;
    }

    public bool TryTakeFunds(int amount)
    {
        if (Funds < amount)
        {
            return false;
        }
        Funds -= amount;
        return true;
    }
}
