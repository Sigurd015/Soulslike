using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public CameraController camcon;

    [Header("===== Movement Settings =====")]
    public float walkSpeed = 2.0f;
    public float runMultiplier = 2.0f;
    public float jumpVelocity = 4.0f;
    public float rollVelocityThreshold = 5.0f;
    public float rollVelocity = 1.0f;
    public float jabVelocity = 3.0f;

    [Header("===== Friction Settings =====")]
    public PhysicMaterial fricitionOne;
    public PhysicMaterial fricitionZero;

    private PlayerInput pi;
    private Animator anim;
    private Rigidbody rigid;
    private CapsuleCollider col;
    private Vector3 planarVec;
    private bool lockPlanar = false;
    private Vector3 thrushVec;
    private bool canAttack;
    private float lerpTarget;
    private Vector3 deltaPos;
    private bool trackDirection = false;

    // Start is called before the first frame update
    void Start()
    {
        pi = GetComponent<PlayerInput>();
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Locomotion();
        Combat();
    }

    void Locomotion()
    {
        if (camcon.lockState)
        {
            Vector3 localDevc = transform.InverseTransformVector(pi.dvec);
            anim.SetFloat("right", localDevc.x * ((pi.run) ? 2.0f : 1.0f));
            anim.SetFloat("forward", localDevc.z * ((pi.run) ? 2.0f : 1.0f));

            if (trackDirection)
                model.transform.forward = planarVec.normalized;
            else
                model.transform.forward = transform.forward;

            if (!lockPlanar)
                planarVec = pi.dvec * walkSpeed * ((pi.run) ? runMultiplier : 1.0f);
        }
        else
        {
            anim.SetFloat("right", 0);
            anim.SetFloat("forward", Mathf.Lerp(anim.GetFloat("forward"), pi.dmag * ((pi.run) ? 2.0f : 1.0f), 0.5f));

            if (pi.dmag > 0.1f && pi.inputEnabled)
                model.transform.forward = Vector3.Slerp(model.transform.forward, pi.dvec, 0.3f);

            if (!lockPlanar)
                planarVec = pi.dmag * model.transform.forward * walkSpeed * ((pi.run) ? runMultiplier : 1.0f);
        }

        // Jump
        if (pi.jump)
        {
            anim.SetTrigger("jump");
            canAttack = false;
        }

        // Roll
        if (pi.roll || rigid.velocity.magnitude > rollVelocityThreshold)
        {
            anim.SetTrigger("roll");
            canAttack = false;
        }
    }

    void Combat()
    {
        if (pi.rightAttack && CheckAnimatorStateWithName("ground") && canAttack)
        {
            anim.SetTrigger("attack");
        }

        anim.SetBool("defense", pi.defense);
    }

    private void FixedUpdate()
    {
        rigid.position += deltaPos;
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrushVec;
        thrushVec = Vector3.zero;
        deltaPos = Vector3.zero;
    }

    // Sensor messages
    public void OnGroundSensor(bool isGround)
    {
        anim.SetBool("isGround", isGround);
    }

    // Default animator layer messages
    void OnJumpEnter()
    {
        trackDirection = true;
        thrushVec.y = jumpVelocity;      
    }

    void OnGroundEnter()
    {
        col.material = fricitionOne;
        pi.inputEnabled = true;
        lockPlanar = false;
        canAttack = true;
        trackDirection = false;
    }

    void OnGroundExit()
    {
        col.material = fricitionZero;
        pi.inputEnabled = false;
        lockPlanar = true;
    }

    void OnRollEnter()
    {
        thrushVec.y = rollVelocity;
        trackDirection = true;
    }

    void OnJabUpdate()
    {
        thrushVec = model.transform.forward * anim.GetFloat("jabVelocity");
    }

    // Attack animator layer messages
    void OnAttack1HAEnter()
    {
        pi.inputEnabled = false;
        lerpTarget = 1.0f;
    }

    void OnAttack1HAUpdate()
    {
        // Changed to use root motion
        //thrushVec = model.transform.forward * anim.GetFloat("attack1HAVelocity");
        anim.SetLayerWeight(anim.GetLayerIndex("attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), lerpTarget, 0.1f));
    }

    void OnAttackIdleEnter()
    {
        pi.inputEnabled = true;
        lerpTarget = 0.0f;
    }

    void OnAttackIdleUpdate()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), lerpTarget, 0.1f));
    }

    // Root motion
    public void OnUpdateRM(object _deltaPos)
    {
        if (CheckAnimatorStateWithName("attack1HA", "attack") || CheckAnimatorStateWithName("attack1HB", "attack") || CheckAnimatorStateWithName("attack1HC", "attack"))
            deltaPos += (Vector3)_deltaPos;
    }

    public bool CheckAnimatorStateWithName(string stateName, string layerName = "base")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName);
    }

    public bool CheckAnimatorStateWithTag(string tagName, string layerName = "base")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsTag(tagName);
    }
}
