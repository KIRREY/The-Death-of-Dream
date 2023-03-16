using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    public ItemName requireItem;
    public bool isDone;

    public virtual void CheckItem(ItemName itemName)
    {
        if (itemName == requireItem && !isDone)
        {
            isDone = true;
            OnAction();
        }
        else
        {
            if(requireItem==ItemName.None)
                EmptyAction();
        }
    }

    /// <summary>
    /// Ĭ������ȷ����Ʒ���������
    /// </summary>
    protected virtual void OnAction()
    {

    }

    public virtual void EmptyAction()
    {
        Debug.Log("�յ�");
    }
}
