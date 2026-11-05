using UnityEngine;

public class House : Building,IResidence
{
    [SerializeField] private int capacity = 4;
    [SerializeField] private int taxPerDay = 20;

    public int Capacity => capacity;

    private void Reset()
    {
        buildingName = "Casa";
        cost = 200;
    }

    protected override void OnDayTick(int day)
    {
        EconomyManager.Instance.AddMoney(taxPerDay);
        Debug.Log("Casa paga impuestos: " + taxPerDay);
    }

    public override string GetInfo()
    {
        return buildingName + " - Capacidad: " + capacity;
    }
}
