using System;

namespace AqoTesting.Core.Utils
{
    public static class Hex
    {
        public static string BytesToString(byte[] data) =>
            BitConverter.ToString(data).Replace("-", "");

        public static byte[] StringToBytes(string data)
        {
            int numberChars = data.Length;
            byte[] bytes = new byte[numberChars / 2];

            for(int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(data.Substring(i, 2), 16);

            return bytes;
        }
    }
}
