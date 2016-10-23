using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMonitor
{
    /// <summary>
    /// 邮件工具类
    /// </summary>
    public class SmtpHelper
    {
        /// <summary>
        /// 服务器
        /// </summary>
        public string Host { get; private set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; private set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; private set; }
        /// <summary>
        /// 默认实例，各项参数在配置文件中设置
        /// </summary>
        public static SmtpHelper Default { get; private set; }

        static SmtpHelper()
        {
            string host = Configuration.MailHost;
            string username = Configuration.MailUsername;
            string password = Configuration.MailPassword;
            Default = new SmtpHelper(host, username, password);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host">服务器</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public SmtpHelper(string host, string username, string password)
        {
            Host = host;
            Username = username;
            Password = password;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="body">正文</param>
        /// <param name="to">收件人</param>
        public void Send(string subject, string body, params string[] to)
        {
            using (SmtpClient sc = new SmtpClient())
            {
                sc.Host = Host;
                sc.Credentials = new NetworkCredential(Username, Password);
                sc.EnableSsl = true;

                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(Username);
                foreach (string toAddress in to)
                {
                    mm.To.Add(toAddress);
                }
                mm.Subject = subject;
                mm.Body = body;

                sc.Send(mm);
                mm.Dispose();
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="from">发信人</param>
        /// <param name="to">收件人</param>
        /// <param name="subject">主题</param>
        /// <param name="body">正文</param>
        /// <param name="cc">抄送</param>
        /// <param name="attachment">附件</param>
        public void Send(string from, string[] to, string subject, string body, string[] cc, params Attachment[] attachment)
        {
            using (SmtpClient sc = new SmtpClient())
            {
                sc.Host = Host;
                sc.Credentials = new NetworkCredential(Username, Password);
                sc.EnableSsl = true;

                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(from);
                foreach (string toAddress in to)
                {
                    mm.To.Add(toAddress);
                }
                mm.Subject = subject;
                mm.Body = body;
                if (cc != null)
                {
                    foreach (string item in cc)
                    {
                        mm.CC.Add(new MailAddress(item));
                    }
                }
                if (attachment.Any())
                {
                    foreach (Attachment item in attachment)
                    {
                        mm.Attachments.Add(item);
                    }
                }
                mm.Sender = new MailAddress(Username);
                sc.Send(mm);
                mm.Dispose();
            }
        }
    }
}
