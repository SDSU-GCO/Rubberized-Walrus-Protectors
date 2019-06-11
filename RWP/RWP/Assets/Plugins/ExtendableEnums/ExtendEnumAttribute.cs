﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field)]
public class ExtendEnumAttribute : PropertyAttribute
{
    public readonly bool display = true;
    public ExtendEnumAttribute(bool displayValues = true)
    {
        display = displayValues;
    }
}
