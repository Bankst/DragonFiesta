namespace DragonFiesta.Game
{
    public enum GameConnectError : ushort
    {
        FailedToConnectToWorldServer = 321,
        FailedToConnectToMapServer = 323,
        ThereIsNoCharacterInTheSlot = 324,
        ErrorKP = 328,//by Reversed
        ErrorUnknown = 1228,
        ErrorInCharacterInfo = 1410,
        ErrorInAppearance = 1411,
        ErrorInOptions = 1412,
        ErrorInStatus = 1413,
        ErrorInSkill = 1414,
        ErrorInQuest = 1415,
        ErrorInHouse = 1416,
        ErrorInFriendInfo = 1417,
        ErrorInMasterAndApprentice = 1418,
        ErrorInGuild = 1419,
        ErrorInEmblem = 1420,
        ErrorInMover = 1421,
        ErrorInArena = 1422,
        ErrorInMarineBatter = 1423,
        RequestedCharacterIDNotMatching = 1424,
        ErrorInItem = 1425,
        ErrorInTreasureChest = 1426,
        ErrorInTitle = 1427,
        ErrorInKingdomQuest = 1428,
        ErrorInPremiumItem = 1429,
        MapUnderMaintenance = 1430
    }
}