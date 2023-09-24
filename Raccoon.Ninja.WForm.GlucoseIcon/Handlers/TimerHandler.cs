using Raccoon.Ninja.Extensions.Desktop.Logging;
using Timer = System.Windows.Forms.Timer;


namespace Raccoon.Ninja.WForm.GlucoseIcon.Handlers;

public class TimerHandler
{
    private readonly IDictionary<Guid, Timer> _timers = new Dictionary<Guid, Timer>();

    public TimerHandler()
    {
        Logger.LogTrace("Initializing TimerHandler instance");
    }

    public Guid AddTimer(int intervalInMinutes)
    {
        Logger.LogTrace("Adding timer with interval of {Interval} minutes", intervalInMinutes);
        var timerId = Guid.NewGuid();
        _timers.Add(timerId, new Timer
        {
            Interval = MinutesToMilliseconds(intervalInMinutes)
        });

        return timerId;
    }

    public void AddTicker(Guid timerId, Action handler)
    {
        EnsureTimerIdExists(timerId);
        _timers[timerId].Tick += (sender, args) => handler();
    }

    public void AddAsyncTicker(Guid timerId, Func<Task> handler)
    {
        Logger.LogTrace("Adding async ticker to timer with id {TimerId}", timerId);
        EnsureTimerIdExists(timerId);
        _timers[timerId].Tick += async (sender, args) => await handler();
    }

    public void StartTimer(Guid timerId)
    {
        EnsureTimerIdExists(timerId);
        _timers[timerId].Start();
    }

    public void StopTimer(Guid timerId)
    {
        EnsureTimerIdExists(timerId);
        _timers[timerId].Stop();
    }

    private void EnsureTimerIdExists(Guid timerId)
    {
        if (_timers.ContainsKey(timerId))
            return;

        throw new ArgumentException($"The timer with id {timerId} was not found.");
    }

    private static int MinutesToMilliseconds(int minutes)
    {
        return minutes * 60 * 1000;
    }
}