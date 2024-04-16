using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ITKANSys_api.Auth
{
    /// <summary>
    /// Classe permettant la gestion du cryptage/décryptage
    /// </summary>
    public static class CryptageHelper
    {
        /// <summary>
        /// Méthode pour passer de tableau de Byte vers Hex 64
        /// </summary>
        /// <param name="ByteArray">Tableau de Byte</param>
        /// <returns></returns>
        public static string ByteArrayToBase64Hex(byte[] ByteArray)
        {
            StringBuilder hex = new StringBuilder(ByteArray.Length * 2);

            foreach (byte b in ByteArray)
                hex.AppendFormat("{0:x2}", b);

            return "0x" + hex.ToString();
        }

        /// <summary>
        /// Méthode pour passer de d'un Hex 64 vers un tableau de Byte
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static byte[] Base64StringToByteArray(string base64String)
        {
            base64String = base64String.Substring(2); // enlever "0x"
            int NumberChars = base64String.Length;
            byte[] bytes = new byte[NumberChars / 2];

            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(base64String.Substring(i, 2), 16);

            return bytes;
        }

        /// <summary>
        /// Function Cryptage AES ( Msg , Key)
        /// </summary>
        /// <param name="Msg">Texte à cryper</param>
        /// <param name="Key">Clé de cryptage</param>
        /// <returns></returns>
        private static byte[] AES_Encryption(byte[] Msg, byte[] Key)
        {
            byte[] encryptedBytes = null;

            //salt is generated randomly as an additional number to hash password or message in order o dictionary attack
            //against pre computed rainbow table
            //dictionary attack is a systematic way to test all of possibilities words in dictionary wheather or not is true?
            //to find decryption key
            //rainbow table is precomputed key for cracking password
            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.  == 16 bits
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(Key, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(Msg, 0, Msg.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        /// <summary>
        /// Function Décryptage AES ( Msg , Key)
        /// </summary>
        /// <param name="Msg"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        private static byte[] AES_Decryption(byte[] Msg, byte[] Key)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(Key, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(Msg, 0, Msg.Length);
                        cs.Close();
                    }

                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        /// <summary>
        /// Méthode publique à appeler pour décrypter
        /// Exemple d'appel : CryptageHelper.Decryption(CryptageHelper.Base64StringToByteArray(ma_valeur, ma_cle);
        /// </summary>
        /// <param name="Valeur"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string Decryption(Byte[] Valeur, string Key)
        {
            // Convert String to Byte
            byte[] KeyBytes = Encoding.UTF8.GetBytes(Key);
            KeyBytes = SHA256.Create().ComputeHash(KeyBytes);

            byte[] bytesDataDecrypted = AES_Decryption(Valeur, KeyBytes);
            string decryptionText = Encoding.UTF8.GetString(bytesDataDecrypted);

            return decryptionText;
        }

        /// <summary>
        /// Méthode publique à appeler pour crypter
        /// Exemple d'appel : CryptageHelper.ByteArrayToBase64Hex(CryptageHelper.Encryption(mon_champ, ma_cle))
        /// </summary>
        /// <param name="Valeur"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static Byte[] Encryption(string Valeur, string Key)
        {
            // Convert String to Byte
            byte[] ValeurBytes = Encoding.UTF8.GetBytes(Valeur);
            byte[] KeyBytes = Encoding.UTF8.GetBytes(Key);

            KeyBytes = SHA256.Create().ComputeHash(KeyBytes);
            byte[] bytesDataEncrypted = AES_Encryption(ValeurBytes, KeyBytes);

            return bytesDataEncrypted;
        }
    }
}
