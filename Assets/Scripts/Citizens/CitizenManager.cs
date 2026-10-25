using UnityEngine;
using System;
using System.Collections.Generic;

public class CitizenManager : MonoBehaviour
{
    public static CitizenManager Instance { get; private set; }


    private readonly List<Citizen> citizens = new List<Citizen>();
    private readonly Dictionary<IResidence, int> residents = new Dictionary<IResidence, int>();
    private readonly Dictionary<IWorkplace, int> employees = new Dictionary<IWorkplace, int>();
    private int citizenCounter;

    public Func<Citizen, int> SalaryCalculator { get; set; }
    public Func<Citizen, int> SpendingCalculator { get; set; }

    public event Action<int> OnPopulationChanged;

    public int Population => citizens.Count;

    public int Employed
    {
        get
        {
            int count = 0;
            foreach (Citizen c in citizens)
                if (c.HasJob) count++;
            return count;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;


        SalaryCalculator = c => c.HasJob ? c.Job.Salary : 0;
        SpendingCalculator = c => c.Wallet / 2;
    }

    private void OnEnable()
    {
        Building.OnBuildingConstructed += HandleBuildingConstructed;
    }

    private void OnDisable()
    {
        Building.OnBuildingConstructed -= HandleBuildingConstructed;
    }

    private void Start()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnDayPassed += HandleDayPassed;
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnDayPassed -= HandleDayPassed;
    }

    private void HandleBuildingConstructed(Building building)
    {
        if (building is IResidence home)
        {
            residents[home] = 0;
            for (int i = 0; i < home.Capacity; i++)
            {
                citizenCounter++;
                Citizen citizen = new Citizen("Ciudadano " + citizenCounter);
                citizen.Home = home;
                residents[home]++;
                citizens.Add(citizen);
            }
        }

        if (building is IWorkplace work)
        {
            employees[work] = 0;
        }

        AssignJobs();
        OnPopulationChanged?.Invoke(Population);
        Debug.Log("Población: " + Population + " - Empleados: " + Employed);
    }

    private void AssignJobs()
    {
        foreach (Citizen citizen in citizens)
        {
            if (citizen.HasJob) continue;

            foreach (KeyValuePair<IWorkplace, int> entry in employees)
            {
                if (entry.Value < entry.Key.Jobs)
                {
                    citizen.Job = entry.Key;
                    employees[entry.Key] = entry.Value + 1;
                    break;
                }
            }
        }
    }

    private void HandleDayPassed(int day)
    {
        int totalSpent = 0;

        foreach (Citizen citizen in citizens)
        {
            citizen.Earn(SalaryCalculator(citizen));

            int spending = SpendingCalculator(citizen);
            if (citizen.TrySpend(spending))
                totalSpent += spending;
        }

        if (totalSpent > 0)
            EconomyManager.Instance.AddMoney(totalSpent);

        Debug.Log("Día " + day + " - Los ciudadanos gastaron: " + totalSpent);
    }
}
