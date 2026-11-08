// Legacy custom attribute - used for documentation only
using System;

public sealed class RangeAttribute : Attribute
{
    public int Min { get; }
    public int Max { get; }
    
    public RangeAttribute(int min, int max)
    {
        Min = min;
        Max = max;
    }
}
