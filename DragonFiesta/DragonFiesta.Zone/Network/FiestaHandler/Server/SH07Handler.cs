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
        /// <param name="collection"></param>
        /// <param name="areCharacters"></param>
        /// <param name="sendAction"></param>
        /// <param name="objectsPerPacket"></param>
        public static void SpawnMultiObject(IMapObject[] collection, bool areCharacters, Action<FiestaPacket> sendAction, int objectsPerPacket = 255)
        {
            for (var i = 0; i < collection.Length; i += objectsPerPacket)
            {
                using (var packet = SpawnMultiObject(collection, i, (i + Math.Min(objectsPerPacket, (collection.Length - 1))), areCharacters))
                {
                    sendAction.Invoke(packet);
                }
            }
        }

	    /// <summary>
	    /// Used for spawning multiple characters / mobs / NPCs based on the start and end position.
	    /// </summary>
	    /// <param name="collection"></param>
	    /// <param name="start"></param>
	    /// <param name="end"></param>
	    /// <param name="areCharacters"></param>
	    /// <returns></returns>
	    public static FiestaPacket SpawnMultiObject(IMapObject[] collection, int start, int end, bool areCharacters)
        {
            var packet = new FiestaPacket(Handler07Type._Header, (areCharacters ? Handler07Type.SMSG_BRIEFINFO_CHARACTER_CMD : Handler07Type.SMSG_BRIEFINFO_MOB_CMD));
            packet.Write<byte>((byte)(end - start));
            for (var i = start; i < end; i++)
            {
                collection[i].WriteDisplay(packet);
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