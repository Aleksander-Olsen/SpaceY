using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DatetimeController : MonoBehaviour {

    private DateTime time;
    private UIController UICtrl;

    public DateTime Time {
        get { return time; }
        set {
            if (time == value) { return; }
            if (OnChangeStatus != null) {
                OnChangeStatus(value);
            }
            time = value;
        }
    }

    public delegate void ChangeStatus(DateTime newTime);
    public static event ChangeStatus OnChangeStatus;

    private void Awake() {
        time = new DateTime(1970, 1, 1);
        UICtrl = GameObject.FindGameObjectWithTag("UI").GetComponent<UIController>();
        UICtrl.DateMoneyCtrl.DateText.text = FormatString();
    }

    public void AddDay() {
        time = time.AddDays(1);
        UICtrl.DateMoneyCtrl.DateText.text = FormatString();
    }

    public string FormatString() {
        return time.Year + "/" + time.Month + "/" + time.Day;
    }

    public bool IsMonday() {
        if(time.DayOfWeek == DayOfWeek.Monday) {
            return true;
        } 

        return false;
    }

    public bool StartOfMonth() {
        if (time.Day == 1) {
            return true;
        }

        return false;
    }

    public float TimeTilPay(float _nextDay) {
        int tmp = 0;



        return tmp;
    }
}
