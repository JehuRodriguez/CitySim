using UnityEngine;

public class Restaurant : Building, IWorkplace
{
    [SerializeField] private int jobs = 4;
    [SerializeField] private int salary = 45;
    [SerializeField] private int salesPerDay = 90;

    public int Jobs => jobs;
    public int Salary => salary;

    private void Reset()
    {
        buildingName = "Restaurante";
        cost = 400;
    }

    protected override void OnDayTick(int day)
    {
        EconomyManager.Instance.AddMoney(salesPerDay);
        Debug.Log("Restaurante vende: " + salesPerDay);
    }

    public override string GetInfo()
    {
        return buildingName + " - Empleos: " + jobs + " - Sueldo: " + salary;
    }

}
