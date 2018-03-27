using System;

namespace DragonFiesta.Game.Attributes
{
    public class GameCommandAttribute : Attribute
    {
        public string Command { get; private set; }

        public GameCommandType CmdType { get; private set; }

        public GameCommandAttribute(string pCommand, GameCommandType pCmdType)
        {
            CmdType = pCmdType;
            Command = pCommand;
        }
    }
}