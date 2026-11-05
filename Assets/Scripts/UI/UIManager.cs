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

    //Imagen de cada boton//
    [SerializeField] private Image[] buttonImages;

    private float messageTimer;

    private void Start()
    {
        moneyText.text = "Dinero: $" + EconomyManager.Instance.Money;
        dayText.text = "Día: " + TimeManager.Instance.CurrentDay;
        populationText.text = "Población: " + CitizenManager.Instance.Population;
        employedText.text = "Empleados: " + CitizenManager.Instance.Employed;
        messageText.text = "";
        ShowSelection();

        EconomyManager.Instance.OnMoneyChanged += HandleMoneyChanged;
        TimeManager.Instance.OnDayPassed += HandleDayPassed;
        CitizenManager.Instance.OnPopulationChanged += HandlePopulationChanged;
        BuildManager.Instance.OnSelectionChanged += HandleSelectionChanged;
        BuildManager.Instance.OnMessage += ShowMessage;
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
    }

    private void Update()
    {
        if (messageTimer > 0f)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0f) messageText.text = "";
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
