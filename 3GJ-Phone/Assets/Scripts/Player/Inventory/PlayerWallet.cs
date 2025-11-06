using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    public static PlayerWallet instance;
    public int currency = 500;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public bool Spend(int amount)
    {
        if (currency >= amount)
        {
            currency -= amount;
            return true;
        }
        return false;
    }

    public void Add(int amount)
    {
        currency += amount;
    }
}