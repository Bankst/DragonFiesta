using DragonFiesta.Messages.World.Character;
using DragonFiesta.Messages.Zone.Character;
using DragonFiesta.Providers.Characters;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Game.Maps;
using DragonFiesta.World.Game.Zone;
using System;

namespace DragonFiesta.World.InternNetwork.InternHandler.Server.Character
{
    public static class CharacterMethods
    {
        public static void SendSetFriendPoint(WorldCharacter Character, ushort NewPoints)
        => (Character.Map as WorldServerMap).Zone.Send(new SetFriendPoints
        {
            Id = Guid.NewGuid(),
            CharacterId = Character.Info.CharacterID,
            FriendPoint = NewPoints,
        });
        
        public static void SendCharacterClassChanged(WorldCharacter Character, ClassId NewClass)
            => ZoneManager.Broadcast(new CharacterClassChanged
            {
                Id = Guid.NewGuid(),
                CharacterId = Character.Info.CharacterID,
                NewClass = NewClass,
            });

        public static void SendCharacterDeleted(WorldCharacter Character)
            => ZoneManager.Broadcast(new CharacterDeleted

            {
                Id = Guid.NewGuid(),
                CharacterId = Character.Info.CharacterID,
            });

        public static void SendCharacterPositionRequest(ZoneServer Server, int CharacterId, Action<IMessage> CallBack)

            => Server.Send(new CharacterPosition
            {
                Id = Guid.NewGuid(),
                CharacterId = CharacterId,
                Callback = CallBack,

            });


        public static void BroadcastCharacterChangeMap(int CharacterId, ushort mapId, Position SpawnPosition, ushort InstanceId = 0)

            => ZoneManager.Broadcast(new CharacterChangeMap
            {
                Id = Guid.NewGuid(),
                CharacterId = CharacterId,
                InstanceId = InstanceId,


                Position = SpawnPosition,
                MapId = mapId,
            });


        public static void BroadcastLevelChanged(int CharacterId, byte NewLevel, ushort MobId = ushort.MaxValue)

            => ZoneManager.Broadcast(new CharacterLevelChanged
            {
                Id = Guid.NewGuid(),
                CharacterId = CharacterId,
                NewLevel = NewLevel,
                MobId = MobId,
            });


        public static void BroadCastOfflineCharacter(WorldCharacter Character)

        => ZoneManager.Broadcast(new CharacterLoggedOut
        {
            Id = Guid.NewGuid(),
            CharacterId = Character.Info.CharacterID,
        });


        public static void BroadcastOnlineCharacter(WorldCharacter Character)

            => ZoneManager.Broadcast(new CharacterLoggedIn
            {
                Id = Guid.NewGuid(),
                CharacterId = Character.Info.CharacterID,
                Instance = Character.AreaInfo.InstanceId,
                MapId = Character.AreaInfo.Map.MapId,
            });
    }
}
