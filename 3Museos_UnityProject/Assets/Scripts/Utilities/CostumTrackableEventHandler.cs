using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumTrackableEventHandler : DefaultTrackableEventHandler
{
    public static event EventHandler<TrackingChangedEventArgs> TrackingChanged;

    protected override void OnTrackingFound()
    {
        InvokeTrackingChanged(true);
        base.OnTrackingFound();
    }

    protected override void OnTrackingLost()
    {
        InvokeTrackingChanged(false);
        base.OnTrackingLost();
    }

    private void InvokeTrackingChanged(bool state)
    {
        var e = TrackingChanged;
        e?.Invoke(this, new TrackingChangedEventArgs(state));

    }
}


public class TrackingChangedEventArgs
{
    public bool IsTracking { get; private set; }

    public TrackingChangedEventArgs(bool isTracking)
    {
        IsTracking = isTracking;
    }
}
