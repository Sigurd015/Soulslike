using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimIKControl : MonoBehaviour
{
    public Vector3 leftArmFix = Vector3.zero;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        // Left arm animation fix
        //Transform leftLowerArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        //anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(leftLowerArm.localEulerAngles += leftArmFix));
    }
}
