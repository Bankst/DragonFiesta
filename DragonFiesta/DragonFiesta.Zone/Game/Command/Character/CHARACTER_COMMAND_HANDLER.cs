using DragonFiesta.Messages.Zone.Character;
using DragonFiesta.Providers.Characters;
using DragonFiesta.Providers.Maps;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Chat;
using DragonFiesta.Zone.Game.Maps.Object;
using DragonFiesta.Zone.Game.Maps.Types;
using DragonFiesta.Zone.InternNetwork.InternHandler.Server.Character;
using System;

namespace DragonFiesta.Zone.Game.Command
{
    [GameCommandCategory("Character")]
    public class CHARACTER_COMMAND_HANDLER
    {
        [ZoneCommand("ChangeClass")]
        public static bool CharacterChangeClass(ZoneCharacter Character, string[] Params)
        {
            if (Params.Length < 2)
            {
                ZoneChat.CharacterNote(Character, "Invalid Parameter use &character changeclass <name> <classId");
                return true;
            }

            if (!ZoneCharacterManager.Instance.GetCharacterByName(Params[0], out ZoneCharacter TargetCharacter, true))
            {
                ZoneChat.CharacterNote(Character, $"Character { Params[0] } not found!");
                return true;
            }

            if (!Enum.TryParse(Params[1], out ClassId NewClass))
            {
                ZoneChat.CharacterNote(Character, "Invalid Class selected");
                return true;
            }
            if (!TargetCharacter.LoginInfo.IsOnline)
            {
                ZoneChat.CharacterNote(Character, "Target Character must be online to change class!");
                return true;
            }

            Character.ChangeClass(NewClass);

            return true;
        }

        [ZoneCommand("Appear")]
        public static bool CharacterAppear(ZoneCharacter Character, string[] Params)
        {

            if (Params.Length < 1)
            {
                ZoneChat.CharacterNote(Character, "Invalid Parameter use &character Appear <name>");
                return true;
            }


            if (!ZoneCharacterManager.Instance.GetCharacterByName(Params[0], out ZoneCharacter Target, true))
            {
                ZoneChat.CharacterNote(Character, "Character not found");
                return true;
            }

            if(Target.LoginInfo.IsOnline)
            {
                if (Target.IsOnThisZone)
                {
                    Character.ChangeMap(
                        Target.AreaInfo.MapInfo.ID,
                        Target.AreaInfo.InstanceId,
                        Target.Position.X,
                        Target.Position.Y);

                    return true;
                }
                else
                {
                    CharacterMethods.SendCharacterPositionRequest(Target, (msg) =>
                     {
                         if (msg is CharacterPosition Pos)
                         {
                             Character.ChangeMap(
                                 Pos.MapId,
                                 Pos.InstanceId,
                                 Pos.Position.X,
                                 Pos.Position.Y);
                         }
                     });
                    return true;
                }
            }
            else
            {
                ZoneChat.CharacterNote(Character, $"Appaer You to Offline Character {Target.Info.Name}");
                Character.ChangeMap(Target.AreaInfo.MapInfo.ID, 0, Target.Position.X, Target.Position.Y);
                return true;
            }
        }
        [ZoneCommand("Summon")]
        public static bool CharacterSummon(ZoneCharacter Character, string[] Params)
        {

            if (Params.Length < 1)
            {
                ZoneChat.CharacterNote(Character, "Invalid Parameter use &character Summon <name>");
                return true;
            }


            if (!ZoneCharacterManager.Instance.GetCharacterByName(Params[0],out ZoneCharacter Target,true))
            {
                ZoneChat.CharacterNote(Character, "Character not found");
                return true;
            }

            if (Target.LoginInfo.IsOnline)
            {
                Target.ChangeMap(
                    Character.AreaInfo.Map.MapId
                    , Character.AreaInfo.InstanceId,
                    Character.Position.X,
                    Character.Position.Y);

            }
            else
            {
                Target.Position = Character.Position;
                Target.AreaInfo.Map = Character.Map;
                Target.Save();

                ZoneChat.CharacterNote(Character, $"Set Offline Position from Character {Target.Info.Name} to You Now Position");
                return true;
            }

            return false;
        }

        [ZoneCommand("Pos")]
        public static bool PositionCharacter(ZoneCharacter Character, string[] Params)
        {
            if (Params.Length < 1)
            {
                ZoneChat.CharacterNote(Character, $"You Position is {Character.Position.ToString()} MapId {Character.Map.MapId} Instance {Character.AreaInfo.InstanceId}");
                return true;
            }


            if (!ZoneCharacterManager.Instance.GetCharacterByName(Params[0], out ZoneCharacter Target, true))
            {
                ZoneChat.CharacterNote(Character, "Character not found");
                return true;
            }

            if (Target.LoginInfo.IsOnline)
            {
                if (Target.IsOnThisZone)
                {
                    ZoneChat.CharacterNote(Character, $"Position is {Character.Position.ToString()} MapId {Character.Map.MapId} Instance {Character.AreaInfo.InstanceId}");
                    return false;
                }
                else
                {
                    CharacterMethods.SendCharacterPositionRequest(Target, (msg) =>
                      {
                          if (msg is CharacterPosition position)
                          {
                              ZoneChat.CharacterNote(Character, $"Character {Target.Info.Name } Position is {position.Position.ToString()} MapId {position.MapId} Instance {position.InstanceId}");
                          }
                      });
                }
            }
            else
            {
                ZoneChat.CharacterNote(Character, $"Character is Offline on Position {Target.AreaInfo.Position.ToString()} MapId {Target.AreaInfo.MapInfo.ID}");
                return true;
            }


            return false;
        }

        [ZoneCommand("GiveExp")]
        public static bool GiveEXP(ZoneCharacter Character, string[] Params)
        {
            if (Params.Length < 2)
            {
                ZoneChat.CharacterNote(Character, "Invalid Use Command!! use &Character giveExp <CharacterName> <amount>");
                return true;
            }

            if (!ZoneCharacterManager.Instance.GetCharacterByName(Params[0], out ZoneCharacter Char))
            {
                ZoneChat.CharacterNote(Character, "Character not found");
                return true;
            }

            if (!uint.TryParse(Params[1], out uint Amount))
            {
                ZoneChat.CharacterNote(Character, $"Cant not Parse Amount");
                return true;
            }

            if(!Char.IsConnected)
            {
                ZoneChat.CharacterNote(Character, $"Char not online");
                return false;
            }
            if (Char.GiveEXP(Amount))
            {
                ZoneChat.CharacterNote(Character, $"Gived Exp {Amount}");
                return true;
            }
            else
            {
                ZoneChat.CharacterNote(Character, $"Has been Alredy Maxium");
                return true;
            }

        }
        [ZoneCommand("ChangeMap")]
        public static bool ChangeMap_Handler(ZoneCharacter Character, string[] Params)
        {
            if (Params.Length < 2)
            {
                ZoneChat.CharacterNote(Character, "Invalid Use Command!! use &Character ChangeMap <CharacterName> <Mapid> <Instance>");
                return true;
            }


            if (!ZoneCharacterManager.Instance.GetCharacterByName(Params[0], out ZoneCharacter Char))
            {
                ZoneChat.CharacterNote(Character, "Character not found");
                return true;
            }

            if (!ushort.TryParse(Params[1], out ushort MapId))
            {
                ZoneChat.CharacterNote(Character, "Invalid Use need MapId");
                return true;
            }

            if (!MapDataProvider.GetFieldInfosByMapID(MapId, out FieldInfo Info))
            {
                ZoneChat.CharacterNote(Character, "MapId Not foud!");
                return true;
            }

            if (Params.Length == 3 && ushort.TryParse(Params[2], out ushort InstanceId))
            {
                if (!MapManager.GetMap(Info, InstanceId, out ZoneServerMap Map))
                {
                    ZoneChat.CharacterNote(Character, "InstanceMap Offline");
                    return true;
                }


                Character.ChangeMap(Info.MapInfo.ID, InstanceId, Info.MapInfo.Regen.X, Info.MapInfo.Regen.Y);

                return true;
            }
            else
            {
                if (!MapManager.GetMap(Info, 0, out ZoneServerMap Map))
                {
                    ZoneChat.CharacterNote(Character, "NormalMap Offline");
                    return true;
                }

                Character.ChangeMap(Info.MapInfo.ID, 0, Info.MapInfo.Regen.X, Info.MapInfo.Regen.Y);

                return true;
            }
        }

        [ZoneCommand("Money")]
        public static bool Money_Handler(ZoneCharacter Character, string[] Params)
        {
            if(Params.Length < 2)
            {
                ZoneChat.CharacterNote(Character, "Invalid Parameters!! use &Character  <Charactername> or <id> <money>");
                return true;
            }

          


            switch (Params[0].ToUpper())
            {
                case "SET" when (Params.Length == 3 && ulong.TryParse(Params[2], out ulong Money)):

                    if (Character.Selection != null && Character.Selection.SelectedObject is ZoneCharacter pChar)
                        pChar.Info.Money = Money;
                    else
                        Character.Info.Money = Money;

                    return true;

                case "Give" when (Params.Length == 3 && ulong.TryParse(Params[2], out ulong GiveMoney)):

                    if (Character.Selection != null && Character.Selection.SelectedObject is ZoneCharacter Select)
                        Select.Info.Money = Select.Info.Money + GiveMoney;
                    else
                        Character.Info.Money = Character.Info.Money + GiveMoney;

                    return true;
                default:
                    ZoneChat.CharacterNote(Character, "Invalid Command Parameters");
                    break;
            }
            return true;

        }
    }
}
