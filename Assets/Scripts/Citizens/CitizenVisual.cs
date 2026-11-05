using UnityEngine;

public class CitizenVisual : MonoBehaviour
{
    [SerializeField] private float limit = 9.5f; 
    [SerializeField] private Color employedColor = Color.cyan;
    [SerializeField] private Color unemployedColor = Color.red;

    private Citizen citizen;
    private Renderer rend;
    private Transform destination;
    private Vector3 offset;
    private float speed;
    private float waitTime;


    private int[] routine = { 0, 1, 2, 3 };
    private int step;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    public void Setup(Citizen newCitizen)
    {
        citizen = newCitizen;
        destination = citizen.HomeTransform;

        speed = Random.Range(2.5f, 5f);
        ShuffleRoutine();
        step = Random.Range(0, routine.Length);

        Vector2 around = Random.insideUnitCircle.normalized * 1.5f;
        offset = new Vector3(around.x, 0f, around.y);

    }

    private void ShuffleRoutine()
    {
        for (int i = routine.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = routine[i];
            routine[i] = routine[j];
            routine[j] = temp;
        }
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
        step = (step + 1) % routine.Length;
        destination = ChooseDestination(routine[step]);
        waitTime = Random.Range(0f, 3f);
    }

    private Transform ChooseDestination(int place)
    {
        if (place == 1 && citizen.WorkTransform != null)
            return citizen.WorkTransform;

        if (place == 2)
        {
            Shop[] shops = FindObjectsByType<Shop>(FindObjectsSortMode.None);
            if (shops.Length > 0)
                return shops[Random.Range(0, shops.Length)].transform;
        }

        if (place == 3)
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

        target.x = Mathf.Clamp(target.x, -limit, limit);
        target.z = Mathf.Clamp(target.z, -limit, limit);
        target.y = transform.position.y;

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        bool arrived = Vector3.Distance(transform.position, target) < 0.05f;
        rend.enabled = !(enters && arrived);
    }
}

