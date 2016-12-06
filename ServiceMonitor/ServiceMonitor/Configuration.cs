using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMonitor
{
    class Configuration
    {
        public static string Description { get; private set; }
        public static string DisplayName { get; private set; }
        public static string ServiceName { get; private set; }
        public static string ServiceMonitorJobCronExpression { get; private set; }
        public static string WebsiteMonitorJobCronExpression { get; private set; }
        public static string MeteorologyDataService { get; private set; }
        public static string MailHost { get; private set; }
        public static string MailUserName { get; private set; }
        public static string MailPassword { get; private set; }
        public static string MailTo { get; private set; }

        static Configuration()
        {
            Description = ConfigurationManager.AppSettings["Description"];
            DisplayName = ConfigurationManager.AppSettings["DisplayName"];
            ServiceName = ConfigurationManager.AppSettings["ServiceName"];
            ServiceMonitorJobCronExpression = ConfigurationManager.AppSettings["ServiceMonitorJobCronExpression"];
            WebsiteMonitorJobCronExpression = ConfigurationManager.AppSettings["WebsiteMonitorJobCronExpression"];
            MeteorologyDataService = ConfigurationManager.AppSettings["MeteorologyDataService"];
            MailHost = ConfigurationManager.AppSettings["MailHost"];
            MailUserName = ConfigurationManager.AppSettings["MailUserName"];
            MailPassword = ConfigurationManager.AppSettings["MailPassword"];
            MailTo = ConfigurationManager.AppSettings["MailTo"];

            string defaultService = "CustomService";
            Description = string.IsNullOrWhiteSpace(Description) ? defaultService : Description;
            DisplayName = string.IsNullOrWhiteSpace(DisplayName) ? defaultService : DisplayName;
            ServiceName = string.IsNullOrWhiteSpace(ServiceName) ? defaultService : ServiceName;

            string defaultExpression = "0 0 * * * ?";
            ServiceMonitorJobCronExpression = string.IsNullOrWhiteSpace(ServiceMonitorJobCronExpression) ? defaultExpression : ServiceMonitorJobCronExpression;
            WebsiteMonitorJobCronExpression = string.IsNullOrWhiteSpace(WebsiteMonitorJobCronExpression) ? defaultExpression : WebsiteMonitorJobCronExpression;

            MeteorologyDataService = string.IsNullOrWhiteSpace(MeteorologyDataService) ? "MeteorologyData" : MeteorologyDataService;
            MailHost = string.IsNullOrWhiteSpace(MailHost) ? "smtp.126.com" : MailHost;
            MailUserName = string.IsNullOrWhiteSpace(MailUserName) ? "zzfxdhz@126.com" : MailUserName;
            MailPassword = string.IsNullOrWhiteSpace(MailPassword) ? "ms5lmxin" : MailPassword;
            MailTo = string.IsNullOrWhiteSpace(MailTo) ? "564030525@qq.com" : MailTo;
        }
    }
}
