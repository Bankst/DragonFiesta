using DragonFiesta.Messages.Accounts;
using DragonFiesta.Messages.Login.Accounts;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Game.Chat;
using DragonFiesta.World.InternNetwork.InternHandler.Server.Accounts;
using DragonFiesta.World.Network;
using System;
using System.Text.RegularExpressions;
using DragonFiesta.World.ServerTask.Accounts;

namespace DragonFiesta.World.Game.Command
{
    [GameCommandCategory("Account")]
    public class ACCOUNT_COMMAND_HANDLER
    {
        [WorldCommand("ban")]
        public static bool Account_Ban(WorldCharacter character, string[] Params)
        {
            if (Params.Length < 2)
            {
                ZoneChat.CharacterNote(character, "Need Account Name use &Account <Accountname> 1 or <Bantime> optional <BanReason>");
                return true;
            }

            var accountName = Params[0];

            if (!int.TryParse(Params[1], out var banTime))
            {
                ZoneChat.CharacterNote(character, $"Invalid Parameter Use 1 or <Bantime>");
                return true;
            }
            var banReason = "No Reason";
            if (Params.Length > 2)
            {
                banReason = string.Join(" ", 3, Params.Length);
            }

            AccountMethods.SendAccountRequestByName(accountName, (msg) =>
            {
                if (msg is Account_Response response) //GetAccount
                {
                    if (response.Account != null)
                    {
                        if (response.Account.IsBanned)
                        {
                            ZoneChat.CharacterNote(character, "Account Already Banned");
                            return;
                        }
                        response.Account.BanDate = DateTime.Now;

                        response.Account.BanTime = banTime > 1 ? banTime : int.MaxValue;

                        response.Account.BanReason = banReason;

                        AccountMethods.SendUpdateAccount(response.Account, (update) =>
                         {
	                         if (!(update is AccountUpdate)) return;
	                         if (WorldSessionManager.Instance.GetAccount(response.Account.ID, out var session)
	                             && session.Ingame)
	                         {
		                         ServerTaskManager.AddObject(new TASK_KICK_TIMER(() =>
		                         {
			                         session.Dispose();

		                         }, (timeToKick) =>
		                         {
			                         ZoneChat.CharacterNote(session.Character, $"You Are Banned by {character.Info.Name} from Server. Disconnect in {timeToKick}");
		                         }, (int)IngameServerTimes.TimeToBan));
	                         }
	                         ZoneChat.CharacterNote(character, "Account Banned Successfully!");
                         });
                    }
                    else
                    {
                        ZoneChat.CharacterNote(character, "Account not Found!");
                    }
                }

            });

            return true;
        }

        [WorldCommand("Unban")]
        public static bool Account_Unban(WorldCharacter character, string[] Params)
        {
            if (Params.Length < 1)
            {
                ZoneChat.CharacterNote(character, "Need Account Name use &Account <Accountname>");
                return true;
            }
            var accountName = Params[0];

            AccountMethods.SendAccountRequestByName(accountName, (msg) =>
            {
	            if (!(msg is Account_Response response)) return;
	            if (response.Account != null)
	            {
		            if (!response.Account.IsBanned)
		            {
			            ZoneChat.CharacterNote(character, "Account is not banned!");
			            return;
		            }

		            response.Account.BanTime = 0;
		            response.Account.BanReason = "";
                  
		            AccountMethods.SendUpdateAccount(response.Account, (update) =>
		            {
			            ZoneChat.CharacterNote(character, $"Account {accountName} is Unbanned!");
		            });
	            }
	            else
	            {
		            ZoneChat.CharacterNote(character, $"Account {accountName} not Found!");

	            }
            });

            return true;
        }

        [WorldCommand("Set")]
        public static bool Account_Set(WorldCharacter character, string[] Params)
        {
            if (Params.Length < 2)
            {
                ZoneChat.CharacterNote(character, "Need Account Name use &Account Set <accountname> <options>");
                return true;
            }

            var accountName = Params[0];

            AccountMethods.SendAccountRequestByName(accountName, (msg) =>
            {
	            if (!(msg is Account_Response response)) return;
	            if (response.Account != null)
	            {
		            switch (Params[1].ToUpper())
		            {
			            case "ROLE" when (Params.Length == 3 && byte.TryParse(Params[2], out var roleID)):
				            response.Account.RoleID = roleID;

				            AccountMethods.SendUpdateAccount(response.Account, (update) =>
				            {
					            ZoneChat.CharacterNote(character, $"Set Role {roleID} from Account {accountName} Success");
				            });

				            break;
			            case "EMAIL" when (Params.Length == 3):
				            var pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";

				            if (!Regex.IsMatch(Params[2], pattern))
				            {
					            ZoneChat.CharacterNote(character, $"Invalid Email Address {Params[2]}");
					            return;
				            }

				            response.Account.EMail = Params[2];

				            AccountMethods.SendUpdateAccount(response.Account, (update) =>
				            {
					            ZoneChat.CharacterNote(character, $"update email to {Params[2]} Success");
				            });
				            break;
			            case "ACTIVE" when (Params.Length == 3 && StringExtensions.ParseBool(Params[2], out bool state)):

				            response.Account.IsActivated = state;

				            AccountMethods.SendUpdateAccount(response.Account, (update) =>
				            {
					            ZoneChat.CharacterNote(character, $"Set Account Active to {state} Success!");
				            });
				            break;
			            case "PASSWORD" when (Params.Length == 3):

				            if (Params[2].Length > 16) // TODO: Make this configurable
				            {
					            ZoneChat.CharacterNote(character, "Invalid Password Lenght the max Lenght is 16");
					            return;
				            }
				            response.Account.Password = Params[2];
				            AccountMethods.SendUpdateAccount(response.Account, (update) =>
				            {
					            ZoneChat.CharacterNote(character, $"Set Account Password to {Params[2]} Success!");
				            });
				            break;
			            default:
				            ZoneChat.CharacterNote(character, "Update Options for Account not found!");
				            break;

		            }
	            }
	            else
	            {
		            ZoneChat.CharacterNote(character, "Account not found!");
	            }
            });
            return true;
        }
    }
}
