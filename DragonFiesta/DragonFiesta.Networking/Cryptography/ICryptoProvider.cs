namespace DragonFiesta.Networking.Cryptography
{
    public interface ICryptoProvider
    {
        byte[] Encrypt(byte[] pData, int pOffset, int pLength);

        byte[] Decrypt(byte[] pData, int pOffset, int pLength);
    }
}