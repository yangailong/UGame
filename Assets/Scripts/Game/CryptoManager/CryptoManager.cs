using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace UGame_Local
{
    public class CryptoManager
    {

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string EncryptStr(string key, string value)
        {
            try
            {
                Byte[] keyByte = Encoding.UTF8.GetBytes(key);
                Byte[] encrypt = Encoding.UTF8.GetBytes(value);
                var aes = Aes.Create();
                aes.Key = keyByte;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;

                Byte[] resultArray = aes.CreateEncryptor().TransformFinalBlock(encrypt, 0, encrypt.Length);

                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception e)
            {
                Debug.LogError(e);

                return null;
            }
        }


        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string DecryptStr(string key, string value)
        {
            try
            {
                Byte[] keyArray = Encoding.UTF8.GetBytes(key);
                Byte[] toEncryptArray = Convert.FromBase64String(value);
                var aes = Aes.Create();
                aes.Key = keyArray;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                Byte[] resultArray = aes.CreateDecryptor().TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return null;
            }
        }


        /// <summary>
        /// AES 算法加密(ECB模式) 将明文加密
        /// </summary>
        /// <param name="toEncryptArray">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>加密后base64编码的密文</returns>
        public static byte[] AesEncrypt(string key, byte[] toEncryptArray)
        {
            try
            {
                byte[] keyArray = Encoding.UTF8.GetBytes(key);
                var aes = Aes.Create();
                aes.Key = keyArray;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                return aes.CreateEncryptor().TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }


        /// <summary>
        /// AES 算法解密(ECB模式) 将密文base64解码进行解密，返回明文
        /// </summary>
        /// <param name="toDecryptArray">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static byte[] AesDecrypt(string key, byte[] toDecryptArray)
        {
            try
            {
                byte[] keyArray = Encoding.UTF8.GetBytes(key);

                var aes = Aes.Create();

                aes.Key = keyArray;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                aes.Padding = PaddingMode.PKCS7;
                return aes.CreateDecryptor().TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }



        public static string MD5Encrypt(string text)
        {
            var md5 = MD5.Create();

            Byte[] buffer = Encoding.Default.GetBytes(text);

            Byte[] MD5buffer = md5.ComputeHash(buffer);

            string ciphertext = BitConverter.ToString(MD5buffer, 4, 8);

            return ciphertext.Replace("-", "");
        }








    }
}

