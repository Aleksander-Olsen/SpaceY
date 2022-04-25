using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DateMoneyController : MonoBehaviour
{

    [SerializeField]
    private Text dateText;
    [SerializeField]
    private Slider daySlider;
    [SerializeField]
    private Text moneyText;

    public Text DateText { get => dateText; set => dateText = value; }
    public Slider DaySlider { get => daySlider; set => daySlider = value; }
    public Text MoneyText { get => moneyText; set => moneyText = value; }

    private void Awake() {
        if (!dateText) {
            Debug.LogError("Date is null");
        }

        if (!moneyText) {
            Debug.LogError("Money is null");
        }
    }
}
