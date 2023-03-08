using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public Transform parent;
    public Transform origin;
    private void OnEnable()
    {
        this.gameObject.transform.SetParent(parent);
    }

    public void Reset()
    {
        try
        {
            this.gameObject.transform.SetParent(origin);
        }
        catch { }
        this.gameObject.SetActive(false);
    }
}
