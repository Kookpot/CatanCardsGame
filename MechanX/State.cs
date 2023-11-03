namespace MechanX;

public class State<TPhase, TTransition> : IState where TPhase : Enum where TTransition : Enum
{
    public FSM<TPhase, TTransition> FSM { get; protected set; }
}
