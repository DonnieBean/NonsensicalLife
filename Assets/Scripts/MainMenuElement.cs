using NonsensicalKit.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuElement : ListTableElement<MainMenuElementData>
{
    [SerializeField] private Button btn_select;
    [SerializeField] private Image img_preview;
    [SerializeField] private TextMeshProUGUI txt_name;

    private string sceneName;

    protected override void Awake()
    {
        base.Awake();
        btn_select.onClick.AddListener(OnSelect);
    }

    public override void SetValue(MainMenuElementData elementData)
    {
        base.SetValue(elementData);

        if (string.IsNullOrEmpty(elementData.previewPath) == false)
        {
            Sprite sp = Resources.Load<Sprite>("Sprites/"+elementData.previewPath);
            img_preview.sprite = sp;
            txt_name.text = elementData.name;
            sceneName = elementData.sceneName;
        }
    }

    private void OnSelect()
    {
        Publish("loadScene", sceneName, true);
    }
}
