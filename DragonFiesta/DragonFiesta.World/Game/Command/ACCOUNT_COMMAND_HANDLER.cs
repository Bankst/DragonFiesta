using DragonFiesta.Messages.Accounts;
using DragonFiesta.Messages.Login.Accounts;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Game.Chat;
using DragonFiesta.World.InternNetwork.InternHandler.Server.Accounts;
using DragonFiesta.World.Network;
using DragonFiesta.World.ServerTask.Accounts;
using System;
using System.Text.RegularExpressions;

namespace DragonFiesta.World.Game.Command
{
    [GameCommandCategory("Account")]
    public class ACCOUNT_COMMAND_HANDLER
    {
        [WorldCommand("ban")]
        public static bool Account_Ban(WorldCharacter Character, string[] Params)
        {
            if (Params.Length < 2)
            {
                ZoneChat.CharacterNote(Character, "Need Account Name use &Account <Accountname> 1 or <Bantime> optional <BanReason>");
                return true;
            }

            string accountName = Params[0];

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

            AccountMethods.SendAccountRequestByName(accountName, (msg) =>
            {
                if (msg is Account_Response Response) //GetAccount
                {
                    if (Response.Account != null)
                    {
                        if (Response.Account.IsBanned)
                        {
                            ZoneChat.CharacterNote(Character, "Account Already Banned");
                            return;
                        }
                        Response.Account.BanDate = DateTime.Now;

                        Response.Account.BanTime = BanTime > 1 ? BanTime : int.MaxValue;

                        Response.Account.BanReason = BanReason;

                        AccountMethods.SendUpdateAccount(Response.Account, (Update) =>
                         {
                             if (Update is AccountUpdate)//update result
                             {

                                 if (WorldSessionManager.Instance.GetAccount(Response.Account.ID, out WorldSession Session)
                                     && Session.Ingame)
                                 {
                                     ServerTaskManager.AddObject(new TASK_KICK_TIMER(() =>
                                        {
                                            Session.Dispose();

                                        }, (TimeToKick) =>
                                        {
                                            ZoneChat.CharacterNote(Session.Character, $"You Are Banned by {Character.Info.Name} from Server Disconnect in {TimeToKick}");
                                        }, (int)IngameServerTimes.TimeToBan));
                                 }
                                 ZoneChat.CharacterNote(Character, "Account Banned Success!");
                             }
                         });
                    }
                    else
                    {
                        ZoneChat.CharacterNote(Character, "Account not Found!");
                    }
                }

            });

            return true;
        }

        [WorldCommand("Unban")]
        public static bool Account_Unban(WorldCharacter Character, string[] Params)
        {
            if (Params.Length < 1)
            {
                ZoneChat.CharacterNote(Character, "Need Account Name use &Account <Accountname>");
                return true;
            }
            string accountName = Params[0];

            AccountMethods.SendAccountRequestByName(accountName, (msg) =>
            {
                if (msg is Account_Response Response) //GetAccount
                {
                    if (Response.Account != null)
                    {
                        if (!Response.Account.IsBanned)
                        {
                            ZoneChat.CharacterNote(Character, "Account is not banned!");
                            return;
                        }

                        Response.Account.BanTime = 0;
                        Response.Account.BanReason = "";
                  
                        AccountMethods.SendUpdateAccount(Response.Account, (Update) =>
                         {
                             ZoneChat.CharacterNote(Character, $"Account {accountName} is Unbanned!");
                         });
                    }
                    else
                    {
                        ZoneChat.CharacterNote(Character, $"Account {accountName} not Found!");

                    }
                }
            });

            return true;
        }

        [WorldCommand("Set")]
        public static bool Account_Set(WorldCharacter Character, string[] Params)
        {
            if (Params.Length < 2)
            {
                ZoneChat.CharacterNote(Character, "Need Account Name use &Account Set <accountname> <options>");
                return true;
            }

            string accountName = Params[0];

            AccountMethods.SendAccountRequestByName(accountName, (msg) =>
    {
        if (msg is Account_Response Response) //GetAccount
        {
            if (Response.Account != null)
            {
                switch (Params[1].ToUpper())
                {
                    case "ROLE" when (Params.Length == 3 && byte.TryParse(Params[2], out byte RoleID)):
                        Response.Account.RoleID = RoleID;

                        AccountMethods.SendUpdateAccount(Response.Account, (Update) =>
                        {
                            ZoneChat.CharacterNote(Character, $"Set Role {RoleID} from Account {accountName} Success");
                        });

                        break;
                    case "EMAIL" when (Params.Length == 3):
                        string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";

                        if (!Regex.IsMatch(Params[2], pattern))
                        {
                            ZoneChat.CharacterNote(Character, $"Invalid Email Address {Params[2]}");
                            return;
                        }

                        Response.Account.EMail = Params[2];

                        AccountMethods.SendUpdateAccount(Response.Account, (Update) =>
                        {
                            ZoneChat.CharacterNote(Character, $"update email to {Params[2]} Success");
                        });
                        break;
                    case "ACTIVE" when (Params.Length == 3 && StringExtensions.ParseBool(Params[2], out bool State)):

                        Response.Account.IsActivated = State;

                        AccountMethods.SendUpdateAccount(Response.Account, (Update) =>
                        {
                            ZoneChat.CharacterNote(Character, $"Set Account Active to {State} Success!");
                        });
                        break;
                    case "PASSWORD" when (Params.Length == 3):

                        if (Params[2].Length > 16)
                        {
                            ZoneChat.CharacterNote(Character, "Invalid Password Lenght the max Lenght is 16");
                            return;
                        }
                        Response.Account.Password = Params[2];
                        AccountMethods.SendUpdateAccount(Response.Account, (Update) =>
                        {
                            ZoneChat.CharacterNote(Character, $"Set Account Password to {Params[2]} Success!");
                        });
                        break;
                    default:
                        ZoneChat.CharacterNote(Character, "Update Options for Account not found!");
                        break;

                }
            }
            else
            {
                ZoneChat.CharacterNote(Character, "Account not found!");
            }
        }
    });
            return true;
        }
    }
}
