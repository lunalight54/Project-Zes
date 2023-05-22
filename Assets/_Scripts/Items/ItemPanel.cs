using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPanel : MonoBehaviour
{
    public ItemContainer itemContainer;
    public List<InventoryButton> buttons;
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        SetIndex();
        Show();
    }

    private void SetIndex()
    {
        for (int i = 0; i < buttons.Count ; i++)
        {
            buttons[i].SetIndex(i);
        }
    }

    private void OnEnable()
    {
        Show();
    }

    public virtual void Show()
    {
        for (int i = 0; i < itemContainer.slots.Count && i<buttons.Count; i++)
        {
            if (itemContainer.slots[i].item == null)
            {
                buttons[i].Clean();
            }
            else
            {
                buttons[i].Set(itemContainer.slots[i]);
            }
        }
    }
    public virtual void OnClick(int id)
    {
        
    }
}
