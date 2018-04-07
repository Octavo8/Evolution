namespace Evolution
{
    /// <summary>
    /// This interface allows us to abstract comparison opertaions.
    /// </summary>
    public interface IComparator
    {
        string Name { get; }
        bool Invoke(double value1, double value2);
    }

}