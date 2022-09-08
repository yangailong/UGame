using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace UGame_Remove
{
    public class CrypManager
    {

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string? EncryptStr(string key, string value)
        {
            try
            {
                Byte[] keyByte = Encoding.UTF8.GetBytes(key);
                Byte[] encrypt = Encoding.UTF8.GetBytes(value);
                var aes = Aes.Create();
                aes.Key = keyByte;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = aes.CreateEncryptor();

                Byte[] resultArray = cTransform.TransformFinalBlock(encrypt, 0, encrypt.Length);

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
        public static string? DecryptStr(string key, string value)
        {
            try
            {
                Byte[] keyArray = Encoding.UTF8.GetBytes(key);
                Byte[] toEncryptArray = Convert.FromBase64String(value);
                var aes = Aes.Create();
                aes.Key = keyArray;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = aes.CreateDecryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
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
        public static byte[]? AesEncrypt(string key, byte[] toEncryptArray)
        {
            try
            {
                byte[] keyArray = Encoding.UTF8.GetBytes(key);

                var aes = Aes.Create();
                aes.Key = keyArray;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = aes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return resultArray;
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
        public static byte[]? AesDecrypt(string key, byte[] toDecryptArray)
        {
            try
            {
                byte[] keyArray = Encoding.UTF8.GetBytes(key);

                var aes = Aes.Create();
                aes.Key = keyArray;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = aes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
                return resultArray;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }


        /// <summary>
        /// AES 算法加密(ECB模式) 无padding填充，用于分块解密
        /// </summary>
        /// <param name="toEncryptArray">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>加密后base64编码的密文</returns>
        public static byte[]? AesEncryptWithNoPadding(string key, byte[] toEncryptArray)
        {
            try
            {
                byte[] keyArray = Encoding.UTF8.GetBytes(key);

                var aes = Aes.Create();
                aes.Key = keyArray;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;

                ICryptoTransform cTransform = aes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return resultArray;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }


        /// <summary>
        /// AES 算法解密(ECB模式) 无padding填充，用于分块解密
        /// </summary>
        /// <param name="toDecryptArray">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static byte[]? AesDecryptWithNoPadding(string key, byte[] toDecryptArray)
        {
            try
            {
                byte[] keyArray = Encoding.UTF8.GetBytes(key);

                var aes = Aes.Create();
                aes.Key = keyArray;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;

                ICryptoTransform cTransform = aes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
                return resultArray;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }


    }
}

