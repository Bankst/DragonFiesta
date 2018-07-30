using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Zone.Game;

namespace DragonFiesta.Zone.Network.FiestaHandler.Server
{
    public static class SH15Handler
    {
		public static void SendQuestion(Question Question, ushort Distance)
        {
            using (var packet = new FiestaPacket(Handler15Type._Header, Handler15Type.SMSG_MENU_SERVERMENU_REQ))
            {
                packet.WriteString(Question.Text, 129);
                packet.Write<ushort>(Question.Character.MapObjectId);
                packet.Write<uint>(Question.Character.AreaInfo.Position.X);
                packet.Write<uint>(Question.Character.AreaInfo.Position.Y);

                packet.Write<ushort>(Distance);

                packet.Write<byte>(Question.Answers.Count);

                for (int i = 0; i < Question.Answers.Count; i++)
                {
                    var answer = Question.Answers[i];
                    packet.Write<byte>(answer.Index);
                    packet.WriteString(answer.Text, 32);
                }

                Question.Character.Session.SendPacket(packet);
            }
        }
	}
}