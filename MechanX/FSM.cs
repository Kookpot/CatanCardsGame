namespace MechanX;

public class FSM<TState, TTransition> where TState : Enum where TTransition : Enum
{
    public TState CurrentState { get; private set; }

    public Dictionary<TState, Dictionary<TTransition, TState>> Transitions { get; } = new();

    public FSM(TState beginState)
    {
        CurrentState = beginState;
    }

    public void AddTransition(TState beginState, TState endState, TTransition transition)
    {
        if (!Transitions.ContainsKey(beginState))
        {
            Transitions.Add(beginState, new Dictionary<TTransition, TState>());
        }
        Transitions[beginState].Add(transition, endState);
    }

    public void PerformTransition(TTransition transition)
    {
        CurrentState = Transitions[CurrentState][transition];
    }
}
