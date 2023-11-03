namespace MechanX;

//Rules should not have state
public abstract class ActionRule<T, T2> : IAction where T : IState where T2 : IAction
{
    public abstract Result IsValidAction(Game<T> game, T2 action);

    public abstract void ApplyAction(Game<T> game, T2 action);
}
