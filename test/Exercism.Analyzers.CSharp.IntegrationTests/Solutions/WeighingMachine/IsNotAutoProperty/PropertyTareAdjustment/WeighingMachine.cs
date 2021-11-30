using System;

class WeighingMachine
{
    private double _weight;
    private double _tareAdjustment;

    public WeighingMachine(int precision)
    {
        Precision = precision;
    }

    public int Precision { get; }

    public double TareAdjustment
    {
        get
        { 
            return _tareAdjustment; 
        }
        set
        {
            _tareAdjustment = value;
        }
    }

    public double Weight
    {
        get
        {
            return _weight;
        }
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException();
            _weight = value;
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
