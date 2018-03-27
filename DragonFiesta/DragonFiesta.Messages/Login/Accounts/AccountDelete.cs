using System;

namespace DragonFiesta.Messages.Accounts
{
    [Serializable]
    public class AccountDelete : ExpectAnswer
    {
        public AccountDelete(int TimeToAnswerExpire)
            : base(TimeToAnswerExpire)
        {
        }

        public int AccountId { get; set; }
    }
}