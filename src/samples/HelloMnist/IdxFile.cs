// Copyright © Aaron Sun and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace HelloMnist;

internal class IdxFile
{
    public string Name;

    public int Dimensions;

    public int[] Lengths;

    public byte[] Values;

    public IdxFile(string fileName)
    {
        Name = fileName;

        using var reader = new BinaryReader(File.OpenRead(fileName));

        var bytes = reader.ReadBytes(4);

        if (bytes[2] != 0x8)
        {
            throw new NotImplementedException();
        }

        Dimensions = bytes[3];

        Lengths = new int[Dimensions];
        var size = 1;

        for (int i = 0; i < Lengths.Length; i++)
        {
            Lengths[i] = (int)ReverseBytes(reader.ReadUInt32());
            size *= Lengths[i];
        }

        Values = reader.ReadBytes(size);

        if (reader.BaseStream.Position != reader.BaseStream.Length)
        {
            Console.WriteLine("Warning, not at end of " + fileName);
        }
    }

    public override string ToString() => Name;

    public static uint ReverseBytes(uint value)
    {
        return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
            (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
    }
}
