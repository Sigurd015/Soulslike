using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionControl : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnAnimatorMove()
    {
        SendMessageUpwards("OnUpdateRM", anim.deltaPosition);
    }
}
