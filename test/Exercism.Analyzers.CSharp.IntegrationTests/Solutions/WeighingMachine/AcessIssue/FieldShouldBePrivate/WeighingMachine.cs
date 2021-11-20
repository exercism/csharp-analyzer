using System;

class WeighingMachine
{
    public double _weightStrangeName;

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
            return _weightStrangeName;
        }
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException();
            _weightStrangeName = value;
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
