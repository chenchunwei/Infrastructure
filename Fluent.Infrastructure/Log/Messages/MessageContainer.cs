using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Log.Messages
{
    /// <summary>
    /// 消息容器
    /// </summary>
    public class MessageContainer
    {
        public MessageContainer(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("容器名字不能为空");
            Name = name;
            Messages = new List<Message>();
        }
        /// <summary>
        /// 容器名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 消自列表
        /// </summary>
        public List<Message> Messages { get; set; }

        /// <summary>
        /// 合并其它消息容器
        /// </summary>
        /// <param name="messageContainer"></param>
        /// <param name="alias"></param>
        public void Merge(MessageContainer messageContainer, string alias = "")
        {
            alias = string.IsNullOrEmpty(alias) ? messageContainer.Name : alias;
            if (alias == messageContainer.Name)
                throw new ArgumentException("并入的MessageContainer.Name与当前容器重名,请使用别名来区分");
            Messages = messageContainer.Messages.OrderBy(o => o.InsertTime).ToList();
        }
        /// <summary>
        /// 写入消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        public void Write(string message, MessageType messageType, params object[] args)
        {
            message = args == null || args.Length == 0 ? message : string.Format(message, args);
            Messages.Add(new Message()
            {
                InsertTime = DateTime.Now,
                MessageContainerName = Name,
                MessageContent = message,
                MessageType = messageType
            });
        }
        /// <summary>
        /// Deug
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Debug(string message, params object[] args)
        {
            Write(message, MessageType.Debug, args);
        }
        /// <summary>
        /// Info
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Info(string message, params object[] args)
        {
            Write(message, MessageType.Info, args);
        }
        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Warn(string message, params object[] args)
        {
            Write(message, MessageType.Warn, args);
        }
        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Error(string message, params object[] args)
        {
            Write(message, MessageType.Error, args);
        }
        /// <summary>
        /// Failed
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Failed(string message, params object[] args)
        {
            Write(message, MessageType.Debug, args);
        }
        /// <summary>
        /// Any
        /// </summary>
        /// <returns></returns>
        public bool Any()
        {
            return Messages.Any();
        }
        /// <summary>
        /// Any
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public bool Any(MessageType messageType)
        {
            return Messages.Any(message => (message.MessageType & messageType) > 0);
        }
        /// <summary>
        /// Predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Predicate(Predicate<MessageContainer> predicate)
        {
            return predicate(this);
        }
    }
}
