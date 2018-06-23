using System;

[Serializable]
public enum InternZoneAuthResult : byte
{
    OK,
    InvalidPassword,
    InvalidZoneId,
    IdAlreadyRegistered,
}