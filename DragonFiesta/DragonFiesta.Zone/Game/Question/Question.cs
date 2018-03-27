using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Network.FiestaHandler.Server;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace DragonFiesta.Zone.Game
{
    public sealed class Question : IDisposable
    {
        public ZoneCharacter Character { get; private set; }
        public string Text { get; private set; }

        public SecureWriteCollection<Answer> Answers { get; private set; }

        public event EventHandler<QuestionAnswerEventArgs> OnAnswer;

        public object[] Parameters { get; private set; }

        public bool IsDisposed { get { return (IsDisposedInt > 0); } }
        private int IsDisposedInt;

        private Func<Answer, bool> AnswerFunc_Add;
        private Func<Answer, bool> AnswerFunc_Remove;
        private Action AnswerFunc_Clear;

        private ConcurrentDictionary<byte, Answer> AnswersByID;

        public Question(ZoneCharacter Character, string Text, params object[] Parameters)
        {
            this.Character = Character;
            this.Text = Text;
            this.Parameters = Parameters;

            Answers = new SecureWriteCollection<Answer>(out AnswerFunc_Add, out AnswerFunc_Remove, out AnswerFunc_Clear);
            AnswersByID = new ConcurrentDictionary<byte, Answer>();
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
            {
                Character = null;
                Text = null;

                Answers.Dispose();
                Answers = null;

                OnAnswer = null;

                Parameters = null;

                AnswerFunc_Add = null;
                AnswerFunc_Remove = null;
                AnswerFunc_Clear = null;

                AnswersByID.Clear();
                AnswersByID = null;
            }
        }

        ~Question()
        {
            Dispose();
        }

        public bool GetFreeAnswerID(out byte ID)
        {
            ID = 0;

            if (IsDisposed)
                return false;

            for (byte i = 0; i < byte.MaxValue; i++)
            {
                if (!AnswersByID.ContainsKey(i))
                {
                    ID = i;
                    return true;
                }
            }

            return false;
        }

        public bool AddAnswer(Answer Answer)
        {
            if (IsDisposed)
                return false;

            if (AnswersByID.TryAdd(Answer.Index, Answer))
            {
                AnswerFunc_Add.Invoke(Answer);

                return true;
            }

            return false;
        }

        public bool GetAnswerByID(byte ID, out Answer Answer)
        {
            return AnswersByID.TryGetValue(ID, out Answer);
        }

        public void Send(ushort Distance = 1000) => SH15Handler.SendQuestion(this, Distance);

        public void HandleAnswer(Answer Answer)
        {
            if (OnAnswer != null)
            {
                var args = new QuestionAnswerEventArgs(this, Answer);

                OnAnswer.Invoke(this, args);
            }
        }

        public sealed class QuestionAnswerEventArgs
        {
            public Question Question { get; private set; }
            public Answer answer { get; private set; }

            public QuestionAnswerEventArgs(Question Question, Answer answer)
            {
                this.Question = Question;
                this.answer = answer;
            }
        }
    }
}