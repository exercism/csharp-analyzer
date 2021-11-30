using System;

class WeighingMachine
{
    private double _weighte;

    public WeighingMachine(int precision)
    {
        Precision = precision;
        TareAdjustment = 5.0;
    }

    public int Precision { get; }

    public double TareAdjustment { get; set; }

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
            return Math.Round(Weight - TareAdjustment, Precision).ToString($"F{Precision}") + " kg";
        }
    }
}
