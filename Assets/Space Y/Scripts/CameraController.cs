using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    private const float MIN_ZOOM = 150.0f;
    private const float MAX_ZOOM = 800.0f;

    private Transform mainCamera;
    private Transform cameraBoom;
    private GameObject target;

    [SerializeField]
    private Vector3 cameraBoomRotation;
    private float cameraDistance = 200.0f;
    private float oldCameraDist;

    [SerializeField]
    private float mouseSensitivity = 4.0f;
    [SerializeField]
    private float scrollSensitivity = 2.0f;
    [SerializeField]
    private float orbitSpeed = 10.0f;
    [SerializeField]
    private float scrollSpeed = 6.0f;

    [SerializeField]
    private bool invertedY = true;
    [SerializeField]
    private bool cameraLocked = false;
    [SerializeField]
    private bool canZoom = true;

    public float MouseSensitivity { get => mouseSensitivity; set => mouseSensitivity = value; }
    public float ScrollSensitivity { get => scrollSensitivity; set => scrollSensitivity = value; }
    public float OrbitSpeed { get => orbitSpeed; set => orbitSpeed = value; }
    public float ScrollSpeed { get => scrollSpeed; set => scrollSpeed = value; }
    public float ZoomMin { get => MIN_ZOOM; }
    public float ZoomMax { get => MAX_ZOOM; }
    public bool InvertedY { get => invertedY; set => invertedY = value; }
    public bool CameraLocked { get => cameraLocked; set => cameraLocked = value; }
    public float CameraDistance { get => cameraDistance; set => cameraDistance = value; }

    // Start is called before the first frame update
    void Start() {
        mainCamera = transform;
        cameraBoom = transform.parent;
    }

    // Update is called once per frame
    void Update() {
        if (cameraLocked) {
            LockedCamera();
        } else {
            FreeCamera();
        }
    }

    public void SetZoomLock(bool _zoom) {
        canZoom = _zoom;
    }

    private void FreeCamera() {
        //Rotation of the camera based on mouse coordinates
        if (Input.GetAxis("Mouse X") != 0 && Input.GetMouseButton(1)
                || Input.GetAxis("Mouse Y") != 0 && Input.GetMouseButton(1)) {

            cameraBoomRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;


            if (InvertedY) {
                cameraBoomRotation.y += Input.GetAxis("Mouse Y") * MouseSensitivity * -1.0f;
            } else {
                cameraBoomRotation.y += Input.GetAxis("Mouse Y") * MouseSensitivity;
            }

            cameraBoomRotation.y = Mathf.Clamp(cameraBoomRotation.y, -70.0f, 70.0f);
        }

        //Zooming based on scroll wheel
        if (Input.GetAxis("Mouse ScrollWheel") != 0.0f && canZoom) {
            float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity;

            ScrollAmount *= (CameraDistance * 0.3f);

            CameraDistance += ScrollAmount * -1.0f;

            CameraDistance = Mathf.Clamp(CameraDistance, ZoomMin, ZoomMax);
        }

        //Calculate new rotation
        Quaternion QT = Quaternion.Euler(cameraBoomRotation.y, cameraBoomRotation.x, 0);
        cameraBoom.rotation = Quaternion.Lerp(cameraBoom.rotation, QT, Time.deltaTime * OrbitSpeed);

        //Calculate new zoom distance
        if (mainCamera.localPosition.z != CameraDistance * -1.0f) {
            mainCamera.localPosition = new Vector3(0.0f, 0.0f, Mathf.Lerp(mainCamera.localPosition.z, CameraDistance * -1.0f, Time.deltaTime * ScrollSpeed));
        }
    }

    private void LockedCamera() {
        cameraBoom.rotation = Quaternion.Lerp(cameraBoom.rotation, Quaternion.LookRotation(-target.transform.position), Time.deltaTime * orbitSpeed / 2);

        //Calculate new zoom distance
        if (mainCamera.localPosition.z != CameraDistance * -1.0f) {
            mainCamera.localPosition = new Vector3(0.0f, 0.0f, Mathf.Lerp(mainCamera.localPosition.z, CameraDistance * -1.0f, Time.deltaTime * scrollSpeed / 2));
        }
    }

    public void Focus(GameObject target) {
        cameraLocked = true;
        this.target = target;
        oldCameraDist = CameraDistance;
        CameraDistance = 200.0f;
    }

    public void UnFocus() {
        if (cameraLocked) {
            CameraDistance = oldCameraDist;
            cameraLocked = false;
        }
    }
}
