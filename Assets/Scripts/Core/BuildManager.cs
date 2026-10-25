using UnityEngine;
using System;
using System.Collections.Generic;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    [SerializeField] private Building[] buildingPrefabs;
    [SerializeField] private float cellSize = 2f;
    [SerializeField] private int minCell = -5;
    [SerializeField] private int maxCell = 4;


    private readonly HashSet<Vector2Int> occupied = new HashSet<Vector2Int>();
    private Camera cam;
    private int selectedIndex;

    
    public event Action<Building> OnSelectionChanged;

    public Building Selected =>
       (buildingPrefabs != null && buildingPrefabs.Length > 0) ? buildingPrefabs[selectedIndex] : null;

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
        cam = Camera.main;
        OnSelectionChanged?.Invoke(Selected);
    }

    private void Update()
    {
       
        for (int i = 0; i < buildingPrefabs.Length; i++)
        {
            if (NumberKeyPressed(i)) Select(i);
        }

        if (LeftClickPressed()) TryBuild();
    }

    public void Select(int index)
    {
        if (index < 0 || index >= buildingPrefabs.Length) return;
        selectedIndex = index;
        Debug.Log("Seleccionado: " + Selected.BuildingName + " (costo " + Selected.Cost + ")");
        OnSelectionChanged?.Invoke(Selected);
    }

    private void TryBuild()
    {
        if (cam == null || Selected == null) return;

        Ray ray = cam.ScreenPointToRay(PointerPosition());
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        if (!ground.Raycast(ray, out float distance)) return;

        Vector3 point = ray.GetPoint(distance);
        Vector2Int cell = new Vector2Int(
            Mathf.FloorToInt(point.x / cellSize),
            Mathf.FloorToInt(point.z / cellSize));

        if (cell.x < minCell || cell.x > maxCell || cell.y < minCell || cell.y > maxCell) return;

        if (occupied.Contains(cell))
        {
            Debug.Log("Esa celda ya está ocupada");
            return;
        }

        if (!EconomyManager.Instance.TrySpend(Selected.Cost))
        {
            Debug.Log("No tienes dinero suficiente para " + Selected.BuildingName);
            return;
        }

        float height = Selected.transform.localScale.y * 0.5f;
        Vector3 position = new Vector3(
            (cell.x + 0.5f) * cellSize,
            height,
            (cell.y + 0.5f) * cellSize);

        Building built = Instantiate(Selected, position, Quaternion.identity);
        occupied.Add(cell);
        Debug.Log("Construido: " + built.GetInfo());
    }

    private bool LeftClickPressed()
    {
#if ENABLE_INPUT_SYSTEM
        return Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
#else
        return Input.GetMouseButtonDown(0);
#endif
    }

    private Vector2 PointerPosition()
    {
#if ENABLE_INPUT_SYSTEM
        return Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero;
#else
        return Input.mousePosition;
#endif
    }

    private bool NumberKeyPressed(int index)
    {
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current == null) return false;
        return Keyboard.current[(Key)((int)Key.Digit1 + index)].wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.Alpha1 + index);
#endif
    }
}
