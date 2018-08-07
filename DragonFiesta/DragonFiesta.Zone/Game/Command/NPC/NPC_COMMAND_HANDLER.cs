using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Chat;
using DragonFiesta.Zone.Game.NPC;

namespace DragonFiesta.Zone.Game.Command.NPC
{
    [GameCommandCategory("NPC")]
    public class NPC_COMMAND_HANDLER
    {
        [ZoneCommand("get")]
        public static bool NPC_Get(ZoneCharacter Sender, string[] Params)
        {
            if (Params.Length < 1)
            {
                ZoneChat.CharacterNote(Sender, $"Invalid Parameters use &get <option>");
                return true;
            }

            if (Sender.Selection == null
                || !(Sender.Selection.SelectedObject is NPCBase NPC))
            {
                ZoneChat.CharacterNote(Sender, $"Invalid Target");
                return true;
            }

            switch (Params[0].ToUpper())
            {
                case "ID":
                    ZoneChat.CharacterNote(Sender, $"MobId is  {NPC.Info.MobInfo.ID}");
                    break;

                case "POSITION":
                    ZoneChat.CharacterNote(Sender, $"Position is  {NPC.Position.ToString()}");
                    break;
                case "ROLE":
                    ZoneChat.CharacterNote(Sender, $"Role is  {NPC.Info.Role}");
                    ZoneChat.CharacterNote(Sender, $"RoleArgument is  {NPC.Info.RoleArgument}");
                    break;
                case "MENU":
                    ZoneChat.CharacterNote(Sender, $"NPCMenu is  {NPC.Info.HasNPCMenu}");
                    break;
                case "WAYPOINT":
                    ZoneChat.CharacterNote(Sender, $"WaypointId is  {NPC.Info.WayPointInfo.Id}");
                    break;
                default:
                    ZoneChat.CharacterNote(Sender, $"Invalid Options");
                    break;
            }
            return true;
        }
    }
}
