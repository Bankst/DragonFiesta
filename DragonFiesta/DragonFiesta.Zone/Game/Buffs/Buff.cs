using System;
using System.Collections.Generic;
using DragonFiesta.Zone.Data.Buffs;
using DragonFiesta.Zone.Game.Maps.Interface;
using System.Threading;
using DragonFiesta.Utils.Logging;
using DragonFiesta.Utils.ServerTask;

namespace DragonFiesta.Zone.Game.Buffs
{
    public class Buff : iExpireAble
    {
        public long ID { get; set; }

        public ILivingObject Owner { get; private set; }
        public AbStateInfo AbStateInfo { get; private set; }

        public SubAbStateInfo SubAbstateInfo { get; private set; }
        public uint Strength { get; private set; }

        public BuffAction[] Actions { get; private set; }
        public DateTime StartTime { get; set; }
        public DateTime ExpireTime { get; set; }

        public bool IsDisposed { get { return (IsDisposedInt > 0); } }
        private int IsDisposedInt;
        private object ThreadLocker;

        public Buff(ILivingObject Owner, AbStateInfo AbStateInfo, uint Strength)
        {
            this.Owner = Owner;
            this.AbStateInfo = AbStateInfo;
            this.Strength = Strength;
            ThreadLocker = new object();
        }

        ~Buff()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
            {
                Owner = null;
                AbStateInfo = null;
                SubAbstateInfo = null;
                Actions = null;
                ThreadLocker = null;
            }
        }

        public void Activate()
        {
            lock (ThreadLocker)
            {
                for (int i = 0; i < Actions.Length; i++)
                {
                    Actions[i].Activate();
                }
            }
        }

        public void Deactivate()
        {
            lock (ThreadLocker)
            {
                for (int i = 0; i < Actions.Length; i++)
                {
                    Actions[i].Deactivate();
                }
            }
        }

        public bool UpdateStrength(uint NewStrength)
        {
            //get new subabstate
            SubAbStateInfo subAbState;
            if (!AbStateInfo.SubAbStates.TryGetValue(NewStrength, out subAbState))
            {
                GameLog.Write(GameLogLevel.Warning, "Can't find strength '{0}' for abstate '{1}'", NewStrength, AbStateInfo.ID);
                return false;
            }

            lock (ThreadLocker)
            {
                SubAbstateInfo = subAbState;
                Strength = NewStrength;

                //refresh actions
                var newActions = new List<BuffAction>();
                for (int i = 0; i < subAbState.Actions.Count; i++)
                {
                    BuffAction buffAction;
                    if (CreateBuffAction(this, subAbState.Actions[i], out buffAction))
                    {
                        newActions.Add(buffAction);
                    }
                }

                Actions = newActions.ToArray();
                newActions.Clear();
                newActions = null;
            }
            return true;
        }


        private static bool CreateBuffAction(Buff Buff, SubAbStateAction SubAction, out BuffAction BuffAction)
        {
            switch (SubAction.Type)
            {
                case SubAbStateActionType.SAA_STRRATE:
                case SubAbStateActionType.SAA_STRPLUS:
                case SubAbStateActionType.SAA_WCPLUS:
                case SubAbStateActionType.SAA_WCRATE:
                case SubAbStateActionType.SAA_ACPLUS:
                case SubAbStateActionType.SAA_ACRATE:
                case SubAbStateActionType.SAA_DEXPLUS:
                case SubAbStateActionType.SAA_TBPLUS:
                case SubAbStateActionType.SAA_TBRATE:
                case SubAbStateActionType.SAA_THPLUS:
                case SubAbStateActionType.SAA_THRATE:
                case SubAbStateActionType.SAA_INTPLUS:
                case SubAbStateActionType.SAA_MAPLUS:
                case SubAbStateActionType.SAA_MENTALPLUS:
                case SubAbStateActionType.SAA_MRPLUS:
                case SubAbStateActionType.SAA_MRRATE:
                case SubAbStateActionType.SAA_SHIELDAMOUNT:
                case SubAbStateActionType.SAA_SHIELDACRATE:
                case SubAbStateActionType.SAA_NOMOVE:
                case SubAbStateActionType.SAA_SPEEDRATE:
                case SubAbStateActionType.SAA_ATTACKSPEEDRATE:
                case SubAbStateActionType.SAA_MAXHPRATE:
                case SubAbStateActionType.SAA_MAXSPRATE:
                case SubAbStateActionType.SAA_DEADHPSPRECOVRATE:
                case SubAbStateActionType.SAA_NOATTACK:
                case SubAbStateActionType.SAA_TICK:
                case SubAbStateActionType.SAA_DOTDAMAGE:
                case SubAbStateActionType.SAA_CONHEAL:
                case SubAbStateActionType.SAA_CASTINGTIMEPLUS:
                case SubAbStateActionType.SAA_HEALAMOUNT:
                case SubAbStateActionType.SAA_POISONRESISTRATE:
                case SubAbStateActionType.SAA_DISEASERESISTRATE:
                case SubAbStateActionType.SAA_CURSERESISTRATE:
                case SubAbStateActionType.SAA_CRITICALRATE:
                case SubAbStateActionType.SAA_MAXHPPLUS:
                case SubAbStateActionType.SAA_MAXSPPLUS:
                case SubAbStateActionType.SAA_INTRATE:
                case SubAbStateActionType.SAA_FEAR:
                case SubAbStateActionType.SAA_ALLSTATEPLUS:
                case SubAbStateActionType.SAA_REVIVEHEALRATE:
                case SubAbStateActionType.SAA_COUNT:
                case SubAbStateActionType.SAA_SILIENCE:
                case SubAbStateActionType.SAA_DEADLYBLESSING:
                case SubAbStateActionType.SAA_DAMAGERATE:
                case SubAbStateActionType.SAA_TARGETENEMY:
                case SubAbStateActionType.SAA_MARATE:
                case SubAbStateActionType.SAA_HEALRATE:
                case SubAbStateActionType.SAA_DOTRATE:
                case SubAbStateActionType.SAA_AWAY:
                case SubAbStateActionType.SAA_TOTALDAMAGERATE:
                case SubAbStateActionType.SAA_DISPELSPEEDRATE:
                case SubAbStateActionType.SAA_SETABSTATEME:
                case SubAbStateActionType.SAA_SETABSTATEFRIEND:
                case SubAbStateActionType.SAA_SETABSTATE:
                case SubAbStateActionType.SAA_AREA:
                case SubAbStateActionType.SAA_GTIRESISTRATE:
                case SubAbStateActionType.SAA_MAXHPRATEDAMAGE:
                case SubAbStateActionType.SAA_METAABILITY:
                case SubAbStateActionType.SAA_METASKIN:
                case SubAbStateActionType.SAA_MISSRATE:
                case SubAbStateActionType.SAA_REFLECTDAMAGE:
                case SubAbStateActionType.SAA_RELESEACTION:
                case SubAbStateActionType.SAA_SCANENEMYUSER:
                case SubAbStateActionType.SAA_TARGETALL:
                case SubAbStateActionType.SAA_HIDEENEMY:
                case SubAbStateActionType.SAA_TARGETNOTME:
                case SubAbStateActionType.SAA_DOTDIEDAMAGE:
                case SubAbStateActionType.SAA_ADDALLDOTDMG:
                case SubAbStateActionType.SAA_ADDBLOODINGDMG:
                case SubAbStateActionType.SAA_ADDPOISONDMG:
                case SubAbStateActionType.SAA_EVASIONAMOUNT:
                case SubAbStateActionType.SAA_USESPRATE:
                case SubAbStateActionType.SAA_ACMINUS:
                case SubAbStateActionType.SAA_ACDOWNRATE:
                case SubAbStateActionType.SAA_SUBTRACTALLDOTDMG:
                case SubAbStateActionType.SAA_SUBTRACTBLOODINGDMG:
                case SubAbStateActionType.SAA_SUBTRACTPOISONDMG:
                case SubAbStateActionType.SAA_ATKSPEEDDOWNRATE:
                case SubAbStateActionType.SAA_AWAYBACK:
                case SubAbStateActionType.SAA_CRITICALDOWNRATE:
                case SubAbStateActionType.SAA_DEXMINUS:
                case SubAbStateActionType.SAA_HEALAMOUNTMINUS:
                case SubAbStateActionType.SAA_MAMINUS:
                case SubAbStateActionType.SAA_MADOWNRATE:
                case SubAbStateActionType.SAA_MAXHPDOWNRATE:
                case SubAbStateActionType.SAA_MRMINUS:
                case SubAbStateActionType.SAA_MRDOWNRATE:
                case SubAbStateActionType.SAA_SPEEDDOWNRATE:
                case SubAbStateActionType.SAA_STRMINUS:
                case SubAbStateActionType.SAA_TBMINUS:
                case SubAbStateActionType.SAA_TBDOWNRATE:
                case SubAbStateActionType.SAA_THMINUS:
                case SubAbStateActionType.SAA_THDOWNRATE:
                case SubAbStateActionType.SAA_WCMINUS:
                case SubAbStateActionType.SAA_WCDOWNRATE:
                case SubAbStateActionType.SAA_DOTWCRATE:
                case SubAbStateActionType.SAA_TARGETNUMVER:
                case SubAbStateActionType.SAA_DOTMARATE:
                case SubAbStateActionType.SAA_MENDOWNRATE:
                case SubAbStateActionType.SAA_USESPDOWN:
                case SubAbStateActionType.SAA_CRIUPRATE:
                case SubAbStateActionType.SAA_MRSHIELDRATE:
                case SubAbStateActionType.SAA_ACSHIELDRATE:
                case SubAbStateActionType.SAA_MONSTERSTICK:
                case SubAbStateActionType.SAA_SETACTIVESKILL:
                case SubAbStateActionType.SAA_HPRATEDAMAGE:
                case SubAbStateActionType.SAA_EXPRATE:
                case SubAbStateActionType.SAA_DROPRATE:
                case SubAbStateActionType.SAA_AWAYBACKSPOT:
                case SubAbStateActionType.SAA_STOPANI:
                case SubAbStateActionType.SAA_SPEEDRESISTRATE:
                case SubAbStateActionType.SAA_DOTDMGDOWNRATE:
                case SubAbStateActionType.SAA_SHIELDRATE:
                case SubAbStateActionType.SAA_LPAMOUNT:
                case SubAbStateActionType.SAA_MINHP:
                case SubAbStateActionType.SAA_DMGDOWNRATE:
                case SubAbStateActionType.SAA_MELEE:
                case SubAbStateActionType.SAA_RANGE:
                case SubAbStateActionType.SAA_ALLSTATPLUS:
                case SubAbStateActionType.SAA_RANGEOVER:
                case SubAbStateActionType.SAA_None:
                default:
                    BuffAction = null;
                    break;
            }
            return (BuffAction != null);
        }

        public void OnExpire(GameTime gameTime)
        {
            /*
            if (Owner.BuffList.Remove(this))
            {
                Dispose();
            }
            */
        }

        public void Update(GameTime gameTime)
        {
            //Update BuffTime?
        }
    }
}