﻿namespace FoosBall.Main
{
    using System;
    using System.Text;
    
    public static class Md5
    {
        // Calculates a MD5 hash from the given string and uses the given encoding.
        public static string CalculateMd5(string input, Encoding useEncoding)
        {
            var cryptoService = new System.Security.Cryptography.MD5CryptoServiceProvider();
 
            var inputBytes = useEncoding.GetBytes(input);
            inputBytes = cryptoService.ComputeHash(inputBytes);
            return BitConverter.ToString(inputBytes).Replace("-", string.Empty);
        }

        public static string CalculateMd5(string input)
        {
            return CalculateMd5(input, Encoding.Default);
        }

        public static string GetGravatarUrl(string email)
        {
            var md5Email = CalculateMd5(email.ToLower().Trim(), Encoding.Default);

            return string.Format("http://www.gravatar.com/avatar/{0}?d=smm", md5Email).ToLower();
        }
    }
}