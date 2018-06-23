using System;
using DragonFiesta.Providers.Text;
using DragonFiesta.Zone.Data.Menu;
using DragonFiesta.Zone.Data.NPC;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Chat;

namespace DragonFiesta.Zone.Game.NPC
{
    public class GateNPC : NPCBase
    {
        public GateNPC(NPCInfo Info) : base(Info)
        {
        }

        public override sealed void OpenMenu(ZoneCharacter Character)
        {
            throw new NotImplementedException();
        }

        internal override void HandleInteraction(ZoneCharacter Character)
        {
            if (!MenutDataProvider.GetMenuByID((uint)NPCMenuData.GateNPCMenu, out MenuDataInfo MenuData))
            {
                GameLog.Write(GameLogLevel.Warning, $"Can't find GateNPCMenu ID {(uint)NPCMenuData.GateNPCMenu} Please Check you Database");
                return;
            }

            var Question = new Question(Character, MenuData.DataText.Text.FormatEx(Info.LinkTable.PortMap.Name));

            for (byte i = 0; i < MenuData.AnswerData.Count; i++)
            {
                Question.AddAnswer(new Answer(i, MenuData.AnswerData[i].Text));
            }

            Question.OnAnswer += Question_OnAnswer;
            Character.Question = Question;
            Question.Send();
        }

        private void Question_OnAnswer(object sender, Question.QuestionAnswerEventArgs args)
        {
            if (args.answer.Index == 0)
            {
                //GetTextData
                if (!TextDataProvider.GetTextDataById((uint)TextDataEntry.MapChangeLevelLimit, out TextData LvLimit))
                {
                    GameLog.Write(GameLogLevel.Warning, "Can't find Text {0} for MapChangeLevelLimit ", (uint)TextDataEntry.MapChangeLevelLimit);
                    return;
                }

                if (!TextDataProvider.GetTextDataById((uint)TextDataEntry.MapPartyOnlyAllow, out TextData PartytextData))
                {
                    GameLog.Write(GameLogLevel.Warning, "Can't find Text {0} for MapPartyOnlyAllow ", (uint)TextDataEntry.MapPartyOnlyAllow);
                    return;
                }

                if ((args.Question.Character as ZoneCharacter).Level < Info.LinkTable.Level.Min
                  || (args.Question.Character as ZoneCharacter).Level > Info.LinkTable.Level.Max)
                {
                    ZoneChat.CharacterNote((args.Question.Character as ZoneCharacter),
                        LvLimit.Text.FormatEx(Info.LinkTable.Level.Min,
                        Info.LinkTable.Level.Max));

                    return;
                }

                //TODO check pt limit


                //ChangeMap
                    (args.Question.Character as ZoneCharacter).ChangeMap(Info.LinkTable.PortMap.ID,
                        0,
                        Info.LinkTable.PortPosition.X,
                        Info.LinkTable.PortPosition.Y);

            }
        }

        protected override void DisposeInternal()
        {
            base.DisposeInternal();
        }
    }
}