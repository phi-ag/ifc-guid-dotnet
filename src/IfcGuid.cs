using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

public static class IfcGuid
{
    private const string IfcGuidPattern = @"^[0-3][\dA-Za-z_$]{21}$";

#if NET
    private static readonly Regex IfcGuidRegex =
        new(IfcGuidPattern, RegexOptions.Compiled | RegexOptions.NonBacktracking);
#else
    private static readonly Regex IfcGuidRegex =
        new(IfcGuidPattern, RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));
#endif

    private const string Chars =
        "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_$";

    private static readonly ImmutableDictionary<char, int> Reverse =
        Chars.Select((c, index) => new KeyValuePair<char, int>(c, index)).ToImmutableDictionary();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void FixGuidByteOrder(Span<byte> guid)
    {
        if (BitConverter.IsLittleEndian)
        {
            guid.Slice(0, 4).Reverse();
            guid.Slice(4, 2).Reverse();
            guid.Slice(6, 2).Reverse();
        }
    }

    public static string ToIfcGuid(Guid input)
    {
#if NET
        Span<byte> guid = stackalloc byte[16];
        if (!input.TryWriteBytes(guid))
            throw new UnreachableException("Failed to convert GUID to bytes");
#else
        Span<byte> guid = input.ToByteArray();
#endif

        FixGuidByteOrder(guid);

        Span<char> result = stackalloc char[22];
        result[0] = Chars[(guid[0] >> 6) & 63];
        result[1] = Chars[guid[0] & 63];

        for (int i = 1, j = 2; i < 16; i += 3, j += 4)
        {
            var u24 = (guid[i] << 16) | (guid[i + 1] << 8) | guid[i + 2];
            result[j] = Chars[(u24 >> 18) & 63];
            result[j + 1] = Chars[(u24 >> 12) & 63];
            result[j + 2] = Chars[(u24 >> 6) & 63];
            result[j + 3] = Chars[u24 & 63];
        }

        return result.ToString();
    }

    public static Guid FromIfcGuid(string ifcGuid)
    {
        if (ifcGuid is null) throw new ArgumentNullException(nameof(ifcGuid));
        if (ifcGuid.Length != 22) throw new ArgumentException("Invalid IFC-GUID length");
        if (!IfcGuidRegex.IsMatch(ifcGuid)) throw new ArgumentException("Invalid character in IFC-GUID");

        Span<byte> result = stackalloc byte[16];
        result[0] = (byte)((Reverse[ifcGuid[0]] << 6) | Reverse[ifcGuid[1]]);

        for (int i = 2, j = 1; j < 16; i += 4, j += 3)
        {
            var u24 =
              (Reverse[ifcGuid[i]] << 18) |
              (Reverse[ifcGuid[i + 1]] << 12) |
              (Reverse[ifcGuid[i + 2]] << 6) |
              Reverse[ifcGuid[i + 3]];

            result[j] = (byte)((u24 >> 16) & 255);
            result[j + 1] = (byte)((u24 >> 8) & 255);
            result[j + 2] = (byte)(u24 & 255);
        }

        FixGuidByteOrder(result);

#if NET
        return new(result);
#else
        return new(result.ToArray());
#endif
    }
}
