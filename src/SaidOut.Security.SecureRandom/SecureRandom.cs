using System;
using System.Security.Cryptography;

namespace SaidOut.Security
{

    /// <summary>
    /// Generate random data/value using a new cryptographic random number generator each time it generates a random data/value.
    /// If you need to generate multiple random data/values at the same time use <see cref="SecureRandomContext"/>.
    /// </summary>
    public sealed class SecureRandom : ISecureRandom
    {

        /// <summary>Create a byte array of <paramref name="size"/> with random data.</summary>
        /// <param name="size">Size of the byte array that should be returned.</param>
        /// <returns>A byte array containing random data.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="size"/> is less than one.</exception>
        public static byte[] GenerateRandomData(int size)
        {
            if (size < 1) throw new ArgumentOutOfRangeException(nameof(size), ExceptionMessages.SizeCannotBeLessThanOne);

            var randomData = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomData);
                return randomData;
            }
        }


        /// <summary>Generate a random value in the range [<paramref name="min"/>, <paramref name="max"/>].</summary>
        /// <param name="min">The lower bound of the range that the random value should be within.</param>
        /// <param name="max">The upper bound of the range that the random value should be within.</param>
        /// <returns>A random number that is in the range [<paramref name="min"/>, <paramref name="max"/>].</returns>
        /// <exception cref="ArgumentException">If <paramref name="max"/> is greater or equal to <paramref name="min"/>.</exception>
        public static int GenerateRandomValue(int min, int max)
        {
            if (max <= min) throw new ArgumentException(ExceptionMessages.MaxCannotBeLessOrEqualToMin, nameof(max));

            // Cast to long is done to avoid overflow when min = int.MinValue and max = int.MaxValue
            var elemInRange = (long)max - min + 1;
            var randomData = new byte[4];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomData);
                var randomInt = BitConverter.ToUInt32(randomData, 0);
                var mod = randomInt % elemInRange;
                return (int)(min + mod);
            }
        }


        int ISecureRandom.GenerateRandomValue(int min, int max) => GenerateRandomValue(min, max);


        byte[] ISecureRandom.GenerateRandomData(int size) => GenerateRandomData(size);
    }
}