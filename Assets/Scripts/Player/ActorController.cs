using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    
    [Header("===== Movement Settings =====")]
    public float walkSpeed = 2.0f;
    public float runMultiplier = 2.0f;
    public float jumpVelocity = 4.0f;
    public float rollVelocityThreshold = 5.0f;
    public float rollVelocity = 1.0f;
    public float jabVelocity = 3.0f;

    private PlayerInput pi;
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 movingVec;
    private bool lockPlanar = false;
    private Vector3 thrushVec;
    // Start is called before the first frame update
    void Start()
    {
        pi = GetComponent<PlayerInput>();
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Locomotion();
    }

    void Locomotion()
    {
        anim.SetFloat("forward", Mathf.Lerp(anim.GetFloat("forward"), pi.dmag * ((pi.run) ? 2.0f : 1.0f), 0.5f));

        if (pi.dmag > 0.1f && pi.inputEnabled)
            model.transform.forward = Vector3.Slerp(model.transform.forward, pi.dvec, 0.3f);

        if (!lockPlanar)
            movingVec = pi.dmag * model.transform.forward * walkSpeed * ((pi.run) ? runMultiplier : 1.0f);

        // Jump
        if (pi.jump)
        {
            anim.SetTrigger("jump");
        }

        // Roll
        if (pi.roll)
        {
            anim.SetTrigger("roll");
        }
        if (rigid.velocity.magnitude > rollVelocityThreshold)
        {
            anim.SetTrigger("roll");
        }
    }

    private void FixedUpdate()
    {
        //rigid.position += movingVec * Time.fixedDeltaTime;
        rigid.velocity = new Vector3(movingVec.x, rigid.velocity.y, movingVec.z) + thrushVec;
        thrushVec = Vector3.zero;
    }

    // Sensor messages
    public void OnGroundSensor(bool isGround)
    {
        anim.SetBool("isGround", isGround);
    }

    // Animation messages
    void OnJumpEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
        thrushVec.y = jumpVelocity;
    }

    void OnGroundEnter()
    {
        pi.inputEnabled = true;
        lockPlanar = false;
    }

    void OnFallEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
    }

    void OnRollEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
        thrushVec.y = rollVelocity;
    }

    void OnJabEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
    }

    void OnJabUpdate()
    {
        thrushVec = model.transform.forward * anim.GetFloat("jabVelocity");
    }
}
