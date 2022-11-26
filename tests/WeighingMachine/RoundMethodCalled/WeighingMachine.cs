using System;

class WeighingMachine
{
    private double _weighte;

    public WeighingMachine(int precision)
    {
        Precision = precision;
    }

    public int Precision { get; }

    public double TareAdjustment { get; set; } = 5.0;

    public double Weight
    {
        get
        {
            return _weighte;
        }
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException();
            _weighte = value;
        }
    }

    public string DisplayWeight
    {
        get
        {
            return (Weight - TareAdjustment).ToString($"F{Precision}") + " kg";
        }
    }
}
