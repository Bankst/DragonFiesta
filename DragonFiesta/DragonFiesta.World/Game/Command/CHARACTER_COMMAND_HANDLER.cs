using DragonFiesta.Messages.Login.Accounts;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Game.Chat;
using DragonFiesta.World.InternNetwork.InternHandler.Server.Accounts;
using System;
using DragonFiesta.World.ServerTask.Accounts;
using static System.String;

namespace DragonFiesta.World.Game.Command
{
    [GameCommandCategory("Character")]
    public class CHARACTER_COMMAND_HANDLER
    {
       
        [WorldCommand("Ban")]
        public static bool Ban(WorldCharacter character, string[] Params)
        {
            if (Params.Length < 2)
            {
                ZoneChat.CharacterNote(character, "invalid Parameter use &Character ban <Charactername> 1 or <Bantime> optional <BanReason>");
                return true;
            }

            var characterName = Params[0];

            if (!int.TryParse(Params[1], out var banTime))
            {
                ZoneChat.CharacterNote(character, $"Invalid Parameter Use 1 or <Bantime>");
                return true;
            }
            var banReason = "No Reason";
            if (Params.Length > 2)
            {
                banReason = Join(" ", 3, Params.Length);
            }

            if (!WorldCharacterManager.Instance.GetCharacterByName(characterName, out var target, true))
            {
                ZoneChat.CharacterNote(character, $"Character {characterName} not found!");
                return true;
            }

            AccountMethods.SendAccountRequestById(target.LoginInfo.AccountID, (msg) =>
            {
	            if (!(msg is Account_Response accountMsg)) return;
	            if (accountMsg.Account != null)
	            {
		            accountMsg.Account.BanDate = DateTime.Now;
		            accountMsg.Account.BanReason = banReason;
		            accountMsg.Account.BanTime = banTime > 1 ? banTime : int.MaxValue;
		            AccountMethods.SendUpdateAccount(accountMsg.Account, (update) =>
		            {
			            ZoneChat.CharacterNote(character, $"You Are Banned {target.Info.Name} from the Server");

			            if (target.IsConnected)
			            {
				            ServerTaskManager.AddObject(new TASK_KICK_TIMER(() =>
				            {
					            target.Session.Dispose();

				            }, (timeToKick) =>
				            {
					            ZoneChat.CharacterNote(target, $"You Are Banned by {character.Info.Name} from Server. Disconnect in {timeToKick}");
				            }, (int)IngameServerTimes.TimeToBan));
			            }
		            });
	            }
	            else
	            {
		            ZoneChat.CharacterNote(character, $"Account with Id {target.LoginInfo.AccountID} not found in Database!");
	            }
            });

            return false;
        }


        [WorldCommand("Unban")]
        public static bool Unban(WorldCharacter character, string[] Params)
        {

            if (Params.Length < 1)
            {
                ZoneChat.CharacterNote(character, "Invalid Parameter use &Character unban <CharacterName>");
                return true;
            }
            var characterName = Params[0];

            if (!WorldCharacterManager.Instance.GetCharacterByName(characterName, out var target))
            {
                ZoneChat.CharacterNote(character, $"Character {characterName} not found!");
                return true;
            }

            AccountMethods.SendAccountRequestById(target.LoginInfo.AccountID, (msg) =>
            {
	            if (!(msg is Account_Response accountMsg)) return;
	            if (accountMsg.Account != null)
	            {
		            AccountMethods.SendUpdateAccount(accountMsg.Account, (update) =>
		            {
			            ZoneChat.CharacterNote(character, $"Unbanned Character {characterName} Successfully!");
		            });
	            }
	            else
	            {
		            ZoneChat.CharacterNote(character, $"Account with id {target.LoginInfo.AccountID} not found in Database");
	            }
            });

            return true;
        }

        [WorldCommand("get")]
        public static bool GetInfo(WorldCharacter character, string[] Params)
        {
            if (Params.Length < 2)
            {
                ZoneChat.CharacterNote(character, "Invalid Parameter");
                return true;
            }

            var characterName = Params[1];

            if (!WorldCharacterManager.Instance.GetCharacterByName(characterName, out var target, true))
            {
                ZoneChat.CharacterNote(character, $"Character {characterName} not found");
                return true;
            }

            AccountMethods.SendAccountRequestById(target.LoginInfo.AccountID, (msg) =>
            {
	            if (!(msg is Account_Response response)) return;
	            if (response.Account != null)
	            {
		            switch (Params[0].ToUpper())
		            {
			            case "ROLE" when (Params.Length == 2):
				            ZoneChat.CharacterNote(character, $"Character Role is {response.Account.RoleID}");
				            break;
			            case "ACCOUNTNAME" when (Params.Length == 2):
				            ZoneChat.CharacterNote(character, $"Character AccountName is {response.Account.Name}");
				            break;
			            case "LASTIP" when (Params.Length == 2):
				            ZoneChat.CharacterNote(character, $"Character Last IP is {response.Account.LastIP}");
				            break;
			            case "EMAIL" when (Params.Length == 2):
				            ZoneChat.CharacterNote(character, $"Character Email is {response.Account.EMail}");
				            break;
			            case "ACTIVE" when (Params.Length == 2):
				            ZoneChat.CharacterNote(character, $"Character Account Active is {response.Account.IsActivated}");
				            break;
			            default:
				            ZoneChat.CharacterNote(character, $"Invalid Options");
				            break;
		            }
	            }
	            else
	            {
		            ZoneChat.CharacterNote(character, $"Account not found");
	            }
            });


            return true;
        }
    }
}
