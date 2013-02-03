using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Log.Messages
{
    /// <summary>
    /// 消息主体
    /// </summary>
    public class Message
    {
        /// <summary>
        /// 容器名称
        /// </summary>
        public string MessageContainerName { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime InsertTime { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string MessageContent { get; set; }
        /// <summary>
        /// 消息类型 
        /// </summary>
        public MessageType MessageType { get; set; }
    }
}
