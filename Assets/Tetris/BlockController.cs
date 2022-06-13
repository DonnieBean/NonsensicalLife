using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 小方块控制类
/// </summary>
public class BlockController : NonsensicalMono
{
    [SerializeField] private Material[] materials;  //材质球数组
    [SerializeField] private MeshRenderer render;   //渲染组件

    public GameObjectPool pool { get; set; }    //所属对象池
    private Vector2Int pos; //当前位置

    protected override void Awake()
    {
        base.Awake();

        Subscribe<int>("tetrisClearLine", Clear);
        Subscribe<int,int>("tetrisFallen", Fallen);
    }

    /// <summary>
    /// 创建
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
    /// 改变位置
    /// </summary>
    /// <param name="pos"></param>
    public void ChangePos(Vector2Int pos)
    {
        this.pos = pos;
        UpdatePos();
    }

    /// <summary>
    /// 清除某一行
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
    /// 某一行下落到另一行
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
    /// 更新实际位置
    /// </summary>
    private void UpdatePos()
    {
        transform.position = new Vector3(pos.x,pos.y,0) ;
    }
}
