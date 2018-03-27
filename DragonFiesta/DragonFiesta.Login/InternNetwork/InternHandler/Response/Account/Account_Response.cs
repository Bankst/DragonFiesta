namespace DragonFiesta.Login.InternNetwork.InternHandler.Response.Account
{
    public class Account_Response
    {
        public static void DeleteResponse(IMessage msg)
        {
            /*
            CharactersDeleteResult mResponse = msg as CharactersDeleteResult;

            if (mResponse.CharacterDataDelete)
            {
                switch (AccountManager.DeleteAccount(mResponse.AccountID))
                {
                    case AccountDeleteResponse.InternalError:
                    case AccountDeleteResponse.NameTaken:
                    case AccountDeleteResponse.SQLError:
                        CommandLog.Write(CommandLogLevel.Execute, "Account {0}  deletet fail! SQL Error!!!!", mResponse.AccountName);
                        break;

                    case AccountDeleteResponse.Success:
                        CommandLog.Write(CommandLogLevel.Execute, "Account {0}  deletet Success!", mResponse.AccountName);
                        break;
                }
            }*/
        }
    }
}