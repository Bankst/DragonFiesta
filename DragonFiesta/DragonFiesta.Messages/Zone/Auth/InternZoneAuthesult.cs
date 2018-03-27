using System;

[Serializable]
public enum InternZoneAuthesult : byte
{
    OK,
    InvalidPassword,
    InvalidZoneId,
    IdAlredyRegister,
}