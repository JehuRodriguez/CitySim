using UnityEngine;

public class Park : Building
{
    [SerializeField] private int upkeepPerDay = 10;
    [SerializeField] private int happinessBonus = 5;

    public int HappinessBonus => happinessBonus;


    private void Reset()
    {
        buildingName = "Parque";
        cost = 150;
    }

    protected override void OnDayTick(int day)
    {
        EconomyManager.Instance.TrySpend(upkeepPerDay);
        Debug.Log("Parque cuesta mantenimiento: " + upkeepPerDay);
    }

    public override string GetInfo()
    {
        return buildingName + " - Felicidad: +" + happinessBonus;
    }

}
