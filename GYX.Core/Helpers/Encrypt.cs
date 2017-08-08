using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace GYX.Core.Helpers
{
    /// <summary>
    /// 字符串加密组件
    /// </summary>
    public static class EncryptHelper
    {

        /// <summary>
        /// Encrypts a string using the SHA256 algorithm.
        /// </summary>
        /// <param name="plainMessage">
        /// The plain Message.
        /// </param>
        /// <returns>
        /// The hash password.
        /// </returns>
        public static string HashPassword(string plainMessage)
        {
            var data = Encoding.UTF8.GetBytes(plainMessage);
            using (HashAlgorithm sha = new SHA256Managed())
            {
                sha.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(sha.Hash);
            }
        }

        /// <summary>
        /// Create salt key
        /// </summary>
        /// <param name="size">Key size</param>
        /// <returns>Salt key</returns>
        public static string CreateSaltKey(int size)
        {
            // Generate a cryptographic random number
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);
            // Return a Base64 string representation of the random number
            return Convert.ToBase64String(buff);
        }


        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetMd5Code(string text = "")
        {
            if (string.IsNullOrEmpty(text)) text = "";

            var result = Encoding.Default.GetBytes(text);
            MD5 md5 = new MD5CryptoServiceProvider();
            var oBytes = md5.ComputeHash(result);
            return BitConverter.ToString(oBytes).Replace("-", "").ToLower();
        }
        /// <summary>
        /// Create a password hash
        /// </summary>
        /// <param name="password">{assword</param>
        /// <param name="saltkey">Salk key</param>
        /// <param name="passwordFormat">Password format (hash algorithm)</param>
        /// <returns>Password hash</returns>
        public static string CreatePasswordHash(string password, string saltkey, string passwordFormat = "SHA1")
        {
            if (String.IsNullOrEmpty(passwordFormat))
                passwordFormat = "SHA1";
            string saltAndPassword = String.Concat(password, saltkey);

            //return FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPassword, passwordFormat);
            var algorithm = HashAlgorithm.Create(passwordFormat);
            if (algorithm == null)
                throw new ArgumentException("Unrecognized hash name");

            var hashByteArray = algorithm.ComputeHash(Encoding.UTF8.GetBytes(saltAndPassword));
            return BitConverter.ToString(hashByteArray).Replace("-", "");
        }

        public static string GenerateOrderNumber()
        {
            string strDateTimeNumber = DateTime.Now.ToString("yyyyMMddHHmmssms");
            Thread.Sleep(1);
            string strRandomResult = NextRandom(1000, 1).ToString(CultureInfo.InvariantCulture);
            return strDateTimeNumber + strRandomResult;
        }
        /// <summary>
        /// 参考：msdn上的RNGCryptoServiceProvider例子
        /// </summary>
        /// <param name="numSeeds"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static int NextRandom(int numSeeds, int length)
        {
            // Create a byte array to hold the random value.  
            byte[] randomNumber = new byte[length];
            // Create a new instance of the RNGCryptoServiceProvider.  
            var rng = new RNGCryptoServiceProvider();
            // Fill the array with a random value.  
            rng.GetBytes(randomNumber);
            // Convert the byte to an uint value to make the modulus operation easier.  
            uint randomResult = 0x0;
            for (int i = 0; i < length; i++)
            {
                randomResult |= ((uint)randomNumber[i] << ((length - 1 - i) * 8));
            }
            return (int)(randomResult % numSeeds) + 1;
        }


        #region Base64加密解密

        /// <summary>
        ///  Base64加密，默认采用utf8编码方式加密
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="CodeType"></param>
        /// <returns></returns>
        public static string EncodeBase64(string Source, string CodeType = "utf-8")
        {
            byte[] bytes = Encoding.GetEncoding(CodeType).GetBytes(Source);
            try
            {
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return Source;
            }
        }


        /// <summary>
        /// Base64解密，默认采用utf8编码方式解密 
        /// </summary>
        /// <param name="Result">密文</param>
        /// <param name="CodeType">编码方式</param>
        /// <returns>明文</returns>
        public static string DecodeBase64(string Result, string CodeType = "utf-8")
        {

            byte[] bytes = Convert.FromBase64String(Result);
            try
            {
                return Encoding.GetEncoding(CodeType).GetString(bytes);
            }
            catch
            {

                return Result;

            }

        }

        #endregion

        #region TripleDES
        /// <summary>
        /// TripleDESHelper 的摘要说明。
        /// </summary>
        private const string DEFAULT_KEY = "{A090CB24-AF38-4544-92F8-A5B9F1A36ABD}";

        private static readonly TripleDESService m_TripleDES = new TripleDESService(DEFAULT_KEY);


        /// <summary>
        /// 加密.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
		public static string Encrypt(string source)
        {
            return m_TripleDES.Encrypt(source);
        }

        /// <summary>
        /// 解密.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
		public static string Decrypt(string source)
        {
            return m_TripleDES.Decrypt(source);
            //return md5(source);
        }
        public static string md5(string data)
        {
            byte[] b = System.Text.Encoding.Default.GetBytes(data);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
            {
                ret += b[i].ToString("x").PadLeft(2, '0');
            }
            return ret;
        }//end method

        /// <summary>
        /// 加密.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Encrypt(string key, string source)
        {
            return new TripleDESService(key).Encrypt(source);
        }

        /// <summary>
        /// 解密.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Decrypt(string key, string source)
        {
            return new TripleDESService(key).Decrypt(source);
        }
    }
    #endregion TripleDES
    /// <summary>
    /// 三重DES.
    /// </summary>
    public class TripleDESService : IDisposable
    {
        private TripleDES mydes;
        /// <summary>
        /// 密钥值.
        /// </summary>
		public string Key;
        /// <summary>
        /// 初始向量值.
        /// </summary>
		public string IV;

        /// <summary>
        /// 对称加密类的构造函数.
        /// </summary>
        public TripleDESService(string key)
        {
            mydes = new TripleDESCryptoServiceProvider();
            Key = key;
            IV = "#$^%&&*Yisifhsfjsljfslhgosdshf26382837sdfjskhf97(*&(*";
        }

        /// <summary>
        /// 对称加密类的构造函数.
        /// </summary>
        public TripleDESService(string key, string iv)
        {
            mydes = new TripleDESCryptoServiceProvider();
            Key = key;
            IV = iv;
        }

        /// <summary>
        /// 获得密钥.
        /// </summary>
        /// <returns>密钥.</returns>
        private byte[] GetLegalKey()
        {
            string sTemp = Key;
            mydes.GenerateKey();
            byte[] bytTemp = mydes.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
                sTemp = sTemp.Substring(0, KeyLength);
            else if (sTemp.Length < KeyLength)
                sTemp = sTemp.PadRight(KeyLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>
        /// 获得初始向量 IV.
        /// </summary>
        /// <returns>初试向量 IV.</returns>
        private byte[] GetLegalIV()
        {
            string sTemp = IV;
            mydes.GenerateIV();
            byte[] bytTemp = mydes.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
                sTemp = sTemp.Substring(0, IVLength);
            else if (sTemp.Length < IVLength)
                sTemp = sTemp.PadRight(IVLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>
        /// 加密方法.
        /// </summary>
        /// <param name="Source">待加密的串.</param>
        /// <returns>经过加密的串.</returns>
        public string Encrypt(string Source)
        {
            try
            {
                byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
                MemoryStream ms = new MemoryStream();
                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();
                ICryptoTransform encrypto = mydes.CreateEncryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                ms.Close();
                byte[] bytOut = ms.ToArray();
                return Convert.ToBase64String(bytOut);
            }
            catch (Exception ex)
            {
                throw new Exception("在文件加密的时候出现错误！错误提示： \n" + ex.Message);
            }
        }

        /// <summary>
        /// 解密方法.
        /// </summary>
        /// <param name="Source">待解密的串.</param>
        /// <returns>经过解密的串.</returns>
        public string Decrypt(string Source)
        {
            try
            {
                byte[] bytIn = Convert.FromBase64String(Source);
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();
                ICryptoTransform encrypto = mydes.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception("在文件解密的时候出现错误！错误提示： \n" + ex.Message);
            }
        }

        /// <summary>
        /// 加密方法byte[] to byte[].
        /// </summary>
        /// <param name="Source">待加密的byte数组.</param>
        /// <returns>经过加密的byte数组.</returns>
        public byte[] Encrypt(byte[] Source)
        {
            try
            {
                byte[] bytIn = Source;
                MemoryStream ms = new MemoryStream();
                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();
                ICryptoTransform encrypto = mydes.CreateEncryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                ms.Close();
                byte[] bytOut = ms.ToArray();
                return bytOut;
            }
            catch (Exception ex)
            {
                throw new Exception("在文件加密的时候出现错误！错误提示： \n" + ex.Message);
            }
        }

        /// <summary>
        /// 解密方法byte[] to byte[].
        /// </summary>
        /// <param name="Source">待解密的byte数组.</param>
        /// <returns>经过解密的byte数组.</returns>
        public byte[] Decrypt(byte[] Source)
        {
            try
            {
                byte[] bytIn = Source;
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();
                ICryptoTransform encrypto = mydes.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return UTF8Encoding.UTF8.GetBytes(sr.ReadToEnd());
            }
            catch (Exception ex)
            {
                throw new Exception("在文件解密的时候出现错误！错误提示： \n" + ex.Message);
            }
        }

        /// <summary>
        /// 加密方法File to File.
        /// </summary>
        /// <param name="inFileName">待加密文件的路径.</param>
        /// <param name="outFileName">待加密后文件的输出路径.</param>

        public void Encrypt(string inFileName, string outFileName)
        {
            try
            {

                FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                FileStream fout = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write);
                fout.SetLength(0);

                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();

                byte[] bin = new byte[100];
                long rdlen = 0;
                long totlen = fin.Length;
                int len;

                ICryptoTransform encrypto = mydes.CreateEncryptor();
                CryptoStream cs = new CryptoStream(fout, encrypto, CryptoStreamMode.Write);
                while (rdlen < totlen)
                {
                    len = fin.Read(bin, 0, 100);
                    cs.Write(bin, 0, len);
                    rdlen = rdlen + len;
                }
                cs.Close();
                fout.Close();
                fin.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("在文件加密的时候出现错误！错误提示： \n" + ex.Message);
            }
        }

        /// <summary>
        /// 解密方法File to File.
        /// </summary>
        /// <param name="inFileName">待解密文件的路径.</param>
        /// <param name="outFileName">待解密后文件的输出路径.</param>
        public void Decrypt(string inFileName, string outFileName)
        {
            try
            {
                FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                FileStream fout = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write);
                fout.SetLength(0);

                byte[] bin = new byte[100];
                long rdlen = 0;
                long totlen = fin.Length;
                int len;
                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();
                ICryptoTransform encrypto = mydes.CreateDecryptor();
                CryptoStream cs = new CryptoStream(fout, encrypto, CryptoStreamMode.Write);
                while (rdlen < totlen)
                {
                    len = fin.Read(bin, 0, 100);
                    cs.Write(bin, 0, len);
                    rdlen = rdlen + len;
                }
                cs.Close();
                fout.Close();
                fin.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("在文件解密的时候出现错误！错误提示： \n" + ex.Message);
            }
        }

        #region IDisposable Members

        /// <summary>
        /// 销毁.
        /// </summary>
        public void Dispose()
        {
            //;
        }

        #endregion
    }
}

