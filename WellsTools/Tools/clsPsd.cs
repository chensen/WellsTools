using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Collections;
using System.Text.RegularExpressions;
#pragma warning disable 0618
//使用RSA加密解密方式的请注意：需要配套使用RSA密钥生成工具『 http://download.csdn.net/detail/downiis6/9464639 』。

namespace Wells.Tools
{
    /// <summary>
    /// 密码加密解密操作相关类
    /// </summary>
    public static class clsPsd
    {
        #region MD5 加密

        /// <summary>
        /// MD5加密
        /// </summary>
        public static string md532(this string source)
        {
            if (String.IsNullOrEmpty(source)) return null;
            var encoding = Encoding.UTF8;
            MD5 md5 = MD5.Create();
            return hashAlgorithmBase(md5, source, encoding);
        }

        /// <summary>
        /// 加盐MD5加密
        /// </summary>
        public static string md532Salt(this string source, string salt)
        {
            return String.IsNullOrEmpty(salt) ? source.md532() : (source + "『" + salt + "』").md532();
        }

        #endregion

        #region SHA 加密

        /// <summary>
        /// SHA1 加密
        /// </summary>
        public static string sha1(this string source)
        {
            if (String.IsNullOrEmpty(source)) return null;
            var encoding = Encoding.UTF8;
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            return hashAlgorithmBase(sha1, source, encoding);
        }

        /// <summary>
        /// SHA256 加密
        /// </summary>
        public static string sha256(this string source)
        {
            if (String.IsNullOrEmpty(source)) return null;
            var encoding = Encoding.UTF8;
            SHA256 sha256 = new SHA256Managed();
            return hashAlgorithmBase(sha256, source, encoding);
        }

        /// <summary>
        /// SHA512 加密
        /// </summary>
        public static string sha512(this string source)
        {
            if (String.IsNullOrEmpty(source)) return null;
            var encoding = Encoding.UTF8;
            SHA512 sha512 = new SHA512Managed();
            return hashAlgorithmBase(sha512, source, encoding);
        }

        #endregion

        #region HMAC 加密

        /// <summary>
        /// HmacSha1 加密
        /// </summary>
        public static string hmacSha1(this string source, string keyVal)
        {
            if (String.IsNullOrEmpty(source)) return null;
            var encoding = Encoding.UTF8;
            byte[] keyStr = encoding.GetBytes(keyVal);
            HMACSHA1 hmacSha1 = new HMACSHA1(keyStr);
            return hashAlgorithmBase(hmacSha1, source, encoding);
        }

        /// <summary>
        /// HmacSha256 加密
        /// </summary>
        public static string hmacSha256(this string source, string keyVal)
        {
            if (String.IsNullOrEmpty(source)) return null;
            var encoding = Encoding.UTF8;
            byte[] keyStr = encoding.GetBytes(keyVal);
            HMACSHA256 hmacSha256 = new HMACSHA256(keyStr);
            return hashAlgorithmBase(hmacSha256, source, encoding);
        }

        /// <summary>
        /// HmacSha384 加密
        /// </summary>
        public static string hmacSha384(this string source, string keyVal)
        {
            if (String.IsNullOrEmpty(source)) return null;
            var encoding = Encoding.UTF8;
            byte[] keyStr = encoding.GetBytes(keyVal);
            HMACSHA384 hmacSha384 = new HMACSHA384(keyStr);
            return hashAlgorithmBase(hmacSha384, source, encoding);
        }

        /// <summary>
        /// HmacSha512 加密
        /// </summary>
        public static string hmacSha512(this string source, string keyVal)
        {
            if (String.IsNullOrEmpty(source)) return null;
            var encoding = Encoding.UTF8;
            byte[] keyStr = encoding.GetBytes(keyVal);
            HMACSHA512 hmacSha512 = new HMACSHA512(keyStr);
            return hashAlgorithmBase(hmacSha512, source, encoding);
        }

        /// <summary>
        /// HmacMd5 加密
        /// </summary>
        public static string hmacMd5(this string source, string keyVal)
        {
            if (String.IsNullOrEmpty(source)) return null;
            var encoding = Encoding.UTF8;
            byte[] keyStr = encoding.GetBytes(keyVal);
            HMACMD5 hmacMd5 = new HMACMD5(keyStr);
            return hashAlgorithmBase(hmacMd5, source, encoding);
        }

        /// <summary>
        /// HmacRipeMd160 加密
        /// </summary>
        public static string hmacRipeMd160(this string source, string keyVal)
        {
            if (String.IsNullOrEmpty(source)) return null;
            var encoding = Encoding.UTF8;
            byte[] keyStr = encoding.GetBytes(keyVal);
            HMACRIPEMD160 hmacRipeMd160 = new HMACRIPEMD160(keyStr);
            return hashAlgorithmBase(hmacRipeMd160, source, encoding);
        }

        #endregion

        #region AES 加密解密

        /// <summary>  
        /// AES加密  
        /// </summary>  
        /// <param name="source">待加密字段</param>  
        /// <param name="keyVal">密钥值</param>  
        /// <param name="ivVal">加密辅助向量</param> 
        /// <returns></returns>  
        public static string aesStr(this string source, string keyVal, string ivVal)
        {
            var encoding = Encoding.UTF8;
            byte[] btKey = keyVal.FormatByte(encoding);
            byte[] btIv = ivVal.FormatByte(encoding);
            byte[] byteArray = encoding.GetBytes(source);
            string encrypt;
            Rijndael aes = Rijndael.Create();
            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(btKey, btIv), CryptoStreamMode.Write))
                {
                    cStream.Write(byteArray, 0, byteArray.Length);
                    cStream.FlushFinalBlock();
                    encrypt = Convert.ToBase64String(mStream.ToArray());
                }
            }
            aes.Clear();
            return encrypt;
        }

        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="source">待加密字段</param>  
        /// <param name="keyVal">密钥值</param>  
        /// <param name="ivVal">加密辅助向量</param>  
        /// <returns></returns>  
        public static string unAesStr(this string source, string keyVal, string ivVal)
        {
            var encoding = Encoding.UTF8;
            byte[] btKey = keyVal.FormatByte(encoding);
            byte[] btIv = ivVal.FormatByte(encoding);
            byte[] byteArray = Convert.FromBase64String(source);
            string decrypt;
            Rijndael aes = Rijndael.Create();
            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(btKey, btIv), CryptoStreamMode.Write))
                {
                    cStream.Write(byteArray, 0, byteArray.Length);
                    cStream.FlushFinalBlock();
                    decrypt = encoding.GetString(mStream.ToArray());
                }
            }
            aes.Clear();
            return decrypt;
        }

        /// <summary>  
        /// AES Byte类型 加密  
        /// </summary>  
        /// <param name="data">待加密明文</param>  
        /// <param name="keyVal">密钥值</param>  
        /// <param name="ivVal">加密辅助向量</param>  
        /// <returns></returns>  
        public static byte[] aesByte(this byte[] data, string keyVal, string ivVal)
        {
            byte[] bKey = new byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(keyVal.PadRight(bKey.Length)), bKey, bKey.Length);
            byte[] bVector = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(ivVal.PadRight(bVector.Length)), bVector, bVector.Length);
            byte[] cryptograph;
            Rijndael aes = Rijndael.Create();
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bVector), CryptoStreamMode.Write))
                    {
                        cStream.Write(data, 0, data.Length);
                        cStream.FlushFinalBlock();
                        cryptograph = mStream.ToArray();
                    }
                }
            }
            catch
            {
                cryptograph = null;
            }
            return cryptograph;
        }

        /// <summary>  
        /// AES Byte类型 解密  
        /// </summary>  
        /// <param name="data">待解密明文</param>  
        /// <param name="keyVal">密钥值</param>  
        /// <param name="ivVal">加密辅助向量</param> 
        /// <returns></returns>  
        public static byte[] unAesByte(this byte[] data, string keyVal, string ivVal)
        {
            byte[] bKey = new byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(keyVal.PadRight(bKey.Length)), bKey, bKey.Length);
            byte[] bVector = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(ivVal.PadRight(bVector.Length)), bVector, bVector.Length);
            byte[] original;
            Rijndael aes = Rijndael.Create();
            try
            {
                using (MemoryStream mStream = new MemoryStream(data))
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bVector), CryptoStreamMode.Read))
                    {
                        using (MemoryStream originalMemory = new MemoryStream())
                        {
                            byte[] buffer = new byte[1024];
                            int readBytes;
                            while ((readBytes = cStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                originalMemory.Write(buffer, 0, readBytes);
                            }

                            original = originalMemory.ToArray();
                        }
                    }
                }
            }
            catch
            {
                original = null;
            }
            return original;
        }

        #endregion

        #region RSA 加密解密

        //密钥对，请配合密钥生成工具使用『 http://download.csdn.net/detail/downiis6/9464639 』
        private const string PublicRsaKey = @"pubKey";
        private const string PrivateRsaKey = @"priKey";

        /// <summary>
        /// RSA 加密
        /// </summary>
        public static string rsa(this string source)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PublicRsaKey);
            var cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(source), true);
            return Convert.ToBase64String(cipherbytes);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        public static string unRsa(this string source)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PrivateRsaKey);
            var cipherbytes = rsa.Decrypt(Convert.FromBase64String(source), true);
            return Encoding.UTF8.GetString(cipherbytes);
        }

        #endregion

        #region DES 加密解密

        /// <summary>
        /// DES 加密
        /// </summary>
        public static string des(this string source, string keyVal, string ivVal)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(source);
                var des = new DESCryptoServiceProvider { Key = Encoding.ASCII.GetBytes(keyVal.Length > 8 ? keyVal.Substring(0, 8) : keyVal), IV = Encoding.ASCII.GetBytes(ivVal.Length > 8 ? ivVal.Substring(0, 8) : ivVal) };
                var desencrypt = des.CreateEncryptor();
                byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return BitConverter.ToString(result);
            }
            catch { return "转换出错！"; }
        }

        /// <summary>
        /// DES 解密
        /// </summary>
        public static string unDes(this string source, string keyVal, string ivVal)
        {
            try
            {
                string[] sInput = source.Split("-".ToCharArray());
                byte[] data = new byte[sInput.Length];
                for (int i = 0; i < sInput.Length; i++)
                {
                    data[i] = byte.Parse(sInput[i], NumberStyles.HexNumber);
                }
                var des = new DESCryptoServiceProvider { Key = Encoding.ASCII.GetBytes(keyVal.Length > 8 ? keyVal.Substring(0, 8) : keyVal), IV = Encoding.ASCII.GetBytes(ivVal.Length > 8 ? ivVal.Substring(0, 8) : ivVal) };
                var desencrypt = des.CreateDecryptor();
                byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return Encoding.UTF8.GetString(result);
            }
            catch { return "解密出错！"; }
        }

        #endregion

        #region TripleDES 加密解密

        /// <summary>
        /// DES3 加密
        /// </summary>
        public static string des3(this string source, string keyVal)
        {
            try
            {
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider
                {
                    Key = keyVal.FormatByte(Encoding.UTF8),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] btArray = Encoding.UTF8.GetBytes(source);
                    try
                    {
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(btArray, 0, btArray.Length);
                            cs.FlushFinalBlock();
                        }
                        return ms.ToArray().Bytes2Str();
                    }
                    catch { return source; }
                }
            }
            catch
            {
                return "TripleDES加密出现错误";
            }
        }

        /// <summary>
        /// DES3 解密
        /// </summary>
        public static string unDes3(this string source, string keyVal)
        {
            try
            {
                byte[] byArray = source.Str2Bytes();
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider
                {
                    Key = keyVal.FormatByte(Encoding.UTF8),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(byArray, 0, byArray.Length);
                        cs.FlushFinalBlock();
                        cs.Close();
                        ms.Close();
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch
            {
                return "TripleDES解密出现错误";
            }
        }

        #endregion

        #region BASE64 加密解密

        /// <summary>
        /// BASE64 加密
        /// </summary>
        /// <param name="source">待加密字段</param>
        /// <returns></returns>
        public static string base64(this string source)
        {
            var btArray = Encoding.UTF8.GetBytes(source);
            return Convert.ToBase64String(btArray, 0, btArray.Length);
        }

        /// <summary>
        /// BASE64 解密
        /// </summary>
        /// <param name="source">待解密字段</param>
        /// <returns></returns>
        public static string unBase64(this string source)
        {
            var btArray = Convert.FromBase64String(source);
            return Encoding.UTF8.GetString(btArray);
        }

        #endregion

        #region 内部方法
		 
        /// <summary>
        /// 转成数组
        /// </summary>
        private static byte[] Str2Bytes(this string source)
        {
            source = source.Replace(" ", "");
            byte[] buffer = new byte[source.Length / 2];
            for (int i = 0; i < source.Length; i += 2) buffer[i / 2] = Convert.ToByte(source.Substring(i, 2), 16);
            return buffer;
        }

        /// <summary>
        /// 转换成字符串
        /// </summary>
        private static string Bytes2Str(this IEnumerable<byte> source, string formatStr = "{0:X2}")
        {
            StringBuilder pwd = new StringBuilder();
            foreach (byte btStr in source) { pwd.AppendFormat(formatStr, btStr); }
            return pwd.ToString();
        }

        private static byte[] FormatByte(this string strVal, Encoding encoding)
        {
            return encoding.GetBytes(strVal.base64().Substring(0, 16).ToUpper());
        }

        /// <summary>
        /// HashAlgorithm 加密统一方法
        /// </summary>
        private static string hashAlgorithmBase(HashAlgorithm hashAlgorithmObj, string source, Encoding encoding)
        {
            byte[] btStr = encoding.GetBytes(source);
            byte[] hashStr = hashAlgorithmObj.ComputeHash(btStr);
            return hashStr.Bytes2Str();
        }

        #endregion
    }

    /// <summary>
    /// DES加密/解密类。
    /// </summary>
    public class DESEncrypt
    {
        public DESEncrypt()
        {
        }
        
        #region 加密

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, "MATICSOFT");
        }
        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }
        #endregion

        #region 解密


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt(string Text)
        {
            return Decrypt(Text, "MATICSOFT");
        }
        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion
    }

    /// <summary>
    /// Encrypt 的摘要说明。
    /// </summary>
    public class DEncrypt
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public DEncrypt()
        {

        }

        #region 使用 缺省密钥字符串 加密/解密string

        /// <summary>
        /// 使用缺省密钥字符串加密string
        /// </summary>
        /// <param name="original">明文</param>
        /// <returns>密文</returns>
        public static string Encrypt(string original)
        {
            return Encrypt(original, "MATICSOFT");
        }
        /// <summary>
        /// 使用缺省密钥字符串解密string
        /// </summary>
        /// <param name="original">密文</param>
        /// <returns>明文</returns>
        public static string Decrypt(string original)
        {
            return Decrypt(original, "MATICSOFT", System.Text.Encoding.Default);
        }

        #endregion

        #region 使用 给定密钥字符串 加密/解密string
        /// <summary>
        /// 使用给定密钥字符串加密string
        /// </summary>
        /// <param name="original">原始文字</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码方案</param>
        /// <returns>密文</returns>
        public static string Encrypt(string original, string key)
        {
            byte[] buff = System.Text.Encoding.Default.GetBytes(original);
            byte[] kb = System.Text.Encoding.Default.GetBytes(key);
            return Convert.ToBase64String(Encrypt(buff, kb));
        }
        /// <summary>
        /// 使用给定密钥字符串解密string
        /// </summary>
        /// <param name="original">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static string Decrypt(string original, string key)
        {
            return Decrypt(original, key, System.Text.Encoding.Default);
        }

        /// <summary>
        /// 使用给定密钥字符串解密string,返回指定编码方式明文
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码方案</param>
        /// <returns>明文</returns>
        public static string Decrypt(string encrypted, string key, Encoding encoding)
        {
            byte[] buff = Convert.FromBase64String(encrypted);
            byte[] kb = System.Text.Encoding.Default.GetBytes(key);
            return encoding.GetString(Decrypt(buff, kb));
        }
        #endregion

        #region 使用 缺省密钥字符串 加密/解密/byte[]
        /// <summary>
        /// 使用缺省密钥字符串解密byte[]
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static byte[] Decrypt(byte[] encrypted)
        {
            byte[] key = System.Text.Encoding.Default.GetBytes("MATICSOFT");
            return Decrypt(encrypted, key);
        }
        /// <summary>
        /// 使用缺省密钥字符串加密
        /// </summary>
        /// <param name="original">原始数据</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static byte[] Encrypt(byte[] original)
        {
            byte[] key = System.Text.Encoding.Default.GetBytes("MATICSOFT");
            return Encrypt(original, key);
        }
        #endregion

        #region  使用 给定密钥 加密/解密/byte[]

        /// <summary>
        /// 生成MD5摘要
        /// </summary>
        /// <param name="original">数据源</param>
        /// <returns>摘要</returns>
        public static byte[] MakeMD5(byte[] original)
        {
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            byte[] keyhash = hashmd5.ComputeHash(original);
            hashmd5 = null;
            return keyhash;
        }

        /// <summary>
        /// 使用给定密钥加密
        /// </summary>
        /// <param name="original">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static byte[] Encrypt(byte[] original, byte[] key)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = MakeMD5(key);
            des.Mode = CipherMode.ECB;

            return des.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
        }

        /// <summary>
        /// 使用给定密钥解密数据
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static byte[] Decrypt(byte[] encrypted, byte[] key)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = MakeMD5(key);
            des.Mode = CipherMode.ECB;

            return des.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
        }

        #endregion
    }

    /// <summary>
	/// 加密解密实用类。
	/// </summary>
	public class Encrypt
    {
        //密钥
        private static byte[] arrDESKey = new byte[] { 42, 16, 93, 156, 78, 4, 218, 32 };
        private static byte[] arrDESIV = new byte[] { 55, 103, 246, 79, 36, 99, 167, 3 };

        /// <summary>
        /// 加密。
        /// </summary>
        /// <param name="m_Need_Encode_String"></param>
        /// <returns></returns>
        public static string Encode(string m_Need_Encode_String)
        {
            if (m_Need_Encode_String == null)
            {
                throw new Exception("Error: \n源字符串为空！！");
            }
            DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
            MemoryStream objMemoryStream = new MemoryStream();
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateEncryptor(arrDESKey, arrDESIV), CryptoStreamMode.Write);
            StreamWriter objStreamWriter = new StreamWriter(objCryptoStream);
            objStreamWriter.Write(m_Need_Encode_String);
            objStreamWriter.Flush();
            objCryptoStream.FlushFinalBlock();
            objMemoryStream.Flush();
            return Convert.ToBase64String(objMemoryStream.GetBuffer(), 0, (int)objMemoryStream.Length);
        }

        /// <summary>
        /// 解密。
        /// </summary>
        /// <param name="m_Need_Encode_String"></param>
        /// <returns></returns>
        public static string Decode(string m_Need_Encode_String)
        {
            if (m_Need_Encode_String == null)
            {
                throw new Exception("Error: \n源字符串为空！！");
            }
            DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
            byte[] arrInput = Convert.FromBase64String(m_Need_Encode_String);
            MemoryStream objMemoryStream = new MemoryStream(arrInput);
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateDecryptor(arrDESKey, arrDESIV), CryptoStreamMode.Read);
            StreamReader objStreamReader = new StreamReader(objCryptoStream);
            return objStreamReader.ReadToEnd();
        }

        /// <summary>
        /// md5
        /// </summary>
        /// <param name="encypStr"></param>
        /// <returns></returns>
        public static string Md5(string encypStr)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();
            byte[] inputBye;
            byte[] outputBye;
            inputBye = System.Text.Encoding.ASCII.GetBytes(encypStr);
            outputBye = m5.ComputeHash(inputBye);
            retStr = Convert.ToBase64String(outputBye);
            return (retStr);
        }
    }

    /// <summary>
	/// 得到随机安全码（哈希加密）。
	/// </summary>
	public class HashEncode
    {
        public HashEncode()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 得到随机哈希加密字符串
        /// </summary>
        /// <returns></returns>
        public static string GetSecurity()
        {
            string Security = HashEncoding(GetRandomValue());
            return Security;
        }
        /// <summary>
        /// 得到一个随机数值
        /// </summary>
        /// <returns></returns>
        public static string GetRandomValue()
        {
            Random Seed = new Random();
            string RandomVaule = Seed.Next(1, int.MaxValue).ToString();
            return RandomVaule;
        }
        /// <summary>
        /// 哈希加密一个字符串
        /// </summary>
        /// <param name="Security"></param>
        /// <returns></returns>
        public static string HashEncoding(string Security)
        {
            byte[] Value;
            UnicodeEncoding Code = new UnicodeEncoding();
            byte[] Message = Code.GetBytes(Security);
            SHA512Managed Arithmetic = new SHA512Managed();
            Value = Arithmetic.ComputeHash(Message);
            Security = "";
            foreach (byte o in Value)
            {
                Security += (int)o + "O";
            }
            return Security;
        }
    }

    /// <summary>
    /// MySecurity(安全类) 的摘要说明。
    /// </summary>
    public class MySecurity
    {
        /// <summary>
        /// 初始化安全类
        /// </summary>
        public MySecurity()
        {
            ///默认密码
            key = "0123456789";
        }
        private string key; //默认密钥

        private byte[] sKey;
        private byte[] sIV;

        #region 加密字符串
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <param name="keyStr">密码，可以为“”</param>
        /// <returns>输出加密后字符串</returns>
        static public string SEncryptString(string inputStr, string keyStr)
        {
            MySecurity ws = new MySecurity();
            return ws.EncryptString(inputStr, keyStr);
        }
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <param name="keyStr">密码，可以为“”</param>
        /// <returns>输出加密后字符串</returns>
        public string EncryptString(string inputStr, string keyStr)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            if (keyStr == "")
                keyStr = key;
            byte[] inputByteArray = Encoding.Default.GetBytes(inputStr);
            byte[] keyByteArray = Encoding.Default.GetBytes(keyStr);
            SHA1 ha = new SHA1Managed();
            byte[] hb = ha.ComputeHash(keyByteArray);
            sKey = new byte[8];
            sIV = new byte[8];
            for (int i = 0; i < 8; i++)
                sKey[i] = hb[i];
            for (int i = 8; i < 16; i++)
                sIV[i - 8] = hb[i];
            des.Key = sKey;
            des.IV = sIV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            cs.Close();
            ms.Close();
            return ret.ToString();
        }
        #endregion

        #region 加密字符串 密钥为系统默认 0123456789
        /// <summary>
        /// 加密字符串 密钥为系统默认
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <returns>输出加密后字符串</returns>
        static public string SEncryptString(string inputStr)
        {
            MySecurity ws = new MySecurity();
            return ws.EncryptString(inputStr, "");
        }
        #endregion


        #region 加密文件
        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="filePath">输入文件路径</param>
        /// <param name="savePath">加密后输出文件路径</param>
        /// <param name="keyStr">密码，可以为“”</param>
        /// <returns></returns>  
        public bool EncryptFile(string filePath, string savePath, string keyStr)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            if (keyStr == "")
                keyStr = key;
            FileStream fs = File.OpenRead(filePath);
            byte[] inputByteArray = new byte[fs.Length];
            fs.Read(inputByteArray, 0, (int)fs.Length);
            fs.Close();
            byte[] keyByteArray = Encoding.Default.GetBytes(keyStr);
            SHA1 ha = new SHA1Managed();
            byte[] hb = ha.ComputeHash(keyByteArray);
            sKey = new byte[8];
            sIV = new byte[8];
            for (int i = 0; i < 8; i++)
                sKey[i] = hb[i];
            for (int i = 8; i < 16; i++)
                sIV[i - 8] = hb[i];
            des.Key = sKey;
            des.IV = sIV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            fs = File.OpenWrite(savePath);
            foreach (byte b in ms.ToArray())
            {
                fs.WriteByte(b);
            }
            fs.Close();
            cs.Close();
            ms.Close();
            return true;
        }
        #endregion

        #region 解密字符串
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="inputStr">要解密的字符串</param>
        /// <param name="keyStr">密钥</param>
        /// <returns>解密后的结果</returns>
        static public string SDecryptString(string inputStr, string keyStr)
        {
            MySecurity ws = new MySecurity();
            return ws.DecryptString(inputStr, keyStr);
        }
        /// <summary>
        ///  解密字符串 密钥为系统默认
        /// </summary>
        /// <param name="inputStr">要解密的字符串</param>
        /// <returns>解密后的结果</returns>
        static public string SDecryptString(string inputStr)
        {
            MySecurity ws = new MySecurity();
            return ws.DecryptString(inputStr, "");
        }
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="inputStr">要解密的字符串</param>
        /// <param name="keyStr">密钥</param>
        /// <returns>解密后的结果</returns>
        public string DecryptString(string inputStr, string keyStr)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            if (keyStr == "")
                keyStr = key;
            byte[] inputByteArray = new byte[inputStr.Length / 2];
            for (int x = 0; x < inputStr.Length / 2; x++)
            {
                int i = (Convert.ToInt32(inputStr.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            byte[] keyByteArray = Encoding.Default.GetBytes(keyStr);
            SHA1 ha = new SHA1Managed();
            byte[] hb = ha.ComputeHash(keyByteArray);
            sKey = new byte[8];
            sIV = new byte[8];
            for (int i = 0; i < 8; i++)
                sKey[i] = hb[i];
            for (int i = 8; i < 16; i++)
                sIV[i - 8] = hb[i];
            des.Key = sKey;
            des.IV = sIV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        #endregion

        #region 解密文件
        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="filePath">输入文件路径</param>
        /// <param name="savePath">解密后输出文件路径</param>
        /// <param name="keyStr">密码，可以为“”</param>
        /// <returns></returns>    
        public bool DecryptFile(string filePath, string savePath, string keyStr)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            if (keyStr == "")
                keyStr = key;
            FileStream fs = File.OpenRead(filePath);
            byte[] inputByteArray = new byte[fs.Length];
            fs.Read(inputByteArray, 0, (int)fs.Length);
            fs.Close();
            byte[] keyByteArray = Encoding.Default.GetBytes(keyStr);
            SHA1 ha = new SHA1Managed();
            byte[] hb = ha.ComputeHash(keyByteArray);
            sKey = new byte[8];
            sIV = new byte[8];
            for (int i = 0; i < 8; i++)
                sKey[i] = hb[i];
            for (int i = 8; i < 16; i++)
                sIV[i - 8] = hb[i];
            des.Key = sKey;
            des.IV = sIV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            fs = File.OpenWrite(savePath);
            foreach (byte b in ms.ToArray())
            {
                fs.WriteByte(b);
            }
            fs.Close();
            cs.Close();
            ms.Close();
            return true;
        }
        #endregion


        #region MD5加密
        /// <summary>
        /// 128位MD5算法加密字符串
        /// </summary>
        /// <param name="text">要加密的字符串</param>    
        public static string MD5(string text)
        {

            //如果字符串为空，则返回
            if (String.IsNullOrEmpty(text))
            {
                return "";
            }
            //返回MD5值的字符串表示
            return MD5(text);
        }

        /// <summary>
        /// 128位MD5算法加密Byte数组
        /// </summary>
        /// <param name="data">要加密的Byte数组</param>    
        public static string MD5(byte[] data)
        {
            //如果Byte数组为空，则返回
            if (null == data)
            {
                return "";
            }

            try
            {
                //创建MD5密码服务提供程序
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

                //计算传入的字节数组的哈希值
                byte[] result = md5.ComputeHash(data);

                //释放资源
                md5.Clear();

                //返回MD5值的字符串表示
                return Convert.ToBase64String(result);
            }
            catch
            {

                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                return "";
            }
        }
        #endregion

        #region Base64加密
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="text">要加密的字符串</param>
        /// <returns></returns>
        public static string EncodeBase64(string text)
        {
            //如果字符串为空，则返回
            if (String.IsNullOrEmpty(text))
            {
                return "";
            }

            try
            {
                char[] Base64Code = new char[]{'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T',
                                            'U','V','W','X','Y','Z','a','b','c','d','e','f','g','h','i','j','k','l','m','n',
                                            'o','p','q','r','s','t','u','v','w','x','y','z','0','1','2','3','4','5','6','7',
                                            '8','9','+','/','='};
                byte empty = (byte)0;
                ArrayList byteMessage = new ArrayList(Encoding.Default.GetBytes(text));
                StringBuilder outmessage;
                int messageLen = byteMessage.Count;
                int page = messageLen / 3;
                int use = 0;
                if ((use = messageLen % 3) > 0)
                {
                    for (int i = 0; i < 3 - use; i++)
                        byteMessage.Add(empty);
                    page++;
                }
                outmessage = new System.Text.StringBuilder(page * 4);
                for (int i = 0; i < page; i++)
                {
                    byte[] instr = new byte[3];
                    instr[0] = (byte)byteMessage[i * 3];
                    instr[1] = (byte)byteMessage[i * 3 + 1];
                    instr[2] = (byte)byteMessage[i * 3 + 2];
                    int[] outstr = new int[4];
                    outstr[0] = instr[0] >> 2;
                    outstr[1] = ((instr[0] & 0x03) << 4) ^ (instr[1] >> 4);
                    if (!instr[1].Equals(empty))
                        outstr[2] = ((instr[1] & 0x0f) << 2) ^ (instr[2] >> 6);
                    else
                        outstr[2] = 64;
                    if (!instr[2].Equals(empty))
                        outstr[3] = (instr[2] & 0x3f);
                    else
                        outstr[3] = 64;
                    outmessage.Append(Base64Code[outstr[0]]);
                    outmessage.Append(Base64Code[outstr[1]]);
                    outmessage.Append(Base64Code[outstr[2]]);
                    outmessage.Append(Base64Code[outstr[3]]);
                }
                return outmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Base64解密
        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="text">要解密的字符串</param>
        public static string DecodeBase64(string text)
        {
            //如果字符串为空，则返回
            if (String.IsNullOrEmpty(text))
            {
                return "";
            }

            //将空格替换为加号
            text = text.Replace(" ", "+");

            try
            {
                if ((text.Length % 4) != 0)
                {
                    return "包含不正确的BASE64编码";
                }
                if (!Regex.IsMatch(text, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase))
                {
                    return "包含不正确的BASE64编码";
                }
                string Base64Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
                int page = text.Length / 4;
                ArrayList outMessage = new ArrayList(page * 3);
                char[] message = text.ToCharArray();
                for (int i = 0; i < page; i++)
                {
                    byte[] instr = new byte[4];
                    instr[0] = (byte)Base64Code.IndexOf(message[i * 4]);
                    instr[1] = (byte)Base64Code.IndexOf(message[i * 4 + 1]);
                    instr[2] = (byte)Base64Code.IndexOf(message[i * 4 + 2]);
                    instr[3] = (byte)Base64Code.IndexOf(message[i * 4 + 3]);
                    byte[] outstr = new byte[3];
                    outstr[0] = (byte)((instr[0] << 2) ^ ((instr[1] & 0x30) >> 4));
                    if (instr[2] != 64)
                    {
                        outstr[1] = (byte)((instr[1] << 4) ^ ((instr[2] & 0x3c) >> 2));
                    }
                    else
                    {
                        outstr[2] = 0;
                    }
                    if (instr[3] != 64)
                    {
                        outstr[2] = (byte)((instr[2] << 6) ^ instr[3]);
                    }
                    else
                    {
                        outstr[2] = 0;
                    }
                    outMessage.Add(outstr[0]);
                    if (outstr[1] != 0)
                        outMessage.Add(outstr[1]);
                    if (outstr[2] != 0)
                        outMessage.Add(outstr[2]);
                }
                byte[] outbyte = (byte[])outMessage.ToArray(Type.GetType("System.Byte"));
                return Encoding.Default.GetString(outbyte);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }

    /// <summary> 
	/// RSA加密解密及RSA签名和验证
	/// </summary> 
	public class RSACryption
    {
        public RSACryption()
        {
        }


        #region RSA 加密解密 

        #region RSA 的密钥产生 

        /// <summary>
        /// RSA 的密钥产生 产生私钥 和公钥 
        /// </summary>
        /// <param name="xmlKeys"></param>
        /// <param name="xmlPublicKey"></param>
        public void RSAKey(out string xmlKeys, out string xmlPublicKey)
        {
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            xmlKeys = rsa.ToXmlString(true);
            xmlPublicKey = rsa.ToXmlString(false);
        }
        #endregion

        #region RSA的加密函数 
        //############################################################################## 
        //RSA 方式加密 
        //说明KEY必须是XML的行式,返回的是字符串 
        //在有一点需要说明！！该加密方式有 长度 限制的！！ 
        //############################################################################## 

        //RSA的加密函数  string
        public string RSAEncrypt(string xmlPublicKey, string m_strEncryptString)
        {

            byte[] PlainTextBArray;
            byte[] CypherTextBArray;
            string Result;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPublicKey);
            PlainTextBArray = (new UnicodeEncoding()).GetBytes(m_strEncryptString);
            CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
            Result = Convert.ToBase64String(CypherTextBArray);
            return Result;

        }
        //RSA的加密函数 byte[]
        public string RSAEncrypt(string xmlPublicKey, byte[] EncryptString)
        {

            byte[] CypherTextBArray;
            string Result;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPublicKey);
            CypherTextBArray = rsa.Encrypt(EncryptString, false);
            Result = Convert.ToBase64String(CypherTextBArray);
            return Result;

        }
        #endregion

        #region RSA的解密函数 
        //RSA的解密函数  string
        public string RSADecrypt(string xmlPrivateKey, string m_strDecryptString)
        {
            byte[] PlainTextBArray;
            byte[] DypherTextBArray;
            string Result;
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPrivateKey);
            PlainTextBArray = Convert.FromBase64String(m_strDecryptString);
            DypherTextBArray = rsa.Decrypt(PlainTextBArray, false);
            Result = (new UnicodeEncoding()).GetString(DypherTextBArray);
            return Result;

        }

        //RSA的解密函数  byte
        public string RSADecrypt(string xmlPrivateKey, byte[] DecryptString)
        {
            byte[] DypherTextBArray;
            string Result;
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPrivateKey);
            DypherTextBArray = rsa.Decrypt(DecryptString, false);
            Result = (new UnicodeEncoding()).GetString(DypherTextBArray);
            return Result;

        }
        #endregion

        #endregion

        #region RSA数字签名 

        #region 获取Hash描述表 
        //获取Hash描述表 
        public bool GetHash(string m_strSource, ref byte[] HashData)
        {
            //从字符串中取得Hash描述 
            byte[] Buffer;
            System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
            Buffer = System.Text.Encoding.GetEncoding("GB2312").GetBytes(m_strSource);
            HashData = MD5.ComputeHash(Buffer);

            return true;
        }

        //获取Hash描述表 
        public bool GetHash(string m_strSource, ref string strHashData)
        {

            //从字符串中取得Hash描述 
            byte[] Buffer;
            byte[] HashData;
            System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
            Buffer = System.Text.Encoding.GetEncoding("GB2312").GetBytes(m_strSource);
            HashData = MD5.ComputeHash(Buffer);

            strHashData = Convert.ToBase64String(HashData);
            return true;

        }

        //获取Hash描述表 
        public bool GetHash(System.IO.FileStream objFile, ref byte[] HashData)
        {

            //从文件中取得Hash描述 
            System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
            HashData = MD5.ComputeHash(objFile);
            objFile.Close();

            return true;

        }

        //获取Hash描述表 
        public bool GetHash(System.IO.FileStream objFile, ref string strHashData)
        {

            //从文件中取得Hash描述 
            byte[] HashData;
            System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
            HashData = MD5.ComputeHash(objFile);
            objFile.Close();

            strHashData = Convert.ToBase64String(HashData);

            return true;

        }
        #endregion

        #region RSA签名 
        //RSA签名 
        public bool SignatureFormatter(string p_strKeyPrivate, byte[] HashbyteSignature, ref byte[] EncryptedSignatureData)
        {

            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPrivate);
            System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
            //设置签名的算法为MD5 
            RSAFormatter.SetHashAlgorithm("MD5");
            //执行签名 
            EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

            return true;

        }

        //RSA签名 
        public bool SignatureFormatter(string p_strKeyPrivate, byte[] HashbyteSignature, ref string m_strEncryptedSignatureData)
        {

            byte[] EncryptedSignatureData;

            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPrivate);
            System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
            //设置签名的算法为MD5 
            RSAFormatter.SetHashAlgorithm("MD5");
            //执行签名 
            EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

            m_strEncryptedSignatureData = Convert.ToBase64String(EncryptedSignatureData);

            return true;

        }

        //RSA签名 
        public bool SignatureFormatter(string p_strKeyPrivate, string m_strHashbyteSignature, ref byte[] EncryptedSignatureData)
        {

            byte[] HashbyteSignature;

            HashbyteSignature = Convert.FromBase64String(m_strHashbyteSignature);
            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPrivate);
            System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
            //设置签名的算法为MD5 
            RSAFormatter.SetHashAlgorithm("MD5");
            //执行签名 
            EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

            return true;

        }

        //RSA签名 
        public bool SignatureFormatter(string p_strKeyPrivate, string m_strHashbyteSignature, ref string m_strEncryptedSignatureData)
        {

            byte[] HashbyteSignature;
            byte[] EncryptedSignatureData;

            HashbyteSignature = Convert.FromBase64String(m_strHashbyteSignature);
            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPrivate);
            System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
            //设置签名的算法为MD5 
            RSAFormatter.SetHashAlgorithm("MD5");
            //执行签名 
            EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

            m_strEncryptedSignatureData = Convert.ToBase64String(EncryptedSignatureData);

            return true;

        }
        #endregion

        #region RSA 签名验证 

        public bool SignatureDeformatter(string p_strKeyPublic, byte[] HashbyteDeformatter, byte[] DeformatterData)
        {

            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPublic);
            System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
            //指定解密的时候HASH算法为MD5 
            RSADeformatter.SetHashAlgorithm("MD5");

            if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool SignatureDeformatter(string p_strKeyPublic, string p_strHashbyteDeformatter, byte[] DeformatterData)
        {

            byte[] HashbyteDeformatter;

            HashbyteDeformatter = Convert.FromBase64String(p_strHashbyteDeformatter);

            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPublic);
            System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
            //指定解密的时候HASH算法为MD5 
            RSADeformatter.SetHashAlgorithm("MD5");

            if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool SignatureDeformatter(string p_strKeyPublic, byte[] HashbyteDeformatter, string p_strDeformatterData)
        {

            byte[] DeformatterData;

            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPublic);
            System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
            //指定解密的时候HASH算法为MD5 
            RSADeformatter.SetHashAlgorithm("MD5");

            DeformatterData = Convert.FromBase64String(p_strDeformatterData);

            if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool SignatureDeformatter(string p_strKeyPublic, string p_strHashbyteDeformatter, string p_strDeformatterData)
        {

            byte[] DeformatterData;
            byte[] HashbyteDeformatter;

            HashbyteDeformatter = Convert.FromBase64String(p_strHashbyteDeformatter);
            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPublic);
            System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
            //指定解密的时候HASH算法为MD5 
            RSADeformatter.SetHashAlgorithm("MD5");

            DeformatterData = Convert.FromBase64String(p_strDeformatterData);

            if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        #endregion


        #endregion

    }
}

