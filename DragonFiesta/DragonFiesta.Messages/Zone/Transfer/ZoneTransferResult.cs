using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Messages.Zone.Transfer
{
    [Serializable]
    public enum ZoneTransferResult
    {
        Success,
        TransferDataError,
        CharacterError,
        MapDataError,
    }

}
