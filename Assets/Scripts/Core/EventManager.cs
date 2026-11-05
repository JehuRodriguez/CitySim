using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    [SerializeField] private int daysBetweenEvents = 6;
    [SerializeField] private int eventDuration = 3;

    
    private string[] eventNames =
    {
        "Festival: ingresos x2",
        "Recesión: ingresos a la mitad",
        "Turismo: ingresos x1.5"
    };

    private float[] eventMultipliers = { 2f, 0.5f, 1.5f };

    private int daysUntilNext;
    private int daysLeft;

    public event Action<string> OnEventChanged;

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
        daysUntilNext = daysBetweenEvents;
        TimeManager.Instance.OnDayPassed += HandleDayPassed;
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnDayPassed -= HandleDayPassed;
    }

    private void HandleDayPassed(int day)
    {
       
        if (daysLeft > 0)
        {
            daysLeft--;
            if (daysLeft == 0) EndEvent();
            return;
        }

        
        daysUntilNext--;
        if (daysUntilNext <= 0) StartRandomEvent();
    }

    private void StartRandomEvent()
    {
        int index = UnityEngine.Random.Range(0, eventNames.Length);
        EconomyManager.Instance.IncomeMultiplier = eventMultipliers[index];
        daysLeft = eventDuration;
        Debug.Log("Evento: " + eventNames[index]);
        OnEventChanged?.Invoke(eventNames[index]);
    }

    private void EndEvent()
    {
        EconomyManager.Instance.IncomeMultiplier = 1f;
        daysUntilNext = daysBetweenEvents;
        Debug.Log("Terminó el evento");
        OnEventChanged?.Invoke("");
    }

}
