using DragonFiesta.Messages;

namespace DragonFiesta.Networking.Cryptography
{
    public sealed class InterCryptoProvider : ICryptoProvider
    {
        public InterCryptoProvider(string EncryptKey)
        {
        }

        public byte[] Decrypt(byte[] pData, int pOffset, int pLength)
        {
            return pData;
        }

        public byte[] Encrypt(IMessage pMessage)
        {
            byte[] MessageBytes = pMessage.MessageToBytes();

            return MessageBytes;
        }

        public byte[] Encrypt(FiestaPacket pWriter)
        {
            byte[] data = pWriter.GetPacketBytes();
            int offset = 1;
            if (data[0] == 0)
            {
                offset = 3;
            }

            return Encrypt(data, offset, data.Length);
        }

        public byte[] Encrypt(byte[] pData, int pOffset, int pLength)
        {
            return pData;
        }
    }
}