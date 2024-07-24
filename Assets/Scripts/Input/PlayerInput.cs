using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("===== Keyborad Settings =====")]
    // TODO: Make a better way to set these keys
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backwardKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode runKey = KeyCode.Space;
    public KeyCode lookUpKey = KeyCode.I;
    public KeyCode lookDownKey = KeyCode.K;
    public KeyCode lookLeftKey = KeyCode.J;
    public KeyCode lookRightKey = KeyCode.L;
    public KeyCode rightAtkKey = KeyCode.H;
    public KeyCode leftAtkKey = KeyCode.LeftShift;
    public KeyCode rightHAtkKey = KeyCode.U;
    public KeyCode leftHAtkKey = KeyCode.Tab;
    public KeyCode lockKey = KeyCode.V;
    public KeyCode actionKey = KeyCode.F;
    public KeyCode leftArrowKey = KeyCode.Q;
    public KeyCode rightArrowKey = KeyCode.E;
    public KeyCode upArrowKey = KeyCode.R;
    public KeyCode downArrowKey = KeyCode.B;
    public KeyCode useItemKey = KeyCode.T;
    //----------------

    public bool inputEnabled { get; set; } = true;
    // ===== Output signals =====
    public float dmag { get; private set; }
    public Vector3 dvec { get; private set; }
    public float jup { get; private set; }
    public float jright { get; private set; }
    public bool run { get; private set; }
    public bool jump { get; private set; }
    public bool roll { get; private set; }
    public bool lockOn { get; private set; }
    public bool leftAttack { get; private set; }
    public bool rightAttack { get; private set; }
    public bool leftHeavyAttack { get; private set; }
    public bool rightHeavyAttack { get; private set; }
    public bool defense { get; private set; }
    public bool action { get; private set; }
    public bool leftArrow { get; private set; }
    public bool rightArrow { get; private set; }
    public bool upArrow { get; private set; }
    public bool downArrow { get; private set; }
    public bool useItem { get; private set; }
    //----------------

    private float dup;
    private float targetDup;
    private float dright;
    private float targetDright;
    private float dupVelovity;
    private float drightVelovity;

    private Button runBtn = new Button();
    private Button rightAtkBtn = new Button();
    private Button leftAtkBtn = new Button();
    private Button rightHAtkBtn = new Button();
    private Button leftHAtkBtn = new Button();
    private Button lockBtn = new Button();
    private Button actionBtn = new Button();
    private Button leftArrowBtn = new Button();
    private Button rightArrowBtn = new Button();
    private Button upArrowBtn = new Button();
    private Button downArrowBtn = new Button();
    private Button useItemBtn = new Button();

    // Elliptical Grid Mapping
    // See: https://arxiv.org/ftp/arxiv/papers/1509/1509.06344.pdf
    private Vector2 SquareToCircle(Vector2 axis)
    {
        Vector2 output = Vector2.zero;
        output.x = axis.x * Mathf.Sqrt(1 - (axis.y * axis.y) / 2.0f);
        output.y = axis.y * Mathf.Sqrt(1 - (axis.x * axis.x) / 2.0f);

        return output;
    }

    private void UpdateDmagDvec(float dup, float dright)
    {
        Vector2 axis = SquareToCircle(new Vector2(dright, dup));
        dmag = Mathf.Sqrt(axis.y * axis.y + axis.x * axis.x);
        dvec = axis.x * transform.right + axis.y * transform.forward;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        runBtn.Tick(Input.GetKey(runKey), Time.deltaTime);
        leftAtkBtn.Tick(Input.GetKey(leftAtkKey), Time.deltaTime);
        rightAtkBtn.Tick(Input.GetKey(rightAtkKey), Time.deltaTime);
        leftHAtkBtn.Tick(Input.GetKey(leftHAtkKey), Time.deltaTime);
        rightHAtkBtn.Tick(Input.GetKey(rightHAtkKey), Time.deltaTime);
        lockBtn.Tick(Input.GetKey(lockKey), Time.deltaTime);
        actionBtn.Tick(Input.GetKey(actionKey), Time.deltaTime);
        leftArrowBtn.Tick(Input.GetKey(leftArrowKey), Time.deltaTime);
        rightArrowBtn.Tick(Input.GetKey(rightArrowKey), Time.deltaTime);
        upArrowBtn.Tick(Input.GetKey(upArrowKey), Time.deltaTime);
        downArrowBtn.Tick(Input.GetKey(downArrowKey), Time.deltaTime);
        useItemBtn.Tick(Input.GetKey(useItemKey), Time.deltaTime);

        jup = (Input.GetKey(lookUpKey) ? 1.0f : 0) - (Input.GetKey(lookDownKey) ? 1.0f : 0);
        jright = (Input.GetKey(lookRightKey) ? 1.0f : 0) - (Input.GetKey(lookLeftKey) ? 1.0f : 0);

        targetDup = (Input.GetKey(forwardKey) ? 1.0f : 0) - (Input.GetKey(backwardKey) ? 1.0f : 0);
        targetDright = (Input.GetKey(rightKey) ? 1.0f : 0) - (Input.GetKey(leftKey) ? 1.0f : 0);

        if (!inputEnabled)
        {
            targetDup = 0;
            targetDright = 0;
        }

        dup = Mathf.SmoothDamp(dup, targetDup, ref dupVelovity, 0.1f);
        dright = Mathf.SmoothDamp(dright, targetDright, ref drightVelovity, 0.1f);

        UpdateDmagDvec(dup, dright);

        run = (runBtn.isPressing && !runBtn.isDelaying) || runBtn.isExtending;
        jump = runBtn.onPressed && run;
        roll = runBtn.onReleased && runBtn.isDelaying;
        leftAttack = leftAtkBtn.onReleased;
        rightAttack = rightAtkBtn.onReleased;
        leftHeavyAttack = leftHAtkBtn.onReleased;
        rightHeavyAttack = rightHAtkBtn.onReleased;
        defense = leftAtkBtn.isPressing;
        lockOn = lockBtn.onPressed;
        action = actionBtn.onPressed;

        leftArrow = leftArrowBtn.onPressed && leftArrowBtn.isDelaying;
        rightArrow = rightArrowBtn.onPressed && rightArrowBtn.isDelaying;
        upArrow = upArrowBtn.onPressed && upArrowBtn.isDelaying;
        downArrow = downArrowBtn.onPressed && downArrowBtn.isDelaying;

        useItem = useItemBtn.onPressed && useItemBtn.isDelaying;
    }
}
