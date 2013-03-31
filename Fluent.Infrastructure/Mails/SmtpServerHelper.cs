using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections;
namespace Fluent.Infrastructure.Mails
{

    public class SmtpServerHelper
    {
        private string CRLF = "\r\n";//回车换行

        /// <summary>
        /// 错误消息反馈
        /// </summary>
        private string _errmsg;

        /// <summary>
        /// TcpClient对象，用于连接服务器
        /// </summary> 
        private TcpClient _tcpClient;

        /// <summary>
        /// NetworkStream对象
        /// </summary> 
        private NetworkStream _networkStream;

        /// <summary>
        /// 服务器交互记录
        /// </summary>
        private string _logs = "";

        /// <summary>
        /// SMTP错误代码哈希表
        /// </summary>
        private readonly Hashtable _errCodeHt = new Hashtable();

        /// <summary>
        /// SMTP正确代码哈希表
        /// </summary>
        private readonly Hashtable _rightCodeHt = new Hashtable();

        public SmtpServerHelper()
        {
            SmtpCodeAdd();//初始化SMTPCode
        }

        ~SmtpServerHelper()
        {
            _networkStream.Close();
            _tcpClient.Close();
        }

        /// <summary>
        /// 将字符串编码为Base64字符串
        /// </summary>
        /// <param name="str">要编码的字符串</param>
        private static string Base64Encode(string str)
        {
            byte[] barray;
            barray = Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(barray);
        }

        /// <summary>
        /// 将Base64字符串解码为普通字符串
        /// </summary>
        /// <param name="str">要解码的字符串</param>
        private string Base64Decode(string str)
        {
            byte[] barray = Convert.FromBase64String(str);
            return Encoding.Default.GetString(barray);
        }

        /// <summary>
        /// 得到上传附件的文件流
        /// </summary>
        /// <param name="filePath">附件的绝对路径</param>
        private string GetStream(string filePath)
        {
            //建立文件流对象
            var fileStr = new System.IO.FileStream(filePath, System.IO.FileMode.Open);
            var by = new byte[System.Convert.ToInt32(fileStr.Length)];
            fileStr.Read(by, 0, by.Length);
            fileStr.Close();
            return (System.Convert.ToBase64String(by));
        }

        /// <summary>
        /// SMTP回应代码哈希表
        /// </summary>
        private void SmtpCodeAdd()
        {
            //[RFC 821 4.2.1.]
            /*
            4.2.2. NUMERIC ORDER LIST OF REPLY CODES

            211 System status, or system help reply
            214 Help message
            [Information on how to use the receiver or the meaning of a
            particular non-standard command; this reply is useful only
            to the human user]
            220 <domain> Service ready
            221 <domain> Service closing transmission channel
            250 Requested mail action okay, completed
            251 User not local; will forward to <forward-path>

            354 Start mail input; end with <CRLF>.<CRLF>

            421 <domain> Service not available,
            closing transmission channel
            [This may be a reply to any command if the service knows it
            must shut down]
            450 Requested mail action not taken: mailbox unavailable
            [E.g., mailbox busy]
            451 Requested action aborted: local error in processing
            452 Requested action not taken: insufficient system storage

            500 Syntax error, command unrecognized
            [This may include errors such as command line too long]
            501 Syntax error in parameters or arguments
            502 Command not implemented
            503 Bad sequence of commands
            504 Command parameter not implemented
            550 Requested action not taken: mailbox unavailable
            [E.g., mailbox not found, no access]
            551 User not local; please try <forward-path>
            552 Requested mail action aborted: exceeded storage allocation
            553 Requested action not taken: mailbox name not allowed
            [E.g., mailbox syntax incorrect]
            554 Transaction failed

            */

            _errCodeHt.Add("421", "服务未就绪，关闭传输信道");
            _errCodeHt.Add("432", "需要一个密码转换");
            _errCodeHt.Add("450", "要求的邮件操作未完成，邮箱不可用（例如，邮箱忙）");
            _errCodeHt.Add("451", "放弃要求的操作；处理过程中出错");
            _errCodeHt.Add("452", "系统存储不足，要求的操作未执行");
            _errCodeHt.Add("454", "临时认证失败");
            _errCodeHt.Add("500", "邮箱地址错误");
            _errCodeHt.Add("501", "参数格式错误");
            _errCodeHt.Add("502", "命令不可实现");
            _errCodeHt.Add("503", "服务器需要SMTP验证");
            _errCodeHt.Add("504", "命令参数不可实现");
            _errCodeHt.Add("530", "需要认证");
            _errCodeHt.Add("534", "认证机制过于简单");
            _errCodeHt.Add("538", "当前请求的认证机制需要加密");
            _errCodeHt.Add("550", "要求的邮件操作未完成，邮箱不可用（例如，邮箱未找到，或不可访问）");
            _errCodeHt.Add("551", "用户非本地，请尝试<forward-path>");
            _errCodeHt.Add("552", "过量的存储分配，要求的操作未执行");
            _errCodeHt.Add("553", "邮箱名不可用，要求的操作未执行（例如邮箱格式错误）");
            _errCodeHt.Add("554", "传输失败");


            /*
            211 System status, or system help reply
            214 Help message
            [Information on how to use the receiver or the meaning of a
            particular non-standard command; this reply is useful only
            to the human user]
            220 <domain> Service ready
            221 <domain> Service closing transmission channel
            250 Requested mail action okay, completed
            251 User not local; will forward to <forward-path>

            354 Start mail input; end with <CRLF>.<CRLF>
            */

            _rightCodeHt.Add("220", "服务就绪");
            _rightCodeHt.Add("221", "服务关闭传输信道");
            _rightCodeHt.Add("235", "验证成功");
            _rightCodeHt.Add("250", "要求的邮件操作完成");
            _rightCodeHt.Add("251", "非本地用户，将转发向<forward-path>");
            _rightCodeHt.Add("334", "服务器响应验证Base64字符串");
            _rightCodeHt.Add("354", "开始邮件输入，以<CRLF>.<CRLF>结束");

        }

        /// <summary>
        /// 发送SMTP命令
        /// </summary> 
        private bool SendCommand(string str)
        {
            byte[] WriteBuffer;
            if (str == null || str.Trim() == String.Empty)
            {
                return true;
            }
            _logs += str;
            WriteBuffer = Encoding.Default.GetBytes(str);
            try
            {
                _networkStream.Write(WriteBuffer, 0, WriteBuffer.Length);
            }
            catch
            {
                _errmsg = "网络连接错误";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 接收SMTP服务器回应
        /// </summary>
        private string RecvResponse()
        {
            int streamSize;
            var returnvalue = String.Empty;
            var readBuffer = new byte[1024];
            try
            {
                streamSize = _networkStream.Read(readBuffer, 0, readBuffer.Length);
            }
            catch
            {
                _errmsg = "网络连接错误";
                return "false";
            }

            if (streamSize == 0)
            {
                return returnvalue;
            }
            returnvalue = Encoding.Default.GetString(readBuffer).Substring(0, streamSize);
            _logs += returnvalue + this.CRLF;
            return returnvalue;
        }

        /// <summary>
        /// 与服务器交互，发送一条命令并接收回应。
        /// </summary>
        /// <param name="str">一个要发送的命令</param>
        /// <param name="errstr">如果错误，要反馈的信息</param>
        private bool Dialog(string str, string errstr)
        {
            if (str == null || str.Trim() == string.Empty)
            {
                return true;
            }
            if (!SendCommand(str))
            {
                return false;
            }
            var RR = RecvResponse();
            if (RR == "false")
            {
                return false;
            }
            //检查返回的代码，根据[RFC 821]返回代码为3位数字代码如220
            var rrCode = RR.Substring(0, 3);
            if (_rightCodeHt[rrCode] != null)
            {
                return true;
            }
            if (_errCodeHt[rrCode] != null)
            {
                _errmsg += (rrCode + _errCodeHt[rrCode].ToString());
                _errmsg += CRLF;
            }
            else
            {
                _errmsg += RR;
            }
            _errmsg += errstr;
            return false;
        }


        /// <summary>
        /// 与服务器交互，发送一组命令并接收回应。
        /// </summary>

        private bool Dialog(string[] str, string errstr)
        {
            if (str.Any(t => !Dialog(t, "")))
            {
                _errmsg += CRLF;
                _errmsg += errstr;
                return false;
            }

            return true;
        }


        //连接服务器
        private bool Connect(string smtpServer, int port)
        {
            //创建Tcp连接
            try
            {
                _tcpClient = new TcpClient(smtpServer, port);
            }
            catch (Exception e)
            {
                _errmsg = e.ToString();
                return false;
            }
            _networkStream = _tcpClient.GetStream();

            //验证网络连接是否正确
            if (_rightCodeHt[RecvResponse().Substring(0, 3)] == null)
            {
                _errmsg = "网络连接失败";
                return false;
            }
            return true;
        }

        private string GetPriorityString(MailPriority mailPriority)
        {
            var priority = "Normal";
            switch (mailPriority)
            {
                case MailPriority.Low:
                    priority = "Low";
                    break;
                case MailPriority.High:
                    priority = "High";
                    break;
            }
            return priority;
        }

        /// <summary>
        /// 发送电子邮件，SMTP服务器不需要身份验证
        /// </summary>
        /// <param name="smtpServer"></param>
        /// <param name="port"></param>
        /// <param name="mailMessage"></param>
        /// <returns></returns>
        public bool SendEmail(string smtpServer, int port, MailMessage mailMessage)
        {
            return SendEmail(smtpServer, port, false, "", "", mailMessage);
        }

        /// <summary>
        /// 发送电子邮件，SMTP服务器需要身份验证
        /// </summary>
        /// <param name="smtpServer"></param>
        /// <param name="port"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="mailMessage"></param>
        /// <returns></returns>
        public bool SendEmail(string smtpServer, int port, string username, string password, MailMessage mailMessage)
        {
            return SendEmail(smtpServer, port, false, username, password, mailMessage);
        }

        private bool SendEmail(string smtpServer, int port, bool ESmtp, string username, string password, MailMessage mailMessage)
        {
            if (Connect(smtpServer, port) == false)//测试连接服务器是否成功
                return false;

            string priority = GetPriorityString(mailMessage.Priority);
            bool html = (mailMessage.BodyFormat == MailFormat.Html);

            string[] sendBuffer;
            string sendBufferstr;

            //进行SMTP验证，现在大部分SMTP服务器都要认证
            if (ESmtp)
            {
                sendBuffer = new String[4];
                sendBuffer[0] = "EHLO " + smtpServer + CRLF;
                sendBuffer[1] = "AUTH LOGIN" + CRLF;
                sendBuffer[2] = Base64Encode(username) + CRLF;
                sendBuffer[3] = Base64Encode(password) + CRLF;
                if (!Dialog(sendBuffer, "SMTP服务器验证失败，请核对用户名和密码。"))
                    return false;
            }
            else
            {//不需要身份认证
                sendBufferstr = "HELO " + smtpServer + CRLF;
                if (!Dialog(sendBufferstr, ""))
                    return false;
            }

            //发件人地址
            sendBufferstr = "MAIL FROM:<" + mailMessage.From + ">" + CRLF;
            if (!Dialog(sendBufferstr, "发件人地址错误，或不能为空"))
                return false;

            //收件人地址
            sendBuffer = new string[mailMessage.Recipients.Count];
            for (var i = 0; i < mailMessage.Recipients.Count; i++)
            {
                sendBuffer[i] = "RCPT TO:<" + (string)mailMessage.Recipients[i] + ">" + CRLF;
            }
            if (!Dialog(sendBuffer, "收件人地址有误"))
                return false;

            /*
            SendBuffer=new string[10];
            for(int i=0;i<RecipientBCC.Count;i++)
            {

            SendBuffer[i]="RCPT TO:<" + RecipientBCC[i].ToString() +">" + CRLF;

            }

            if(!Dialog(SendBuffer,"密件收件人地址有误"))
            return false;
            */

            sendBufferstr = "DATA" + CRLF;
            if (!Dialog(sendBufferstr, ""))
                return false;

            //发件人姓名
            sendBufferstr = "From:" + mailMessage.FromName + "<" + mailMessage.From + ">" + CRLF;

            //if(ReplyTo.Trim()!="")
            //{
            // SendBufferstr+="Reply-To: " + ReplyTo + CRLF;
            //}

            //SendBufferstr+="To:" + ToName + "<" + Recipient[0] +">" +CRLF;
            //至少要有一个收件人
            if (mailMessage.Recipients.Count == 0)
            {
                return false;
            }
            else
            {
                sendBufferstr += "To:=?" + mailMessage.Charset.ToUpper() + "?B?" +
                Base64Encode((string)mailMessage.Recipients[0]) + "?=" + "<" + (string)mailMessage.Recipients[0] + ">" + CRLF;
            }

            //SendBufferstr+="CC:";
            //for(int i=0;i<Recipient.Count;i++)
            //{
            // SendBufferstr+=Recipient[i].ToString() + "<" + Recipient[i].ToString() +">,";
            //}
            //SendBufferstr+=CRLF;

            sendBufferstr +=
            (string.IsNullOrEmpty(mailMessage.Subject) ? "Subject:" : ((mailMessage.Charset == "") ? ("Subject:" +
            mailMessage.Subject) : ("Subject:" + "=?" + mailMessage.Charset.ToUpper() + "?B?" +
            Base64Encode(mailMessage.Subject) + "?="))) + CRLF;
            sendBufferstr += "X-Priority:" + priority + CRLF;
            sendBufferstr += "X-MSMail-Priority:" + priority + CRLF;
            sendBufferstr += "Importance:" + priority + CRLF;
            sendBufferstr += "X-Mailer: Lion.Web.Mail.SmtpMail Pubclass [cn]" + CRLF;
            sendBufferstr += "MIME-Version: 1.0" + CRLF;
            if (mailMessage.Attachments.Count != 0)
            {
                sendBufferstr += "Content-Type: multipart/mixed;" + CRLF;
                sendBufferstr += " boundary=\"=====" +
                (html ? "001_Dragon520636771063_" : "001_Dragon303406132050_") + "=====\"" + CRLF + CRLF;
            }

            if (html)
            {
                if (mailMessage.Attachments.Count == 0)
                {
                    sendBufferstr += "Content-Type: multipart/alternative;" + CRLF;//内容格式和分隔符
                    sendBufferstr += " boundary=\"=====003_Dragon520636771063_=====\"" + CRLF + CRLF;
                    sendBufferstr += "This is a multi-part message in MIME format." + CRLF + CRLF;
                }
                else
                {
                    sendBufferstr += "This is a multi-part message in MIME format." + CRLF + CRLF;
                    sendBufferstr += "--=====001_Dragon520636771063_=====" + CRLF;
                    sendBufferstr += "Content-Type: multipart/alternative;" + CRLF;//内容格式和分隔符
                    sendBufferstr += " boundary=\"=====003_Dragon520636771063_=====\"" + CRLF + CRLF;
                }
                sendBufferstr += "--=====003_Dragon520636771063_=====" + CRLF;
                sendBufferstr += "Content-Type: text/plain;" + CRLF;
                sendBufferstr += ((mailMessage.Charset == "") ? (" charset=\"iso-8859-1\"") : (" charset=\"" +

                mailMessage.Charset.ToLower() + "\"")) + CRLF;
                sendBufferstr += "Content-Transfer-Encoding: base64" + CRLF + CRLF;
                sendBufferstr += Base64Encode("邮件内容为HTML格式，请选择HTML方式查看") + CRLF + CRLF;

                sendBufferstr += "--=====003_Dragon520636771063_=====" + CRLF;


                sendBufferstr += "Content-Type: text/html;" + CRLF;
                sendBufferstr += ((mailMessage.Charset == "") ? (" charset=\"iso-8859-1\"") : (" charset=\"" +
                mailMessage.Charset.ToLower() + "\"")) + CRLF;
                sendBufferstr += "Content-Transfer-Encoding: base64" + CRLF + CRLF;
                sendBufferstr += Base64Encode(mailMessage.Body) + CRLF + CRLF;
                sendBufferstr += "--=====003_Dragon520636771063_=====--" + CRLF;
            }
            else
            {
                if (mailMessage.Attachments.Count != 0)
                {
                    sendBufferstr += "--=====001_Dragon303406132050_=====" + CRLF;
                }
                sendBufferstr += "Content-Type: text/plain;" + CRLF;
                sendBufferstr += ((mailMessage.Charset == "") ? (" charset=\"iso-8859-1\"") : (" charset=\"" +
                mailMessage.Charset.ToLower() + "\"")) + CRLF;
                sendBufferstr += "Content-Transfer-Encoding: base64" + CRLF + CRLF;
                sendBufferstr += Base64Encode(mailMessage.Body) + CRLF;
            }

            //SendBufferstr += "Content-Transfer-Encoding: base64"+CRLF;

            if (mailMessage.Attachments.Count != 0)
            {
                for (int i = 0; i < mailMessage.Attachments.Count; i++)
                {
                    string filepath = (string)mailMessage.Attachments[i];
                    sendBufferstr += "--=====" +
                    (html ? "001_Dragon520636771063_" : "001_Dragon303406132050_") + "=====" + CRLF;
                    //SendBufferstr += "Content-Type: application/octet-stream"+CRLF;
                    sendBufferstr += "Content-Type: text/plain;" + CRLF;
                    sendBufferstr += " name=\"=?" + mailMessage.Charset.ToUpper() + "?B?" +
                    Base64Encode(filepath.Substring(filepath.LastIndexOf("\\") + 1)) + "?=\"" + CRLF;
                    sendBufferstr += "Content-Transfer-Encoding: base64" + CRLF;
                    sendBufferstr += "Content-Disposition: attachment;" + CRLF;
                    sendBufferstr += " filename=\"=?" + mailMessage.Charset.ToUpper() + "?B?" +
                    Base64Encode(filepath.Substring(filepath.LastIndexOf("\\") + 1)) + "?=\"" + CRLF + CRLF;
                    sendBufferstr += GetStream(filepath) + CRLF + CRLF;
                }
                sendBufferstr += "--=====" +
                (html ? "001_Dragon520636771063_" : "001_Dragon303406132050_") + "=====--" + CRLF + CRLF;
            }

            sendBufferstr += CRLF + "." + CRLF;//内容结束

            if (!Dialog(sendBufferstr, "错误信件信息"))
                return false;

            sendBufferstr = "QUIT" + CRLF;
            if (!Dialog(sendBufferstr, "断开连接时错误"))
                return false;

            _networkStream.Close();
            _tcpClient.Close();
            return true;
        }
    }
}











