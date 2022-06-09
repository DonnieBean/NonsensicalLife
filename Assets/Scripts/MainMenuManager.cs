using NonsensicalKit.Manager;
using NonsensicalKit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : ListTableManager<MainMenuElement,MainMenuElementData>
{
    protected override void Start()
    {
        base.Start();

        if (AppConfigManager.Instance.TryGetConfig<MainMenuData>(out var v))
        {
            UpdateUI(v.datas);
        }
    }
}
