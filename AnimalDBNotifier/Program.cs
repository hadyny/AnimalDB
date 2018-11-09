using System.ServiceProcess;

namespace AnimalDB.Notifier
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new AnimalDBNotifier() 
            };
            ServiceBase.Run(ServicesToRun);

        }
    }
}
