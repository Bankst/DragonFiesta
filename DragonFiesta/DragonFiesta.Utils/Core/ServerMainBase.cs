using DragonFiesta.Utils.ServerConsole;
using System;
using System.Linq;
using System.Reflection;
using DragonFiesta.Utils.Logging;

namespace DragonFiesta.Utils.Core
{
    public class ServerMainBase
    {
        public static ServerMainBase InternalInstance { get; private set; }

        public static bool DebugMessages { get; set; }

        public ServerType ServerType { get; private set; }
        public string StartDirectory { get; private set; }
        public string StartExecutable { get; private set; }
        public virtual bool ServerIsReady { get; set; } = false;

        public GameTime CurrentTime { get; internal set; }
        public TimeSpan TotalUpTime { get; internal set; }

        public ServerMainBase(ServerType pType)
        {
            if (InternalInstance != null)
            {
                throw new InvalidOperationException("Can only load one instance of this class at once.");
            }

            InternalInstance = this;
            LoadExsternAssemblys();//Load in Assmebly cache and fix gloabals load bug-...
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            StartDirectory = AppDomain.CurrentDomain.BaseDirectory.ToEscapedString();
            StartExecutable = (Assembly.GetEntryAssembly().CodeBase.Replace("file:///", "").Replace("/", "\\"));
            CurrentTime = (GameTime)DateTime.Now;
            ServerType = pType;
        }

        private void LoadExsternAssemblys()
        {
            Assembly.Load(@"DragonFiesta.Networking");
            Assembly.Load(@"DragonFiesta.Providers");
            Assembly.Load(@"DragonFiesta.Utils");
            Assembly.Load(@"DragonFiesta.Game");
        }

        public void WriteConsoleLogo()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*************************************************************************");
            Console.WriteLine("*                                                                       *");
            Console.WriteLine("*   _____                               _______ _                       *");
            Console.WriteLine("*  (____ \\                             (_______|_)           _          *");
            Console.WriteLine("*   _   \\ \\ ____ ____  ____  ___  ____  _____   _  ____  ___| |_  ____  *");
            Console.WriteLine("*  | |   | / ___) _  |/ _  |/ _ \\|  _ \\|  ___) | |/ _  )/___)  _)/ _  | *");
            Console.WriteLine("*  | |__/ / |  ( ( | ( ( | | |_| | | | | |     | ( (/ /|___ | |_( ( | | *");
            Console.WriteLine("*  |_____/|_|   \\_||_|\\_|| |\\___/|_| |_|_|     |_|\\____|___/ \\___)_||_| *");
            Console.WriteLine("*                    (_____|                                            *");
            Console.WriteLine("*               Copyright 2018 by Mathias1000, Sequess, SeerOfVoid420   *");
            Console.WriteLine("*************************************************************************");
            Console.ResetColor();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            EngineLog.Write(EngineLogLevel.Exception, e.ExceptionObject.ToString());
        }

        public virtual bool LoadBaseServerModule()
        {
            if (LoadServerModules())
            {
                ConsoleThread.Start();
                return true;
            }
            return false;
        }

        public bool LoadGameServerModules() //Load All Game Class After Authenticate
        {
            try
            {
                if (Reflector.GetInitializerGameMethods(ServerType).Any(method =>
                {
	                if (method.Invoke()) return false;
	                GameLog.Write(GameLogLevel.Exception, "Invalid Initial GameServerModule {0}", method.Method.ReflectedType?.FullName);
	                return true;
                }))
                {
                    GameLog.Write(GameLogLevel.Exception, "GameServer could not be started. Errors occured.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                GameLog.Write(GameLogLevel.Exception, "GameServer could not be started. Errors occured {0}.", ex.ToString());
                return false;
            }

            return true;
        }

        public bool LoadServerModules()
        {
            try
            {
                if (Reflector.GetInitializerServerMethods(ServerType).Any(method =>
                {
                    if (!method.Invoke())
                    {
                        GameLog.Write(GameLogLevel.Exception, "Invalid Initial ServerModule {0}", method.Method.ReflectedType.FullName);
                        return true;
                    }
                    return false;
                }))
                {
                    EngineLog.Write(EngineLogLevel.Exception, "Server could not be started. Errors occured.");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                EngineLog.Write(EngineLogLevel.Exception, "Server could not be started. Errors occured {0}.", ex.ToString());
                return false;
            }
        }

        public virtual void Shutdown()
        {
            ConsoleThread.Stop();

            foreach (var m in Reflector.GetServerShutdownMethods())
            {
                m.Invoke();
            }
            ThreadPool.Stop();
        }
    }
}