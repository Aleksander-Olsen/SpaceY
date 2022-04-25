using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteController : MonoBehaviour {

    public const float PATH_STEP = 0.05f;
    public const float MIN_POINTS = 165.0f;
    public const float MAX_POINTS = 170.0f;
    public const float MAX_ORBIT_OFFSET =  50.0f;
    public const float MIN_ORBIT_OFFSET = -50.0f;
    public const float LAUNCH_TIME_DIST = 45.0f;
    public const float LAUNCH_TIME_SPEED = 30.0f;

    private List<Vector3> pathRendererPoints;
    
    private ResourceController rCtrl;
    private SatInfoController infoPanel;
    private Transform satMesh;

    // DEPRECATED
    private Transform lineOfSight;

    private bool selected;
    private float pathCounter = 0.0f;
    private float numLinerendererPoints;

    [SerializeField]
    private Satellite satInfo;

    public bool Selected { get => selected; set => selected = value; }
    public Satellite SatInfo { get => satInfo; set => satInfo = value; }
    public SatInfoController InfoPanel { get => infoPanel; set => infoPanel = value; }

    private void Awake() {
        pathRendererPoints = new List<Vector3>();

        rCtrl = GameObject.FindGameObjectsWithTag("Resources")[0].GetComponent<ResourceController>();

        satMesh = transform.Find("Mesh");
        lineOfSight = transform.Find("LineOfSight");

        satInfo = new Satellite();
        satInfo.Init();

        /* DEPRECATED
         * 
        LoSRendererPoints = new List<Vector3>();
        stations = new List<GameObject>();
        stations.AddRange(GameObject.FindGameObjectsWithTag("Station"));
        stations.AddRange(GameObject.FindGameObjectsWithTag("LaunchPad"));
        */
    }

    // Start is called before the first frame update
    void Start() {

        // Normalize the amount of line renderer points based on speed
        float normal = Mathf.InverseLerp(Satellite.MIN_ORBIT_SPEED, Satellite.MAX_ORBIT_SPEED, -satInfo.TargetOrbitSpeed);
        numLinerendererPoints = Mathf.Lerp(MIN_POINTS, MAX_POINTS, normal);

        StartCoroutine(LaunchRoutine());
    }

    // Update is called once per frame
    void Update() {
        DrawPathLine();
    }

    public void SetLaunchPos(Vector3 _launchPadPos) {
        transform.rotation = Quaternion.LookRotation(_launchPadPos);

        float dist = Vector3.Distance(_launchPadPos, transform.position);
        satMesh.localPosition = new Vector3(0.0f, 0.0f, dist + 0.5f);
    }

    public float GetLifetimePercent() {
        return satInfo.GetLifetimePercent(rCtrl.DailyTimer / ResourceController.DAILY_RATE);
    }

    private void DrawPathLine() {
        if (pathRendererPoints.Count > 0) {
            if (pathRendererPoints[0].Equals(satMesh.position)) {
                GetComponent<LineRenderer>().positionCount = 0;
                Destroy(gameObject);
            }
        }

        if (pathCounter >= PATH_STEP) {
            pathRendererPoints.Add(satMesh.position);
            pathCounter -= PATH_STEP;
        }

        while (pathRendererPoints.Count > numLinerendererPoints) {
            pathRendererPoints.RemoveAt(0);
        }

        GetComponent<LineRenderer>().positionCount = pathRendererPoints.Count;
        GetComponent<LineRenderer>().SetPositions(pathRendererPoints.ToArray());

        pathCounter += Time.deltaTime;
    }

    private IEnumerator LaunchRoutine() {
        float startSpeed = satInfo.CurrentOrbitSpeed;
        float t = 0.0f;
        satInfo.Status = SatelliteStatus.LAUNCHING;
        satInfo.CurrentOrbitDistance = satMesh.position.magnitude;
        Vector3 startPosLocal = satMesh.localPosition;
        Vector3 endPosLocal = new Vector3(0.0f, 0.0f, satInfo.TargetOrbitDistance);

        transform.Rotate(Vector3.forward, Random.Range(MIN_ORBIT_OFFSET, MAX_ORBIT_OFFSET));

        while (t <= Mathf.Max(LAUNCH_TIME_DIST, LAUNCH_TIME_SPEED)) {
            if (t <= LAUNCH_TIME_DIST) {
                satMesh.localPosition = Vector3.Lerp(startPosLocal, new Vector3(0.0f, 0.0f, satInfo.TargetOrbitDistance), Mathf.SmoothStep(0.0f, 1.0f, t / LAUNCH_TIME_DIST));
            }

            if (t <= LAUNCH_TIME_SPEED) {
                satInfo.CurrentOrbitSpeed = Mathf.SmoothStep(startSpeed, satInfo.TargetOrbitSpeed, t / LAUNCH_TIME_SPEED);
            }

            if (t >= Mathf.Max(LAUNCH_TIME_DIST, LAUNCH_TIME_SPEED) / 4) {
                satInfo.Status = SatelliteStatus.APPROACHING_ORBIT;
            }

            t += Time.deltaTime;
            satInfo.CurrentOrbitDistance = satMesh.position.magnitude;
            transform.Rotate(Vector3.up, satInfo.CurrentOrbitSpeed * Time.deltaTime);
            yield return null;
        }

        satMesh.localPosition = endPosLocal;
        satInfo.CurrentOrbitSpeed = satInfo.TargetOrbitSpeed;

        StartCoroutine(OrbitRoutine());
    }

    private IEnumerator OrbitRoutine() {
        //float LoSTimer = 0.0f;
        satInfo.Status = SatelliteStatus.ORBITING;
        rCtrl.CalculateIncome();
        infoPanel.OnSatStatusChange(satInfo.Status);

        while (satInfo.Status == SatelliteStatus.ORBITING) {

            /* DEPRECATED
             * 
            if (LoSTimer >= LOS_CHECK_RATE) {
                LineOfSightCheck();
                timer = 0.0f;
            } */

            transform.Rotate(Vector3.up, satInfo.CurrentOrbitSpeed * Time.deltaTime);

            //LoSTimer += Time.deltaTime;

            yield return null;
        }

        infoPanel.OnSatStatusChange(satInfo.Status);
        if (satInfo.Status == SatelliteStatus.DEORBITING) {
            StartCoroutine(DeOrbitRoutine());
        } else if (satInfo.Status == SatelliteStatus.DECOMMISSIONED) {
            transform.parent = rCtrl.DecomissionedContainer;
            satMesh.gameObject.SetActive(false);
            rCtrl.Satellites.Remove(satInfo);
        }
    }

    private IEnumerator DeOrbitRoutine() {
        while (true) {
            yield return null;
        }
    }


    /* DEPRECATED
     * Keep in case of use later
     * 
    private const float LOS_CHECK_RATE = 0.001f;
    
    private List<Vector3> LoSRendererPoints;
    private List<GameObject> stations;

    [SerializeField]
    private float LoSChecks;
    [SerializeField]
    private float LoSFails;
    [SerializeField]
    private float LoSRate;
    
    private void LineOfSightCheck() {
        LoSRendererPoints.Clear();
        bool LoS = true;
        LoSChecks++;
        RaycastHit hit;

        foreach (GameObject station in stations) {
            if (station.GetComponent<StationController>().Level < 1) {
                continue;
            }

            Vector3 SatPos = transform.Find("Mesh").position;
            Vector3 StationPos = station.transform.Find("SatDetector").position;

            //Debug.DrawRay(SatPos, Vector3.Normalize(StationPos - SatPos) * (Vector3.Distance(SatPos, StationPos) + 1.0f), Color.yellow);
            if (Physics.Raycast(SatPos, Vector3.Normalize(StationPos - SatPos), out hit, Vector3.Distance(SatPos, StationPos) + 10.0f)) {

                if (!hit.transform.gameObject.CompareTag("Earth")) {
                    LoS = false;
                    LoSRendererPoints.Add(SatPos);
                    LoSRendererPoints.Add(StationPos);
                }
            }
        }

        if (LoS) {
            //Debug.Log("LoS (Loss of Signal)");
            LoSFails++;
        }

        // Calculate the line of sight rate
        LoSRate = 1.0f - ((LoSFails / LoSChecks));

        if (Selected) {
            LineOfSight.GetComponent<LineRenderer>().positionCount = LoSRendererPoints.Count;
            LineOfSight.GetComponent<LineRenderer>().SetPositions(LoSRendererPoints.ToArray());
        }
    }
    */
}
