using NonsensicalKit;
using NonsensicalKit.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MainMenuConfig", menuName = "ScriptableObjects/MainMenuConfig")]
public class MainMenuConfig : NonsensicalConfigDataBase
{
    public MainMenuData data;
    public override ConfigDataBase GetData()
    {
        return data;
    }

    public override void SetData(ConfigDataBase cd)
    {
        if (CheckType<MainMenuData>(cd))
        {
            data = cd as MainMenuData;
        }
    }
}

[System.Serializable]
public class MainMenuData: ConfigDataBase
{
    public List<MainMenuElementData> datas;
}

[System.Serializable]
public class MainMenuElementData
{
    public string name;
    public string previewPath;
    public string sceneName;
}
