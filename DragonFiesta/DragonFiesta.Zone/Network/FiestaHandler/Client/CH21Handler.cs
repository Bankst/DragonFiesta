using DragonFiesta.Messages.World.Character;
using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Utils.Config;
using DragonFiesta.Zone.Data.Friends;
using DragonFiesta.Zone.Game.Chat;
using DragonFiesta.Zone.InternNetwork.InternHandler.Server.Character;
using DragonFiesta.Zone.Network.FiestaHandler.Server;

namespace DragonFiesta.Zone.Network.FiestaHandler.Client
{
    [PacketHandlerClass(Handler21Type._Header)]
    public static class CH21Handler
    {
        [PacketHandler(Handler21Type.CMSG_FRIEND_UES_FRIEND_POINT_REQ)]
        public static void CMSG_FRIEND_UES_FRIEND_POINT_REQ(ZoneSession sender, FiestaPacket packet)
        {
            if (!sender.Ingame || sender.Character.Info.FriendPoints <= 0)
            {
                sender.Dispose();
                return;
            }

            if (sender.Character.Info.FriendPoints < GameConfiguration.Instance.FriendSettings.UseFriendPointCost)
            {
                ZoneChat.CharacterNote(sender.Character, "Friend Points to Low!");
                return;
            }


            if (!FriendDataProvider.GetReward(out FriendPointReward Reward))
            {
                ZoneChat.CharacterNote(sender.Character, "Database Error");
                return;
            }

            CharacterMethods.SendSetFriendPoints(sender.Character, (sender.Character.Info.FriendPoints -= GameConfiguration.Instance.FriendSettings.UseFriendPointCost), (msg) =>
              {
                  if (msg is SetFriendPoints re)
                  {

                      sender.Character.Info.FriendPoints = re.FriendPoint;
                      //TODO Add ItemHere...

                      SH21Handler.SendFriendPointUse(sender, Reward);
                  }
              });
        }
    }
}
