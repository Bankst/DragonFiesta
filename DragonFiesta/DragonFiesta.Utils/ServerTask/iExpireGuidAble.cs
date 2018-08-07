using System;

namespace DragonFiesta.Utils.ServerTask
{
    public interface IExpireGuidAble : iExpireAble
    {
        Guid Id { get; }
    }
}