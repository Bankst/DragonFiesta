using System;
using System.Data;

namespace DragonFiesta.Utils.IO.SHN
{
    public class TableCreator
    {
        public static DataTable CreateDefaultQuestDataOdin()
        {
            DataTable QuestDataTable = new DataTable();
            QuestDataTable.Columns.Add("ID", typeof(UInt16));
            QuestDataTable.Columns.Add("TitleID", typeof(UInt16));
            QuestDataTable.Columns.Add("DescriptionID", typeof(UInt16));
            QuestDataTable.Columns.Add("QuestGrade", typeof(Byte));
            QuestDataTable.Columns.Add("Repeatable", typeof(Boolean));
            QuestDataTable.Columns.Add("IsLevelBased", typeof(Boolean));
            QuestDataTable.Columns.Add("MinLevel", typeof(Byte));
            QuestDataTable.Columns.Add("MaxLevel", typeof(Byte));
            QuestDataTable.Columns.Add("Enabled", typeof(Boolean));
            QuestDataTable.Columns.Add("StartingNPCID", typeof(UInt16));
            QuestDataTable.Columns.Add("NeedItemForQuest", typeof(Boolean));
            QuestDataTable.Columns.Add("QuestItemUnk", typeof(Byte));
            QuestDataTable.Columns.Add("QuestItemID", typeof(UInt16));
            QuestDataTable.Columns.Add("QuestItemAmount", typeof(Byte));
            QuestDataTable.Columns.Add("HasPreQuest", typeof(Boolean));
            QuestDataTable.Columns.Add("PreQuestID", typeof(UInt16));
            QuestDataTable.Columns.Add("ClassSpecific", typeof(Boolean));
            QuestDataTable.Columns.Add("ClassID", typeof(Byte));

            for (Int32 Counter = 0; Counter < 5; Counter++)
            {
                QuestDataTable.Columns.Add(String.Format("Mob{0}IsActive", Counter), typeof(Boolean));
                QuestDataTable.Columns.Add(String.Format("Mob{0}IsMob", Counter), typeof(Boolean));
                QuestDataTable.Columns.Add(String.Format("Mob{0}ID", Counter), typeof(UInt16));
                QuestDataTable.Columns.Add(String.Format("Mob{0}HasToKill", Counter), typeof(Boolean));
                QuestDataTable.Columns.Add(String.Format("Mob{0}Amount", Counter), typeof(Byte));
            }

            for (Int32 Counter = 0; Counter < 10; Counter++)
            {
                QuestDataTable.Columns.Add(String.Format("Item{0}IsActive", Counter), typeof(Boolean));
                QuestDataTable.Columns.Add(String.Format("Item{0}Type", Counter), typeof(Byte));
                QuestDataTable.Columns.Add(String.Format("Item{0}ID", Counter), typeof(UInt16));
                QuestDataTable.Columns.Add(String.Format("Item{0}Amount", Counter), typeof(UInt16));
            }

            QuestDataTable.Columns.Add("MobDropCount", typeof(UInt32));

            for (Int32 Counter = 0; Counter < 10; Counter++)
            {
                QuestDataTable.Columns.Add(String.Format("MobDrop{0}Unk01", Counter), typeof(UInt32));
                QuestDataTable.Columns.Add(String.Format("MobDrop{0}MobID", Counter), typeof(UInt32));
                QuestDataTable.Columns.Add(String.Format("MobDrop{0}Unk02", Counter), typeof(UInt32));
                QuestDataTable.Columns.Add(String.Format("MobDrop{0}ItemID", Counter), typeof(UInt32));
                QuestDataTable.Columns.Add(String.Format("MobDrop{0}DropRate", Counter), typeof(UInt32));
                QuestDataTable.Columns.Add(String.Format("MobDrop{0}DropAmount", Counter), typeof(UInt32));
                QuestDataTable.Columns.Add(String.Format("MobDrop{0}Unk03", Counter), typeof(UInt32));
            }

            for (Int32 Counter = 0; Counter < 12; Counter++)
            {
                QuestDataTable.Columns.Add(String.Format("Reward{0}IsGiven", Counter), typeof(Byte));
                QuestDataTable.Columns.Add(String.Format("Reward{0}Type", Counter), typeof(Byte));
                QuestDataTable.Columns.Add(String.Format("Reward{0}Unk", Counter), typeof(UInt16));
                QuestDataTable.Columns.Add(String.Format("Reward{0}Value", Counter), typeof(UInt16));
                QuestDataTable.Columns.Add(String.Format("Reward{0}ItemCount", Counter), typeof(UInt16));
                QuestDataTable.Columns.Add(String.Format("Reward{0}Amount", Counter), typeof(UInt64));
            }

            QuestDataTable.Columns.Add("StartScript", typeof(String));
            QuestDataTable.Columns.Add("ActionScript", typeof(String));
            QuestDataTable.Columns.Add("FinishScript", typeof(String));

            for (Int32 Counter = 0; Counter < 6; Counter++) { QuestDataTable.Columns.Add(String.Format("UnkData{0}", Counter), typeof(String)); }

            QuestDataTable.Columns.Add("UnkRewardData", typeof(String));

            return QuestDataTable;
        }

        public static DataTable CreateDefaultQuestDataHK()
        {
            DataTable QuestDataTable = new DataTable();
            QuestDataTable.Columns.Add("ID", typeof(UInt16));
            QuestDataTable.Columns.Add("TitleID", typeof(UInt16));
            QuestDataTable.Columns.Add("DescriptionID", typeof(UInt16));
            QuestDataTable.Columns.Add("QuestGrade", typeof(Byte));
            QuestDataTable.Columns.Add("Repeatable", typeof(Boolean));
            QuestDataTable.Columns.Add("DailyQuest", typeof(Boolean));
            QuestDataTable.Columns.Add("Enabled", typeof(Boolean));
            QuestDataTable.Columns.Add("InstantAccept", typeof(Boolean));
            QuestDataTable.Columns.Add("IsLevelBased", typeof(Boolean));
            QuestDataTable.Columns.Add("MinLevel", typeof(Byte));
            QuestDataTable.Columns.Add("MaxLevel", typeof(Byte));
            QuestDataTable.Columns.Add("NeedStartingNPC", typeof(Boolean));
            QuestDataTable.Columns.Add("StartingNPCID", typeof(UInt16));
            QuestDataTable.Columns.Add("NeedItemForQuest", typeof(Boolean));
            QuestDataTable.Columns.Add("QuestItemID", typeof(UInt16));
            QuestDataTable.Columns.Add("QuestItemVanish", typeof(Boolean));
            QuestDataTable.Columns.Add("HasPreQuest", typeof(Boolean));
            QuestDataTable.Columns.Add("PreQuestID", typeof(UInt16));
            QuestDataTable.Columns.Add("ClassSpecific", typeof(Boolean));
            QuestDataTable.Columns.Add("ClassID", typeof(Byte));
            QuestDataTable.Columns.Add("InstantHandIn", typeof(Boolean));

            for (Int32 Counter = 0; Counter < 5; Counter++)
            {
                QuestDataTable.Columns.Add(String.Format("Mob{0}IsActive", Counter), typeof(Boolean));
                QuestDataTable.Columns.Add(String.Format("Mob{0}IsMob", Counter), typeof(Boolean));
                QuestDataTable.Columns.Add(String.Format("Mob{0}ID", Counter), typeof(UInt16));
                QuestDataTable.Columns.Add(String.Format("Mob{0}HasToKill", Counter), typeof(Boolean));
                QuestDataTable.Columns.Add(String.Format("Mob{0}Amount", Counter), typeof(Byte));
                QuestDataTable.Columns.Add(String.Format("Mob{0}Unk00", Counter), typeof(Byte));
                QuestDataTable.Columns.Add(String.Format("Mob{0}Unk01", Counter), typeof(Byte));
            }

            for (Int32 Counter = 0; Counter < 10; Counter++)
            {
                QuestDataTable.Columns.Add(String.Format("Item{0}IsActive", Counter), typeof(Boolean));
                QuestDataTable.Columns.Add(String.Format("Item{0}Type", Counter), typeof(Byte));
                QuestDataTable.Columns.Add(String.Format("Item{0}ID", Counter), typeof(UInt16));
                QuestDataTable.Columns.Add(String.Format("Item{0}Amount", Counter), typeof(UInt16));
            }

            QuestDataTable.Columns.Add("MobDropCount", typeof(UInt32));

            for (Int32 Counter = 0; Counter < 10; Counter++)
            {
                QuestDataTable.Columns.Add(String.Format("MobDrop{0}Unk01", Counter), typeof(UInt32));
                QuestDataTable.Columns.Add(String.Format("MobDrop{0}MobID", Counter), typeof(UInt32));
                QuestDataTable.Columns.Add(String.Format("MobDrop{0}Unk02", Counter), typeof(UInt32));
                QuestDataTable.Columns.Add(String.Format("MobDrop{0}ItemID", Counter), typeof(UInt32));
                QuestDataTable.Columns.Add(String.Format("MobDrop{0}DropRate", Counter), typeof(UInt32));
                QuestDataTable.Columns.Add(String.Format("MobDrop{0}DropAmount", Counter), typeof(UInt32));
                QuestDataTable.Columns.Add(String.Format("MobDrop{0}Unk03", Counter), typeof(UInt32));
            }

            for (Int32 Counter = 0; Counter < 12; Counter++)
            {
                QuestDataTable.Columns.Add(String.Format("Reward{0}IsGiven", Counter), typeof(Byte));
                QuestDataTable.Columns.Add(String.Format("Reward{0}Type", Counter), typeof(Byte));
                QuestDataTable.Columns.Add(String.Format("Reward{0}Unk", Counter), typeof(UInt16));
                QuestDataTable.Columns.Add(String.Format("Reward{0}Value", Counter), typeof(UInt16));
                QuestDataTable.Columns.Add(String.Format("Reward{0}ItemCount", Counter), typeof(UInt16));
                QuestDataTable.Columns.Add(String.Format("Reward{0}Amount", Counter), typeof(UInt64));
            }

            QuestDataTable.Columns.Add("StartScript", typeof(String));
            QuestDataTable.Columns.Add("ActionScript", typeof(String));
            QuestDataTable.Columns.Add("FinishScript", typeof(String));

            for (Int32 Counter = 0; Counter < 8; Counter++) { QuestDataTable.Columns.Add(String.Format("UnkData{0}", Counter), typeof(String)); }

            QuestDataTable.Columns.Add("UnkDropData", typeof(String));
            QuestDataTable.Columns.Add("UnkRewardData", typeof(String));

            return QuestDataTable;
        }
    }
}
