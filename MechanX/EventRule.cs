namespace MechanX;

public abstract class EventRule<T> where T : IState
{
    public abstract Result IsValidEvent<TEvent>(Game<T> game, TEvent? evnt) where TEvent : class, IEvent;

    public abstract void ApplyEvent<TEvent>(Game<T> game, TEvent? evnt) where TEvent : class, IEvent;
}