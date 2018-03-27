using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Zone.Game;
using DragonFiesta.Zone.Network.Helpers;

namespace DragonFiesta.Zone.Network.FiestaHandler.Client
{
    [PacketHandlerClass(Handler15Type._Header)]
    public static class CH15Handler
    {
        [PacketHandler(Handler15Type.CMSG_MENU_SERVERMENU_ACK)]
        public static void CMSG_QUESTION_RESPONSE(ZoneSession sender, FiestaPacket packet)
        {
            if (!sender.Ingame || !packet.Read(out byte AnswerIndex))
            {
                SH04Helpers.SendZoneError(sender, ConnectionError.ClientManipulation);
                sender.Dispose();
                return;
            }

            if (sender.Character.Question != null)
            {
                if (sender.Character.Question.GetAnswerByID(AnswerIndex, out Answer answer))
                {
                    sender.Character.Question.HandleAnswer(answer);
                    sender.Character.SetQuestion(null);
                }
            }
        }
    }
}