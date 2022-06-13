using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 来源https://blog.csdn.net/suprman/article/details/1506363
/// </summary>
public static class SokobanData
{
    public static Dictionary<int, int[,]> levels;

    //与实际展示上下镜像
    private static int[,] level0 = new int[,] {
        { 0,2,2,2,0 },
        { 2,2,5,2,2 },
        { 2,1,1,1,2 },
        { 2,3,1,3,2 },
        { 2,4,1,4,2 },
        { 2,2,2,2,2 },
    };
    private static int[,] level1 = new int[,] {
        { 0,0,0,0,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,2,1,1,1,1,1,2,2,2,2,2,2,2,2,2 },
        { 2,2,2,2,2,1,2,2,2,1,2,5,2,2,1,1,4,4,2 },
        { 2,1,3,1,1,3,1,1,1,1,1,1,1,1,1,1,4,4,2 },
        { 2,1,1,1,2,1,2,2,1,2,2,2,2,2,1,1,4,4,2 },
        { 2,2,2,1,2,1,2,2,1,2,0,0,0,2,2,2,2,2,2 },
        { 0,0,2,1,1,3,1,3,1,2,0,0,0,0,0,0,0,0,0 },
        { 0,0,2,2,2,1,1,3,2,2,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,2,3,1,1,2,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,2,1,1,1,2,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0 },
    };

    static SokobanData()
    {
        levels = new Dictionary<int, int[,]>();
        levels.Add(0, level0);
        levels.Add(1, level1);
    }
}


/// <summary>
/// 0：虚空，1：空地，2：墙，3：箱子，4：目标点，5：玩家
/// 运行时额外状态：6：箱子在目标点上，7：玩家在目标点上
/// </summary>
public enum SokobanState
{
    @void = 0,
    floor = 1,
    wall = 2,
    box = 3,
    target = 4,
    player = 5,
    boxOnTarget = 6,
    playerOnTarget = 7,
}
