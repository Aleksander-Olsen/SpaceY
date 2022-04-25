using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour {

    public const float DAILY_RATE = 6.0f;
    public const int NUM_CONTRACTS = 3;


    [SerializeField]
    private GameObject satPrefab;
    [SerializeField]
    private GameObject satInfoPrefab;
    [SerializeField]
    private GameObject contractInfoPrefab;

    private Transform satelliteContainer;
    private Transform decomissionedContainer;
    private Transform activeContractContainer;
    private Transform availableContractContainer;
    private UIController UICtrl;

    [SerializeField]
    private List<Satellite> satellites;
    [SerializeField]
    private List<Contract> activeContracts;

    [SerializeField]
    private DatetimeController date;

    [SerializeField]
    private Money money;
    [SerializeField]
    private float monthlyTimer = 0.0f;
    [SerializeField]
    private float dailyTimer = 0.0f;
    [SerializeField]
    private double expectedIncome;
    [SerializeField]
    private int decomissionedLow;
    [SerializeField]
    private int decomissionedHigh;
    [SerializeField]
    private int decomissionedGeo;

    public double ExpectedIncome { get => expectedIncome; set => expectedIncome = value; }
    public float DailyTimer { get => dailyTimer; set => dailyTimer = value; }
    public Money Money { get => money; set => money = value; }
    public Transform DecomissionedContainer { get => decomissionedContainer; set => decomissionedContainer = value; }
    public List<Satellite> Satellites { get => satellites; set => satellites = value; }


    private void Awake() {
        UICtrl = GameObject.FindGameObjectWithTag("UI").GetComponent<UIController>();
        satelliteContainer = GameObject.FindGameObjectsWithTag("SatelliteContainer")[0].transform;
        decomissionedContainer = GameObject.FindGameObjectsWithTag("DecomissionedContainer")[0].transform;
        activeContractContainer = GameObject.FindGameObjectsWithTag("ActiveContractContainer")[0].transform;
        availableContractContainer = GameObject.FindGameObjectsWithTag("AvailableContractContainer")[0].transform;
        
        satellites = new List<Satellite>();
        activeContracts = new List<Contract>();

        date = GetComponent<DatetimeController>();
        money = new Money(1_000.00m);

        RefreshContracts();
        UICtrl.MenuCtrl.ContractMenu.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start() {
        UICtrl.DateMoneyCtrl.MoneyText.text = money.TextFormatting();
        UICtrl.DateMoneyCtrl.DaySlider.maxValue = DAILY_RATE;
    }

    // Update is called once per frame
    void Update() {
        UICtrl.DateMoneyCtrl.DaySlider.value = dailyTimer;

        if (dailyTimer >= DAILY_RATE) {
            foreach (Satellite satellite in satellites) {
                money.BaseMoney += satellite.GetIncome();
            }

            dailyTimer -= DAILY_RATE;
            date.AddDay();

            if (date.IsMonday()) {
                RefreshContracts();
            }

            UICtrl.DateMoneyCtrl.MoneyText.text = money.TextFormatting();
            CalculateIncome();
        }

        dailyTimer += Time.deltaTime;

    }

    public void LaunchSatellite(Transform _launchPad) {
        GameObject tmp = Instantiate(satPrefab);
        tmp.transform.parent = satelliteContainer;

        SatelliteController tmpSatCtrl = tmp.GetComponent<SatelliteController>();
        tmpSatCtrl.SetLaunchPos(_launchPad.position);
        satellites.Add(tmpSatCtrl.SatInfo);

        SatInfoController tmpSatInfoPanel = Instantiate(satInfoPrefab, UICtrl.MenuCtrl.SatMenu.transform).GetComponent<SatInfoController>();
        tmpSatInfoPanel.SatCtrl = tmpSatCtrl;
        tmpSatCtrl.InfoPanel = tmpSatInfoPanel;
    }

    public void CalculateIncome() {
        expectedIncome = 0.0d;
        foreach (Satellite sat in satellites) {
            expectedIncome += (double)sat.CalculateIncome();
        } 
    }

    private void RefreshContracts() {
        foreach (Transform contract in availableContractContainer.transform) {
            if (contract.CompareTag("ContractInfo")) {
                Destroy(contract.gameObject);
            }
        }

        for (int i = 0; i < NUM_CONTRACTS; i++) {

            GameObject tmp = Instantiate(contractInfoPrefab, availableContractContainer);
            tmp.GetComponent<ContractInfoController>().ContractInfo = new Contract();
            tmp.GetComponent<ContractInfoController>().ContractInfo.Init();
            tmp.GetComponent<ContractInfoController>().Init();
        }
    }

    public void AcceptContract(ContractInfoController _contractInfo) {
        if (!activeContracts.Contains(_contractInfo.ContractInfo)) {
            activeContracts.Add(_contractInfo.ContractInfo);
            _contractInfo.transform.parent = activeContractContainer;
        }
    }
}
