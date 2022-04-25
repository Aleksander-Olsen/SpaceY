using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    private ResourceController rController;


    private void Awake() {
        rController = GameObject.Find("ResourceHandler").GetComponent<ResourceController>();
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("escape")) {
            Debug.Log("ESCAPE");
            Application.Quit();
        }

        if(Input.GetKeyDown("f")) {
            rController.LaunchSatellite(GameObject.FindGameObjectsWithTag("LaunchPad")[0].transform);
        }
    }
}
