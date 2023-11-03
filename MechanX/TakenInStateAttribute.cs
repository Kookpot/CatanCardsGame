namespace MechanX;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class TakenInStateAttribute<T> : Attribute where T : Enum
{
    public T State { get; }

    public TakenInStateAttribute(T state)
    {
        State = state;
    }
}