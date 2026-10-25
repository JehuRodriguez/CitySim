using UnityEngine;

public class Shop : Building,IWorkplace
{
    [SerializeField] private int jobs = 3;
    [SerializeField] private int salary = 40;
    [SerializeField] private int salesPerDay = 60;

    public int Jobs => jobs;
    public int Salary => salary;

    private void Reset()
    {
        buildingName = "Tienda";
        cost = 300;
    }

    protected override void OnDayTick(int day)
    {
        EconomyManager.Instance.AddMoney(salesPerDay);
        Debug.Log("Tienda vende: " + salesPerDay);
    }

    public override string GetInfo()
    {
        return buildingName + " - Empleos: " + jobs;
    }
}
