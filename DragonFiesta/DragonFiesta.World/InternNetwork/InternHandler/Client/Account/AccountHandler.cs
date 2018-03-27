using DragonFiesta.Messages.Accounts;
using DragonFiesta.Networking.Helpers;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Game.Chat;
using DragonFiesta.World.Network;
using DragonFiesta.World.ServerTask.Accounts;

namespace DragonFiesta.World.InternNetwork.InternHandler.Client.Account
{
    public class AccountHandler
    {
        [InternMessageHandler(typeof(DublicateLoginFound))] //For update commands World World=> Login=> World
        public static void HandleDublicateLoginFound(DublicateLoginFound mMessage, InternLoginConnector pSession)
        {
            if (WorldSessionManager.Instance.GetAccount(mMessage.AccountID, out WorldSession GameSession))
            {
                _SH03Helpers.SendDublicateLogin(GameSession);
                GameSession.Dispose();

                GameLog.Write(GameLogLevel.Warning, "Dublicate Login Found!! Can't find Session By Account {0}", mMessage.AccountID);
            }
        }


        [InternMessageHandler(typeof(AccountDelete))] //For update commands World World=> Login=> World
        public static void HandleAccountDelte(AccountDelete mMessage, InternLoginConnector pSession)
        {
            if (!WorldCharacterManager.Instance.GetCharacterByCharacterID(mMessage.AccountId, out WorldCharacter pChar, out CharacterErrors Res, true))
            {
                return;
            }

            WorldCharacterManager.Instance.DeleteCharacter(pChar);
        }

        [InternMessageHandler(typeof(AccountUpdate))] //For Updae from Login=> World
        public static void HandleUpdateAccount(AccountUpdate mMessage, InternLoginConnector pSession)
        {
            if (WorldSessionManager.Instance.GetAccount(mMessage.Account.ID, out WorldSession Session)
                            && Session.UserAccount.Name.Equals(mMessage.Account.Name))
            {
                if (!mMessage.Account.IsBanned)
                {
                    //Update account
                    Session.UserAccount = mMessage.Account;
                }
                else
                {
                    if (Session.Ingame) // oh Fuck Will be me Bann :D
                    {
                        ServerTaskManager.AddObject(new TASK_KICK_TIMER(() =>
                         {
                             Session.Dispose();

                         }, (TimeToKick) =>
                         {
                             ZoneChat.CharacterNote(Session.Character, $"You Are Bannes By Console Disconnect in {TimeToKick}");
                         }, (int)IngameServerTimes.TimeToBan));
                    }
                }
            }
        }
    }
}