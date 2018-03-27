using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Providers.Characters;
using DragonFiesta.Providers.Chat;
using DragonFiesta.Utils.Config;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Network.FiestaHandler.Server;

namespace DragonFiesta.World.Network.FiestaHandler.Client
{
    [PacketHandlerClass(Handler05Type._Header)]
    public static class CH05Handler
    {
        [PacketHandler(Handler05Type.CMSG_AVATAR_CREATE_REQ)]
        public static void HandlerCreateCharacter(WorldSession sender, FiestaPacket packet)
        {
            //new Data no idea :D
            if (sender.Ingame
                || !sender.AccountIsLoggedIn
                || !packet.Read(out byte Slot)
                || !packet.ReadEncodeString(out string Name, 20)
                || !packet.Read(out byte Job)
                || !packet.Read(out byte HairID)
                || !packet.Read(out byte HairColorID)
                || !packet.Read(out byte FaceID))
            {
                sender.Dispose();
                return;
            }

            var IsMale = (((byte)((Job >> 7) & 0x01)) > 0);
            var ClassID = (byte)((Job >> 2) & 0x1F);

            if (sender.CharacterList.Count >= GameConfiguration.Instance.MaxCharacterPerWorld)
            {
                SH05Handler.SendCharacterCreationError(sender, CharacterCreationError.ErrorInMaxSlot);
                return;
            }

            if (ChatDataProvider.GetBadName(Name))
            {
                SH05Handler.SendCharacterCreationError(sender, CharacterCreationError.NameTaken);
                return;
            }

            if (!CharacterClass.IsValidClass(ClassID)
                || !CharacterLookProvider.GetHairInfoByID(HairID, out HairInfo hairInfo)
                || !CharacterLookProvider.GetHairColorInfoByID(HairColorID, out HairColorInfo hairColorInfo)
                || !CharacterLookProvider.GetFaceInfoByID(FaceID, out FaceInfo faceInfo))
            {
                SH05Handler.SendCharacterCreationError(sender, CharacterCreationError.FailedToCreate);
                return;
            }

            if (!WorldCharacterManager.Instance.CreateCharacter(sender.UserAccount,
                                     Name,
                                     Slot,
                                     ClassID,
                                     IsMale,
                                     hairInfo,
                                     hairColorInfo,
                                     faceInfo,
                                     out WorldCharacter character,
                                     out CharacterCreationError error))
            {
                SH05Handler.SendCharacterCreationError(sender, error);
                return;
            }

            // add to Client:)
            if (!sender.CharacterList.Add(character))
            {
                SH05Handler.SendCharacterCreationError(sender, CharacterCreationError.FailedToCreate);
                return;
            }

            SH05Handler.SendCharacterCreationOK(sender, character);
        }

        [PacketHandler(Handler05Type.CMSG_AVATAR_ERASE_REQ)]
        public static void HandlerDeleteCharacter(WorldSession sender, FiestaPacket packet)
        {
            if (!sender.AccountIsLoggedIn
                || sender.Ingame
                || !packet.Read(out byte slot))
            {
                sender.Dispose();
                return;
            }

            if (!sender.CharacterList.GetCharacterBySlot(slot, out WorldCharacter character))
            {
                SH05Handler.SendCharacterDeleteFail(sender);
                return;
            }

            if (WorldCharacterManager.Instance.DeleteCharacter(character))
            {
                sender.CharacterList.Remove(character);

                SH05Handler.SendCharacterDeleteComplete(sender, character.Info.Slot);

                character.Dispose();
            }
            else
            {
                SH05Handler.SendCharacterDeleteFail(sender);
            }
        }

        [PacketHandler(Handler05Type.CMSG_AVATAR_RENAME_REQ)]
        public static void HandleChangeCharacterName(WorldSession sender, FiestaPacket packet)
        {
            if (!sender.AccountIsLoggedIn
            || sender.Ingame
            || !packet.Read(out byte Slot)
            || !packet.ReadEncodeString(out string NewName, 20)
            || !packet.Read(out int unk))
            {
                sender.Dispose();
                return;
            }

            if (ChatDataProvider.GetBadName(NewName))
            {
                SH05Handler.SendCharacterChangeName(sender, Slot, NewName, false);
                return;
            }
            if (sender.CharacterList[Slot] == null
                || !sender.CharacterList[Slot].Info.Name.StartsWith("-"))
            {
                //    SH04Helpers.SendWorldError(sender, (WorldGameErrors)11);
                return;
            }
            /*
            if (!WorldCharacterManager.Instance.CharacterChangeName(sender.CharacterList[Slot], NewName))
            {
                SH05Handler.SendCharacterChangeName(sender, Slot, NewName, false);
                return;
            }*/

            sender.CharacterList[Slot].Info.Name = NewName;

            SH05Handler.SendCharacterChangeName(sender, Slot, NewName, sender.CharacterList[Slot].Info.Name == NewName ? true : false);
        }
    }
}