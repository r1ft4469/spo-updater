using System;
using System.IO;
using ComponentAce.Compression.Libs.zlib;

namespace Aki.Launcher.MiniCommon
{
	public enum ZlibCompression
	{
		Store = 0,
		Fastest = 1,
		Fast = 3,
		Normal = 5,
		Ultra = 7,
		Maximum = 9
	}

	public static class Zlib
	{
		// Level |     hex CM/CI FLG    | byte[]
		// 1     | 78 01                | 120 1
		// 2     | 78 5E                | 120 94
		// 3     | 78 5E			    | 120 94
		// 4     | 78 5E			    | 120 94
		// 5     | 78 5E			    | 120 94
		// 6     | 78 9C			    | 120 156
		// 7     | 78 DA			    | 120 218
		// 8     | 78 DA			    | 120 218
		// 9     | 78 DA			    | 120 218

		private const byte CompressionMethodHeader = 120;
		private const byte FastestCompressionHeader = 1;
		private const byte LowCompressHeader = 94;
		private const byte NormalCompressHeader = 156;
		private const byte MaxCompressHeader = 218;

		public static bool CheckHeader(byte[] Data)
		{
			//we need the first two data, if they arn't there, just return false.
			//(first byte) Compression Method / Info (CM/CINFO) Header should always be 120
			if (Data == null || Data.Length < 3 || Data[0] != CompressionMethodHeader)
			{
				return false;
			}

			//(second byte) Flags (FLG) Header, should define our compression level.
			switch (Data[1])
			{
				case FastestCompressionHeader:
				case LowCompressHeader:
				case NormalCompressHeader:
				case MaxCompressHeader:
					return true;
			}

			return false;
		}

		/// <summary>
		/// Deflate data.
		/// </summary>
		public static byte[] Compress(byte[] data, ZlibCompression level)
		{
			byte[] buffer = new byte[data.Length + 24];

			ZStream zs = new ZStream()
			{
				avail_in = data.Length,
				next_in = data,
				next_in_index = 0,
				avail_out = buffer.Length,
				next_out = buffer,
				next_out_index = 0
			};

			zs.deflateInit((int)level);
			zs.deflate(zlibConst.Z_FINISH);

			data = new byte[zs.next_out_index];
			Array.Copy(zs.next_out, 0, data, 0, zs.next_out_index);

			return data;
		}

		/// <summary>
		/// Inflate data.
		/// </summary>
		public static byte[] Decompress(byte[] data)
		{
			byte[] buffer = new byte[4096];

			ZStream zs = new ZStream()
			{
				avail_in = data.Length,
				next_in = data,
				next_in_index = 0,
				avail_out = buffer.Length,
				next_out = buffer,
				next_out_index = 0
			};

			zs.inflateInit();

			using (MemoryStream ms = new MemoryStream())
			{
				do
				{
					zs.avail_out = buffer.Length;
					zs.next_out = buffer;
					zs.next_out_index = 0;

					int result = zs.inflate(0);

					if (result != 0 && result != 1)
					{
						break;
					}

					ms.Write(zs.next_out, 0, zs.next_out_index);
				}
				while (zs.avail_in > 0 || zs.avail_out == 0);

				return ms.ToArray();
			}
		}
	}
}