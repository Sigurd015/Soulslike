using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    public ActorController ac;
    public BattleManager bm;

    public void DoDamage()
    {
        // Temp
        ac.SentTrigger("hit");
    }
}
