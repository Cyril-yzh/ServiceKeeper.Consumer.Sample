﻿using MediatR;
using ServiceKeeper.Consumer.Sample.Domain.Entity;

namespace ServiceKeeper.Consumer.Sample.Domain
{
    public class DingTaskDomainService
    {
        private readonly IMediator mediator;
        private readonly DingTalkMessageClient messageClient;

        public DingTaskDomainService(IMediator mediator, DingTalkMessageClient messageClient)
        {
            this.mediator = mediator;
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
           
            //if (successfulSendCount > 0)
            //{
            //    await mediator.Publish(new SendCompletedEvent(successfulSendCount, task.SendMessage.Template.ToString()));
            //}
        }
    }
}
