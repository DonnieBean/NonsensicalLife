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
/// ����˹���鶨��
/// </summary>
public static class TetrisDefine
{
    public static int baseWidth = 10;   //����������
    public static int baseHeight = 20;  //��������߶�
    public static int extraHeight = 24; //��������������ĸ߶ȣ���ߵ������������i����20+4
    public static int blockNum = 4; //ÿ�����������С����

    public static Vector2Int[] offsets; //С��ƫ������

    /// <summary>
    /// ��̬���캯��,��ʼ��С��ƫ������
    /// </summary>
    static TetrisDefine()
    {
        offsets = new Vector2Int[112];    //7�����ͣ�ÿ���ĸ���ת״̬��ÿ��״̬�ĸ�С�飬��7*4*4=112������
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
    /// ��ÿ�ַ����������ת״̬�������ĸ�С���λ��
    /// </summary>
    /// <param name="blockType"></param>
    /// <param name="rotate">ֻ����0��1��2��3���ֱ�����ʼ״̬��˳ʱ����ת��1��2��3�κ��״̬</param>
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
