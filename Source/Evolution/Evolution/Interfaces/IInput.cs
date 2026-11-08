namespace Evolution
{
    /// <summary>
    /// This interface is to abstract input types
    /// </summary>
    public interface IInput
    {
        string Name { get; set; }
        double Value { get; set; }
        
        InputTypes InputType { get; }
    }
}