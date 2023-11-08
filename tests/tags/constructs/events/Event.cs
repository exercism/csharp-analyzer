namespace DefaultNamespace;

using System;

public delegate int PerformCalculation(int x, int y);

public class ThresholdReachedEventArgs : EventArgs
{
    public int Threshold { get; set; }
    public DateTime TimeReached { get; set; }
}

class Counter
{
    public event EventHandler ThresholdReached;

    protected virtual void OnThresholdReached(EventArgs e)
    {
        ThresholdReached?.Invoke(this, e);
    }
}

class ProgramTwo
{
    static void Test()
    {
        var c = new Counter();
        c.ThresholdReached += c_ThresholdReached;
        c.ThresholdReached -= c_ThresholdReached;
    }

    static void c_ThresholdReached(object sender, EventArgs e)
    {
        Console.WriteLine("The threshold was reached.");
    }
}