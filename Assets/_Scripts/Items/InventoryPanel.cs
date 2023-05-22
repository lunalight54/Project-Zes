using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class InventoryPanel : ItemPanel
{
    public override void OnClick(int id)
    {
        //GameManager.instance.itemDragAndDropController.OnClick(itemContainer.slots[id]);
        Show();
        
    }
}