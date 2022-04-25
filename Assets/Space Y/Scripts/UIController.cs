using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    [SerializeField]
    private MenuController menuCtrl;
    [SerializeField]
    private DateMoneyController dateMoneyCtrl;

    public MenuController MenuCtrl { get => menuCtrl; set => menuCtrl = value; }
    public DateMoneyController DateMoneyCtrl { get => dateMoneyCtrl; set => dateMoneyCtrl = value; }

    private void Awake() {

        if (!MenuCtrl) {
            Debug.LogError("Money is null");
        }
    }

}
