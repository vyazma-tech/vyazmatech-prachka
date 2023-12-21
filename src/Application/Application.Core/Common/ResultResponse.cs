using Domain.Common.Result;

namespace Application.Core.Common;

public readonly struct ResultResponse<TValue>
{
    private readonly ResultState _state;

    public ResultResponse(TValue value)
    {
        _state = ResultState.Success;
        Value = value;
        Error = default!;
    }

    public ResultResponse(Exception exception)
    {
        _state = ResultState.Faulted;
        Error = exception;
        Value = default!;
    }

    public static implicit operator ResultResponse<TValue>(TValue value)
    {
        return new ResultResponse<TValue>(value);
    }

    public bool IsFaulted => _state == ResultState.Faulted;
    public bool IsSuccess => _state == ResultState.Success;

    public TValue Value { get; }

    public Exception Error { get; }
}