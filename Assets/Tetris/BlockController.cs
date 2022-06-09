using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : NonsensicalMono
{
    [SerializeField] private Material[] materials;
    [SerializeField] private MeshRenderer render;

    public GameObjectPool pool { get; set; }
    public Vector2Int pos { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Subscribe<int>("tetrisClearLine", Clear);
        Subscribe<int,int>("tetrisFallen", Fallen);
    }


    public void Create(Vector2Int pos,BlockType blockType)
    {
        this.pos = pos;
        UpdatePos();
        render.material = materials[(int)blockType-1];
    }

    public void ChangePos(Vector2Int pos)
    {
        this.pos = pos;
        UpdatePos();
    }

    public void Down()
    {
        this.pos=new Vector2Int(pos.x,pos.y-1);
        UpdatePos();
    }

    public void Left()
    {
        this.pos = new Vector2Int(pos.x - 1, pos.y );
        UpdatePos();
    }

    public void Right()
    {
        this.pos = new Vector2Int(pos.x+1, pos.y );
        UpdatePos();
    }

    public void Clear(int line)
    {
        if (gameObject.activeSelf&& line==pos.y)
        {
            pool.Store(gameObject);
        }
    }

    private void Fallen(int oldLine,int newLine)
    {
        if (gameObject.activeSelf && oldLine == pos.y)
        {
            this.pos = new Vector2Int(pos.x, newLine);
            UpdatePos();
        }
    }

    private void UpdatePos()
    {
        transform.position = new Vector3(pos.x,pos.y,0) ;
    }
}
