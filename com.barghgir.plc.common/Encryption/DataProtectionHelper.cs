using System.Security.Cryptography;

namespace com.barghgir.plc.infra.common.Encryption;

public static class DataProtectionHelper
{
    #region // Simple encrypt/decrypt via Aes

    public static string EncryptDataWithAes(
        string plainText, string secretKeyBase64, string initVectorBase64)
    {
        return EncryptDataWithAes(plainText, ref secretKeyBase64, ref initVectorBase64);
    }

    public static string EncryptDataWithAes(
        string plainText, ref string keyBase64, ref string vectorBase64)
    {
        // ref: https://www.siakabaro.com/how-to-perform-aes-encryption-in-net/

        using Aes aesAlgorithm = Aes.Create();
        //aesAlgorithm.Mode= CipherMode.CBC;
        //aesAlgorithm.Padding = PaddingMode.PKCS7;
        aesAlgorithm.Key = keyBase64 == null ? aesAlgorithm.Key : Convert.FromBase64String(keyBase64);
        aesAlgorithm.IV = vectorBase64 == null ? aesAlgorithm.IV : Convert.FromBase64String(vectorBase64);
        keyBase64 = Convert.ToBase64String(aesAlgorithm.Key);
        vectorBase64 ??= Convert.ToBase64String(aesAlgorithm.IV);
        ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor();
        byte[] encryptedData;
        using MemoryStream ms = new();
        using CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write);
        using (StreamWriter sw = new(cs))
            sw.Write(plainText);
        encryptedData = ms.ToArray();
        return Convert.ToBase64String(encryptedData);
    }

    public static string DecryptDataWithAes(
        string cipherText, string keyBase64, string vectorBase64)
    {
        using Aes aesAlgorithm = Aes.Create();
        //aesAlgorithm.Mode = CipherMode.CBC;
        //aesAlgorithm.Padding = PaddingMode.PKCS7;
        aesAlgorithm.Key = Convert.FromBase64String(keyBase64);
        aesAlgorithm.IV = Convert.FromBase64String(vectorBase64);
        using ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor();
        byte[] cipher = Convert.FromBase64String(cipherText);
        using MemoryStream ms = new(cipher);
        using CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read);
        using StreamReader sr = new(cs);
        return sr.ReadToEnd();
    }
    #endregion // region working simple encrypt/decrypt via Aes
}