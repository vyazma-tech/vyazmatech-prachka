using Domain.Common.Errors;

namespace Domain.Common.Result;

public class Result<TValue> : Result
{
    private readonly TValue _value;

    public Result(TValue value)
        : base(default!, ResultState.Success)
    {
        _value = value;
    }

    public Result(Error error)
        : base(error, ResultState.Faulted)
    {
        _value = default!;
    }

    public static implicit operator Result<TValue>(TValue value)
    {
        return new Result<TValue>(value);
    }

    public TValue Value => IsSuccess ? _value : throw new InvalidOperationException(Error.Message);

    public TResult Match<TResult>(Func<TValue, TResult> successAction, Func<Error, TResult> failAction)
    {
        return IsSuccess ? successAction(_value) : failAction(Error);
    }
}

public abstract class Result
{
    private readonly Error _error;
    private readonly ResultState _state;

    protected Result(Error error, ResultState state)
    {
        _state = state;
        _error = error;
    }

    public bool IsFaulted => _state == ResultState.Faulted;
    public bool IsSuccess => _state == ResultState.Success;

    public Error Error => IsFaulted ? _error : throw new InvalidOperationException();
}