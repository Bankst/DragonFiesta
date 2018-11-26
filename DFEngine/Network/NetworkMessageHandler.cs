using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DFEngine.Logging;

namespace DFEngine.Network
{
	/// <summary>
	/// The callback type for a network message handler.
	/// </summary>
	/// <param name="message">The message being handled.</param>
	/// <param name="connection">The connection that sent the message.</param>
	public delegate void NetworkMessageHandlerMethod(NetworkMessage message, NetworkConnection connection);

	/// <summary>
	/// Class to register handlers for network messages.
	/// </summary>
	public class NetworkMessageHandler
	{
		/// <summary>
		/// The maximum time a task can run for before it is considered dead
		/// (can be used for debugging any locking issues with certain areas of code).
		/// </summary>
		public const int MaxTaskRuntime = 300; // In seconds, so 5 minutes.

		/// <summary>
		/// Returns the number of handlers.
		/// </summary>
		public static int Count => Handlers.Count;

		/// <summary>
		/// All registered message handlers.
		/// </summary>
		private static readonly Dictionary<NetworkCommand, NetworkMessageHandlerMethod> Handlers = new Dictionary<NetworkCommand, NetworkMessageHandlerMethod>();

		/// <summary>
		/// Used for running asynchronous tasks, in this case executing packets.
		/// </summary>
		private static readonly TaskFactory HandlerFactory = new TaskFactory(TaskCreationOptions.PreferFairness, TaskContinuationOptions.None);

		/// <summary>
		/// Currently running tasks to keep track of what the current load is.
		/// </summary>
		private static readonly ConcurrentDictionary<int, Task> RunningHandlers = new ConcurrentDictionary<int, Task>();

		/// <summary>
		/// Invokes the message handler.
		/// </summary>
		public static void Invoke(NetworkMessageHandlerMethod method, NetworkMessage message, NetworkConnection connection)
		{
			if (method == null || !connection.IsConnected)
			{
				return;
			}

			SocketLog.Write(SocketLogLevel.Debug, $"Calling handler for {message.Command}");

			var stopwatch = new Stopwatch();
			stopwatch.Start();

			var cancelSource = new CancellationTokenSource();
			var token = cancelSource.Token;

			var task = HandlerFactory.StartNew(() =>
			{
				method.Invoke(message, connection);
				token.ThrowIfCancellationRequested();
			}, token);

			RunningHandlers.TryAdd(task.Id, task);

			try
			{
				if (!task.Wait(MaxTaskRuntime * 1000, token))
				{
					cancelSource.Cancel();
				}
			}
			catch (AggregateException ex)
			{
				foreach (var e in ex.Flatten().InnerExceptions)
				{
					SocketLog.Write(SocketLogLevel.Exception, $"Unhandled exception: {e.Message} - {e.StackTrace}");
				}

				connection.Disconnect();
			}
			catch (OperationCanceledException)
			{
				connection.Disconnect();
			}
			finally
			{
				stopwatch.Stop();
				RunningHandlers.TryRemove(task.Id, out var unused);
				cancelSource.Dispose();
				Object.Destroy(message);

				SocketLog.Write(SocketLogLevel.Debug, $"Handler took {stopwatch.ElapsedMilliseconds}ms to complete.");
			}
		}

		/// <summary>
		/// Registers a message handler to the collection of handlers.
		/// </summary>
		/// <param name="command">The command being handled.</param>
		/// <param name="method">The method to invoke.</param>
		public static void Store(NetworkCommand command, NetworkMessageHandlerMethod method)
		{
			if (!Handlers.ContainsKey(command))
			{
				Handlers.Add(command, method);
			}
		}

		/// <summary>
		/// Tries to get a message handler from the collection.
		/// </summary>
		/// <param name="command">The message's command.</param>
		/// <param name="method">The handler method.</param>
		/// <returns></returns>
		public static bool TryFetch(NetworkCommand command, out NetworkMessageHandlerMethod method)
		{
			return Handlers.TryGetValue(command, out method);
		}
	}
}
