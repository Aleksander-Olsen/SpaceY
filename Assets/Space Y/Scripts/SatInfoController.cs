using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SatInfoController : MonoBehaviour {

    private SatelliteController satCtrl;

    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Text incomeText;

    [SerializeField]
    private Button deOrbitBtn;
    [SerializeField]
    private Button decomissionBtn;

    public SatelliteController SatCtrl { get => satCtrl; set => satCtrl = value; }

    private void Awake() {
        if (!deOrbitBtn) {
            Debug.LogError("Button is empty");
        }

        if (!decomissionBtn) {
            Debug.LogError("Button is empty");
        }
    }

    // Update is called once per frame
    void Update() {
        slider.value = satCtrl.GetLifetimePercent();
        incomeText.text = FormatIncomeText();
        if (slider.value <= 0.05f) {
            deOrbitBtn.interactable = false;
        }
    }

    public void Decomission() {
        satCtrl.SatInfo.Status = SatelliteStatus.DECOMMISSIONED;
    }

    public void DeOrbit() {
        satCtrl.SatInfo.Status = SatelliteStatus.DEORBITING;
    }

    public void OnSatStatusChange(SatelliteStatus _status) {
        if (_status == SatelliteStatus.ORBITING) {
            deOrbitBtn.interactable = true;
            decomissionBtn.interactable = true;

        } else if(_status == SatelliteStatus.DECOMMISSIONED || _status == SatelliteStatus.DEORBITING) {
            deOrbitBtn.interactable = false;
            decomissionBtn.interactable = false;
            Destroy(gameObject);
        }
    }


    private string FormatIncomeText() {
        string temp = "Income: $";
        string income;
        if (satCtrl.SatInfo.CalculateIncome() == 0.00m) {
            income = satCtrl.SatInfo.BaseIncome.ToString();
        } else {
            income = (decimal.Floor(satCtrl.SatInfo.CalculateIncome())).ToString();
        }

        temp += income.Insert(income.Length - 3, ".");

        return temp;
    }
}
