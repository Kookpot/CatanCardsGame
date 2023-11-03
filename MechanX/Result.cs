namespace MechanX;

public class Result
{
    public static Result Ok() => new();
    public bool IsFailed => Reasons.Any();

    public bool IsSuccess => !IsFailed;

    public List<Reason> Reasons { get; } = new();

    public Result WithReason(Reason reason)
    {
        Reasons.Add(reason);
        return this;
    }

    public Result WithReasons(IEnumerable<Reason> reasons)
    {
        Reasons.AddRange(reasons);
        return this;
    }
}

public class Result<TResult> : Result
{
    public new Result<TResult> WithReason(Reason reason)
    {
        Reasons.Add(reason);
        return this;
    }

    public new Result<TResult> WithReasons(IEnumerable<Reason> reasons)
    {
        Reasons.AddRange(reasons);
        return this;
    }

    private TResult _value;

    public TResult ValueOrDefault => _value;

    public TResult Value
    {
        get
        {
            ThrowIfFailed();

            return _value;
        }
        private set
        {
            ThrowIfFailed();

            _value = value;
        }
    }

    public Result<TResult> WithValue(TResult value)
    {
        Value = value;
        return this;
    }

    private void ThrowIfFailed()
    {
        if (IsFailed)
        {
            throw new InvalidOperationException($"Result is in status failed. Value is not set. Having: {string.Join(",", Reasons.Select(x => x.Message))}");
        }
    }
}