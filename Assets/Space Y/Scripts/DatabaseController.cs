using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox;

public class DatabaseController : MonoBehaviour
{

    [SerializeField]
    private DataboxObjectManager dataManager;

    public DataboxObjectManager DataManager { get => dataManager; set => dataManager = value; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public DataboxObject GetSatellites() {
        return dataManager.GetDataboxObject("Satellites");
    }

    public DataboxObject GetStations() {
        return dataManager.GetDataboxObject("Stations");
    }
}
