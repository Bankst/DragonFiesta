﻿using System;

public sealed class GameCommandCategory : Attribute
{
    public string Category { get; private set; }

    public GameCommandCategory(string pCategory)
    {
        Category = pCategory;
    }
}