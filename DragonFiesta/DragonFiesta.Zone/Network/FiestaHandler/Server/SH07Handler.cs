using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Zone.Game.Maps.Interface;
using System;

namespace DragonFiesta.Zone.Network.FiestaHandler.Server
{
    public static class SH07Handler
    {
        public static FiestaPacket SpawnSingleObject(IMapObject Object)
        {
            var packet = new FiestaPacket(Handler07Type._Header, ((Object.Type == MapObjectType.Character) ? Handler07Type.SMSG_BRIEFINFO_LOGINCHARACTER_CMD : Handler07Type.SMSG_BRIEFINFO_REGENMOB_CMD));
            Object.WriteDisplay(packet);
            return packet;
        }

        /// <summary>
        /// Used for spawning multiple characters / mobs / NPCs
        /// </summary>
        /// <param name="Collection"></param>
        /// <param name="AreCharacters"></param>
        /// <param name="SendAction"></param>
        /// <param name="ObjectsPerPacket"></param>
        public static void SpawnMultiObject(IMapObject[] Collection, bool AreCharacters, Action<FiestaPacket> SendAction, int ObjectsPerPacket = 255)
        {
            for (int i = 0; i < Collection.Length; i += ObjectsPerPacket)
            {
                using (var packet = SpawnMultiObject(Collection, i, (i + Math.Min(ObjectsPerPacket, (Collection.Length - 1))), AreCharacters))
                {
                    SendAction.Invoke(packet);
                }
            }
        }

        /// <summary>
        /// Used for spawning multiple characters / mobs / NPCs based on the start and end position.
        /// </summary>
        /// <param name="Collection"></param>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <returns></returns>
        public static FiestaPacket SpawnMultiObject(IMapObject[] Collection, int Start, int End, bool AreCharacters)
        {
            var packet = new FiestaPacket(Handler07Type._Header, (AreCharacters ? Handler07Type.SMSG_BRIEFINFO_CHARACTER_CMD : Handler07Type.SMSG_BRIEFINFO_MOB_CMD));
            packet.Write<byte>((byte)(End - Start));
            for (int i = Start; i < End; i++)
            {
                Collection[i].WriteDisplay(packet);
            }
            return packet;
        }

        /// <summary>
        /// Used for removing an object
        /// </summary>
        /// <param name="Object"></param>
        /// <returns></returns>
        public static FiestaPacket RemoveObject(IMapObject Object)
        {
            var packet = new FiestaPacket(Handler07Type._Header, Handler07Type.SMSG_BRIEFINFO_BRIEFINFODELETE_CMD);
            packet.Write<ushort>(Object.MapObjectId);
            return packet;
        }
    }
}