namespace MechanX;

//Rules should not have state
public abstract class Rule<T> where T : IState
{
    public abstract Result IsValidAction(Game<T> game, IAction action);

    public abstract void ApplyAction(Game<T> game, IAction action);
}
