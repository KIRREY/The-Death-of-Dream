using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationData_SO", menuName = "Animation/AnimationData_SO")]
public class AnimationData_SO : ScriptableObject
{
    public List<AnimationDatas> animDatas;
}
[System.Serializable]
public class AnimationDatas
{
    public Texture2D animation;
}
