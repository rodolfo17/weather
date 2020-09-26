using System;
using System.Threading.Tasks;
using System.ServiceProcess;

namespace Dallas
{
    static class Program
    {
        static async Task Main()
        {
            if (Environment.UserInteractive)
            {
                Service1 testSer = new Service1();
                await testSer.WriteToFile();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new Service1()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
