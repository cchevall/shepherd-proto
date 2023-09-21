using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfig
{
    // Map Movement Boundaries
    public static readonly float xBound = 250f;
    public static readonly float xForestBound = 120f;
    public static readonly float yTopBound = 60f;
    public static readonly float yBottomBound = 2f;
    public static readonly float projectilesBottomBound = 4f;
    public static readonly float offLimitZPos = 900f; // Destroy off limit objects
    public static readonly float offLimitYTopPos = 800f; // Destroy off limit objects
    public static readonly float offLimitYBottomPos = -50f; // Destroy off limit objects
}
