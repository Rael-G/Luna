using System;

namespace Luna.Core.Animations;

public static class Timing
{
    /// <summary>
    /// Executes an action repeatedly with a specific interval in milliseconds.
    /// </summary>
    /// <param name="interval">The interval between each execution of the action in milliseconds.</param>
    /// <param name="action">The action to be executed repeatedly.</param>
    /// <param name="cancellationToken">The cancellation token to stop the repeated execution.</param>
    public static async void EveryAsync(int interval, Action action, CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(interval, cancellationToken);

            if (cancellationToken.IsCancellationRequested) break;

            action();
        }
    }

    /// <summary>
    /// Executes an action after a specific interval in milliseconds.
    /// </summary>
    /// <param name="interval">The time interval to wait in milliseconds before executing the action.</param>
    /// <param name="action">The action to be executed after the time interval.</param>
    public static async void AfterAsync(int interval, Action action, CancellationToken cancellationToken = default)
    {
        await Task.Delay(interval, cancellationToken);
        action();
    }
}
