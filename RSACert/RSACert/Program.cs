using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RSACert
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("c:\\SampleRSAKey.xml");

            string text = "This is a test";
            var privateKey = GetKey(doc.OuterXml, true);
            byte[] encData = EncryptData(text, privateKey);

            byte[] keyData = GetKeyData(doc.OuterXml, false);
            RSACryptoServiceProvider publicKey1 = GetKey(keyData, false);
            string decString = DecryptData(publicKey1, encData);

        }

        static byte[] EncryptData(string text, RSACryptoServiceProvider rsa)
        {

            byte[] byteData = Encoding.ASCII.GetBytes(text);
            return rsa.Encrypt(byteData, false);
        }

        static string DecryptData(RSACryptoServiceProvider rsa, byte[] encData)
        {            
            byte[] byteData = rsa.Decrypt(encData, false);
            return Encoding.ASCII.GetString(byteData);
        }

        static byte[] GetKeyData(string xml, bool isPrivate)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xml);
            Console.WriteLine(rsa.KeySize);
            Console.WriteLine(rsa.SignatureAlgorithm);
            return rsa.ExportCspBlob(true);
        }

        static RSACryptoServiceProvider GetKey(string xml, bool isPrivate)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xml);
            return rsa;
        }

        static RSACryptoServiceProvider GetKey(byte[] key, bool isPrivate)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportCspBlob(key);
            return rsa;
        }

        static string ExportKey(byte[] key)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportCspBlob(key);
            Console.WriteLine(rsa.KeySize);
            Console.WriteLine(rsa.SignatureAlgorithm);
            return rsa.ToXmlString(true);
        }
    }
}
