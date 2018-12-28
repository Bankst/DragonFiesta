﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DFEngine.Threading
{
	public enum MessageRequestTimeOuts : int
	{
		LOGIN_TRANSFER_ACCOUNT = 1000, //LOGIN-WORLD
		ZONE_TRANSFER_MAP = 10000,//ZONE -> WORLD
		ZONE_TRANSFER_CHARACTER = 10000,//WORLD -> ZONE

		WORLD_PING_LOGIN = 1000,
		WORLD_CHARACTER_POSITION = 10000,//WORLD->Zone
		ZONE_WORLD_COMMAND_REQUEST = 5000,//ZONE -> WORLD
		ZONE_FIND_CHARACTER = 10000,

		ZONE_SET_BUFF = 3000,

	}

	//Times in Miliseconds
	public enum ServerTaskTimes : int
	{
		INGAME_TIMER_INTERVALL = 1000,

		SERVER_INTERN_MSG_RESPONSE_CHECK = 100,

		//Gabage Collector
		SERVER_GC_INTERVAL = 30000,

		//Shutdown Timer
		SERVER_SHUTDOWN_INTERVAL = 1000,

		ACCOUNT_LOGIN_TIMEOUT_CHECK = 300,

		//Auth
		SERVER_AUTH_WORLD = 12000,
		SERVER_AUTH_ZONE = 10000,

		//Update Player..

		SERVER_WORLD_UPDATE = 10000,
		SERVER_ZONE_UPDATE = 10000,

		SERVER_WORLD_RECONNECT = 4000,
		SERVER_ZONE_RECONNECT = 4000,

		SESSION_GAME_PING_SYNC = 2500,
		SESSION_GAME_PING_TIMEOUT = 5000,
		SESSION_CLOCK_GAMETIME_SYNC = 60000,

		//Transfer
		SERVER_LOGIN_TRANSFER_CHECK = 2000,
		SERVER_WORLD_TRANSFER_CHECK = 2000,
		SERVER_ZONE_TRANSFER_CHECK = 2000,

		MAP_GROUP_UPDATE_INTERVAL = 100,
		MAP_NPC_UPDATE_INTERVAL = 600,
	}

	public static class ServerTaskTimesExtensions
	{
		public static int Milliseconds(this ServerTaskTimes time)
		{
			return (int) time;
		}
	}

	//Times in Seconds
	public enum IngameServerTimes : int
	{
		TimeToKick = 30,
		TimeToBan = 5
	}
}
