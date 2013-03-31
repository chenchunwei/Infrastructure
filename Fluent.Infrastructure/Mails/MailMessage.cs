using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Mails
{
    public class MailMessage
    {
        private const int MaxRecipientNum = 10;
        public MailMessage()
        {
            Recipients = new List<string>();
            Attachments = new MailAttachments();
            BodyFormat = MailFormat.Text;
            Priority = MailPriority.Normal;
            Charset = "GB2312";
        }

        public string Charset { get; set; }

        public string From { get; set; }

        public string FromName { get; set; }

        public string Body { get; set; }

        public string Subject { get; set; }

        public MailAttachments Attachments { get; set; }

        public MailPriority Priority { get; set; }

        public IList Recipients { get; private set; }

        public void AddRecipient(string recipient)
        {
            if (Recipients.Count < MaxRecipientNum)
            {
                Recipients.Add(recipient);
            }
        }

        public void AddRecipients(params string[] recipients)
        {
            if (recipients == null)
            {
                throw (new ArgumentException("收件人不能为空."));
            }
            foreach (var recipient in recipients)
            {
                AddRecipients(recipient);
            }
        }
        public MailFormat BodyFormat { get; set; }
    }
}
