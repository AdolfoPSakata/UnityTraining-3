public class EventArgs : IEventArgs
{
    public bool State { get; }
    public float Value { get; }
    
    public EventArgs(bool state)
    {
        State = state;
    }
    public EventArgs(float value)
    {
        Value = value;
    }
}
