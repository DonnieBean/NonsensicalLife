using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    S,
    Z,
    J,
    L,
    I,
    O,
    T,
}


/// <summary>
/// 俄罗斯方块定义
/// </summary>
public static class TetrisDefine
{
    public static int baseWidth = 10;   //基础区域宽度
    public static int baseHeight = 20;  //基础区域高度
    public static int extraHeight = 24; //包含了生成区域的高度，最高的情况是生成了i，即20+4
    public static int blockNum = 4; //每个方块包含的小块数

    public static Vector2Int[] offsets; //小块偏移数组

    /// <summary>
    /// 静态构造函数,初始化小块偏移数组
    /// </summary>
    static TetrisDefine()
    {
        offsets = new Vector2Int[112];    //7种类型，每种四个旋转状态，每个状态四个小块，共7*4*4=112个变量
        int index = 0;
        //S
        offsets[index++] = new Vector2Int(-1, 0);
        offsets[index++] = new Vector2Int(0, 0);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(1, 1);

        offsets[index++] = new Vector2Int(0, 2);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(1, 1);
        offsets[index++] = new Vector2Int(1, 0);

        offsets[index++] = new Vector2Int(-1, 0);
        offsets[index++] = new Vector2Int(0, 0);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(1, 1);

        offsets[index++] = new Vector2Int(0, 2);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(1, 1);
        offsets[index++] = new Vector2Int(1, 0);

        //Z
        offsets[index++] = new Vector2Int(-1, 1);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(0, 0);
        offsets[index++] = new Vector2Int(1, 0);

        offsets[index++] = new Vector2Int(0, 2);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(-1, 1);
        offsets[index++] = new Vector2Int(-1, 0);

        offsets[index++] = new Vector2Int(-1, 1);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(0, 0);
        offsets[index++] = new Vector2Int(1, 0);

        offsets[index++] = new Vector2Int(0, 2);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(-1, 1);
        offsets[index++] = new Vector2Int(-1, 0);

        //J
        offsets[index++] = new Vector2Int(0, 2);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(0, 0);
        offsets[index++] = new Vector2Int(-1, 0);

        offsets[index++] = new Vector2Int(1, 1);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(-1, 1);
        offsets[index++] = new Vector2Int(-1, 2);

        offsets[index++] = new Vector2Int(0, 0);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(0, 2);
        offsets[index++] = new Vector2Int(1, 2);

        offsets[index++] = new Vector2Int(-1, 1);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(1, 1);
        offsets[index++] = new Vector2Int(1, 0);

        //L
        offsets[index++] = new Vector2Int(0, 2);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(0, 0);
        offsets[index++] = new Vector2Int(1, 0);

        offsets[index++] = new Vector2Int(1, 1);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(-1, 1);
        offsets[index++] = new Vector2Int(-1, 0);

        offsets[index++] = new Vector2Int(0, 0);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(0, 2);
        offsets[index++] = new Vector2Int(-1, 2);

        offsets[index++] = new Vector2Int(-1, 1);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(1, 1);
        offsets[index++] = new Vector2Int(1, 2);

        //I
        offsets[index++] = new Vector2Int(0, 3);
        offsets[index++] = new Vector2Int(0, 2);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(0, 0);

        offsets[index++] = new Vector2Int(-1, 1);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(1, 1);
        offsets[index++] = new Vector2Int(2, 1);

        offsets[index++] = new Vector2Int(0, 3);
        offsets[index++] = new Vector2Int(0, 2);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(0, 0);

        offsets[index++] = new Vector2Int(-1, 1);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(1, 1);
        offsets[index++] = new Vector2Int(2, 1);

        //O
        offsets[index++] = new Vector2Int(0, 0);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(1, 0);
        offsets[index++] = new Vector2Int(1, 1);

        offsets[index++] = new Vector2Int(0, 0);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(1, 0);
        offsets[index++] = new Vector2Int(1, 1);

        offsets[index++] = new Vector2Int(0, 0);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(1, 0);
        offsets[index++] = new Vector2Int(1, 1);

        offsets[index++] = new Vector2Int(0, 0);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(1, 0);
        offsets[index++] = new Vector2Int(1, 1);

        //T
        offsets[index++] = new Vector2Int(0, 0);
        offsets[index++] = new Vector2Int(-1, 0);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(1, 0);

        offsets[index++] = new Vector2Int(0, 0);
        offsets[index++] = new Vector2Int(0, 1);
        offsets[index++] = new Vector2Int(1, 0);
        offsets[index++] = new Vector2Int(0, -1);

        offsets[index++] = new Vector2Int(0, 0);
        offsets[index++] = new Vector2Int(1, 0);
        offsets[index++] = new Vector2Int(0, -1);
        offsets[index++] = new Vector2Int(-1, 0);

        offsets[index++] = new Vector2Int(0, 0);
        offsets[index++] = new Vector2Int(0, -1);
        offsets[index++] = new Vector2Int(-1, 0);
        offsets[index++] = new Vector2Int(0, 1);
    }

    /// <summary>
    /// 对每种方块的四种旋转状态定义其四个小块的位置
    /// </summary>
    /// <param name="blockType"></param>
    /// <param name="rotate">只能是0、1、2、3，分别代表初始状态和顺时针旋转了1、2、3次后的状态</param>
    /// <returns></returns>
    public static Vector2Int[] GetOffsets(BlockType blockType, int rotate)
    {
        Vector2Int[] result = new Vector2Int[4];
        int baseIndex = (int)blockType * 16 + rotate * 4;
        result[0] = offsets[baseIndex++];
        result[1] = offsets[baseIndex++];
        result[2] = offsets[baseIndex++];
        result[3] = offsets[baseIndex++];
        return result;
    }
}
