using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Money
{
    public const int ONE_MILLION = 1000000;
    public const int THOUSAND = 1000;

    [SerializeField]
    private decimal baseMoney;
    [SerializeField]
    private int million;
    [SerializeField]
    private int billion;

    public int Billion { get => billion; set => billion = value; }
    public int Million { get => million; set => million = value; }
    public decimal BaseMoney {
        get { return baseMoney; }
        set {
            if (baseMoney == value) return;
            baseMoney = value;
            OnVariableChange?.Invoke();
        }
    }

    public delegate void OnVariableChangeDelegate();
    public event OnVariableChangeDelegate OnVariableChange;


    public Money() { OnVariableChange += MoneyFormatting; }
    public Money(decimal _money) { OnVariableChange += MoneyFormatting; baseMoney = _money; }
    

    public string TextFormatting() {
        char pad = '0';
        string money = "$";

        if (billion > 0) {
            money += "B " + billion.ToString() + ".";
            string next = million.ToString().PadLeft(3, pad);
            money += next;
        } else if (million > 0) {
            money += "M " + million.ToString() + ".";
            string next = decimal.Floor(baseMoney).ToString().PadLeft(6, pad);
            next = next.Substring(0, 3);
            money += next;
        } else if (decimal.Floor(baseMoney) == 0.00m) {
            money += " 0";
        } else {
            string next = decimal.Floor(baseMoney).ToString();
            next = next.Insert(next.Length - 3, ".");
            money += " " + next;
        }

        return money;
    }

    private void MoneyFormatting() {
        // Check from low to high.
        while (baseMoney >= ONE_MILLION) {
            million += 1;
            baseMoney -= ONE_MILLION;

            if (million >= THOUSAND) {
                billion += 1;
                million -= THOUSAND;
            }
        }

        // Check from high to low
        if (baseMoney < 0.00m) {
            if (million >= 1 || billion >= 1) {
                while (baseMoney < 0.00m) {
                    million -= 1;
                    baseMoney += ONE_MILLION - 1;

                    if (million < 0) {
                        billion -= 1;
                        million += THOUSAND - 1;
                    }
                }
            }
        }


        // = Helper.MoneyTextFormat(moneyBase, moneyMillion, moneyBillion);
    }
}
