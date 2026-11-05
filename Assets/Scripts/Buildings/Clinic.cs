using UnityEngine;

public class Clinic : Building, IWorkplace
{
    [SerializeField] private int jobs = 3;
    [SerializeField] private int salary = 60;
    [SerializeField] private int upkeepPerDay = 40;

    public int Jobs => jobs;
    public int Salary => salary;

    private void Reset()
    {
        buildingName = "Posta médica";
        cost = 600;
    }

    protected override void OnDayTick(int day)
    {
        EconomyManager.Instance.TrySpend(upkeepPerDay);
        Debug.Log("Posta médica cuesta mantenimiento: " + upkeepPerDay);
    }

    public override string GetInfo()
    {
        return buildingName + " - Empleos: " + jobs + " - Sueldo: " + salary;
    }
}
