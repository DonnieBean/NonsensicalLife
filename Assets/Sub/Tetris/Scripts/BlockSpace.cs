using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏逻辑管理
/// </summary>
public class BlockSpace : NonsensicalMono
{
    [SerializeField] private GameObject blockPrefab;    //方块预制体
    [SerializeField] private GameObject whiteCube;    //边框方块预制体
    [SerializeField] private float inputInterval = 0.1f;   //输入间隔

    private bool[,] board; //只保存已经固定的方块

    private BlockType nextBlock;    //下一个方块类型
    private BlockType crtBlock;     //当前方块类型
    private bool isRunning; //是否正在运行

    private GameObjectPool blockPool;   //方块对象池

    private BlockController[] crtControl = new BlockController[TetrisDefine.blockNum];   //当前控制的方块

    private float dropInterval; //下落间隔

    private int crtRotate; //当前控制方块的旋转状态

    private Vector2Int pointer;    //保存控制方块的指针位置，保证连续生成时的位置不会乱动

    private float inputTimer;   //输入计时器

    BlockController[] previews = new BlockController[TetrisDefine.blockNum];    //预览方块
    protected override void Awake()
    {
        base.Awake();
        blockPool = new GameObjectPool(blockPrefab, (go) => go.SetActive(false), (go) => go.SetActive(true), (go) => { go.GetComponent<BlockController>().pool = blockPool; go.transform.SetParent(transform); });
        Subscribe<int>("startTetris", OnStart);
        Borders();
    }

    private void Update()
    {
        if (isRunning)
        {
            inputTimer += Time.deltaTime;
            if (inputTimer > inputInterval)
            {
                if (Input.GetKey(KeyCode.S))
                {
                    inputTimer = 0;
                    Move(new Vector2Int(0, -1), false);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    inputTimer = 0;
                    Move(new Vector2Int(1, 0), false);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    inputTimer = 0;
                    Move(new Vector2Int(-1, 0), false);
                }
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                RotateBlock();
            }
        }
    }

    /// <summary>
    /// 填充区域，测试用
    /// </summary>
    private void Fill()
    {
        for (int i = 0; i < TetrisDefine.baseWidth; i++)
        {
            for (int j = 0; j < TetrisDefine.baseHeight; j++)
            {
                var go = Instantiate(blockPrefab, transform);
                go.transform.position = new Vector3(i, j, 0);
            }
        }
    }

    /// <summary>
    /// 创建边框
    /// </summary>
    private void Borders()
    {
        for (int i = -1; i < TetrisDefine.baseWidth + 1; i++)
        {
            var go = Instantiate(whiteCube, transform);
            go.transform.position = new Vector3(i, -1, 0);
        }
        for (int i = 0; i < TetrisDefine.baseHeight; i++)
        {
            var go = Instantiate(whiteCube, transform);
            go.transform.position = new Vector3(-1, i, 0);
            go = Instantiate(whiteCube, transform);
            go.transform.position = new Vector3(TetrisDefine.baseWidth, i, 0);
        }
    }

    /// <summary>
    /// 开始
    /// </summary>
    /// <param name="difficulty">难度</param>
    private void OnStart(int difficulty)
    {
        Debug.Log("start");
        Publish("scoreReset");
        if (isRunning)
        {
            return;
        }
        isRunning = true;
        if (difficulty < 0)
        {
            difficulty = 0;
        }
        else if (difficulty > 9)
        {
            difficulty = 9;
        }
        pointer = new Vector2Int(5, 21);
        dropInterval = 0.1f + 0.1f * (9 - difficulty);
        board = new bool[TetrisDefine.baseWidth, TetrisDefine.extraHeight];
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
            Move(new Vector2Int(0, -1), true);
            yield return wfs;
        }
    }

    /// <summary>
    /// 当前操作对象停靠后进行一次检测，消除满了的行数
    /// </summary>
    private void ParkingCheck()
    {
        bool[] full = new bool[TetrisDefine.baseHeight];

        int fullLineCount = 0;
        //检测满了的行数
        for (int y = 0; y < TetrisDefine.baseHeight; y++)
        {
            bool havaHole = false;
            for (int x = 0; x < TetrisDefine.baseWidth; x++)
            {
                if (board[x, y] == false)
                {
                    havaHole = true;
                    break;
                }
            }
            if (havaHole)
            {
                full[y] = false;
            }
            else
            {
                fullLineCount++;
                full[y] = true;
            }
        }

        if (fullLineCount > 0)
        {
            Publish<int>("scoreAdd", (int)(Mathf.Pow(2, fullLineCount + 1) * 100) / (int)(dropInterval * 10));

            //行下坠
            int baseLine = 0;
            for (int y = 0; y < TetrisDefine.baseHeight; y++)
            {
                if (full[y])
                {
                    for (int x = 0; x < TetrisDefine.baseWidth; x++)
                    {
                        board[x, y] = false;
                    }
                    Publish("tetrisClearLine", y);
                }
                else
                {
                    if (y != baseLine)
                    {
                        for (int x = 0; x < TetrisDefine.baseWidth; x++)
                        {
                            board[x, baseLine] = board[x, y];
                            board[x, y] = false;
                        }
                        Publish("tetrisFallen", y, baseLine);
                    }

                    baseLine++;
                }
            }
        }

        //检查区域外的位置，存在方块则意味着达成失败条件
        for (int y = TetrisDefine.baseHeight; y < TetrisDefine.extraHeight; y++)
        {
            for (int x = 0; x < TetrisDefine.baseWidth; x++)
            {
                if (board[x, y])
                {
                    GameOver();
                    return;
                }
            }
        }
    }

    /// <summary>
    /// 达成失败条件
    /// </summary>
    private void GameOver()
    {
        isRunning = false;
        StopAllCoroutines();

        Debug.Log("游戏结束");
    }

    /// <summary>
    /// 随机出下一个方块
    /// </summary>
    private void RandomNext()
    {
        nextBlock = (BlockType)Random.Range(0, 7);
        ChangePreview();
    }


    private void ChangePreview()
    {
        foreach (var item in previews)
        {
            if (item != null)
            {
                blockPool.Store(item.gameObject);
            }
        }
        var offsets = TetrisDefine.GetOffsets(nextBlock, 0);
        for (int i = 0; i < TetrisDefine.blockNum; i++)
        {
            var go = blockPool.New();
            previews[i] = go.GetComponent<BlockController>();
            previews[i].Create(new Vector2Int(13, 13) + offsets[i], nextBlock);
        }
    }

    /// <summary>
    /// 移动方块
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="checkParking">是否检查停靠</param>
    private void Move(Vector2Int offset, bool checkParking)
    {
        var targetPointer = pointer + offset;

        var blockOffsets = TetrisDefine.GetOffsets(crtBlock, crtRotate);
        bool parking = false;
        for (int i = 0; i < TetrisDefine.blockNum; i++)
        {
            var targetPoint = blockOffsets[i] + targetPointer;
            if (CheckNewPos(targetPoint.x, targetPoint.y) == false)
            {
                parking = true;
                break;
            }
        }

        if (parking)
        {
            if (checkParking)
            {
                //停靠则使用当前位置进行处理，然后创建新对象
                for (int i = 0; i < TetrisDefine.blockNum; i++)
                {
                    var point = blockOffsets[i] + pointer;

                    board[point.x, point.y] = true;
                }
                ParkingCheck();
                CreateNew();
            }
        }
        else
        {
            //未停靠则正常移动
            for (int i = 0; i < TetrisDefine.blockNum; i++)
            {
                crtControl[i].ChangePos(blockOffsets[i] + targetPointer);
            }

            pointer = targetPointer;
        }
    }

    /// <summary>
    /// 旋转方块
    /// </summary>
    private void RotateBlock()
    {
        int targetRotate = crtRotate + 1;
        if (targetRotate > 3)
        {
            targetRotate = 0;
        }
        var newPoints = TetrisDefine.GetOffsets(crtBlock, targetRotate);

        for (int i = 0; i < TetrisDefine.blockNum; i++)
        {
            var newPoint = newPoints[i] + pointer;
            if (CheckNewPos(newPoint.x, newPoint.y) == false)
            {
                return;
            }
        }
        crtRotate = targetRotate;
        for (int i = 0; i < TetrisDefine.blockNum; i++)
        {
            crtControl[i].ChangePos(newPoints[i] + pointer);
        }
    }

    /// <summary>
    /// 检查新位置是否合法（是否超出边界或者已经有方块）
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool CheckNewPos(int x, int y)
    {
        if (x < 0 || x > 9 || y < 0)  //不检测上方
        {
            return false;
        }
        if (board[x, y] == true)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 生成新方块
    /// </summary>
    private void CreateNew()
    {
        crtRotate = 0;
        crtBlock = nextBlock;
        switch (crtBlock)
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
                return;
        }

        var offsets = TetrisDefine.GetOffsets(crtBlock, crtRotate);
        for (int i = 0; i < TetrisDefine.blockNum; i++)
        {
            var go = blockPool.New();
            crtControl[i] = go.GetComponent<BlockController>();
            crtControl[i].Create(pointer + offsets[i], crtBlock);
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
        int y = 20;
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
