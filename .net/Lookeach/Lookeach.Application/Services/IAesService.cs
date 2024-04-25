namespace Lookeach.Application.Services
{
    public interface IAesService
    {
        byte[] Encrypt(string plainText);
        string Decrypt(byte[] cipherText);
    }
}
