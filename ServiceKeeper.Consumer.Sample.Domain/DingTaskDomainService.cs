using MediatR;
using ServiceKeeper.Consumer.Sample.Domain.Entity;

namespace ServiceKeeper.Consumer.Sample.Domain
{
    public class DingTaskDomainService
    {
        private readonly DingTalkMessageClient messageClient;

        public DingTaskDomainService(DingTalkMessageClient messageClient)
        {
            this.messageClient = messageClient;
        }

        public async Task Send(DingTask task)
        {
            try
            {
                List<Dictionary<MessageSendMode, ParseReceiver>> receivers = await messageClient.GetReceiver(task.SendReceivers);
                int successfulSendCount = 0;

                await Task.WhenAll(receivers.Select(async receiver =>
                {
                    foreach (var item in receiver)
                    {
                        string message = DingTalkMessageClient.GetMessage(task.SendMessage, item.Key, item.Value);
                        bool sendCompleted = await messageClient.Send(item.Key, item.Value, message);

                        if (sendCompleted)
                        {
                            Interlocked.Increment(ref successfulSendCount);
                        }
                    }
                }));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
