using System;

namespace DFEngine.Logging
{
	public class ProgressBar : IDisposable
	{
		public ProgressBar(int rowCount)
		{
			if (rowCount > 0)
			{
				Init(rowCount);
			}
			else
			{
				Init(1);
				Step();
			}
		}

		public void Dispose()
		{
			if (!_mShowOutput)
			{
				return;
			}

			Console.Write("\n");
			Console.Write("\n");
			Console.Out.Flush();
		}

		public void Step()
		{
			int i;
			int n;

			if (_numRec == 0)
			{
				return;
			}
			++_recNo;
			n = _recNo * _indicLen / _numRec;
			if (n == _recPos) return;

			Console.ForegroundColor = ConsoleColor.Green;

			Console.Write("\r\x3D");

			for (i = 0; i < n; ++i)
			{
				Console.Write(Full);
			}
			for (; i < _indicLen; ++i)
			{
				Console.Write(Empty);
			}
			var percent = n / (float)_indicLen * 100;

			Console.Write("\x3D {0:D}%  \r\x3D", (int)percent);

			Console.Out.Flush();

			_recPos = n;

			Console.ResetColor();
		}

		// avoid use inline version because linking problems with private static field
		public static void SetOutputState(bool on)
		{
			_mShowOutput = on;
		}

		private void Init(int rowCount)
		{
			_recNo = 0;
			_recPos = 0;
			_indicLen = 50;
			_numRec = rowCount;

			if (!_mShowOutput)
			{
				return;
			}

			Console.Write("\x3D");

			for (var i = 0; i < _indicLen; ++i)
			{
				Console.Write(Empty);
			}
			Console.Write("\x3D 0%%\r\x3D");
			Console.Out.Flush();
		}

		private static bool _mShowOutput = true; // not recommended change with existed active bar
		private const string Empty = " ";
		private const string Full = "\x3D";

		private int _recNo;
		private int _recPos;
		private int _numRec;
		private int _indicLen;
	}
}