using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public ActorManager am;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Weapon")
        {
            am.DoDamage();
        }
    }
}
