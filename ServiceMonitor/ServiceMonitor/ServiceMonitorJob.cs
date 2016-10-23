using Common.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMonitor
{
    class ServiceMonitorJob : IJob
    {
        private static ILog logger;

        public static string CronExpression { get; set; }
        public static string MonitorServiceName { get; set; }

        static ServiceMonitorJob()
        {
            logger = LogManager.GetLogger<ServiceMonitorJob>();
            CronExpression = Configuration.ServiceMonitorJobCronExpression;
            MonitorServiceName = Configuration.MeteorologyDataService;
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                ServiceController sc = ServiceController.GetServices().FirstOrDefault(o => o.ServiceName == MonitorServiceName);
                if (sc != null)
                {
                    if (sc.Status == ServiceControllerStatus.Stopped)
                    {
                        try
                        {
                            sc.Start();
                            logger.InfoFormat("服务{0}启动成功。", sc.ServiceName);
                        }
                        catch (Exception e)
                        {
                            string subject = string.Format("服务{0}启动失败。", sc.ServiceName);
                            SmtpHelper.Default.Send(subject, e.Message, Configuration.MailTo.Split(','));
                            logger.Error(subject, e);
                        }
                    }
                    else
                    {
                        logger.InfoFormat("服务{0}正常运行。", sc.ServiceName);
                    }
                }
                else
                {
                    logger.WarnFormat("服务{0}不存在，请检查服务配置。", MonitorServiceName);
                }
            }
            catch (Exception e)
            {
                logger.Error("监控服务出错。", e);
            }
        }
    }
}
