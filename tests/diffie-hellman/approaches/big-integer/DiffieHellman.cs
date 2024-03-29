using System;
using System.Numerics;

public static class DiffieHellman
{
    public static BigInteger PrivateKey(BigInteger p) =>
        new(Random.Shared.NextInt64(1, (long)p));

    public static BigInteger PublicKey(BigInteger p, BigInteger g, BigInteger privateKey) =>
        BigInteger.ModPow(g, privateKey, p);

    public static BigInteger Secret(BigInteger p, BigInteger publicKey, BigInteger privateKey) =>
        BigInteger.ModPow(publicKey, privateKey, p);
}
