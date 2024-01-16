using System.Net;
using Domain.Common.Errors;

namespace Presentation.WebAPI.Models;

internal sealed class ExceptionInformation
{
    public HttpStatusCode StatusCode { get; set; }

    public IReadOnlyCollection<Error> Errors { get; set; } = default!;
}