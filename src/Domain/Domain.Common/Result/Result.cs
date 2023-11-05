using Domain.Common.Errors;

namespace Domain.Common.Result;

public readonly struct Result<TValue>
{
    private readonly Exception _exception;
    private readonly TValue _value;
    private readonly ResultState _state;

    public Result(TValue value)
    {
        _state = ResultState.Success;
        _value = value;
        _exception = default!;
    }

    public Result(Exception exception)
    {
        _state = ResultState.Faulted;
        _exception = exception;
        _value = default(TValue)!;
    }

    public static implicit operator Result<TValue>(TValue value)
        => new Result<TValue>(value);

    public bool IsFaulted => _state == ResultState.Faulted;
    public bool IsSuccess => _state == ResultState.Success;
    public TValue Value => IsSuccess ? _value : throw new InvalidOperationException(_exception.Message);
    public Exception Error => IsFaulted ? _exception : throw new InvalidOperationException();

    public TResult Match<TResult>(Func<TValue, TResult> successAction, Func<Exception, TResult> failAction)
        => IsSuccess ? successAction(_value) : failAction(_exception);
}