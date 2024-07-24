public class Timer
{
    private enum STATE
    {
        IDLE, RUN, FINISHED
    }
    private STATE state;
    private float duration;
    private float elapsedTime;

    public void Tick(float dt)
    {
        switch (state)
        {
            case STATE.IDLE:
                break;
            case STATE.RUN:
                elapsedTime += dt;
                if (elapsedTime >= duration)
                    state = STATE.FINISHED;
                break;
            case STATE.FINISHED:
                break;
            default:
                break;
        }
    }

    public void Go(float duration)
    {
        elapsedTime = 0;
        this.duration = duration;
        state = STATE.RUN;
    }

    public void Stop()
    {
        state = STATE.IDLE;
    }

    public bool IsRunning()
    {
        return state == STATE.RUN ? true : false;
    }

    public bool IsFinished()
    {
        return state == STATE.FINISHED ? true : false;
    }
}