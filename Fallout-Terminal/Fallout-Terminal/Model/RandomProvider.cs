using System;
using System.Threading;

/// <summary>
/// This class provides a better, "more random" way of getting random numbers,
/// since creating a new instance of random each time we need a random number is
/// far from random, as testing has revealed.
/// 
/// This code comes directly from the legendary Jon Skeet, god of C#.
/// Thanks, Jon.
/// </summary>
public static class RandomProvider
{
    private static int seed = Environment.TickCount;

    private static ThreadLocal<Random> randomWrapper = new ThreadLocal<Random>
        (() => new Random(Interlocked.Increment(ref seed)));

    /// <summary>
    /// Returns a reusable random object, courtesy of Jon Skeet himself.
    /// </summary>
    /// <returns>A new Random object. Thanks again, Jon.</returns>
    public static Random GetThreadRandom()
    {
        return randomWrapper.Value;
    }
}