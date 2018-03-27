/*
   A C-program for MT19937, with initialization improved 2002/1/26.
   Coded by Takuji Nishimura and Makoto Matsumoto,
   rewrote to work with C#.Net by noodl. Also added auto reseed feature.
*/

using System;

public sealed class RandomMT
{
    // Period parameters
    private const ulong N = 624;

    private const ulong M = 397;
    private const ulong MATRIX_A = 0x9908B0DFUL;		// constant vector a
    private const ulong UPPER_MASK = 0x80000000UL;		// most significant w-r bits
    private const ulong LOWER_MASK = 0X7FFFFFFFUL;		// least significant r bits
    private const uint DEFAULT_SEED = 4357;

    private static ulong[] mt = new ulong[N + 1];	    // the array for the state vector
    private static ulong mti = N + 1;			        // mti==N+1 means mt[N] is not initialized

    // Auto reseed
    private const ulong SeedsLength = 4;

    public bool AutoReseed { get; set; }
    private ulong[] CurrentSeeds;
    private int SeedCounter;

    public RandomMT(bool AutoReseed = true)
    {
        this.AutoReseed = AutoReseed;

        CurrentSeeds = new ulong[SeedsLength];
        //lets create an random seed counter
        var now = DateTime.Now;
        SeedCounter = (int)(now.Millisecond |
                            now.Second +
                          ((now.Minute >>
                            now.Hour) &
                            now.Day) +
                            now.Month >>
                            now.Year);
        ReSeed();
    }

    public void Dispose()
    {
        mt = null;
        CurrentSeeds = null;
    }

    ~RandomMT()
    {
        Dispose();
    }

    //Recreates seeds
    public void ReSeed()
    {
        var rand = new Random(SeedCounter++);

        for (ulong i = 0; i < SeedsLength; i++)
        {
            CurrentSeeds[i] = (ulong)(rand.Next() + SeedCounter++);
        }

        init_by_array(CurrentSeeds, SeedsLength);
    }

    // initializes mt[N] with a seed
    private void init_genrand(ulong s)
    {
        mt[0] = s & 0xffffffffUL;
        for (mti = 1; mti < N; mti++)
        {
            mt[mti] = (1812433253UL * (mt[mti - 1] ^ (mt[mti - 1] >> 30)) + mti);
            mt[mti] &= 0xffffffffUL;
        }
    }

    // initialize by an array with array-length
    // init_key is the array for initializing keys
    // key_length is its length
    public void init_by_array(ulong[] init_key, ulong key_length)
    {
        ulong i, j, k;
        init_genrand(19650218UL);
        i = 1; j = 0;
        k = (N > key_length ? N : key_length);
        for (; k > 0; k--)
        {
            mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * 1664525UL))
            + init_key[j] + j;
            mt[i] &= 0xffffffffUL;
            i++; j++;
            if (i >= N) { mt[0] = mt[N - 1]; i = 1; }
            if (j >= key_length) j = 0;
        }
        for (k = N - 1; k > 0; k--)
        {
            mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * 1566083941UL))
            - i;
            mt[i] &= 0xffffffffUL;
            i++;
            if (i >= N) { mt[0] = mt[N - 1]; i = 1; }
        }
        mt[0] = 0x80000000UL;
    }

    // generates a random number on [0,0x7fffffff]-interval
    public long genrand_int31()
    {
        return (long)(genrand_int32() >> 1);
    }

    // generates a random number on [0,1]-real-interval
    public double genrand_real1()
    {
        return (double)genrand_int32() * (1.0 / 4294967295.0); // divided by 2^32-1
    }

    // generates a random number on [0,1)-real-interval
    public double genrand_real2()
    {
        return (double)genrand_int32() * (1.0 / 4294967296.0); // divided by 2^32
    }

    // generates a random number on (0,1)-real-interval
    public double genrand_real3()
    {
        return (((double)genrand_int32()) + 0.5) * (1.0 / 4294967296.0); // divided by 2^32
    }

    // generates a random number on [0,1) with 53-bit resolution
    public double genrand_res53()
    {
        ulong a = genrand_int32() >> 5;
        ulong b = genrand_int32() >> 6;
        return (double)(a * 67108864.0 + b) * (1.0 / 9007199254740992.0);
    }

    // generates a random number on [0,0xffffffff]-interval
    public ulong genrand_int32()
    {
        if (AutoReseed)
            ReSeed();

        return _genrand_int32();
    }

    private ulong _genrand_int32()
    {
        ulong y = 0;
        ulong[] mag01 = new ulong[2];
        mag01[0] = 0x0UL;
        mag01[1] = MATRIX_A;
        /* mag01[x] = x * MATRIX_A  for x=0,1 */

        if (mti >= N)
        {
            // generate N words at one time
            ulong kk;

            if (mti == N + 1)   /* if init_genrand() has not been called, */
                init_genrand(5489UL); /* a default initial seed is used */

            for (kk = 0; kk < N - M; kk++)
            {
                y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                mt[kk] = mt[kk + M] ^ (y >> 1) ^ mag01[y & 0x1UL];
            }
            for (; kk < N - 1; kk++)
            {
                y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                //mt[kk] = mt[kk+(M-N)] ^ (y >> 1) ^ mag01[y & 0x1UL];
                mt[kk] = mt[kk - 227] ^ (y >> 1) ^ mag01[y & 0x1UL];
            }
            y = (mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK);
            mt[N - 1] = mt[M - 1] ^ (y >> 1) ^ mag01[y & 0x1UL];

            mti = 0;
        }

        y = mt[mti++];

        /* Tempering */
        y ^= (y >> 11);
        y ^= (y << 7) & 0x9d2c5680UL;
        y ^= (y << 15) & 0xefc60000UL;
        y ^= (y >> 18);

        return y;
    }

    public int RandomRange(int lo, int hi)
    {
        return (Math.Abs((int)genrand_int32() % (hi - lo + 1)) + lo);
    }

    public byte[] RandomBytes(int Length)
    {
        var data = new byte[Length];

        for (int i = 0; i < Length; i++)
        {
            data[i] = (byte)RandomRange(byte.MinValue, byte.MaxValue);
        }

        return data;
    }

    public bool PercentChance(double Percent)
    {
        return (genrand_real1() <= (Percent / 100.0));
    }
}