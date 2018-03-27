using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Zone.Game.Maps.Interface;
using DragonFiesta.Zone.Game.Maps.Types;

namespace DragonFiesta.Zone.Network.FiestaHandler.Client
{
    [PacketHandlerClass(Handler09Type._Header)]
    public static class CH09Handler
    {
        [PacketHandler(Handler09Type.CMSG_BAT_TARGETTING_REQ)]
        public static void CMSG_ENTRY_OBJECT_SELECT(ZoneSession sender, FiestaPacket packet)
        {
            if (!sender.Ingame || !packet.Read(out ushort MapObjectId))
            {
                sender.Dispose();
                return;
            }
            if ((sender.Character.Map as LocalMap).GetObjectByID(MapObjectId, out IMapObject Obj))
            {
                sender.Character.Selection.SelectObject((Obj as ILivingObject));
            }
        }

        [PacketHandler(Handler09Type.CMSG_BAT_UNTARGET_REQ)]
        public static void MSG_ENTRY_OBJECT_UNSELECT(ZoneSession sender, FiestaPacket packet)
        {
            if (!sender.Ingame)
            {
                sender.Dispose();
                return;
            }

            sender.Character.Selection.DeselectObject();
        }
    }
}