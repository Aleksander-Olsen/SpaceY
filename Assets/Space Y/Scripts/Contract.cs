using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Contract {
    public const int YEAR = 365;
    public const int MONTH = 30;
    public static readonly int[] times = new int[] { 1, 3, 5, 7, 10, 15, 20 };

    private ContractType type;
    private Size satSize;
    private Money income;
    private Money bonus;
    private int lifetime;
    private int deadline;

    public ContractType Type { get => type; set => type = value; }
    public Size SatSize { get => satSize; set => satSize = value; }
    public Money Income { get => income; set => income = value; }
    public Money Bonus { get => bonus; set => bonus = value; }
    public int Lifetime { get => lifetime; set => lifetime = value; }
    public int Deadline { get => deadline; set => deadline = value; }

    public Contract() {
        type = ContractType.PRIVATE;
        satSize = Size.TINY;
        income = new Money(1_000.00m);
        bonus = new Money(10_000.00m);
        lifetime = 7;
        deadline = 30;
    }

    public Contract(ContractType _type) {
        type = _type;

        // Do something based on contract type.
    }

    public Contract(ContractType _type, Size _size, decimal _income, decimal _bonus, int _duration, int _deadline) {
        type = _type;
        satSize = _size;
        income = new Money(_income);
        bonus = new Money(_bonus);
        lifetime = _duration;
        deadline = _deadline;
    }

    public void Init() {
        int index = Random.Range(0, times.Length - 1);

        income.BaseMoney *= times[index];
        bonus.BaseMoney *= times[index];
        lifetime *= times[index];
        deadline += index * 7;
    }

    public string GetTimeText(int _days) {

        if (_days >= YEAR) {
            return (_days / YEAR).ToString() + " Years";
        } else if (_days >= MONTH) {
            return (_days).ToString() + " Months";
        } else if (_days >= 7) {
            return _days.ToString() + " Weeks";
        } else {
            return _days.ToString() + " Days";
        }
    }
}
