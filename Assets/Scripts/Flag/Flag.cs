using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Flag : MonoBehaviour
{
    public abstract void FlagEvent();
}
