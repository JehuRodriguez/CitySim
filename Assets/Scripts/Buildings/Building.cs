using UnityEngine;


public delegate void BuildingEventHandler(Building building);

public abstract class Building : MonoBehaviour
{
    [SerializeField] protected string buildingName = "Edificio";
    [SerializeField] protected int cost = 100;

    public string BuildingName => buildingName;
    public int Cost => cost;

    public static event BuildingEventHandler OnBuildingConstructed;

    protected virtual void Start()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnDayPassed += HandleDayPassed;

        OnBuildingConstructed?.Invoke(this);
    }

    protected virtual void OnDestroy()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnDayPassed -= HandleDayPassed;
    }

    private void HandleDayPassed(int day)
    {
        OnDayTick(day);
    }

    protected abstract void OnDayTick(int day);

    public abstract string GetInfo();

}
