using DragonFiesta.Zone.Game.Maps.Interface;
using System;

namespace DragonFiesta.Zone.Game.Maps.Object
{
    public class LivingObjectSelectionBase
    {
        public ILivingObject Owner { get; private set; }
        public ILivingObject SelectedObject { get; private set; }

        public SecureWriteCollection<ILivingObject> SelectedBy { get; private set; }
        private Func<ILivingObject, bool> SelectedByAddFunc;
        private Func<ILivingObject, bool> SelectedByRemoveFunc;

        private Action SelectedByClearFunc;
        protected object ThreadLocker { get; private set; }

        protected LivingObjectSelectionBase(ILivingObject Owner)
        {
            this.Owner = Owner;
            SelectedBy = new SecureWriteCollection<ILivingObject>(out SelectedByAddFunc, out SelectedByRemoveFunc, out SelectedByClearFunc);
            ThreadLocker = new object();
        }

        public virtual void Dispose()
        {
            Owner = null;
            SelectedObject = null;
            SelectedBy.Dispose();
            SelectedBy = null;
            SelectedByAddFunc = null;
            SelectedByRemoveFunc = null;
            SelectedByClearFunc = null;
            ThreadLocker = null;
        }

        public virtual bool SelectObject(ILivingObject Object)
        {
            lock (ThreadLocker)
            {
                //deselect old object
                DeselectObject();
                //update selected object
                SelectedObject = Object;
                //add owner to objects selected by list
                Object.Selection.AddObject(Owner);
                return true;
            }
        }

        public virtual void DeselectObject()
        {
            lock (ThreadLocker)
            {
                if (SelectedObject != null)
                {
                    //remove owner from objects selected by list
                    SelectedObject.Selection.RemoveObject(Owner);
                    //clear selected object
                    SelectedObject = null;
                }
            }
        }

        public void SelectedByAction(Action<ILivingObject> Action, bool OnlyCharacters = false)
        {
            lock (ThreadLocker)
            {
                for (int i = 0; i < SelectedBy.Count; i++)
                {
                    try
                    {
                        var obj = SelectedBy[i];

                        if (OnlyCharacters
                            && obj.Type != MapObjectType.Character)
                            continue;
                        Action.Invoke(SelectedBy[i]);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }

        protected virtual bool AddObject(ILivingObject Object)
        {
            lock (ThreadLocker)
            {
                //add to list
                if (SelectedByAddFunc.Invoke(Object))
                {
                    //add owner to objects list
                    Object.Selection.AddObject(Object);

                    return true;
                }
                return false;
            }
        }

        protected virtual bool RemoveObject(ILivingObject Object)
        {
            lock (ThreadLocker)
            {
                //remove from list
                if (SelectedByRemoveFunc.Invoke(Object))
                {
                    //remove owner from objects list
                    Object.Selection.RemoveObject(Object);

                    return true;
                }
                return false;
            }
        }
    }
}