using System.Collections.Generic;
using System.IO;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.Zone.Data.Maps
{
    public sealed class BlockInfo
    {
        /// <summary>
        /// The full width of the map.
        /// </summary>
        public uint Width { get; private set; }

        /// <summary>
        /// The full height of the map.
        /// </summary>
        public uint Height { get; private set; }

        /// <summary>
        /// The small width of this map as it stands in the blockinfo.
        /// </summary>
        public uint SmallWidth { get; private set; }

        /// <summary>
        /// The small height of this map as it stands in the blockinfo.
        /// </summary>
        public uint SmallHeight { get; private set; }

        public bool[,] BlockData { get; private set; }
        public List<Position> WalkPositions { get; private set; }
        public const float WidthMultiplier = 50f;
        public const float HeightMultiplier = 6.25f;

        public bool ForceLoading { get; private set; }

        public BlockInfo(string Filename)
        {
            Load(Filename);
        }

        public BlockInfo(SQLResult pResult, int i)
        {
            SmallWidth = pResult.Read<uint>(i, "SmallWidth");
            SmallHeight = pResult.Read<uint>(i, "SmallHeight");
            Width = (uint)(SmallWidth * WidthMultiplier);
            Height = (uint)(SmallHeight * HeightMultiplier);
            WalkPositions = new List<Position>();
            ForceLoading = false;
        }

        private void Load(string Filename)
        {
            var data = File.ReadAllBytes(Filename);
            using (var stream = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(stream))
                {
                    SmallWidth = reader.ReadUInt32();
                    SmallHeight = reader.ReadUInt32();
                    Width = (uint)(SmallWidth * WidthMultiplier);
                    Height = (uint)(SmallHeight * HeightMultiplier);

                    BlockData = new bool[SmallWidth, SmallHeight];
                    WalkPositions = new List<Position>();
                    for (uint y = 0; y < SmallHeight; y++)
                    {
                        for (uint x = 0; x < SmallWidth; x++)
                        {
                            var blockVal = reader.ReadByte();

                            BlockData[x, y] = (blockVal != 0xff);

                            if (blockVal != 0xff)
                            {
                                WalkPositions.Add(new Position((uint)(x * WidthMultiplier), (uint)(y * HeightMultiplier)));
                            }
                        }
                    }
                }
            }
            data = null;
        }

        public bool CanWalk(uint X, uint Y)
        {
            if (ForceLoading)
            {
                if (X == 0
                    || X >= Width
                    || Y == 0
                    || Y >= Height)
                    return false;
                int realPosX = (int)(X / 50f),
                    realPosY = (int)(Y / 6.25f);
                return (realPosX > 0
                    && realPosY > 0
                    && BlockData.GetLength(0) >= realPosX
                    && BlockData.GetLength(1) >= realPosY)
                    && BlockData[realPosX, realPosY];
            }
            else
            {
                //force loading for Dev Mode :D
                return true;
            }
        }
    }
}