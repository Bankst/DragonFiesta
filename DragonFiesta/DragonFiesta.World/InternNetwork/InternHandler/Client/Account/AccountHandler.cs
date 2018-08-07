using DragonFiesta.Messages.Accounts;
using DragonFiesta.Networking.Helpers;
using DragonFiesta.Utils.Logging;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Game.Chat;
using DragonFiesta.World.Network;
using DragonFiesta.World.ServerTask.Accounts;

namespace DragonFiesta.World.InternNetwork.InternHandler.Client.Account
{
    public class AccountHandler
    {
        [InternMessageHandler(typeof(DuplicateLoginFound))] //For update commands World World=> Login=> World
        public static void HandleDuplicateLoginFound(DuplicateLoginFound mMessage, InternLoginConnector pSession)
        {
	        if (!WorldSessionManager.Instance.GetAccount(mMessage.AccountID, out var gameSession)) return;
	        _SH03Helpers.SendDuplicateLogin(gameSession);
	        gameSession.Dispose();

	        GameLog.Write(GameLogLevel.Warning, "Duplicate Login Found!! Can't find Session By Account {0}", mMessage.AccountID);
        }


        [InternMessageHandler(typeof(AccountDelete))] //For update commands World World=> Login=> World
        public static void HandleAccountDelte(AccountDelete mMessage, InternLoginConnector pSession)
        {
            if (!WorldCharacterManager.Instance.GetCharacterByCharacterID(mMessage.AccountId, out var pChar, out var res, true))
            {
                return;
            }

            WorldCharacterManager.Instance.DeleteCharacter(pChar);
        }

        [InternMessageHandler(typeof(AccountUpdate))] //For Updae from Login=> World
        public static void HandleUpdateAccount(AccountUpdate mMessage, InternLoginConnector pSession)
        {
	        if (!WorldSessionManager.Instance.GetAccount(mMessage.Account.ID, out var session) ||
	            !session.UserAccount.Name.Equals(mMessage.Account.Name)) return;
	        if (!mMessage.Account.IsBanned)
	        {
		        //Update account
		        session.UserAccount = mMessage.Account;
	        }
	        else
	        {
		        if (session.Ingame) // oh Fuck Will be me Bann :D
		        {
			        ServerTaskManager.AddObject(new TASK_KICK_TIMER(() =>
			        {
				        session.Dispose();

			        }, (timeToKick) =>
			        {
				        ZoneChat.CharacterNote(session.Character, $"You Are Banned By Console. Disconnect in {timeToKick}");
			        }, (int)IngameServerTimes.TimeToBan));
		        }
	        }
        }
    }
}