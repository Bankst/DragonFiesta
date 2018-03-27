public enum LoginGameError
{
    None = 1,
    UNKOWN_ERROR = 66,//Ein unbekannter Fehler ist aufgetreten.
    DATABASE_ERROR = 67,//DB error
    INVALID_CREDENTIALS = 68,//Authentifizierung fehlgeschlagen.
    INVALID_ID_OR_PW = 69,//ID und Passwort überprüfen.
    DATABASE_ERROR2 = 70,//DB error
    BLOCKED = 71, //ID wurde gesperrt.
    SERVER_MAINTENANCE = 72,//Die Server werden gewartet. Bitte später versuchen.
    TIMEOUT = 73,//Authentifizierungsversuch ausgelaufen. Bitte später probieren.
    LOGIN_FAILED = 74, //Login fehlgeschlagen.
    AGREEMENT_MISSING = 75,//Zum Fortfahren Vereinbarung annehmen.
    WRONG_REGION = 81,//Du befindest dich außerhalb unseres Servicegebiets.
}