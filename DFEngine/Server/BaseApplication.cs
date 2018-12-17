using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using DFEngine.Config;
using DFEngine.Content.Game;
using DFEngine.Content.Game.Engines;
using DFEngine.Logging;
using DFEngine.Utils;
using DFEngine.Utils.ServerConsole;

namespace DFEngine.Server
{
	public abstract class BaseApplication
	{
		public static BaseApplication InternalInstance { get; set; }

		public ServerType ServerType { get; private set; }
		public string StartDirectory { get; private set; }
		public string StartExecutable { get; private set; }

		public GameTime CurrentTime { get; internal set; }
		public TimeSpan TotalUpTime { get; internal set; }

		public static NetworkConfiguration NetConfig { get; set; }
		public static DatabaseConfiguration DbConfig { get; set; }

		private static IEnumerable<IEngine> _engines;

		public void Initialize(ServerType pType)
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			StartDirectory = AppDomain.CurrentDomain.BaseDirectory.ToEscapedString();
			StartExecutable = (Assembly.GetEntryAssembly().CodeBase.Replace("file:///", "").Replace("/", "\\"));
			CurrentTime = (GameTime)DateTime.Now;
			ServerType = pType;
			
			Logo.PrintLogo(ConsoleColor.DarkYellow);

			var stopwatch = new Stopwatch();
			stopwatch.Start();

			// Configuration
			if (!NetworkConfiguration.Load(out var netConfigMsg))
			{
				throw new StartupException(netConfigMsg);
			}
			NetConfig = NetworkConfiguration.Instance;

			if (!DatabaseConfiguration.Load(out var dbConfigMsg))
			{
				throw new StartupException(dbConfigMsg);
			}
			DbConfig = DatabaseConfiguration.Instance;

			Start();

			new Thread(() =>
			{
				while (true)
				{
					Update(Time.Milliseconds);
					Thread.Sleep(10);
				}
			}).Start();

			_engines = from type in Assembly.GetEntryAssembly().GetTypes()
				where type.GetInterfaces().Contains(typeof(IEngine)) && type.GetConstructor(Type.EmptyTypes) != null
				select Activator.CreateInstance(type) as IEngine;

			foreach (var engine in _engines)
			{
				new Thread(() =>
				{
					engine.Main(Time.Milliseconds);
					Thread.Sleep(10);
				}).Start();
			}

			EngineLog.Write(EngineLogLevel.Startup, $"Started {_engines.Count()} engines.");

			stopwatch.Stop();
			EngineLog.Write(EngineLogLevel.Startup, $"Time taken to start: {stopwatch.ElapsedMilliseconds}ms");
		}

		/// <summary>
		/// Called by the server itself. Performs server-specific actions.
		/// </summary>
		protected virtual void Start()
		{
		}


		/// <summary>
		/// Called by the server itself. Performs server-specific update loop.
		/// </summary>
		protected virtual void Update(long now)
		{
		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			EngineLog.Write(EngineLogLevel.Exception, e.ExceptionObject.ToString());
		}
	}
}
