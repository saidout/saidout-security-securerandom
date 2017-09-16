﻿using NUnit.Framework;
using System;
using System.Linq;
using System.Text;

namespace SaidOut.Security.Tests
{

    public class SecureRandomContext_Tests
    {

        #region GenerateRandomData
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(250)]
        public void GenerateRandomData_Size_ReturnAnArrayWithLengthEqualToSize(int size)
        {
            using (var uat = new SecureRandomContext())
            {
                var actual = uat.GenerateRandomData(size);

                Assert.That(actual.Length, Is.EqualTo(size));
            }
        }


        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public void GenerateRandomData_SizeIsLessThanOne_ThrowsArgumentOutOfRangeException(int size)
        {
            using (var uat = new SecureRandomContext())
            {
                var ex = Assert.Throws<ArgumentOutOfRangeException>(() => uat.GenerateRandomData(size));
                Assert.That(ex.ParamName, Is.EqualTo("size"), nameof(ex.ParamName));
            }
        }


        /// <remarks>Test can generate false positive so rerun if the test fails.</remarks>
        [Test(Description = "Test can generate false positive so rerun if the test fails.")]
        public void GenerateRandomData_GenerateRandomDataSamples_SamplesAreSpreadRandomly()
        {
            var samples = 256 * 256 * 256;
            var tolerance = 0.02;
            var meanBucketSize = (samples / 256);
            var lowerBound = (int)(meanBucketSize * (1 - tolerance));
            var upperBound = (int)(meanBucketSize * (1 + tolerance));
            var buckets = new int[256];

            using (var uat = new SecureRandomContext())
            {
                for (var i = 0; i < samples; ++i)
                {
                    var randomData = uat.GenerateRandomData(1);
                    buckets[randomData[0]] += 1;
                }
            }

            var sb = new StringBuilder();
            var allValuesIsMean = buckets.All(x => x == meanBucketSize);
            if (allValuesIsMean)
            {
                sb.AppendLine("All bucket are equal");
            }
            else
            {
                for (var i = 0; i < buckets.Length; ++i)
                {
                    if (buckets[i] < lowerBound || buckets[i] > upperBound)
                    {
                        sb.AppendLine($"Bucket for value {i} contained {buckets[i]} samples which is outside the range [{lowerBound}, {upperBound}]");
                    }
                }
            }

            if (sb.Length > 0)
                Assert.Fail(sb.ToString());
        }
        #endregion


        #region GenerateRandomValue
        [TestCase(0, 1)]
        [TestCase(-5, 5)]
        [TestCase(int.MinValue, int.MaxValue)]
        [TestCase(int.MinValue, int.MinValue + 1)]
        [TestCase(int.MinValue, 0)]
        [TestCase(int.MaxValue - 1, int.MaxValue)]
        [TestCase(0, int.MaxValue)]
        public void GenerateRandomValue_MaxIsGreaterThanMin_DoesNotThrow(int min, int max)
        {
            using (var uat = new SecureRandomContext())
            {
                var actual = uat.GenerateRandomValue(min, max);

                Assert.That(actual, Is.GreaterThanOrEqualTo(min).And.LessThanOrEqualTo(max));
            }
        }


        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(-1, -2)]
        public void GenerateRandomValue_MaxIsLessOrEqualToMin_ThrowsArgumentException(int min, int max)
        {
            using (var uat = new SecureRandomContext())
            {
                var ex = Assert.Throws<ArgumentException>(() => uat.GenerateRandomValue(min, max));
                Assert.That(ex.ParamName, Is.EqualTo("max"), nameof(ex.ParamName));
            }
        }


        /// <remarks>Test can generate false positive so rerun if the test fails.</remarks>>
        [TestCase(0, 5, 5000, 0.055, Description = "Test can generate false positive so rerun if the test fails.")]
        [TestCase(-5, 5, 5000, 0.055, Description = "Test can generate false positive so rerun if the test fails.")]
        [TestCase(int.MinValue, int.MinValue + 9, 5000, 0.055, Description = "Test can generate false positive so rerun if the test fails.")]
        [TestCase(int.MaxValue - 9, int.MaxValue, 5000, 0.055, Description = "Test can generate false positive so rerun if the test fails.")]
        public void GenerateRandomValue_GenerateRandomDataSamples_SamplesAreSpreadRandomly(int min, int max, int meanBucketSize, decimal tolerance)
        {
            var elemInRange = (long)max - min + 1;
            var samples = elemInRange * meanBucketSize;
            var lowerBound = (int)(meanBucketSize * (1 - tolerance));
            var upperBound = (int)(meanBucketSize * (1 + tolerance));
            var buckets = new int[elemInRange];

            using (var uat = new SecureRandomContext())
            {
                for (var i = 0; i < samples; ++i)
                {
                    var sample = uat.GenerateRandomValue(min, max);
                    var sampleBucketIndex = sample - min;
                    buckets[sampleBucketIndex] += 1;
                }
            }

            var sb = new StringBuilder();
            var allValuesIsMean = buckets.All(x => x == meanBucketSize);
            if (allValuesIsMean)
            {
                sb.AppendLine("All bucket are equal");
            }
            else
            {
                for (var i = 0; i < buckets.Length; ++i)
                {
                    if (buckets[i] < lowerBound || buckets[i] > upperBound)
                    {
                        sb.AppendLine($"Bucket for value {i + min} contained {buckets[i]} samples which is outside the range [{lowerBound}, {upperBound}]");
                    }
                }
            }

            if (sb.Length > 0)
                Assert.Fail(sb.ToString());
        }
        #endregion


        #region DisposeTest
        [Test]
        public void Dispose_CalledTwice_DoesNotThrow()
        {
            var uat = new SecureRandomContext();

            Assert.DoesNotThrow(() => { uat.Dispose(); uat.Dispose(); });
        }


        [Test]
        public void GenerateRandomData_DisposeHasBeenCalled_ThrowsObjectDisposedException()
        {
            var uat = new SecureRandomContext();
            uat.Dispose();

            var ex = Assert.Throws<ObjectDisposedException>(() => { uat.GenerateRandomData(1); });
            Assert.That(ex.Message, Does.Contain(uat.GetType().FullName));
        }


        [Test]
        public void GenerateRandomValue_DisposeHasBeenCalled_ThrowsObjectDisposedException()
        {
            var uat = new SecureRandomContext();
            uat.Dispose();

            var ex = Assert.Throws<ObjectDisposedException>(() => { uat.GenerateRandomValue(0, 1); });
            Assert.That(ex.Message, Does.Contain(uat.GetType().FullName));
        }
        #endregion
    }
}