namespace MechanX;

//game has state and rules and players
//can take action and relays events
public abstract class Game<T> where T : IState
{
    public T State { get; set; }

    public List<Rule<T>> Rules { get; set; } = new();
    public Dictionary<Type, List<IAction>> ActionRules { get; set; } = new();
    public Dictionary<Type, List<EventRule<T>>> EventRules { get; set; } = new();

    public List<Player> Players { get; set; } = new();

    public bool GameOver { get; set; }

    public List<Player> Winners { get; set; } = new ();

    private readonly object _lockObject = new();

    public Game(List<Player> players, T state)
    {
        Players = players;
        State = state;
    }

    public Result TakeAction<T2>(T2 action) where T2 : IAction
    {
        lock (_lockObject)
        {
            var result = new Result();
            foreach (Rule<T> rule in Rules)
            {
                var ruleResult = rule.IsValidAction(this, action);
                if (!ruleResult.IsSuccess)
                {
                    result.WithReasons(ruleResult.Reasons);
                }
            }
            foreach (ActionRule<T, T2> rule in GetActionRules<T2>())
            {
                var ruleResult = rule.IsValidAction(this, action);
                if (!ruleResult.IsSuccess)
                {
                    result.WithReasons(ruleResult.Reasons);
                }
            }
            if (!result.IsSuccess)
            {
                return result;
            }
            foreach (Rule<T> rule in Rules)
            {
                rule.ApplyAction(this, action);
            }
            foreach (ActionRule<T, T2> rule in GetActionRules<T2>())
            {
                rule.ApplyAction(this, action);
            }
            return Result.Ok();
        }
    }

    private IEnumerable<ActionRule<T, T2>> GetActionRules<T2>() where T2 : IAction
    {
        return ActionRules[typeof(T2)].Cast<ActionRule<T, T2>>();
    }

    public void AddRule(Rule<T> rule)
    {
        Rules.Add(rule);
    }

    public void AddActionRule<T2>(ActionRule<T, T2> rule) where T2 : IAction
    {
        if (!ActionRules.ContainsKey(typeof(T2)))
        {
            ActionRules.Add(typeof(T2), new List<IAction>());
        }
        ActionRules[typeof(T2)].Add(rule);
    }

    //mediator stuff
    public void AddEventRule<TEvent>(EventRule<T> rule) where TEvent : IEvent
    {
        if (!EventRules.ContainsKey(typeof(TEvent)))
        {
            EventRules.Add(typeof(TEvent), new List<EventRule<T>>());
        }
        EventRules[typeof(TEvent)].Add(rule);
    }

    public Result RaiseEvent<TEvent>() where TEvent : class, IEvent
    {
        return RaiseEventInternal((TEvent?) null);
    }

    public Result RaiseEvent<TEvent>(TEvent evnt) where TEvent : class, IEvent
    {
        return RaiseEventInternal(evnt);
    }

    private Result RaiseEventInternal<TEvent>(TEvent? evnt = null) where TEvent : class, IEvent
    {
        var result = new Result();
        foreach (EventRule<T> rule in EventRules[typeof(TEvent)])
        {
            var ruleResult = rule.IsValidEvent(this, evnt);
            if (!ruleResult.IsSuccess)
            {
                result.WithReasons(ruleResult.Reasons);
            }
        }
        if (!result.IsSuccess)
        {
            return result;
        }
        foreach (EventRule<T> rule in EventRules[typeof(TEvent)])
        {
            rule.ApplyEvent(this, evnt);
        }
        return Result.Ok();
    }
}