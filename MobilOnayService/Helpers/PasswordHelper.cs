using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MobilOnayService.Helpers
{
    public static class PasswordHelper
    {
        readonly static string ClassName = "MobilOnayService.Helpers.PasswordHelper";
        const string Key = "sfimobile!=2020";

        public static string Encrypt(string plainText)
        {
            try
            {
                RijndaelManaged RijndaelCipher = new RijndaelManaged();

                byte[] Salt = Encoding.ASCII.GetBytes(Key.Length.ToString());

                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Key, Salt);
                ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(16), SecretKey.GetBytes(16));
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

                cryptoStream.Write(System.Text.Encoding.Unicode.GetBytes(plainText), 0, System.Text.Encoding.Unicode.GetBytes(plainText).Length);
                cryptoStream.FlushFinalBlock();

                byte[] CipherBytes = memoryStream.ToArray();

                memoryStream.Close();
                cryptoStream.Close();

                string EncryptedData = System.Convert.ToBase64String(CipherBytes);

                return EncryptedData;
            }
            catch (System.Exception ex)
            {
                throw ExceptionHelper.Throw(ex, ClassName, "Encrypt");
            }
        }

        public static string Decrypt(string encryptedText)
        {
            try
            {
                encryptedText = encryptedText.Replace(" ", "+");

                RijndaelManaged RijndaelCipher = new RijndaelManaged();
                byte[] EncryptedData = System.Convert.FromBase64String(encryptedText);
                byte[] Salt = Encoding.ASCII.GetBytes(Key.Length.ToString());
                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Key, Salt);

                ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(16), SecretKey.GetBytes(16));
                MemoryStream memoryStream = new MemoryStream(EncryptedData);

                CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

                byte[] PlainText = new byte[EncryptedData.Length];

                int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);
                memoryStream.Close();
                cryptoStream.Close();

                string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
                return DecryptedData;
            }
            catch (System.Exception ex)
            {
                throw ExceptionHelper.Throw(ex, ClassName, "Decrypt");
            }
        }
    }
}
