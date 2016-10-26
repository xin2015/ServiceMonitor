using Common.Logging;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMonitor
{
    class WebsiteMonitorJob : IJob
    {
        private static ILog logger;

        public static string CronExpression { get; set; }

        private static string url;

        static WebsiteMonitorJob()
        {
            logger = LogManager.GetLogger<WebsiteMonitorJob>();
            CronExpression = Configuration.WebsiteMonitorJobCronExpression;
            url = "http://www.nmc.cn/f/rest/real/59287";
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                string html = wc.DownloadString(url);
                if (string.IsNullOrWhiteSpace(html))
                {
                    logger.Warn("抓取广州市实时气象数据为空。");
                }
                else
                {
                    RealtimeWeatherData data = JsonConvert.DeserializeObject<RealtimeWeatherData>(html);
                    logger.Info("抓取广州市实时气象数据成功。");
                }
            }
            catch (WebException e)
            {
                string subject = "抓取广州市实时气象数据失败。";
                SmtpHelper.Default.Send(subject, e.Message, Configuration.MailTo.Split(','));
                logger.Error(subject, e);
            }
            catch (Exception e)
            {
                string subject = "网站监控服务出错。";
                SmtpHelper.Default.Send(subject, e.Message, Configuration.MailTo.Split(','));
                logger.Fatal(subject, e);
            }
        }
    }

    public class RealtimeWeatherData
    {
        /// <summary>
        /// 站点信息
        /// </summary>
        public NMCStation station { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime publish_time { get; set; }
        /// <summary>
        /// 天气
        /// </summary>
        public Weather weather { get; set; }
        /// <summary>
        /// 风
        /// </summary>
        public Wind wind { get; set; }
    }

    public class NMCStation
    {
        /// <summary>
        /// 中央气象台城市预报页面URL
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string province { get; set; }
    }

    public class Weather
    {
        /// <summary>
        /// 气温
        /// </summary>
        public decimal temperature { get; set; }
        /// <summary>
        /// 气压
        /// </summary>
        public decimal airpressure { get; set; }
        /// <summary>
        /// 相对湿度
        /// </summary>
        public decimal humidity { get; set; }
        /// <summary>
        /// 降水
        /// </summary>
        public decimal rain { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal rcomfort { get; set; }
        /// <summary>
        /// 舒适度
        /// </summary>
        public decimal icomfort { get; set; }
        /// <summary>
        /// 天气现象
        /// </summary>
        public string info { get; set; }
        /// <summary>
        /// 体感温度
        /// </summary>
        public decimal feelst { get; set; }
    }

    public class Wind
    {
        /// <summary>
        /// 风向
        /// </summary>
        public string direct { get; set; }
        /// <summary>
        /// 风力
        /// </summary>
        public string power { get; set; }
        /// <summary>
        /// 风速
        /// </summary>
        public string speed { get; set; }
    }
}
