using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image lockOnIcon;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLockOnIcon(bool visiable, Vector3 screenPos)
    {
        lockOnIcon.gameObject.SetActive(visiable);
        lockOnIcon.transform.position = screenPos;
    }
}
