/*******************************************
 *  Desc:       Smtp mail 发送接口
 *  Author:     July 
 *  Date:       2014-09-28
********************************************/
using System;
using System.Net;
using System.Text;
using System.Net.Mail;

namespace Common
{
    /// <summary>SmtpHelper</summary>
    public class SmtpHelper
    {
        /// <summary>
        /// 发送Domain
        /// </summary>
        public string Domain = "";

        /// <summary>
        /// smtp 用户
        /// </summary>
        public string SmtpUser = "";

        /// <summary>
        /// smtp 密码
        /// </summary>
        public string SmtpPass = "";

        /// <summary>
        /// from 地址
        /// </summary>
        public string FromMail = "";

        /// <summary>
        /// from 名称
        /// </summary>
        public string FromUser = "";

        /// <summary>
        /// 单个地址
        /// </summary>
        public string ToMail = "";

        /// <summary>
        /// 多个地址
        /// </summary>
        public string[] ToMailArray = null;

        /// <summary>
        /// 发送标题
        /// </summary>
        public string Subject = "";

        /// <summary>
        /// 发送内容
        /// </summary>
        public string Body = "";

        /// <summary>
        /// 是否html邮件,true = html,false = text
        /// </summary>
        public bool IsHtml = true;


        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <returns>发送成功返回"",发送失败返回：出错信息</returns>
        public string SendSmtpMail()
        {
            //--------------------------------------------------
            if (string.IsNullOrEmpty(Subject))
            {
                return "缺少标题！";
            }
            //--------------------------------------------------
            if (ToMailArray == null && string.IsNullOrEmpty(ToMail))
            {
                return "缺少发送对象地址！";
            }

            //--------------------------------------------------
            if (string.IsNullOrEmpty(FromMail))
            {
                return "配置出错，缺少smtp邮箱地址！";
            }

            //--------------------------------------------------
            if (string.IsNullOrEmpty(SmtpUser))
            {
                return "配置出错，缺少smtp用户！";
            }
            //--------------------------------------------------
            if (string.IsNullOrEmpty(Domain))
            {
                return "配置出错，缺少Domain！";
            }


            //--------------------------------------------------
            string msg = "";

            //--------------------------------------------------
            var message = new MailMessage();
            message.Body = Body;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = Subject;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = IsHtml;
            message.From = new MailAddress(FromMail, FromUser, System.Text.Encoding.UTF8);

            //--------------------------------------------------
            if (!string.IsNullOrEmpty(ToMail))
            {
                message.To.Add(new MailAddress(ToMail, "", System.Text.Encoding.UTF8));
            }
            
            if (ToMailArray != null)
            {
                foreach (var item in ToMailArray)
                {
                    message.To.Add(new MailAddress(item, "", System.Text.Encoding.UTF8));
                }
            }

            //--------------------------------------------------
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = Domain;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(SmtpUser, SmtpPass);
            try
            {
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                //Error, could not send the message
                msg = (ex.Message);
            }
            return msg;
        }
    }
}
