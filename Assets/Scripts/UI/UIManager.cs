using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private int money;
    private int day = 1;
    private int population;
    private int employed;
    private string selectedName = "-";
    private string message = "";
    private float messageTimer;

    private GUIStyle labelStyle;
    private GUIStyle buttonStyle;
    private GUIStyle messageStyle;

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
        money = EconomyManager.Instance.Money;
        day = TimeManager.Instance.CurrentDay;
        population = CitizenManager.Instance.Population;
        employed = CitizenManager.Instance.Employed;
        if (BuildManager.Instance.Selected != null)
            selectedName = BuildManager.Instance.Selected.BuildingName;

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
        if (messageTimer > 0f) messageTimer -= Time.deltaTime;
    }

    private void HandleMoneyChanged(int newMoney)
    {
        money = newMoney;
    }

    private void HandleDayPassed(int newDay)
    {
        day = newDay;
        employed = CitizenManager.Instance.Employed;
    }

    private void HandlePopulationChanged(int newPopulation)
    {
        population = newPopulation;
        employed = CitizenManager.Instance.Employed;
    }

    private void HandleSelectionChanged(Building building)
    {
        selectedName = building != null ? building.BuildingName : "-";
    }

    private void ShowMessage(string text)
    {
        message = text;
        messageTimer = 3f;
    }

    private Rect TopBarRect()
    {
        return new Rect(10, 10, Screen.width - 20, 50);
    }

    private Rect BottomBarRect()
    {
        return new Rect(10, Screen.height - 80, Screen.width - 20, 70);
    }

    public bool IsPointerOverUI(Vector2 screenPosition)
    {
        
        Vector2 gui = new Vector2(screenPosition.x, Screen.height - screenPosition.y);
        return TopBarRect().Contains(gui) || BottomBarRect().Contains(gui);
    }

    private void CreateStyles()
    {
        if (labelStyle != null) return;

        labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontSize = 20;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.alignment = TextAnchor.MiddleLeft;
        labelStyle.normal.textColor = Color.white;

        buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 18;

        messageStyle = new GUIStyle(GUI.skin.label);
        messageStyle.fontSize = 22;
        messageStyle.fontStyle = FontStyle.Bold;
        messageStyle.alignment = TextAnchor.MiddleCenter;
        messageStyle.normal.textColor = Color.yellow;
    }

    private void OnGUI()
    {
        CreateStyles();

        Rect top = TopBarRect();
        GUI.Box(top, GUIContent.none);
        GUI.Label(top,
            "  Dinero: $" + money +
            "     Día: " + day +
            "     Población: " + population +
            "     Empleados: " + employed +
            "     Elegido: " + selectedName,
            labelStyle);


        if (messageTimer > 0f)
        {
            GUI.Label(new Rect(0, Screen.height - 130, Screen.width, 40), message, messageStyle);
        }

        Rect bottom = BottomBarRect();
        GUI.Box(bottom, GUIContent.none);

        Building[] prefabs = BuildManager.Instance.Prefabs;
        if (prefabs == null || prefabs.Length == 0) return;

        float buttonWidth = (bottom.width - 10f * (prefabs.Length + 1)) / prefabs.Length;
        Color previous = GUI.backgroundColor;

        for (int i = 0; i < prefabs.Length; i++)
        {
            Rect r = new Rect(bottom.x + 10f + i * (buttonWidth + 10f), bottom.y + 10f, buttonWidth, bottom.height - 20f);

            GUI.backgroundColor = (i == BuildManager.Instance.SelectedIndex) ? Color.green : Color.white;

            string label = (i + 1) + ": " + prefabs[i].BuildingName + "  ($" + prefabs[i].Cost + ")";
            if (GUI.Button(r, label, buttonStyle))
            {
                BuildManager.Instance.Select(i);
            }
        }

        GUI.backgroundColor = previous;
    }
}
