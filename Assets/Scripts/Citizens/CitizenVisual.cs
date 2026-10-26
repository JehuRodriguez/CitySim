using UnityEngine;

public class CitizenVisual : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private Color employedColor = Color.cyan;
    [SerializeField] private Color unemployedColor = Color.red;

    private Citizen citizen;
    private Renderer rend;
    private Transform destination;
    private Vector3 offset;
    private int step;     
    private float waitTime;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    public void Setup(Citizen newCitizen)
    {
        citizen = newCitizen;
        destination = citizen.HomeTransform;

        Vector2 around = Random.insideUnitCircle.normalized * 1.5f;
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
        step = (step + 1) % 4;
        destination = ChooseDestination();
        waitTime = Random.Range(0f, 2f);
    }

    private Transform ChooseDestination()
    {
        if (step == 1 && citizen.WorkTransform != null)
            return citizen.WorkTransform;

        if (step == 2)
        {
            Shop[] shops = FindObjectsByType<Shop>(FindObjectsSortMode.None);
            if (shops.Length > 0)
                return shops[Random.Range(0, shops.Length)].transform;
        }

        if (step == 3)
        {
            Park[] parks = FindObjectsByType<Park>(FindObjectsSortMode.None);
            if (parks.Length > 0)
                return parks[Random.Range(0, parks.Length)].transform;
        }

        return citizen.HomeTransform;
    }

    private void Update()
    {
        if (citizen == null || destination == null) return;

        rend.material.color = citizen.HasJob ? employedColor : unemployedColor;

        if (waitTime > 0f)
        {
            waitTime -= Time.deltaTime;
            return;
        }

        bool enters = destination.GetComponent<IWorkplace>() != null;

        Vector3 target = destination.position;
        if (!enters) target += offset;
        target.y = transform.position.y;

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        bool arrived = Vector3.Distance(transform.position, target) < 0.05f;
        rend.enabled = !(enters && arrived);

    }

}
