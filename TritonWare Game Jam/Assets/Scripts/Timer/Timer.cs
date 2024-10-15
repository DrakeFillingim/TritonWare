using UnityEngine;

public class Timer : MonoBehaviour
{
    public float MaxTime { get; set; }
    public float CurrentTime { get; set; } = 0;
    public bool Autostart { get; set; }
    public bool Repeatable { get; set; }

    public System.Action onTimeout;

    public static Timer CreateTimer(GameObject obj, System.Action onTimeout_, float maxTime_, bool autostart_ = false, bool repeatable_ = true)
    {
        Timer timer = obj.AddComponent<Timer>();
        timer.onTimeout = onTimeout_;
        timer.MaxTime = maxTime_;
        timer.Autostart = autostart_;
        timer.Repeatable = repeatable_;

        timer.enabled = timer.Autostart;
        return timer;
    }

    private void Update()
    {
        Tick();
    }

    private void Tick()
    {
        CurrentTime += Time.deltaTime;
        if (CurrentTime >= MaxTime)
        {
            onTimeout();

            if (Repeatable)
            {
                Reset();
            }
            else
            {
                Destroy(this);
            }
        }
    }

    public void Reset()
    {
        CurrentTime = 0;
        enabled = Autostart;
    }

    public void Start()
    {
        enabled = true;
    }

    public void Stop()
    {
        enabled = false;
    }
}