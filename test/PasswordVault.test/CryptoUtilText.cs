using System.Text;
using System.Data;
using PasswordVault.core;
using Xunit;
using System.Security.Cryptography;

namespace PasswordVault.test;

public class CryptoUtilTest
{
    [Fact]
    public void CalculateSHA256()
    {
        //https://emn178.github.io/online-tools/sha256.html
        string hash_of_hello_txt = "185f8db32271fe25f561a6fc938b2e264306ec304eda518007d1764826381969";
        string hash_calculated_of_hello_txt;
        hash_calculated_of_hello_txt = CryptoUtil.ComputeSha256Hash("Hello");

        Assert.Equal(hash_of_hello_txt,hash_calculated_of_hello_txt);
    }
}