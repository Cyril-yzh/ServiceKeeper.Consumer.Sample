using MediatR;

namespace ServiceKeeper.Consumer.Sample.Domain.DomianEvents
{
    public record SendCompletedEvent(int SuccessfulSendCount, string Message) : INotification;
}
