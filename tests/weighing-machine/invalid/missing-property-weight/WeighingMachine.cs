using System;

class WeighingMachine
{
    private double _weight;

    public WeighingMachine(int precision)
    {
        Precision = precision;
    }

    public int Precision { get; }

    public double TareAdjustment { get; set; } = 5.0;

    public string DisplayWeight
    {
        get
        {
            return Math.Round(Weight - TareAdjustment, Precision).ToString($"F{Precision}") + " kg";
        }
    }
}
