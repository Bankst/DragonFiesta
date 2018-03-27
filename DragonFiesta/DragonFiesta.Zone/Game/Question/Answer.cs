using System;

namespace DragonFiesta.Zone.Game
{
    public sealed class Answer : IDisposable
    {
        //iAnswer
        public byte Index { get; private set; }

        public string Text { get; private set; }

        public Answer(byte ID, string Text)
        {
            this.Index = ID;
            this.Text = Text;
        }

        public void Dispose()
        {
            Text = null;
        }

        ~Answer()
        {
            Dispose();
        }
    }
}