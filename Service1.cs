using System;
using System.IO;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;
using System.Net.Http;
using System.Collections.Generic;
using System.Web.Script.Serialization;


namespace Dallas
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteToFile();
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 300000;
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            WriteToFile();
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            WriteToFile();
        }

        public async Task WriteToFile()
        {
            string appId = "8113fcc5a7494b0518bd91ef3acc074f";
            int cityId = 4684888;
            string units = "metric";
            string url =
                string.Format("https://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}&units={2}",
                    cityId, appId, units);
            var client = new HttpClient();
            var content = await client.GetStringAsync(url);
            RootObject weatherInfo = (new JavaScriptSerializer()).Deserialize<RootObject>(content);
            string main = weatherInfo.weather[0].main;
            double temp = weatherInfo.main.temp;
            string precipitation = "";
            if (main == "Rain")
            {
                precipitation = "True";
            }
            else
            {
                precipitation = "False";
            }

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\Dallas_TX_Weather.csv";
            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(temp + " | °C | " + precipitation);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(temp + " | °C | " + precipitation);
                }
            }
        }

        public class Coord
        {
            public double lon { get; set; }
            public double lat { get; set; }
        }

        public class Weather
        {
            public int id { get; set; }
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }

        public class Main
        {
            public double temp { get; set; }
            public double feels_like { get; set; }
            public double temp_min { get; set; }
            public double temp_max { get; set; }
            public double pressure { get; set; }
            public double humidity { get; set; }
        }

        public class Wind
        {
            public double speed { get; set; }
            public int deg { get; set; }
        }

        public class Clouds
        {
            public int all { get; set; }
        }

        public class Sys
        {
            public int type { get; set; }
            public int id { get; set; }
            public string country { get; set; }
            public int sunrise { get; set; }
            public int sunset { get; set; }
        }

        public class RootObject
        {
            public Coord coord { get; set; }
            public List<Weather> weather { get; set; }
            public string @base { get; set; }
            public Main main { get; set; }
            public int visibility { get; set; }
            public Wind wind { get; set; }
            public Clouds clouds { get; set; }
            public int dt { get; set; }
            public Sys sys { get; set; }
            public int timezone { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public int cod { get; set; }
        }
    }
}
