using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox;

public class StartUp : MonoBehaviour
{
    private DatabaseController dbCtrl;

    void Awake() {
        dbCtrl = FindObjectOfType<DatabaseController>();

        if (!Directory.Exists(Application.persistentDataPath + "/Data")) {
            Directory.CreateDirectory(Application.persistentDataPath + "/Data");
        } else {
            dbCtrl.DataManager.GetDataboxObject("SaveData").LoadDatabase();
        }

        DataboxObject.Database _tempDB;
        if (!dbCtrl.DataManager.GetDataboxObject("SaveData").DB.TryGetValue("Satellites", out _tempDB)) {
            dbCtrl.DataManager.GetDataboxObject("SaveData").AddDatabaseTable("Satellites");
            dbCtrl.DataManager.GetDataboxObject("SaveData").SaveDatabase();
        }

        if (!dbCtrl.DataManager.GetDataboxObject("SaveData").DB.TryGetValue("Stations", out _tempDB)) {
            dbCtrl.DataManager.GetDataboxObject("SaveData").AddDatabaseTable("Stations");
            dbCtrl.DataManager.GetDataboxObject("SaveData").SaveDatabase();
        }
    }
}
