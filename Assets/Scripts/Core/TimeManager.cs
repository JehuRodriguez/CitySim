using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [SerializeField] private float secondsPerDay = 5f;

    public int CurrentDay { get; private set; } = 1;

    public event Action<int> OnDayPassed;

    private float timer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= secondsPerDay)
        {
            timer = 0f;
            CurrentDay++;
            Debug.Log("Día " + CurrentDay);
            OnDayPassed?.Invoke(CurrentDay);
        }
    }
}
