using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public ActorManager am;
    public GameObject whL;
    public GameObject whR;

    private Collider whLCol;
    private Collider whRCol;

    private void Start()
    {
        whLCol = whL.GetComponentInChildren<Collider>();
        whRCol = whR.GetComponentInChildren<Collider>();
    }

    void WeaponEnable()
    {
        if (am.ac.CheckAnimatorStateWithTag("attack1HL"))
        {
            whLCol.enabled = true;
            print("Left Weapon Enable");
        }
        else
        {
            whRCol.enabled = true;
            print("Right Weapon Enable");
        }
    }

    void WeaponDisable()
    {
        if (am.ac.CheckAnimatorStateWithTag("attack1HL"))
        {
            whLCol.enabled = false;
            print("Left Weapon Disabled");
        }
        else
        {
            whRCol.enabled = false;
            print("Right Weapon Disabled");
        }
    }
}
