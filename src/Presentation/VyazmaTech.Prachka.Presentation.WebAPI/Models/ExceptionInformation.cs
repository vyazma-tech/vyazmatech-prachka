using System.Net;
using VyazmaTech.Prachka.Domain.Common.Errors;

namespace VyazmaTech.Prachka.Presentation.WebAPI.Models;

internal sealed class ExceptionInformation
{
    public HttpStatusCode StatusCode { get; set; }

    public Error[] Errors { get; set; } = default!;
}