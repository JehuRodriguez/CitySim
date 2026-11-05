using UnityEngine;
using System;

public class GoalManager : MonoBehaviour
{
    public static GoalManager Instance { get; private set; }

    [SerializeField] private int targetPopulation = 20;
    [SerializeField] private int targetMoney = 5000;

    public bool Reached { get; private set; }

    
    public event Action<string> OnProgressChanged;
    public event Action OnGoalReached;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        TimeManager.Instance.OnDayPassed += HandleDayPassed;
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnDayPassed -= HandleDayPassed;
    }

    public string GetProgressText()
    {
        return "Meta: " + CitizenManager.Instance.Population + "/" + targetPopulation +
               " habitantes y $" + EconomyManager.Instance.Money + "/" + targetMoney;
    }

    private void HandleDayPassed(int day)
    {
        if (Reached) return;

        OnProgressChanged?.Invoke(GetProgressText());

        bool enoughPeople = CitizenManager.Instance.Population >= targetPopulation;
        bool enoughMoney = EconomyManager.Instance.Money >= targetMoney;

        if (enoughPeople && enoughMoney)
        {
            Reached = true;
            Debug.Log("Meta cumplida en el día " + day);
            OnGoalReached?.Invoke();
        }
    }




}
