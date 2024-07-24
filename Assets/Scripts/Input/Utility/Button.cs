public class Button
{
    public bool isPressing { get; private set; }
    public bool onPressed { get; private set; }
    public bool onReleased { get; private set; }
    public bool isExtending { get; private set; }
    public bool isDelaying { get; private set; }

    public float extendingDuration = 0.15f;
    public float delayingDuration = 0.15f;
    private bool curState = false;
    private bool lastState = false;

    private Timer extTimer = new Timer();
    private Timer delayTimer = new Timer();

    public void Tick(bool input, float dt)
    {
        extTimer.Tick(dt);
        delayTimer.Tick(dt);
        curState = input;
        isPressing = curState;

        onPressed = false;
        onReleased = false;

        if (curState != lastState)
        {
            if (curState)
            {
                onPressed = true;
                delayTimer.Go(delayingDuration);
            }
            else
            {
                onReleased = true;
                extTimer.Go(extendingDuration);
            }
        }

        lastState = curState;

        isExtending = extTimer.IsRunning();
        isDelaying = delayTimer.IsRunning();
    }
}