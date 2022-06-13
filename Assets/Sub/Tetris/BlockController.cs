using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// С���������
/// </summary>
public class BlockController : NonsensicalMono
{
    [SerializeField] private Material[] materials;  //����������
    [SerializeField] private MeshRenderer render;   //��Ⱦ���

    public GameObjectPool pool { get; set; }    //���������
    private Vector2Int pos; //��ǰλ��

    protected override void Awake()
    {
        base.Awake();

        Subscribe<int>("tetrisClearLine", Clear);
        Subscribe<int,int>("tetrisFallen", Fallen);
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="blockType"></param>
    public void Create(Vector2Int pos,BlockType blockType)
    {
        this.pos = pos;
        UpdatePos();
        render.material = materials[(int)blockType];
    }

    /// <summary>
    /// �ı�λ��
    /// </summary>
    /// <param name="pos"></param>
    public void ChangePos(Vector2Int pos)
    {
        this.pos = pos;
        UpdatePos();
    }

    /// <summary>
    /// ���ĳһ��
    /// </summary>
    /// <param name="line"></param>
    private void Clear(int line)
    {
        if (gameObject.activeSelf&& line==pos.y)
        {
            pool.Store(gameObject);
        }
    }

    /// <summary>
    /// ĳһ�����䵽��һ��
    /// </summary>
    /// <param name="oldLine"></param>
    /// <param name="newLine"></param>
    private void Fallen(int oldLine,int newLine)
    {
        if (gameObject.activeSelf && oldLine == pos.y)
        {
            this.pos = new Vector2Int(pos.x, newLine);
            UpdatePos();
        }
    }

    /// <summary>
    /// ����ʵ��λ��
    /// </summary>
    private void UpdatePos()
    {
        transform.position = new Vector3(pos.x,pos.y,0) ;
    }
}
