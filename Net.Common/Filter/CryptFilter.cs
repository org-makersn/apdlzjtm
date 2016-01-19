using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Net.Common.Filter
{
    public static class CryptFilter
    {
        private static string EncryptionKey = "[W][E][0][A][R][E][9][M][A][K][E][R][S][8][!][^^][_][-][^][0]";
        private static byte[] byteKeys = { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };

        /// <summary>
        /// encode 
        /// </summary>
        /// <param name="clearText"></param>
        /// <returns></returns>
        public static string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, byteKeys);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        /// <summary>
        /// decode
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public static string Decrypt(string cipherText)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, byteKeys);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        #region DES암복호화
        // Public Function
        public static string DESEncrypt(string sValue)
        {
            return DesEncrypt(sValue, EncryptionKey);
        }

        // Public Function
        public static string DESDecrypt(string sValue) // 복호화
        {
            return DesDecrypt(sValue, EncryptionKey);
        }

        //문자열 암호화
        private static string DesEncrypt(string sValue, string sPubKey)
        {
            //키 유효성 검사
            byte[] btKey = ConvertStringToByteArrayA(sPubKey);

            //키가 8Byte가 아니면 예외발생
            if (btKey.Length != 8)
            {
                throw (new Exception("Invalid key. Key length must be 8 byte."));
            }

            //소스 문자열
            byte[] btSrc = ConvertStringToByteArray(sValue);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            des.Key = btKey;
            des.IV = btKey;

            ICryptoTransform desencrypt = des.CreateEncryptor();

            MemoryStream ms = new MemoryStream();

            CryptoStream cs = new CryptoStream(ms, desencrypt,
             CryptoStreamMode.Write);

            cs.Write(btSrc, 0, btSrc.Length);
            cs.FlushFinalBlock();


            byte[] btEncData = ms.ToArray();

            return (ConvertByteArrayToStringB(btEncData));
        }//end of func DesEncrypt

        //문자열 복호화
        private static string DesDecrypt(string sValue, string sPubKey)
        {
            //키 유효성 검사
            byte[] btKey = ConvertStringToByteArrayA(sPubKey);

            //키가 8Byte가 아니면 예외발생
            if (btKey.Length != 8)
            {
                throw (new Exception("Invalid key. Key length must be 8 byte."));
            }


            byte[] btEncData = ConvertStringToByteArrayB(sValue);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            des.Key = btKey;
            des.IV = btKey;

            ICryptoTransform desdecrypt = des.CreateDecryptor();

            MemoryStream ms = new MemoryStream();

            CryptoStream cs = new CryptoStream(ms, desdecrypt,
             CryptoStreamMode.Write);

            cs.Write(btEncData, 0, btEncData.Length);

            cs.FlushFinalBlock();

            byte[] btSrc = ms.ToArray();


            return (ConvertByteArrayToString(btSrc));

        }
        //end of func DesDecrypt

        #endregion //DES암복호화

        //문자열->유니코드 바이트 배열
        private static Byte[] ConvertStringToByteArray(String s)
        {
            return (new UnicodeEncoding()).GetBytes(s);
        }

        //유니코드 바이트 배열->문자열
        private static string ConvertByteArrayToString(byte[] b)
        {
            return (new UnicodeEncoding()).GetString(b, 0, b.Length);
        }

        //문자열->안시 바이트 배열
        private static Byte[] ConvertStringToByteArrayA(String s)
        {
            return (new ASCIIEncoding()).GetBytes(s);
        }

        //안시 바이트 배열->문자열
        private static string ConvertByteArrayToStringA(byte[] b)
        {
            return (new ASCIIEncoding()).GetString(b, 0, b.Length);
        }

        //문자열->Base64 바이트 배열
        private static Byte[] ConvertStringToByteArrayB(String s)
        {
            return Convert.FromBase64String(s);
        }

        //Base64 바이트 배열->문자열
        private static string ConvertByteArrayToStringB(byte[] b)
        {
            return Convert.ToBase64String(b);
        }
    }
}
