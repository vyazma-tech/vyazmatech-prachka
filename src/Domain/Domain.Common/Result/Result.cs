using Domain.Common.Errors;

namespace Domain.Common.Result;

public readonly struct Result<TValue>
{
    private readonly Error _error;
    private readonly TValue _value;
    private readonly ResultState _state;

    public Result(TValue value)
    {
        _state = ResultState.Success;
        _value = value;
        _error = default!;
    }

    public Result(Error error)
    {
        _state = ResultState.Faulted;
        _error = error;
        _value = default!;
    }

    public static implicit operator Result<TValue>(TValue value)
    {
        return new Result<TValue>(value);
    }

    public bool IsFaulted => _state == ResultState.Faulted;
    public bool IsSuccess => _state == ResultState.Success;
    public TValue Value => IsSuccess ? _value : throw new InvalidOperationException(_error.Message);
    public Error Error => IsFaulted ? _error : throw new InvalidOperationException();

    public TResult Match<TResult>(Func<TValue, TResult> successAction, Func<Error, TResult> failAction)
    {
        return IsSuccess ? successAction(_value) : failAction(_error);
    }
}