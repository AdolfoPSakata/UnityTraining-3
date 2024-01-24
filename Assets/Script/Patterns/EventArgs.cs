public class EventArgs : IEventArgs
{
    public bool State { get; }
    
    public EventArgs(bool state)
    {
        State = state;
    }
}
