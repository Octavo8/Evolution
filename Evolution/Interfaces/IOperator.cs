namespace Evolution
{
    /// <summary>
    /// Abstracting away mathematical operations.
    /// </summary>
    public interface IOperator
    {
        string Name { get;  }
        double Invoke(double value1, double value2);
    }

}