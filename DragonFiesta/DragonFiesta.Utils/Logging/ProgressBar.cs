using System;

public class ProgressBar : IDisposable
{
    public ProgressBar(int row_count)
    {
        if (row_count > 0)
        {
            Init(row_count);
        }
        else
        {
            Init(1);
            Step();
        }
    }

    public void Dispose()
    {
        if (!m_showOutput)
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

        if (num_rec == 0)
        {
            return;
        }
        ++rec_no;
        n = rec_no * indic_len / num_rec;
        if (n != rec_pos)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            Console.Write("\r\x3D");

            for (i = 0; i < n; ++i)
            {
                Console.Write(full);
            }
            for (; i < indic_len; ++i)
            {
                Console.Write(empty);
            }
            float percent = n / (float)indic_len * 100;

            Console.Write("\x3D {0:D}%  \r\x3D", (int)percent);

            Console.Out.Flush();

            rec_pos = n;

            Console.ResetColor();
        }
    }

    // avoid use inline version because linking problems with private static field
    public static void SetOutputState(bool on)
    {
        m_showOutput = on;
    }

    private void Init(int row_count)
    {
        rec_no = 0;
        rec_pos = 0;
        indic_len = 50;
        num_rec = row_count;

        if (!m_showOutput)
        {
            return;
        }

        Console.Write("\x3D");

        for (int i = 0; i < indic_len; ++i)
        {
            Console.Write(empty);
        }
        Console.Write("\x3D 0%%\r\x3D");
        Console.Out.Flush();
    }

    private static bool m_showOutput = true; // not recommended change with existed active bar
    private readonly string empty = " ";
    private readonly string full = "\x3D";

    private int rec_no;
    private int rec_pos;
    private int num_rec;
    private int indic_len;
}