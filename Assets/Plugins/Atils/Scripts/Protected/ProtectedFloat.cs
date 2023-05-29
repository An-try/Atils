using Atils.Runtime.Attributes;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

[Serializable]
public class ProtectedFloat
{
    [SerializeField, JsonRequired] private byte[] _key;
    [SerializeField, JsonRequired] private byte[] _IV;
    [SerializeField, JsonRequired] private byte[] _encryptedValue;

#if UNITY_EDITOR
#pragma warning disable IDE0052 // Remove unread private members
    [SerializeField, JsonIgnore, ReadOnly] private float _real;
#pragma warning restore IDE0052 // Remove unread private members
#endif

    [JsonIgnore]
    public float Value => DecryptNumber(_encryptedValue, _key, _IV);

    public override string ToString() => Value.ToString();

    // can return ProtectedFloat as float
    public static implicit operator float(ProtectedFloat value) => value.Value;

    // can return float as ProtectedFloat
    public static implicit operator ProtectedFloat(float value) => new ProtectedFloat(value);

    public ProtectedFloat() : this(0f) { }
    
    public ProtectedFloat(float value)
    {
        RijndaelManaged enc = new RijndaelManaged();
        enc.GenerateKey();
        enc.GenerateIV();
        _key = enc.Key;
        _IV = enc.IV;
        
        _encryptedValue = EncryptNumber(value, _key, _IV);

#if UNITY_EDITOR
        _real = Value;
#endif
    }
    
    private static byte[] EncryptNumber(float number, byte[] publickeybyte, byte[] secretkeyByte)
    {
        byte[] encArray;
        RijndaelManaged algo = new RijndaelManaged();

        algo.Key = publickeybyte;
        algo.IV = secretkeyByte;
        ICryptoTransform encryptor = algo.CreateEncryptor(algo.Key, algo.IV);
        MemoryStream msEncrypt = new MemoryStream();
        CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

        using (StreamWriter Encrypt = new StreamWriter(csEncrypt))
        {
            Encrypt.Write(number.ToString());
        }
        encArray = msEncrypt.ToArray();

        return encArray;
    }

    private static float DecryptNumber(byte[] encryptedNumber, byte[] publickeybyte, byte[] secretkeyByte)
    {
        string plaintext = null;
        RijndaelManaged algo = new RijndaelManaged();

        algo.Key = publickeybyte;
        algo.IV = secretkeyByte;
        ICryptoTransform decryptor = algo.CreateDecryptor(algo.Key, algo.IV);
        MemoryStream msDecrypt = new MemoryStream(encryptedNumber);

        CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

        using (StreamReader Decrypt = new StreamReader(csDecrypt))
        {
            plaintext = Decrypt.ReadToEnd();
        }
        return float.Parse(plaintext);
    }

    public static ProtectedFloat operator +(ProtectedFloat value1, ProtectedFloat value2)
    {
        return new ProtectedFloat(value1.Value + value2.Value);
    }

    public static ProtectedFloat operator -(ProtectedFloat value1, ProtectedFloat value2)
    {
        return new ProtectedFloat(value1.Value - value2.Value);
    }

}
