using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : CommonInteractive
{
    public override void CheckItem(ItemName itemName)
    {
        EmptyAction();
    }

    public override void EmptyAction()
    {
        base.EmptyAction();
        EventHandler.CallAlienationEvent();
    }
}
