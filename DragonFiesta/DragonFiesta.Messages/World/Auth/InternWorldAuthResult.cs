using System;

[Serializable]
public enum InternWorldAuthResult : byte
{
    Error,
    InvalidPassword,
    InvalidWorldID,
    AlredyRegister,
    OK,
}