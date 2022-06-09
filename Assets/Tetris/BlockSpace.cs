using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpace : NonsensicalMono
{
    [SerializeField] private GameObject blockPrefab;    //方块预制体
    private bool[,] board; //只保存已经固定的方块

    private BlockType nextBlock;    //下一个方块类型

    private GameObjectPool blockPool;   //方块对象池

    private BlockController[] crtControl;   //当前控制的方块

    private float dropInterval; //下落间隔

    private int crtRotate; //当前控制方块的旋转状态

    private Vector2Int pointer;    //保存控制方块的指针位置，保证连续生成时的位置不会乱动

    protected override void Awake()
    {
        base.Awake();

        blockPool = new GameObjectPool(blockPrefab, (go) => go.SetActive(false), (go) => go.SetActive(true), (go) => go.GetComponent<BlockController>().pool = blockPool);
        Fill();
        Subscribe<int>("startTetris", OnStart);
    }

    /// <summary>
    /// 填充区域，测试用
    /// </summary>
    private void Fill()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 24; j++)
            {
                var go = Instantiate(blockPrefab, transform);
                go.transform.position = new Vector3(i, j, 0);
            }
        }
    }

    /// <summary>
    /// 开始
    /// </summary>
    /// <param name="difficulty">难度</param>
    private void OnStart(int difficulty)
    {
        if (difficulty < 0)
        {
            difficulty = 0;
        }
        else if (difficulty > 9)
        {
            difficulty = 9;
        }
        pointer = new Vector2Int(5, 21);
        dropInterval = 0.1f + 0.1f * difficulty;
        board = new bool[10, 24];//由于方块是从屏幕外生成的，最高的情况可能是生成一个I型方块后直接停靠，即最高高度为20+4
        RandomNext();   //一开始需要随机出下一个方块类型用于第一次创建
        CreateNew();
        StartCoroutine(Running());
    }

    /// <summary>
    /// 运行，负责不停的调用下坠方法
    /// </summary>
    /// <returns></returns>
    private IEnumerator Running()
    {
        float interval = dropInterval;
        WaitForSeconds wfs = new WaitForSeconds(interval);
        while (true)
        {
            if (interval != dropInterval)
            {
                interval = dropInterval;
                wfs = new WaitForSeconds(interval);
            }
            Drop();
            yield return wfs;
        }
    }


    /// <summary>
    /// 当前操作对象下落
    /// </summary>
    private void Drop()
    {

    }


    /// <summary>
    /// 随机出下一个方块
    /// </summary>
    private void RandomNext()
    {
        nextBlock = (BlockType)Random.Range(1, 8);
    }

    /// <summary>
    /// 旋转方块
    /// </summary>
    private void RotateBlock()
    {

    }

    /// <summary>
    /// 生成新方块
    /// </summary>
    private void CreateNew()
    {
        crtRotate = 0;
        switch (nextBlock)
        {
            case BlockType.S:
                CorrectPointerPos(true, true);
                break;
            case BlockType.Z:
                CorrectPointerPos(true, true);
                break;
            case BlockType.J:
                CorrectPointerPos(true, false);
                break;
            case BlockType.L:
                CorrectPointerPos(false, true);
                break;
            case BlockType.I:
                CorrectPointerPos(false, false);
                break;
            case BlockType.O:
                CorrectPointerPos(false, true);
                break;
            case BlockType.T:
                CorrectPointerPos(true, true);
                break;
            default:
                Debug.LogError("下一个方块的类型错误");
                break;
        }

        var offsets = TetrisBlockDefine.GetOffsets(nextBlock, crtRotate);
        for (int i = 0; i < 4; i++)
        {
            var go = blockPool.New();
            crtControl[i] = go.GetComponent<BlockController>();
            crtControl[i].Create(pointer+ offsets[i], nextBlock);
        }

        RandomNext();
    }

    /// <summary>
    /// 纠正指针位置
    /// 比如，当上一个方块为I，在靠边时停靠，下一个方块为Z，如果指针位置不变的话方块会在边框外生成
    /// </summary>
    /// <param name="left">左侧是否需要留空</param>
    /// <param name="right">右侧是否需要留空</param>
    private void CorrectPointerPos(bool left, bool right)
    {
        int x = pointer.x;
        int y = 21;
        if (left && pointer.x == 0)
        {
            x = 1;
        }
        if (right && pointer.x == 9)
        {
            x = 8;
        }
        pointer = new Vector2Int(x, y);
    }
}
