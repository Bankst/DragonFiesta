using DragonFiesta.Messages.World.Character;
using DragonFiesta.Messages.Zone.Character;
using DragonFiesta.Providers.Characters;
using DragonFiesta.Zone.Game.Character;
using System;

namespace DragonFiesta.Zone.InternNetwork.InternHandler.Server.Character
{
    public static class CharacterMethods
    {
        public static void SendSetFriendPoints(ZoneCharacter Character, ushort NewFriendPoint, Action<IMessage> CallBack)
        => InternWorldConnector.Instance.SendMessage(new SetFriendPoints
        {
            Id = Guid.NewGuid(),
            CharacterId = Character.Info.CharacterID,
            FriendPoint = NewFriendPoint,
            Callback = CallBack,

        });

        public static void SendCharacterChangeClass(ZoneCharacter Character, ClassId NewClass)
            => InternWorldConnector.Instance.SendMessage(new CharacterClassChanged
            {
                Id = Guid.NewGuid(),
                NewClass = NewClass,
                CharacterId = Character.Info.CharacterID,
            });

        public static void SendCharacterPositionRequest(ZoneCharacter Character, Action<IMessage> CallBack)
          
            => InternWorldConnector.Instance.SendMessage(new CharacterPosition()
            {
                Id = Guid.NewGuid(),
                CharacterId = Character.Info.CharacterID,
                Callback = CallBack,

            });
        
        public static void SendCharacterLevelChanged(int CharacterId, byte NewLevel, ushort mobId = ushort.MaxValue)

            => InternWorldConnector.Instance.SendMessage(new CharacterLevelChanged
            {
                Id = Guid.NewGuid(),
                CharacterId = CharacterId,
                NewLevel = NewLevel,
                MobId = mobId
            });

            

        public static void SendCharacterChangeMap(int CharacterId, ushort mapId, Position SpawnPosion, ushort InstanceId = 0)
       
            => InternWorldConnector.Instance.SendMessage(new CharacterChangeMap
            {
                Id = Guid.NewGuid(),
                CharacterId = CharacterId,
                InstanceId = InstanceId,
                Position = SpawnPosion,
                MapId = mapId,
            });
        

        public static void SendCharacterLoggedOut(ZoneCharacter Character, bool CharacterListSending = false)

          => InternWorldConnector.Instance.SendMessage(new CharacterLoggedOut
          {
              Id = Guid.NewGuid(),
              SendCharacterList = CharacterListSending,
              CharacterId = Character.Info.CharacterID,
          });

      

        public static void SendCharacterUpdateMoney(ZoneCharacter Character)

            => InternWorldConnector.Instance.SendMessage(new SetCharacterMoney
            {
                Id = Guid.NewGuid(),
                CharacterId = Character.Info.CharacterID,
                NewMoney = Character.Info.Money,
            });

    }
}
