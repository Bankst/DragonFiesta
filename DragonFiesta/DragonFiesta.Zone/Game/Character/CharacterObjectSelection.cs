using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Maps.Object;
using DragonFiesta.Zone.Game.Maps.Interface;
using DragonFiesta.Zone.Game.Maps.Event;
using DragonFiesta.Zone.Network.FiestaHandler.Server;

namespace DragonFiesta.Zone.Game.Maps
{
    public class CharacterObjectSelection : LivingObjectSelectionBase
    {
        private ZoneCharacter Character;

        public CharacterObjectSelection(ILivingObject Owner) : base(Owner)
        {
            Character = (Owner as ZoneCharacter);

            Owner.LivingStats.OnHPChanged += HP_SP_OnChanged;
            Owner.LivingStats.OnSPChanged += HP_SP_OnChanged;
        }

        private void HP_SP_OnChanged(object sender, LivingObjectInterActiveStatsChangedEventArgs e)
        {
            SelectedByAction((obj) =>
            {
                //update to all objects which have selected us
                SH09Handler.SendSelectionUpdate((obj as ZoneCharacter).Session, Owner, true);

                //update "selected by" to all objects which have selected obj
                obj.Selection.SelectedByAction((sObj) =>
                {
                    SH09Handler.SendSelectionUpdate((sObj as ZoneCharacter).Session, Owner, true);
                }, true);
            }, true);
        }

        public override bool SelectObject(ILivingObject Object)
        {
            lock (ThreadLocker)
            {
                if (base.SelectObject(Object))
                {
                    if (Character != null)
                    {
                        //update new selection to character
                        SH09Handler.SendSelectionUpdate(Character.Session, Object, false);

                        //update selected object of new selection to character
                        if (Object.Selection.SelectedObject != null)
                        {
                            SH09Handler.SendSelectionUpdate(Character.Session, SelectedObject, true);
                        }
                    }

                    SelectedByAction((obj) =>
                    {
                        SH09Handler.SendSelectionUpdate((obj as ZoneCharacter).Session, Object, true);
                    }, true);

                    return true;
                }

                return false;
            }
        }

        public override void Dispose()
        {
            Character = null;
            base.Dispose();
        }
    }
}