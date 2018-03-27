using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Chat;

namespace DragonFiesta.Zone.Game.Command
{
    [GameCommandCategory("Note")]
    public class NOTE_COMMAND_HANDLER
    {

        [ZoneCommand("Map")]
        public static bool MapNote(ZoneCharacter Sender, string[] Parameters)
        {

            ZoneChat.MapNote(Sender.Map, StringExtensions.ToString(Parameters));


            return true;
        }

        [ZoneCommand("Server")]
        public static bool ServerNote(ZoneCharacter Sender, string[] Parameters)
        {
            ZoneChat.ServerNote(StringExtensions.ToString(Parameters));

            return true;
        }

        [ZoneCommand("Char")]
        public static bool CharacterNote(ZoneCharacter Sender, string[] Parameters)
        {


            if (Parameters.Length <= 0)
            {


                ZoneChat.CharacterNote(Sender, "Invalid Input Please Use &Note Char <Messages>");
                return false;
            }

            string charname = Parameters[0];
            if (!ZoneCharacterManager.Instance.GetCharacterByName(charname, out ZoneCharacter pChar))
            {
                ZoneChat.CharacterNote(Sender, $"Character {charname}  not found!");
                return false;
            }

            if (!pChar.LoginInfo.IsOnline)
            {
                ZoneChat.CharacterNote(Sender, $"Character { charname } Not online!");
                return false;
            }

            ZoneChat.CharacterNote(Sender, StringExtensions.ToString(Parameters));


            return true;
        }


    }
}
