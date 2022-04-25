using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    private const float MIN_SCALE = 5.0f;
    private const float MAX_SCALE = 15.0f;

    private GameObject mark;
    private CameraController camCtrl;

    [SerializeField]
    private int level = 0;

    public int Level { get => level; set => level = value; }

    private void Awake() {
        camCtrl = Camera.main.GetComponent<CameraController>();
        mark = transform.Find("Mark").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float normal = Mathf.InverseLerp(camCtrl.ZoomMin, camCtrl.ZoomMax, camCtrl.CameraDistance);
        float scale = Mathf.Lerp(MIN_SCALE, MAX_SCALE, normal);
        mark.transform.localScale = new Vector3(scale, 1.0f, scale);
    }

    public virtual void OnSelect() {
        camCtrl.Focus(gameObject);
    }
}
