﻿using DragonFiesta.Utils.Core;
using DragonFiesta.Zone.Config;
using DragonFiesta.Zone.Network;
using DragonFiesta.Zone.ServerConsole.Title;
using System;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.Zone.Core
{
    public class ServerMain : ServerMainBase
    {
        public static new ServerMain InternalInstance { get; private set; }

        public ZoneConsoleTitle Title { get; set; }

        public bool IsDataLoaded = false;

        public ServerMain() : base(ServerType.Zone)
        {
            Title = new ZoneConsoleTitle();
            Title.Update();
        }

        public override bool LoadBaseServerModule()
        {

            return base.LoadBaseServerModule();
        }

        public override void Shutdown()
        {
            base.Shutdown();

            InternWorldConnector.Instance.Dispose();
            ZoneServer.Shutdown();
           
            DB.DisposeManager(DatabaseType.World);
            DB.DisposeManager(DatabaseType.Data);

            Environment.Exit(0);
          
        }
        public static bool LoadGameServer()
        {
            if (InternalInstance.LoadGameServerModules())
            {
                return true;
            }
            return true;
        }


        public static bool Initialize(byte ZoneId = 0)
        {
            InternalInstance = new ServerMain();
            InternalInstance.WriteConsoleLogo();
			System.Threading.Thread.Sleep(10000);
			if (!ZoneConfiguration.Initialize(ZoneId))
            {
                throw new StartupException("Invalid Load ZoneConfiguration");
            }


            ThreadPool.Start(ZoneConfiguration.Instance.WorkThreadCount);

            ThreadPool.AddUpdateAbleServer(ServerTaskManager.InitialInstance());

            if (!DB.AddManager(DatabaseType.World, ZoneConfiguration.Instance.WorldDatabaseSettings))
            {
                throw new StartupException("Invalid Add World DatabaseManager");
            }
            if (!DB.AddManager(DatabaseType.Data, ZoneConfiguration.Instance.DataDatabaseSettings))
            {
                throw new StartupException("Invalid Add Data DatabaseManager");
            }

            if (!InternalInstance.LoadBaseServerModule())
            {
                throw new StartupException("Invalid Load Server");
            }

            if (!DB.AddDBMonitor(DatabaseType.Data) || !DB.AddDBMonitor(DatabaseType.World))
            {
                throw new StartupException("Invalid Add Database Monitor");
            }

            return true;
        }
    }
}