using MediatR.NotificationPublishers;

namespace JobBoard.WebApi.Application.Response
{
    public class JWTResponse
    {
        public string? AuthToken { get; set; }
    }
}
