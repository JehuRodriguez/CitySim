using UnityEngine;

public class Factory : Building,IWorkplace
{
    [SerializeField] private int jobs = 10;
    [SerializeField] private int salary = 50;
    [SerializeField] private int incomePerDay = 150;

    public int Jobs => jobs;
    public int Salary => salary;

    private void Reset()
    {
        buildingName = "Fábrica";
        cost = 500;
    }

    protected override void OnDayTick(int day)
    {
        EconomyManager.Instance.AddMoney(incomePerDay);
        Debug.Log("Fábrica produce: " + incomePerDay);
    }

    public override string GetInfo()
    {
        return buildingName + " - Empleos: " + jobs + " - Sueldo: " + salary;
    }

}
