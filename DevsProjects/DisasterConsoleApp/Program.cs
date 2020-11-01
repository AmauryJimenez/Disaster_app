using Commons.Services;
using Contracts.Services;
using Models;
using Models.Disasters;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DisasterConsoleApp
{
    class Program
    {
        private static Container container;
        private const string _welcomeMessage = "This program was made for learning porpuses. Here you can query for some disasters from the https://reliefweb.int/ web service. Here you will find the latest reported floods, earthquakes and epidemic disasters.";
        private static DisasterType[] _supportedDisasterTypes = new DisasterType[] { DisasterType.Flood, DisasterType.Earthquake, DisasterType.Epidemic };

        static void Main(string[] args)
        {
            container = new Container();
            container.Register<IServicesFabric, ServicesFabric>(Lifestyle.Singleton);
            container.Register<IDisasterInfoPersister, FileSystemServicePersister>(Lifestyle.Singleton);


            try
            {
                ShowWelcome();
                ShowMainWIndow();
            }
            catch (Exception exe)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(exe.ToString());
                Console.ForegroundColor = ConsoleColor.White;
            }
            finally
            {
                Console.Clear();
                Console.WriteLine("Program exited successfully!!!");
                Console.ReadKey();
            }
        }

        private static void ShowWelcome()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(_welcomeMessage);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private static void ShowMainWIndow()
        {
            IList<Disaster> disasters = new List<Disaster>();
            int option = -1;

            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("============================================");
                Console.WriteLine("===          DISASTERS MONITOR           ===");
                Console.WriteLine("============================================");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("1. Request disaster from Reliefweb");
                Console.WriteLine("2. Save disaster to disk");
                Console.WriteLine("3. Load disaster from disk");
                Console.WriteLine("4. Order disasters by date");
                Console.WriteLine("5. Order epidemic disasters by name");
                Console.WriteLine("6. Alert disaster");
                Console.WriteLine("0. Exit");
                Console.WriteLine();

                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Wait a sec please!...");
                Console.ForegroundColor = ConsoleColor.White;

                if (Int32.TryParse(key.KeyChar.ToString(), out option))
                {
                    DisasterInfoProvider disasterInfoProvider = option == 1 ? DisasterInfoProvider.Reliefweb : option == 3 ? DisasterInfoProvider.FileSystem : DisasterInfoProvider.None;

                    switch (option)
                    {
                        case 1:
                        case 3:
                            disasters = RequestDisasters(disasterInfoProvider);
                            break;
                        case 2:
                            PersistDisasters(disasters);
                            break;
                        case 4:
                            OrderDisasters(disasters);
                            break;
                        case 5:
                            OrderEpidemicDisastersByName(disasters);
                            break;
                        case 6:
                            AlertDisaster(disasters);
                            break;
                    }
                }

            } while (option != 0);
        }

        private static void AlertDisaster(IList<Disaster> disasters)
        {
            ShowDisasters(disasters);
            Console.WriteLine("Select the id of the disaster you want to alert: ");
            int alertId = Convert.ToInt32(Console.ReadLine().Trim());
            var alertDisaster = disasters.FirstOrDefault(d => d.Id == alertId);

            if(alertDisaster != null)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Alert: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(alertDisaster.AlertIt());
            }
            else
            {
                Console.WriteLine("Cannot find Disaster with id "+ alertId);
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Press any key to continue");

            Console.ReadKey();
        }

        private static void OrderEpidemicDisastersByName(IList<Disaster> disasters)
        {
            var ordededEpidemicDisasters = disasters.Where(d => d.DisasterType == DisasterType.Epidemic).OrderBy(d => d.Name);
            ShowDisasters(ordededEpidemicDisasters.ToList());

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Press any key to continue");

            Console.ReadKey();
        }

        private static void OrderDisasters(IList<Disaster> disasters)
        {
            var orderedDisasters = disasters.OrderBy(d => d.ReportedOn);
            ShowDisasters(orderedDisasters.ToList());

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Press any key to continue");

            Console.ReadKey();
        }

        private static void PersistDisasters(IList<Disaster> disasters)
        {
            IDisasterInfoPersister persister = container.GetInstance<IServicesFabric>().CreateIDisasterInfoPersister();
            persister.SaveDisasters(disasters);
            Console.WriteLine("Disasters saved");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Press any key to continue");

            Console.ReadKey();
        }

        private static IList<Disaster> RequestDisasters(DisasterInfoProvider disasterInfoProvider)
        {
            IList<Disaster> disasters;
            using (IDisasterInfoProvider provider = container.GetInstance<IServicesFabric>().CreateDisasterInfoProvider(disasterInfoProvider))
            {
                disasters = provider.RequestDisasters(_supportedDisasterTypes);
            }
            ShowDisasters(disasters);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Press any key to continue");

            Console.ReadKey();
            return disasters;
        }

        private static void ShowDisasters(IList<Disaster> disasters)
        {
            int count = 1;
            foreach(Disaster disaster in disasters)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{count++} ");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write($"<{disaster.DisasterType.ToString().ToUpper()}> ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"Id:{disaster.Id}, Name:{disaster.Name}, Created:{disaster.ReportedOn.ToString()}");
                Console.WriteLine();
            }
        }
    }
}
