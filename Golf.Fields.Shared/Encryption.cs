using System.Security.Cryptography;
using System.Text;
using Golf.Fields.Shared;

namespace Golf.Fields.Shared
{

    public static class Encryption
    {
        private static byte[] _salt = new byte[] { 42, 53, 177, 63, 180, 200, 191, 2, 34, 6 };

        private const string CTK = "cbqkyv-aucpep-neHye2";


        /// <summary>
        /// Encrypt the given string using the default key.
        /// </summary>
        /// <param name="strToEncrypt">The string to be encrypted.</param>
        /// <returns>The encrypted string in base 64 string form</returns>
        public static string Encrypt(string strToEncrypt)
        {
            return Encrypt(strToEncrypt, CTK);
        }


        /// <summary>
        /// Decrypt the given string using the default key.
        /// </summary>
        /// <param name="strEncrypted">The string to be decrypted.</param>
        /// <returns>The decrypted string in base 64 string form</returns>
        public static string? Decrypt(string? strEncrypted)
        {
            return Decrypt(strEncrypted, CTK);
        }


        /// <summary>
        /// Encrypt data with a password
        /// </summary>
        /// <param name="plainData"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Encrypt(string plainData, string password)
        {
            if (string.IsNullOrWhiteSpace(plainData))
                throw new EncryptionException(Resources.Messages.EmptyStringCanNotBeEncrypted);

            byte[] passBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] bytesData = System.Text.Encoding.UTF8.GetBytes(plainData);
            byte[] encrypted = Encrypt(bytesData, passBytes);
            var encryptedStr = Convert.ToBase64String(encrypted);
            return encryptedStr;
        }


        /// <summary>
        /// Encrypt bytes to byte array with a password
        /// </summary>
        /// <param name="bytesToBeEncrypted"></param>
        /// <param name="passwordBytes"></param>
        /// <returns></returns>
        public static byte[]? Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            try
            {
                byte[]? encryptedBytes = null;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (var AES = Aes.Create("AesManaged"))
                    {
                        if (AES != null)
                        {
                            AES.KeySize = 256;
                            AES.BlockSize = 128;
                            var key = new Rfc2898DeriveBytes(passwordBytes, _salt, 1000);
                            AES.Key = key.GetBytes(AES.KeySize / 8);
                            AES.IV = key.GetBytes(AES.BlockSize / 8);
                            AES.Mode = CipherMode.CBC;
                            using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                                cs.Close();
                            }
                            encryptedBytes = ms.ToArray();
                        }
                    }
                }
                return encryptedBytes;
            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                Console.WriteLine(message);
                throw;
            }
        }


        /// <summary>
        /// Decrypt data with a given passord
        /// </summary>
        /// <param name="encrypted"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string? Decrypt(string? encrypted, string password)
        {
            if (string.IsNullOrWhiteSpace(encrypted))
                throw new ArgumentNullException(nameof(encrypted));

            var encryptedData = Convert.FromBase64String(encrypted);
            byte[] passBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[]? decrypted = Decrypt(encryptedData, passBytes);

            if (decrypted == null)
            {
                return null;
            }
            else
            {
                var decryptedStr = System.Text.UTF8Encoding.UTF8.GetString(decrypted);
                return decryptedStr;
            }
        }



        /// <summary>
        /// Decrypt bytes to byte array with password
        /// </summary>
        /// <param name="bytesToBeDecrypted"></param>
        /// <param name="passwordBytes"></param>
        /// <returns></returns>
        public static byte[]? Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[]? decryptedBytes = null;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var AES = Aes.Create("AesManaged"))
                    {
                        if (AES != null)
                        {
                            AES.KeySize = 256;
                            AES.BlockSize = 128;
                            var key = new Rfc2898DeriveBytes(passwordBytes, _salt, 1000);
                            AES.Key = key.GetBytes(AES.KeySize / 8);
                            AES.IV = key.GetBytes(AES.BlockSize / 8);
                            AES.Mode = CipherMode.CBC;

                            using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                                cs.Close();
                            }
                            decryptedBytes = ms.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                Console.WriteLine(message);
                throw;
            }

            return decryptedBytes;
        }


        /// <summary>
        /// Get hash of a string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetSHA256HashData(string data)
        {
            string result = string.Empty;
            using (SHA256 sha = SHA256.Create())
            {
                //convert the input text to array of bytes
                byte[] hashData = sha.ComputeHash(Encoding.UTF8.GetBytes(data));
                result = Convert.ToBase64String(hashData);
            }
            // return hexadecimal string
            return result;
        }
    }

}

