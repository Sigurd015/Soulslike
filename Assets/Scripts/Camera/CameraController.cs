using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CameraController : MonoBehaviour
{
    public GameObject playerHandle;
    public GameObject camPivot;
    public GameObject model;
    public PlayerInput pi;

    [Header("Camera Settings")]
    public float cameraDampValue = 0.05f;
    public float horizontalSpeed = 100.0f;
    public float verticalSpeed = 50.0f;
    [Range(-60.0f, 100.0f)]
    public float maxEulerX = 30.0f;
    [Range(-60.0f, 100.0f)]
    public float minEulerX = -40.0f;

    private GameObject mainCamera;  
    private Vector3 currentVelocity;
    private float eulerX;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector3 temp = model.transform.eulerAngles;

        playerHandle.transform.Rotate(Vector3.up, pi.jright * horizontalSpeed * Time.fixedDeltaTime);
        eulerX -= pi.jup * verticalSpeed * Time.fixedDeltaTime;
        eulerX = Mathf.Clamp(eulerX, minEulerX, maxEulerX);
        camPivot.transform.localEulerAngles = new Vector3(eulerX, 0, 0);

        model.transform.eulerAngles = temp;

        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, transform.position, ref currentVelocity, cameraDampValue);
        mainCamera.transform.LookAt(camPivot.transform);
    }
}
