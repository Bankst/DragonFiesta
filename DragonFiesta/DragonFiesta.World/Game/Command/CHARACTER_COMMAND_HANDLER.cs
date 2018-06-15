using DragonFiesta.Messages.Login.Accounts;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Game.Chat;
using DragonFiesta.World.InternNetwork.InternHandler.Server.Accounts;
using DragonFiesta.World.ServerTask.Accounts;
using System;

namespace DragonFiesta.World.Game.Command
{
    [GameCommandCategory("Character")]
    public class CHARACTER_COMMAND_HANDLER
    {
       
        [WorldCommand("Ban")]
        public static bool Ban(WorldCharacter Character, string[] Params)
        {
            if (Params.Length < 2)
            {
                ZoneChat.CharacterNote(Character, "invalid Parameter use &Character ban <Charactername> 1 or <Bantime> optional <BanReason>");
                return true;
            }

            string CharacterName = Params[0];

            if (!int.TryParse(Params[1], out int BanTime))
            {
                ZoneChat.CharacterNote(Character, $"Invalid Parameter Use 1 or <Bantime>");
                return true;
            }
            string BanReason = "No Reason";
            if (Params.Length > 2)
            {
                BanReason = String.Join(" ", 3, Params.Length);
            }

            if (!WorldCharacterManager.Instance.GetCharacterByName(CharacterName, out WorldCharacter Target, true))
            {
                ZoneChat.CharacterNote(Character, $"Character {CharacterName} not found!");
                return true;
            }

            AccountMethods.SendAccountRequestById(Target.LoginInfo.AccountID, (MSG) =>
             {
                 if (MSG is Account_Response AccountMSG)
                 {

                     if (AccountMSG.Account != null)
                     {
                         AccountMSG.Account.BanDate = DateTime.Now;
                         AccountMSG.Account.BanReason = BanReason;
                         AccountMSG.Account.BanTime = BanTime > 1 ? BanTime : int.MaxValue;
                         AccountMethods.SendUpdateAccount(AccountMSG.Account, (Update) =>
                          {
                              ZoneChat.CharacterNote(Character, $"You Are Banned {Target.Info.Name} from the Server");

                              if (Target.IsConnected)
                              {
                                 ServerTaskManager.AddObject(new TASK_KICK_TIMER(() =>
                                  {
                                      Target.Session.Dispose();

                                  }, (TimeToKick) =>
                                  {
                                      ZoneChat.CharacterNote(Target, $"You Are Banned by {Character.Info.Name} from Server Disconnect in {TimeToKick}");
                                  }, (int)IngameServerTimes.TimeToBan));
                              }
                          });
                     }
                     else
                     {
                         ZoneChat.CharacterNote(Character, $"Account with Id {Target.LoginInfo.AccountID} not found in Database!");
                     }
                 }
             });

            return false;
        }


        [WorldCommand("Unban")]
        public static bool Unban(WorldCharacter Character, string[] Params)
        {

            if (Params.Length < 1)
            {
                ZoneChat.CharacterNote(Character, "Invalid Parameter use &Character unban <CharacterName>");
                return true;
            }
            string CharacterName = Params[0];

            if (!WorldCharacterManager.Instance.GetCharacterByName(CharacterName, out WorldCharacter Target))
            {
                ZoneChat.CharacterNote(Character, $"Character {CharacterName} not found!");
                return true;
            }

            AccountMethods.SendAccountRequestById(Target.LoginInfo.AccountID, (msg) =>
             {
                 if (msg is Account_Response AccountMsg)
                 {
                     if (AccountMsg.Account != null)
                     {
                         AccountMethods.SendUpdateAccount(AccountMsg.Account, (update) =>
                          {
                              ZoneChat.CharacterNote(Character, $"Unbanned Character {CharacterName} Success!");
                          });
                     }
                     else
                     {
                         ZoneChat.CharacterNote(Character, $"Account with id {Target.LoginInfo.AccountID} not found in Database");
                     }
                 }
             });

            return true;
        }

        [WorldCommand("get")]
        public static bool GetInfo(WorldCharacter Character, string[] Params)
        {
            if (Params.Length < 2)
            {
                ZoneChat.CharacterNote(Character, "Invalid Parameter");
                return true;
            }

            string CharacterName = Params[1];

            if (!WorldCharacterManager.Instance.GetCharacterByName(CharacterName, out WorldCharacter Target, true))
            {
                ZoneChat.CharacterNote(Character, $"Character {CharacterName} not found");
                return true;
            }

            AccountMethods.SendAccountRequestById(Target.LoginInfo.AccountID, (msg) =>
             {
                 if (msg is Account_Response Response)
                 {
                     if (Response.Account != null)
                     {
                         switch (Params[0].ToUpper())
                         {
                             case "ROLE" when (Params.Length == 2):
                                 ZoneChat.CharacterNote(Character, $"Character Role is {Response.Account.RoleID}");
                                 break;
                             case "ACCOUNTNAME" when (Params.Length == 2):
                                 ZoneChat.CharacterNote(Character, $"Character AccountName is {Response.Account.Name}");
                                 break;
                             case "LASTIP" when (Params.Length == 2):
                                 ZoneChat.CharacterNote(Character, $"Character Last IP is {Response.Account.LastIP}");
                                 break;
                             case "EMAIL" when (Params.Length == 2):
                                 ZoneChat.CharacterNote(Character, $"Character Email is {Response.Account.EMail}");
                                 break;
                             case "ACTIVE" when (Params.Length == 2):
                                 ZoneChat.CharacterNote(Character, $"Character Account Active is {Response.Account.IsActivated}");
                                 break;
                             default:
                                 ZoneChat.CharacterNote(Character, $"Invalid Options");
                                 break;
                         }
                     }
                     else
                     {
                         ZoneChat.CharacterNote(Character, $"Account not found");
                     }
                 }
             });


            return true;
        }
    }
}
