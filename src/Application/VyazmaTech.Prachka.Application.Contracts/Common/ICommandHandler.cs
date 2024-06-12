﻿using Mediator;

namespace VyazmaTech.Prachka.Application.Contracts.Common;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse> { }