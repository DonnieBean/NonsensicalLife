using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpace : NonsensicalMono
{
    [SerializeField] private GameObject blockPrefab;    //����Ԥ����
    private bool[,] board; //ֻ�����Ѿ��̶��ķ���

    private BlockType nextBlock;    //��һ����������

    private GameObjectPool blockPool;   //��������

    private BlockController[] crtControl;   //��ǰ���Ƶķ���

    private float dropInterval; //������

    private int crtRotate; //��ǰ���Ʒ������ת״̬

    private Vector2Int pointer;    //������Ʒ����ָ��λ�ã���֤��������ʱ��λ�ò����Ҷ�

    protected override void Awake()
    {
        base.Awake();

        blockPool = new GameObjectPool(blockPrefab, (go) => go.SetActive(false), (go) => go.SetActive(true), (go) => go.GetComponent<BlockController>().pool = blockPool);
        Fill();
        Subscribe<int>("startTetris", OnStart);
    }

    /// <summary>
    /// ������򣬲�����
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
    /// ��ʼ
    /// </summary>
    /// <param name="difficulty">�Ѷ�</param>
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
        board = new bool[10, 24];//���ڷ����Ǵ���Ļ�����ɵģ���ߵ��������������һ��I�ͷ����ֱ��ͣ��������߸߶�Ϊ20+4
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
            Drop();
            yield return wfs;
        }
    }


    /// <summary>
    /// ��ǰ������������
    /// </summary>
    private void Drop()
    {

    }


    /// <summary>
    /// �������һ������
    /// </summary>
    private void RandomNext()
    {
        nextBlock = (BlockType)Random.Range(1, 8);
    }

    /// <summary>
    /// ��ת����
    /// </summary>
    private void RotateBlock()
    {

    }

    /// <summary>
    /// �����·���
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
                Debug.LogError("��һ����������ʹ���");
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
    /// ����ָ��λ��
    /// ���磬����һ������ΪI���ڿ���ʱͣ������һ������ΪZ�����ָ��λ�ò���Ļ�������ڱ߿�������
    /// </summary>
    /// <param name="left">����Ƿ���Ҫ����</param>
    /// <param name="right">�Ҳ��Ƿ���Ҫ����</param>
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
