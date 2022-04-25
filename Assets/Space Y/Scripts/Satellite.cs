using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Satellite
{
    public const float MIN_ORBIT_SPEED = 20.0f;
    public const float MAX_ORBIT_SPEED = 30.0f;
    public const float MIN_ORBIT_DISTANCE = 125.0f;
    public const float MAX_ORBIT_DISTANCE = 145.0f;
    public const decimal INCOME_END_RATE = -0.5m;


    [SerializeField]
    private SatelliteStatus status;
    [SerializeField]
    private decimal baseIncome;
    [SerializeField]
    private int startLifetime;
    [SerializeField]
    private int currentLifetime;
    [SerializeField]
    private int endLifetime;
    [SerializeField]
    private float currentOrbitSpeed;
    [SerializeField]
    private float targetOrbitSpeed;
    [SerializeField]
    private float currentOrbitDistance;
    [SerializeField]
    private float targetOrbitDistance;

    // For testing
    [SerializeField]
    private decimal incomeRate;

    public decimal BaseIncome { get => baseIncome; set => baseIncome = value; }
    public int StartLifetime { get => startLifetime; set => startLifetime = value; }
    public int CurrentLifetime { get => currentLifetime; set => currentLifetime = value; }
    public int EndLifetime { get => endLifetime; set => endLifetime = value; }
    public float CurrentOrbitSpeed { get => currentOrbitSpeed; set => currentOrbitSpeed = value; }
    public float TargetOrbitSpeed { get => targetOrbitSpeed; set => targetOrbitSpeed = value; }
    public float CurrentOrbitDistance { get => currentOrbitDistance; set => currentOrbitDistance = value; }
    public float TargetOrbitDistance { get => targetOrbitDistance; set => targetOrbitDistance = value; }

    public SatelliteStatus Status {
        get { return status; }
        set {
            if (status == value) return;
            status = value;
            if(OnChangeStatus != null) {
                OnChangeStatus(this);
            }
        }
    }

    public delegate void ChangeStatus(Satellite _satellite);
    public static event ChangeStatus OnChangeStatus;

    public Satellite() {
        Status = SatelliteStatus.PLANNING;
        baseIncome = 10_000.00m;

        startLifetime = currentLifetime = 30;
        endLifetime = -30;
    }


    public void Init() {
        currentOrbitSpeed = GameObject.FindGameObjectsWithTag("Earth")[0].GetComponent<EarthController>().OrbitSpeed;

        targetOrbitSpeed = Random.Range(-MIN_ORBIT_SPEED, -MAX_ORBIT_SPEED);
        targetOrbitDistance = Random.Range(MIN_ORBIT_DISTANCE, MAX_ORBIT_DISTANCE);
    }


    public decimal GetIncome() {
        decimal temp = 0.00m;

        if (status == SatelliteStatus.ORBITING) {
            currentLifetime--;
            incomeRate = GetIncomeRate(currentLifetime);
            temp = baseIncome * GetIncomeRate(currentLifetime);
        }

        return temp;
    }

    public decimal CalculateIncome(int _days = 1) {
        if (status != SatelliteStatus.ORBITING) {
            return 0.00m;
        }

        if (currentLifetime - _days >= 0) {
            return (BaseIncome * GetIncomeRate(currentLifetime)) * _days;
        }

        decimal temp = 0.00m;
        for (int i = 1; i <= _days; i++) {
            int tempLiftime = currentLifetime - i;

            temp += BaseIncome * GetIncomeRate(tempLiftime);
        }

        return temp;
    }

    private decimal GetIncomeRate(int _lifetime) {
        if (_lifetime >= 0) {
            return 1.00m;
        } else if (_lifetime <= endLifetime) {
            return INCOME_END_RATE;
        } else {

            // Use sine waves to smooth interpolate from 1 to -0.5
            float iTimesT = (Mathf.PI / endLifetime) * _lifetime;
            float sinWave1 = (0.5f * Mathf.Sin(iTimesT + Mathf.PI * 0.5f));
            float sinWave2 = (-0.25f * Mathf.Sin(iTimesT - Mathf.PI * 0.5f));

            return (decimal)(sinWave1 + sinWave2 + 0.25f);
        }
    }

    public float GetLifetimePercent(float dayProgress) {
        if (status == SatelliteStatus.ORBITING) {
            return ((float)currentLifetime - dayProgress) / (float)startLifetime;
        }

        return 1.0f;

    }

}
