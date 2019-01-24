using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class EncryptionEngine
{
    #region Member Variables..
    private const string _hash = "SHA1";
    private const string _initialVector = "OhLna7^m*bDD$12s";
    private const int _iterations = 2;
    private const int _keySize = 256;
    private const string _password = "ItsNotJustABoulder";
    private const string _salt = "ItsARock";
    #endregion Member Variables..


    #region Public Methods..
    public static string Decrypt(string encryptedText)
    {
        int ByteCount = 0;
        byte[] PlainTextBytes = null;
        RijndaelManaged SymmetricKey = new RijndaelManaged();

        if (!string.IsNullOrEmpty(encryptedText))
        {
            byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(_initialVector);
            byte[] SaltValueBytes = Encoding.ASCII.GetBytes(_salt);
            byte[] CipherTextBytes = Convert.FromBase64String(encryptedText);
            PlainTextBytes = new byte[CipherTextBytes.Length];
            PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(_password, SaltValueBytes, _hash, _iterations);
            byte[] KeyBytes = DerivedPassword.GetBytes(_keySize / 8);
            SymmetricKey.Mode = CipherMode.CBC;
            using (ICryptoTransform Decryptor = SymmetricKey.CreateDecryptor(KeyBytes, InitialVectorBytes))
            {
                using (MemoryStream MemStream = new MemoryStream(CipherTextBytes))
                {
                    using (CryptoStream CryptoStream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read))
                    {
                        ByteCount = CryptoStream.Read(PlainTextBytes, 0, PlainTextBytes.Length);
                        MemStream.Close();
                        CryptoStream.Close();
                    }
                }
            }
        }

        SymmetricKey.Clear();
        return Encoding.UTF8.GetString(PlainTextBytes, 0, ByteCount);
    }

    public static string Encrypt(string rawText)
    {
        RijndaelManaged SymmetricKey = new RijndaelManaged();
        byte[] CipherTextBytes = null;

        if (!string.IsNullOrEmpty(rawText))
        {
            byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(_initialVector);
            byte[] SaltValueBytes = Encoding.ASCII.GetBytes(_salt);
            byte[] PlainTextBytes = Encoding.UTF8.GetBytes(rawText);
            PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(_password, SaltValueBytes, _hash, _iterations);
            byte[] KeyBytes = DerivedPassword.GetBytes(_keySize / 8);
            SymmetricKey.Mode = CipherMode.CBC;

            using (ICryptoTransform Encryptor = SymmetricKey.CreateEncryptor(KeyBytes, InitialVectorBytes))
            {
                using (MemoryStream MemStream = new MemoryStream())
                {
                    using (CryptoStream CryptoStream = new CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write))
                    {
                        CryptoStream.Write(PlainTextBytes, 0, PlainTextBytes.Length);
                        CryptoStream.FlushFinalBlock();
                        CipherTextBytes = MemStream.ToArray();
                        MemStream.Close();
                        CryptoStream.Close();
                    }
                }
            }
        }

        SymmetricKey.Clear();
        return Convert.ToBase64String(CipherTextBytes);
    }
    #endregion Public Methods..
}
