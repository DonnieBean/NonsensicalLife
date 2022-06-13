using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Դhttps://blog.csdn.net/suprman/article/details/1506363
/// </summary>
public static class SokobanData
{
    public static Dictionary<int, int[,]> levels;

    //��ʵ��չʾ���¾���
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
/// 0����գ�1���յأ�2��ǽ��3�����ӣ�4��Ŀ��㣬5�����
/// ����ʱ����״̬��6��������Ŀ����ϣ�7�������Ŀ�����
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
