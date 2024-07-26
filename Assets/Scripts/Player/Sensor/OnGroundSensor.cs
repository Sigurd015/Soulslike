using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour
{
    public float offset = 0.1f;
    public LayerMask checkLayer;

    private CapsuleCollider capcol;
    private ActorController ac;
    private Vector3 pointTop;
    private Vector3 pointBottom;
    private float radius;

    void Start()
    {
        capcol = transform.parent.GetComponent<CapsuleCollider>();
        radius = capcol.radius - 0.05f;
        ac = transform.parent.GetComponent<ActorController>();
    }

    void FixedUpdate()
    {
        pointTop = transform.position + transform.up * (radius - offset);
        pointBottom = transform.position + transform.up * (capcol.height - offset) - transform.up * radius;
        Collider[] outputCols = Physics.OverlapCapsule(pointTop, pointBottom, radius, checkLayer);
        ac.OnGroundSensor(outputCols.Length != 0);
    }
}
