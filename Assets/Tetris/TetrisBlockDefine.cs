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
/// ���鶨��
/// </summary>
public class TetrisBlockDefine 
{
    /// <summary>
    /// ��ÿ�ַ����������ת״̬�������ĸ�С���λ��
    /// </summary>
    /// <param name="blockType"></param>
    /// <param name="rotate">ֻ����0��1��2��3���ֱ�����ʼ״̬��˳ʱ����ת��1��2��3�κ��״̬</param>
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
