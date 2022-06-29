using NonsensicalKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SokobanMap : NonsensicalMono
{
    [SerializeField] private GameObject prefab_player;
    [SerializeField] private GameObject prefab_box;
    [SerializeField] private GameObject prefab_floor;
    [SerializeField] private GameObject prefab_wall;
    [SerializeField] private GameObject prefab_target;

    private GameObjectPool pool_player;
    private GameObjectPool pool_box;
    private GameObjectPool pool_wall;
    private GameObjectPool pool_floor;
    private GameObjectPool pool_target;

    private List<GameObject> crtPlayers;
    private List<GameObject> crtBoxes;
    private List<GameObject> crtFloors;
    private List<GameObject> crtWalls;
    private List<GameObject> crtTargets;

    private SokobanState[,] map;
    private GameObject[,] mapBoxes;
    private GameObject player;
    private Stack<SokobanState[,]> history;
    private Vector2Int playerPos;
    private int crtLevel;

    protected override void Awake()
    {
        base.Awake();
        crtPlayers = new List<GameObject>();
        crtBoxes = new List<GameObject>();
        crtWalls = new List<GameObject>();
        crtFloors = new List<GameObject>();
        crtTargets = new List<GameObject>();

        pool_player = new GameObjectPool(prefab_player, OnStoreObject, OnNewObject, OnFirstObject);
        pool_box = new GameObjectPool(prefab_box, OnStoreObject, OnNewObject, OnFirstObject);
        pool_wall = new GameObjectPool(prefab_wall, OnStoreObject, OnNewObject, OnFirstObject);
        pool_floor = new GameObjectPool(prefab_floor, OnStoreObject, OnNewObject, OnFirstObject);
        pool_target = new GameObjectPool(prefab_target, OnStoreObject, OnNewObject, OnFirstObject);

        history = new Stack<SokobanState[,]>();

        Subscribe<Vector2Int>("sokobanMove", Move);
        Subscribe("sokobanWithdraw", Withdraw);
        Subscribe("sokobanRestart", Restart);
        Subscribe<int>("sokobanStart", StartGame);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartGame(1);
        }
    }

    private void StartGame(int id)
    {
        crtLevel = id;
        if (GetStatesByMap(crtLevel, out var v))
        {
            history.Clear();
            CreateMap(v);
        }
    }

    private void Restart()
    {
        if (GetStatesByMap(crtLevel,out var v))
        {
            history.Clear();
            CreateMap(v);
        }
    }

    private bool GetStatesByMap(int id,out SokobanState[,]  result)
    {
        var v = SokobanData.levels[id];
        if (v == null)
        {
            result = null; 
            return false;
        }
        result = new SokobanState[v.GetLength(0), v.GetLength(1)];

        for (int i = 0; i < v.GetLength(0); i++)
        {
            for (int j = 0; j < v.GetLength(1); j++)
            {
                result[i, j] = (SokobanState)v[i, j];
            }
        }

        return true ;
    }

    private void Withdraw()
    {
        if (history.Count>0)
        {
            var v = history.Pop();
            CreateMap(v);
        }
    }

    private void OnNewObject(GameObject go)
    {
        go.SetActive(true);
    }

    private void OnStoreObject(GameObject go)
    {
        go.SetActive(false);
    }

    private void OnFirstObject(GameObject go)
    {
        go.transform.SetParent(transform);
    }

    private void ClearMap()
    {
        foreach (var item in crtPlayers)
        {
            pool_player.Store(item);
        }
        crtPlayers.Clear();
        foreach (var item in crtBoxes)
        {
            pool_box.Store(item);
        }
        crtBoxes.Clear();
        foreach (var item in crtWalls)
        {
            pool_wall.Store(item);
        }
        crtWalls.Clear();
        foreach (var item in crtTargets)
        {
            pool_target.Store(item);
        }
        crtTargets.Clear();
        foreach (var item in crtFloors)
        {
            pool_floor.Store(item);
        }
        crtFloors.Clear();

        if (mapBoxes != null)
        {
            Array.Clear(mapBoxes, 0, mapBoxes.Length);
        }
    }

    private void CreateMap(SokobanState[,] map)
    {
        ClearMap();
        this.map = new SokobanState[map.GetLength(0), map.GetLength(1)];
        mapBoxes = new GameObject[map.GetLength(0), map.GetLength(1)];

        Publish("setCameraPosition",
            new Vector3(map.GetLength(1) * 0.5f, map.GetLength(0) * 0.5f, -10)
            );
        Publish("setCameraSize", map.GetLength(0) * 1f);

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                this.map[y, x] = map[y, x];

                switch (map[y, x])
                {
                    case SokobanState.floor:
                        {
                            GameObject crt = pool_floor.New();
                            crtFloors.Add(crt);
                            crt.transform.position = new Vector3(x, y, 0);
                        }
                        break;
                    case SokobanState.wall:
                        {
                            GameObject crt = pool_wall.New();
                            crtWalls.Add(crt);
                            crt.transform.position = new Vector3(x, y, 0);

                            crt = pool_floor.New();
                            crtFloors.Add(crt);
                            crt.transform.position = new Vector3(x, y, 0);
                        }
                        break;
                    case SokobanState.box:
                        {
                            GameObject crt = pool_box.New();
                            crtBoxes.Add(crt);
                            crt.transform.position = new Vector3(x, y, 0);
                            mapBoxes[y, x] = crt;

                            crt = pool_floor.New();
                            crtFloors.Add(crt);
                            crt.transform.position = new Vector3(x, y, 0);
                        }
                        break;
                    case SokobanState.target:
                        {
                            GameObject crt = pool_target.New();
                            crtTargets.Add(crt);
                            crt.transform.position = new Vector3(x, y, 0);

                            crt = pool_floor.New();
                            crtFloors.Add(crt);
                            crt.transform.position = new Vector3(x, y, 0);
                        }
                        break;
                    case SokobanState.player:
                        {
                            GameObject crt = pool_player.New();
                            crtPlayers.Add(crt);
                            crt.transform.position = new Vector3(x, y, 0);
                            playerPos = new Vector2Int(x, y);
                            player = crt;

                            crt = pool_floor.New();
                            crtFloors.Add(crt);
                            crt.transform.position = new Vector3(x, y, 0);

                        }
                        break;
                    case SokobanState.boxOnTarget:
                        {
                            GameObject crt = pool_box.New();
                            crtBoxes.Add(crt);
                            crt.transform.position = new Vector3(x, y, 0);
                            mapBoxes[y, x] = crt;

                            crt = pool_target.New();
                            crtTargets.Add(crt);
                            crt.transform.position = new Vector3(x, y, 0);

                            crt = pool_floor.New();
                            crtFloors.Add(crt);
                            crt.transform.position = new Vector3(x, y, 0);

                        }
                        break;
                    case SokobanState.playerOnTarget:
                        {
                            GameObject crt = pool_player.New();
                            crtPlayers.Add(crt);
                            crt.transform.position = new Vector3(x, y, 0);
                            playerPos = new Vector2Int(x, y);
                            player = crt;

                            crt = pool_target.New();
                            crtTargets.Add(crt);
                            crt.transform.position = new Vector3(x, y, 0);

                            crt = pool_floor.New();
                            crtFloors.Add(crt);
                            crt.transform.position = new Vector3(x, y, 0);

                        }
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 不用对目标位置以及后面的位置进行范围判断，因为只要地图合理，超出范围的移动之前一定会碰到墙
    /// </summary>
    /// <param name="offset"></param>
    private void Move(Vector2Int offset)
    {
        var target = playerPos + offset;
        var targetBack = target + offset;
        bool moveBox = false;

        //筛选掉无效移动
        switch (map[target.y, target.x])
        {
            case SokobanState.floor:
            case SokobanState.target:
                break;
            case SokobanState.box:
            case SokobanState.boxOnTarget:
                moveBox = true;
                switch (map[targetBack.y, targetBack.x])
                {
                    case SokobanState.floor:
                    case SokobanState.target:
                        break;
                    default:
                        return;
                }
                break;
            default:
                return;
        }

        history.Push(map.Clone() as SokobanState[,]);

        player.transform.position = new Vector3(target.x, target.y, 0);

        if (moveBox)
        {
            var temp = mapBoxes[target.y, target.x];
            mapBoxes[target.y, target.x] = null;
            mapBoxes[targetBack.y, targetBack.x] = temp;
            temp.transform.position = new Vector3(targetBack.x, targetBack.y, 0);
        }

        if (map[playerPos.y, playerPos.x] == SokobanState.player)
        {
            map[playerPos.y, playerPos.x] = SokobanState.floor;
        }
        else
        {
            map[playerPos.y, playerPos.x] = SokobanState.target;
        }

        switch (map[target.y, target.x])
        {
            case SokobanState.floor:
            case SokobanState.box:
                map[target.y, target.x] = SokobanState.player;
                break;
            case SokobanState.target:
            case SokobanState.boxOnTarget:
                map[target.y, target.x] = SokobanState.playerOnTarget;
                break;
        }

        if (moveBox)
        {
            if (map[targetBack.y, targetBack.x] == SokobanState.floor)
            {
                map[targetBack.y, targetBack.x] = SokobanState.box;
            }
            else
            {
                map[targetBack.y, targetBack.x] = SokobanState.boxOnTarget;
            }
        }

        playerPos = target;
        CheckWin();
    }

    private void CheckWin()
    {
        foreach (var item in map)
        {
            if (item==SokobanState.box)
            {
                return;
            }
        }

        Publish("sokobanWin");
    }
}
