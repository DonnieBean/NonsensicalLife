using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Empty,
    S,
    Z,
    J,
    L,
    I,
    O,
    T,
}


/// <summary>
/// 方块定义
/// </summary>
public class TetrisBlockDefine 
{
    /// <summary>
    /// 对每种方块的四种旋转状态定义其四个小块的位置
    /// </summary>
    /// <param name="blockType"></param>
    /// <param name="rotate">只能是0、1、2、3，分别代表初始状态和顺时针旋转了1、2、3次后的状态</param>
    /// <returns></returns>
    public static Vector2Int[] GetOffsets(BlockType blockType,int rotate)
    {
        Vector2Int[] offsets = new Vector2Int[3];

        switch (blockType)
        {
            case BlockType.S:
                break;
            case BlockType.Z:
                break;
            case BlockType.J:
                break;
            case BlockType.L:
                break;
            case BlockType.I:
                break;
            case BlockType.O:
                break;
            case BlockType.T:
                break;
        }

        return offsets;
    }
}
