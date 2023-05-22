using System.Security.Cryptography;

namespace Shorti.Identity.Api.Services
{
    public class HashService
    {
        public string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer;

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }

            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer = bytes.GetBytes(0x20);
            }

            byte[] dst = new byte[0x31];

            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer, 0, dst, 0x11, 0x20);

            return Convert.ToBase64String(dst);
        }

        public bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer;

            if (string.IsNullOrEmpty(hashedPassword))
            {
                throw new ArgumentNullException(nameof(hashedPassword));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            byte[] src = Convert.FromBase64String(hashedPassword);

            if (src.Length != 0x31 || src[0] != 0)
            {
                return false;
            }

            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);

            byte[] buffer2 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer2, 0, 0x20);

            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer = bytes.GetBytes(0x20);
            }

            return ByteArraysEqual(buffer2, buffer);
        }

        private bool ByteArraysEqual(IEnumerable<byte> buffer1, IEnumerable<byte> buffer2)
        {
            ArgumentNullException.ThrowIfNull(buffer1, nameof(buffer1));
            ArgumentNullException.ThrowIfNull(buffer2, nameof(buffer2));

            if (buffer1.Count() != buffer2.Count())
            {
                return false;
            }

            for (int i = 0; i < buffer1.Count(); i++)
            {
                if (buffer1.ElementAt(i) != buffer2.ElementAt(i))
                    return false;
            }

            return true;
        }
    }
}
