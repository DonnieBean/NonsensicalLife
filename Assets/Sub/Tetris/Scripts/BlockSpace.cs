using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϸ�߼�����
/// </summary>
public class BlockSpace : NonsensicalMono
{
    [SerializeField] private GameObject blockPrefab;    //����Ԥ����
    [SerializeField] private GameObject whiteCube;    //�߿򷽿�Ԥ����
    [SerializeField] private float inputInterval = 0.1f;   //������

    private bool[,] board; //ֻ�����Ѿ��̶��ķ���

    private BlockType nextBlock;    //��һ����������
    private BlockType crtBlock;     //��ǰ��������
    private bool isRunning; //�Ƿ���������

    private GameObjectPool blockPool;   //��������

    private BlockController[] crtControl = new BlockController[TetrisDefine.blockNum];   //��ǰ���Ƶķ���

    private float dropInterval; //������

    private int crtRotate; //��ǰ���Ʒ������ת״̬

    private Vector2Int pointer;    //������Ʒ����ָ��λ�ã���֤��������ʱ��λ�ò����Ҷ�

    private float inputTimer;   //�����ʱ��

    BlockController[] previews = new BlockController[TetrisDefine.blockNum];    //Ԥ������
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
    /// ������򣬲�����
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
    /// �����߿�
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
    /// ��ʼ
    /// </summary>
    /// <param name="difficulty">�Ѷ�</param>
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
        RandomNext();   //һ��ʼ��Ҫ�������һ�������������ڵ�һ�δ���
        CreateNew();
        StartCoroutine(Running());
    }

    /// <summary>
    /// ���У�����ͣ�ĵ�����׹����
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
    /// ��ǰ��������ͣ�������һ�μ�⣬�������˵�����
    /// </summary>
    private void ParkingCheck()
    {
        bool[] full = new bool[TetrisDefine.baseHeight];

        int fullLineCount = 0;
        //������˵�����
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

            //����׹
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

        //����������λ�ã����ڷ�������ζ�Ŵ��ʧ������
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
    /// ���ʧ������
    /// </summary>
    private void GameOver()
    {
        isRunning = false;
        StopAllCoroutines();

        Debug.Log("��Ϸ����");
    }

    /// <summary>
    /// �������һ������
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
    /// �ƶ�����
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="checkParking">�Ƿ���ͣ��</param>
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
                //ͣ����ʹ�õ�ǰλ�ý��д���Ȼ�󴴽��¶���
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
            //δͣ���������ƶ�
            for (int i = 0; i < TetrisDefine.blockNum; i++)
            {
                crtControl[i].ChangePos(blockOffsets[i] + targetPointer);
            }

            pointer = targetPointer;
        }
    }

    /// <summary>
    /// ��ת����
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
    /// �����λ���Ƿ�Ϸ����Ƿ񳬳��߽�����Ѿ��з��飩
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool CheckNewPos(int x, int y)
    {
        if (x < 0 || x > 9 || y < 0)  //������Ϸ�
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
    /// �����·���
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
                Debug.LogError("��һ����������ʹ���");
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
    /// ����ָ��λ��
    /// ���磬����һ������ΪI���ڿ���ʱͣ������һ������ΪZ�����ָ��λ�ò���Ļ�������ڱ߿�������
    /// </summary>
    /// <param name="left">����Ƿ���Ҫ����</param>
    /// <param name="right">�Ҳ��Ƿ���Ҫ����</param>
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
