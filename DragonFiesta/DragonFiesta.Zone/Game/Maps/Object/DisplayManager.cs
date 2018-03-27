using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.NPC;
using DragonFiesta.Zone.Network.FiestaHandler.Server;
using DragonFiesta.Zone.Game.Maps.Event;
using DragonFiesta.Zone.Game.Maps.Types;

namespace DragonFiesta.Zone.Game.Maps.Object
{
    public class DisplayManager
    {
        public LocalMap Map { get; private set; }

        public DisplayManager(LocalMap Map)
        {
            this.Map = Map;
            Map.OnObjectAdded += On_Map_ObjectAdded;
            Map.OnObjectRemoved += On_Map_ObjectRemoved;
        }

        private void On_Map_ObjectAdded(object sender, MapObjectEventArgs args)
        {
            using (var packet = SH07Handler.SpawnSingleObject(args.MapObject))
            {
                args.MapObject.Broadcast(packet, false);
            }

            //send other objects to new object if its a character
            var character = (args.MapObject as ZoneCharacter);
            if (character != null)
            {
                //bind events
                character.InRange.OnObjectAdded += On_Character_InRange_ObjectAdded;
                character.InRange.OnObjectRemoved += On_Character_InRange_ObjectRemoved;

                if (character.IsConnected)
                {
                    //send npcs to character
                    /*  NPCBase[] npcs;
                      if (NPCManager.GetNPCsByMapInstanceID(Map.InstanceID, out npcs))
                      {
                          /*
                          for (int i = 0; i < npcs.Length; i++)
                          {
                              using (var packet = SH7Helpers.SpawnSingleObject(npcs[i]))
                              {
                                  character.Client.Send(packet);
                              }
                          }

                          SH07Handler.SpawnMultiObject(npcs, false, (packet) =>
                          {
                              character.Client.Send(packet);
                          }, 255);
                      }*/

                    //send other characters to character
                    var characters = character.MapSector.GetCharacters(true, character);

                    for (int i = 0; i < characters.Length; i++)
                    {
                        using (var packet = SH07Handler.SpawnSingleObject(characters[i]))
                        {
                            character.Session.SendPacket(packet);
                        }
                    }
                    /*
                    SH7Helpers.SpawnMultiObject(characters, true, (packet) =>
                        {
                            character.Client.Send(packet);
                        }, 255);
                    */

                    characters = null;

                    //send mobs to character
                    //  var mobs = character.MapSector.GetMobs(true);

                    /*
                    for (int i = 0; i < mobs.Length; i++)
                    {
                        if (mobs[i].IsVisible)
                        {
                            using (var packet = SH7Helpers.SpawnSingleObject(mobs[i]))
                            {
                                character.Client.Send(packet);
                            }
                        }
                    }

                    SH07Handler.SpawnMultiObject(mobs, false, (p) => character.Session.SendPacket(p));

                    mobs = null;*/
                }
            }
        }

        private void On_Map_ObjectRemoved(object sender, MapObjectEventArgs args)
        {
            if (args.MapObject is ZoneCharacter)
            {
                args.MapObject.InRange.OnObjectAdded -= On_Character_InRange_ObjectAdded;
                args.MapObject.InRange.OnObjectRemoved -= On_Character_InRange_ObjectRemoved;
            }

            using (var packet = SH07Handler.RemoveObject(args.MapObject))
            {
                args.MapObject.MapSector.Broadcast(packet, true, (args.MapObject as ZoneCharacter));
            }
        }

        private void On_Character_InRange_ObjectAdded(object sender, MapObjectEventArgs args)
        {
            //we dont add npcs
            if (!(args.MapObject is NPCBase))
            {
                var col = (InRangeCollection)sender;
                var character = (col.Owner as ZoneCharacter);

                if (character != null
                    && character.IsConnected)
                {
                    //GameLog.Write(GameLogType.Debug, "Sending object {0} ({1}) to character {2}.", args.MapObject.ID, args.MapObject.Type, character.Name);

                    //send new object to character
                    using (var packet = SH07Handler.SpawnSingleObject(args.MapObject))
                    {
                        character.Session.SendPacket(packet);
                    }

                    /*
                    var livingObject = (args.MapObject as iLivingObject);
                    if (livingObject != null)
                    {
                        //send buffs of object to character
                        for (int i = 0; i < livingObject.Buffs.Count; i++)
                        {
                            var buff = livingObject.Buffs[i];

                            using (var packet = new GamePacket(GameOpCode.Server.H9.SetBuffObject))
                            {
                                packet.WriteUInt16(livingObject.ID);
                                packet.WriteUInt32(buff.AbStateInfo.AbStateIndex);
                                packet.WriteUInt32((uint)(buff.ExpireTime - ZoneEngine.Instance.CurrentTime.Time).TotalMilliseconds);

                                character.Client.Send(packet);
                            }
                        }
                    }*/
                }
            }
        }

        private void On_Character_InRange_ObjectRemoved(object sender, MapObjectEventArgs args)
        {
            //we dont remove npcs
            if (!(args.MapObject is NPCBase))
            {
                var col = (InRangeCollection)sender;
                var character = (col.Owner as ZoneCharacter);

                if (character != null
                    && character.IsConnected)
                {
                    //send new object to character
                    using (var packet = SH07Handler.RemoveObject(args.MapObject))
                    {
                        character.Session.SendPacket(packet);
                    }
                }
            }
        }

        public void Dispose()
        {
            Map = null;
        }
    }
}