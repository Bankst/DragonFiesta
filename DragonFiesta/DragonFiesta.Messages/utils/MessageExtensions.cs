using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DragonFiesta.Messages
{
    public static class MessageExtensions
    {
        public static IMessage BytesToMessage<IMessage>(this byte[] _byteArray)
        {
            try
            {
                IMessage ReturnValue;
                using (var _MemoryStream = new MemoryStream(_byteArray))
                {
                    IFormatter _BinaryFormatter = new BinaryFormatter();
                    ReturnValue = (IMessage)_BinaryFormatter.Deserialize(_MemoryStream);
                }
                return ReturnValue;
            }
            catch (Exception ex)
            {
                GameLog.Write(ex, "Packet Error");
                return default(IMessage);
            }

        }

        public static string ToString(this IMessage msg)
        {
            string MyString = string.Empty;

            foreach (var StickString in msg.GetType()
                                 .GetFields(BindingFlags.Instance |
                       BindingFlags.NonPublic |
                       BindingFlags.Public))
            {
                MyString += StickString.Name + " Value " + StickString.GetValue(msg) + "\n";
            }
            return MyString;
        }

        public static byte[] MessageToBytes(this IMessage Message)
        {
            try
            {
                byte[] bytes;
                using (var _MemoryStream = new MemoryStream())
                {
                    IFormatter _BinaryFormatter = new BinaryFormatter();
                    _BinaryFormatter.Serialize(_MemoryStream, Message);
                    bytes = _MemoryStream.ToArray();

                    byte[] newData = new byte[bytes.Length + 4];

                    byte[] BodySize = BitConverter.GetBytes(bytes.Length);

                    //LenghtBytes
                    newData[0] = BodySize[0];
                    newData[1] = BodySize[1];
                    newData[3] = BodySize[2];
                    newData[4] = BodySize[3];

                    Buffer.BlockCopy(bytes, 0, newData, 4, bytes.Length);

                    return newData;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"invalid Data Can't Convert message to byte[] type of object { Message.GetType() } : {ex} ");
            }
        }
    }
}