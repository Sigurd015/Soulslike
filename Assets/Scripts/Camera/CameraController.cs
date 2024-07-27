using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public struct LockTarget
{
    public GameObject target;
    public float halfHeight;
}

public class CameraController : MonoBehaviour
{
    public GameObject playerHandle;
    public GameObject camPivot;
    public GameObject model;
    public PlayerInput pi;
    public UIController uc;

    [Header("===== Camera Settings =====")]
    public float cameraDampValue = 0.05f;
    public float horizontalSpeed = 100.0f;
    public float verticalSpeed = 50.0f;
    [Range(-60.0f, 100.0f)]
    public float maxEulerX = 30.0f;
    [Range(-60.0f, 100.0f)]
    public float minEulerX = -40.0f;

    [Header("===== Lock Settings =====")]
    public LayerMask lockOnLayer;
    public float maxLockDistance = 10.0f;

    // ===== Output signals =====
    public bool lockState { get; private set; } = false;

    private GameObject mainCamera;
    private Vector3 currentVelocity;
    private float eulerX;
    private LockTarget lockTarget = new LockTarget();

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (pi.lockOn)
        {
            LockUnlock();
        }

        if (lockState)
        {
            Vector3 woldPos = lockTarget.target.transform.position + Vector3.up * lockTarget.halfHeight;
            uc.SetLockOnIcon(true, Camera.main.WorldToScreenPoint(woldPos));

            if (Vector3.Distance(model.transform.position, lockTarget.target.transform.position) >= maxLockDistance)
            {
                lockTarget.target = null;
                lockTarget.halfHeight = 0.0f;
                lockState = false;
                uc.SetLockOnIcon(false, Vector3.zero);
            }

            //if (pi.jright > 0)
            //{
            //    Vector3 boxCenter = lockTarget.target.transform.position + Vector3.up * lockTarget.halfHeight + lockTarget.target.transform.right * 1.0f;
            //    Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f, 5f), lockTarget.target.transform.rotation, lockOnLayer);
            //    if (cols.Length != 0)
            //    {
            //        Collider temp = cols[0];
            //        lockTarget.target = temp.gameObject;
            //        lockTarget.halfHeight = temp.bounds.extents.y;
            //    }
            //}

            //if (pi.jright < 0)
            //{
            //    Vector3 boxCenter = lockTarget.target.transform.position + Vector3.up * lockTarget.halfHeight + lockTarget.target.transform.right * -1.0f;
            //    Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f, 5f), lockTarget.target.transform.rotation, lockOnLayer);
            //    if (cols.Length != 0)
            //    {
            //        Collider temp = cols[0];
            //        lockTarget.target = temp.gameObject;
            //        lockTarget.halfHeight = temp.bounds.extents.y;
            //    }
            //}
        }
    }

    private void FixedUpdate()
    {
        if (!lockState)
        {
            Vector3 temp = model.transform.eulerAngles;

            playerHandle.transform.Rotate(Vector3.up, pi.jright * horizontalSpeed * Time.fixedDeltaTime);
            eulerX -= pi.jup * verticalSpeed * Time.fixedDeltaTime;
            eulerX = Mathf.Clamp(eulerX, minEulerX, maxEulerX);
            camPivot.transform.localEulerAngles = new Vector3(eulerX, 0, 0);

            model.transform.eulerAngles = temp;
        }
        else
        {
            Vector3 tempForward = lockTarget.target.transform.position - model.transform.position;
            tempForward.y = 0;
            playerHandle.transform.forward = tempForward;
            camPivot.transform.LookAt(lockTarget.target.transform);
        }

        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, transform.position, ref currentVelocity, cameraDampValue);
        mainCamera.transform.LookAt(camPivot.transform);
    }

    public void LockUnlock()
    {
        if (lockTarget.target != null)
        {
            lockTarget.target = null;
            lockTarget.halfHeight = 0.0f;
            lockState = false;
            uc.SetLockOnIcon(false, Vector3.zero);
            return;
        }

        Vector3 boxCenter = model.transform.position + Vector3.up + model.transform.forward * 5.0f;
        Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f, 5f), model.transform.rotation, lockOnLayer);

        if (cols.Length != 0)
        {
            Collider temp = cols[0];
            lockTarget.target = temp.gameObject;
            lockTarget.halfHeight = temp.bounds.extents.y;
            lockState = true;
            return;
        }
        lockTarget.target = null;
        lockTarget.halfHeight = 0.0f;
        lockState = false;
        uc.SetLockOnIcon(false, Vector3.zero);
    }
}
