using HashLib;

namespace Logic.Utilities
{
    public class HashingUtility
    {
        /// <summary>
        /// Secure hashing of passwords
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string SecureHashPassword(string password)
        {
            var hash = HashFactory.Crypto.SHA3.CreateKeccak256();
            var res = hash.ComputeString(password, System.Text.Encoding.ASCII);
            var digest = res.ToString();

            return digest;
        }
    }
}