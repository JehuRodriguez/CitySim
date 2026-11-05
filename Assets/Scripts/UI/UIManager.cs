using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //Textos de la barra superior//
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text populationText;
    [SerializeField] private TMP_Text employedText;
    [SerializeField] private TMP_Text selectedText;

    //Mensaje//
    [SerializeField] private TMP_Text messageText;

    //Eventos y metas//
    [SerializeField] private TMP_Text eventText;
    [SerializeField] private TMP_Text goalText;
    [SerializeField] private TMP_Text victoryText;

    //Imagen de cada boton//
    [SerializeField] private Image[] buttonImages;

    private float messageTimer;
    private float victoryTimer;


    private void Start()
    {
        moneyText.text = "Dinero: $" + EconomyManager.Instance.Money;
        dayText.text = "Día: " + TimeManager.Instance.CurrentDay;
        populationText.text = "Población: " + CitizenManager.Instance.Population;
        employedText.text = "Empleados: " + CitizenManager.Instance.Employed;
        messageText.text = "";
        eventText.text = "";
        victoryText.text = "";
        goalText.text = GoalManager.Instance.GetProgressText();
        ShowSelection();

        EconomyManager.Instance.OnMoneyChanged += HandleMoneyChanged;
        TimeManager.Instance.OnDayPassed += HandleDayPassed;
        CitizenManager.Instance.OnPopulationChanged += HandlePopulationChanged;
        BuildManager.Instance.OnSelectionChanged += HandleSelectionChanged;
        BuildManager.Instance.OnMessage += ShowMessage;
        EventManager.Instance.OnEventChanged += HandleEventChanged;
        GoalManager.Instance.OnProgressChanged += HandleGoalProgress;
        GoalManager.Instance.OnGoalReached += HandleGoalReached;
    }

    private void OnDestroy()
    {
        if (EconomyManager.Instance != null)
            EconomyManager.Instance.OnMoneyChanged -= HandleMoneyChanged;
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnDayPassed -= HandleDayPassed;
        if (CitizenManager.Instance != null)
            CitizenManager.Instance.OnPopulationChanged -= HandlePopulationChanged;
        if (BuildManager.Instance != null)
        {
            BuildManager.Instance.OnSelectionChanged -= HandleSelectionChanged;
            BuildManager.Instance.OnMessage -= ShowMessage;
        }
        if (EventManager.Instance != null)
            EventManager.Instance.OnEventChanged -= HandleEventChanged;
        if (GoalManager.Instance != null)
        {
            GoalManager.Instance.OnProgressChanged -= HandleGoalProgress;
            GoalManager.Instance.OnGoalReached -= HandleGoalReached;
        }
    }

    private void Update()
    {
        if (messageTimer > 0f)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0f) messageText.text = "";
        }

        if (victoryTimer > 0f)
        {
            victoryTimer -= Time.deltaTime;
            if (victoryTimer <= 0f) victoryText.text = "";
        }
    }

    private void HandleMoneyChanged(int money)
    {
        moneyText.text = "Dinero: $" + money;
    }

    private void HandleDayPassed(int day)
    {
        dayText.text = "Día: " + day;
        employedText.text = "Empleados: " + CitizenManager.Instance.Employed;
    }

    private void HandlePopulationChanged(int population)
    {
        populationText.text = "Población: " + population;
        employedText.text = "Empleados: " + CitizenManager.Instance.Employed;
    }

    private void HandleSelectionChanged(Building building)
    {
        ShowSelection();
    }

    private void HandleEventChanged(string eventName)
    {
        eventText.text = eventName == "" ? "" : "Evento: " + eventName;
    }

    private void HandleGoalProgress(string text)
    {
        goalText.text = text;
    }

    private void HandleGoalReached()
    {
        goalText.text = "¡Meta cumplida!";
        victoryText.text = "¡GANASTE!\nTu ciudad tiene " + CitizenManager.Instance.Population +
                           " habitantes y $" + EconomyManager.Instance.Money;
        victoryTimer = 8f;
    }

    private void ShowSelection()
    {
        Building selected = BuildManager.Instance.Selected;
        selectedText.text = selected != null ? "Elegido: " + selected.BuildingName : "Elegido: -";

        for (int i = 0; i < buttonImages.Length; i++)
        {
            bool isSelected = (i == BuildManager.Instance.SelectedIndex);
            buttonImages[i].color = isSelected ? Color.green : Color.white;
        }
    }

    private void ShowMessage(string text)
    {
        messageText.text = text;
        messageTimer = 3f;
    }
}
