using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Log.Messages;

namespace Fluent.Infrastructure.Log
{
    public static class MessageContainerExtensions
    {
        /// <summary>
        /// log4Net记录信息
        /// </summary>
        /// <param name="messageContainer"></param>
        /// <param name="messageType"></param>
        public static void Log4NetWith(this MessageContainer messageContainer, MessageType messageType)
        {
            var log = new DefaultLoggerFactory().GetLogger(messageContainer.Name);
            var messageText = string.Join(Environment.NewLine, messageContainer.Messages.Select(message => string.Format("{0}-{1}-{2}-{3}", message.InsertTime,
                    message.MessageContainerName, message.MessageType,
                    message.MessageContent)));
            switch (messageType)
            {
                case MessageType.Debug:
                    log.DebugFormat(messageText);
                    break;
                case MessageType.Info:
                    log.InfoFormat(messageText);
                    break;
                case MessageType.Warn:
                    log.WarnFormat(messageText);
                    break;
                case MessageType.Error:
                    log.ErrorFormat(messageText);
                    break;
                case MessageType.Fatal:
                    log.FatalFormat(messageText);
                    break;
            }
        }

        /// <summary>
        /// log4Net记录信息
        /// </summary>
        /// <param name="messageContainer"></param>
        public static void Log4Net(this MessageContainer messageContainer)
        {
            var log = new DefaultLoggerFactory().GetLogger(messageContainer.Name);

            var debugs = messageContainer.Messages.Where(o => o.MessageType == MessageType.Debug);
            var infos = messageContainer.Messages.Where(o => o.MessageType == MessageType.Info);
            var warns = messageContainer.Messages.Where(o => o.MessageType == MessageType.Warn);
            var errors = messageContainer.Messages.Where(o => o.MessageType == MessageType.Error);
            var fatals = messageContainer.Messages.Where(o => o.MessageType == MessageType.Fatal);
            if (debugs.Any())
            {
                log.DebugFormat(string.Join(Environment.NewLine, debugs.Select(message => string.Format("{0}-{1}-{2}-{3}", message.InsertTime,
                 message.MessageContainerName, message.MessageType,
                 message.MessageContent))));
            }
            if (infos.Any())
            {
                log.DebugFormat(string.Join(Environment.NewLine, infos.Select(message => string.Format("{0}-{1}-{2}-{3}", message.InsertTime,
                 message.MessageContainerName, message.MessageType,
                 message.MessageContent))));
            }
            if (warns.Any())
            {
                log.DebugFormat(string.Join(Environment.NewLine, warns.Select(message => string.Format("{0}-{1}-{2}-{3}", message.InsertTime,
                 message.MessageContainerName, message.MessageType,
                 message.MessageContent))));
            }
            if (errors.Any())
            {
                log.DebugFormat(string.Join(Environment.NewLine, errors.Select(message => string.Format("{0}-{1}-{2}-{3}", message.InsertTime,
                 message.MessageContainerName, message.MessageType,
                 message.MessageContent))));
            }
            if (fatals.Any())
            {
                log.DebugFormat(string.Join(Environment.NewLine, fatals.Select(message => string.Format("{0}-{1}-{2}-{3}", message.InsertTime,
                 message.MessageContainerName, message.MessageType,
                 message.MessageContent))));
            }
        }
    }
}
