using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Mails
{
    public class MailAttachments
    {
        private const int MaxAttachmentNum = 10;
        private readonly IList<string> _attachments;

        public MailAttachments()
        {
            _attachments = new List<string>();
        }

        public string this[int index]
        {
            get { return _attachments[index]; }
        }

        /// <summary>
        /// 添加邮件附件
        /// </summary>
        /// <param name="filePaths">附件的绝对路径</param>
        public void Add(params string[] filePaths)
        {
            if (filePaths == null)
            {
                throw (new ArgumentNullException("filePaths", " 附件不能为空"));
            }
            foreach (var filePath in filePaths)
            {
                Add(filePath);
            }
        }

        /// <summary>
        /// 添加一个附件,当指定的附件不存在时，忽略该附件，不产生异常。
        /// </summary>
        /// <param name="filePath">附件的绝对路径</param>
        public void Add(string filePath)
        {
            //当附件存在时才加入,否则忽略
            if (!System.IO.File.Exists(filePath)) return;
            if (_attachments.Count < MaxAttachmentNum)
            {
                _attachments.Add(filePath);
            }
        }

        public void Clear()//清除所有附件
        {
            _attachments.Clear();
        }

        public int Count//获取附件个数
        {
            get { return _attachments.Count; }
        }
    }
}
