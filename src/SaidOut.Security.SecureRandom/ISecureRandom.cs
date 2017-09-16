namespace SaidOut.Security
{

    /// <summary>Generate random data/value using a cryptographic random number generator.</summary>
    public interface ISecureRandom
    {

        /// <summary>Create a byte array of <paramref name="size"/> with random data.</summary>
        /// <param name="size">Size of the byte array that should be returned.</param>
        /// <returns>A byte array containing random data.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">If <paramref name="size"/> is less than one.</exception>
        byte[] GenerateRandomData(int size);


        /// <summary>Generate a random value in the range [<paramref name="min"/>, <paramref name="max"/>].</summary>
        /// <param name="min">The lower bound of the range that the random value should be within.</param>
        /// <param name="max">The upper bound of the range that the random value should be within.</param>
        /// <returns>A random number that is in the range [<paramref name="min"/>, <paramref name="max"/>].</returns>
        /// <exception cref="System.ArgumentException">If <paramref name="max"/> is greater or equal to <paramref name="min"/>.</exception>
        int GenerateRandomValue(int min, int max);
    }
}