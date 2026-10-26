using UnityEngine;

public class CitizenVisual : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private Color employedColor = Color.cyan;
    [SerializeField] private Color unemployedColor = Color.red;

    private Citizen citizen;
    private Renderer rend;
    private bool goingToWork;
    private Vector3 offset;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    public void Setup(Citizen newCitizen)
    {
        citizen = newCitizen;

        
        Vector2 around = Random.insideUnitCircle.normalized * 1.2f;
        offset = new Vector3(around.x, 0f, around.y);
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

    private void HandleDayPassed(int day)
    {
        goingToWork = !goingToWork;
    }

    private void Update()
    {
        if (citizen == null) return;

        Transform destination = citizen.HomeTransform;
        if (goingToWork && citizen.WorkTransform != null)
            destination = citizen.WorkTransform;

        Vector3 target = destination.position + offset;
        target.y = transform.position.y;

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        rend.material.color = citizen.HasJob ? employedColor : unemployedColor;
    }

}
