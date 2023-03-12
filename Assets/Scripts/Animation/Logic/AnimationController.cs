using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private AnimationManager animationManager;
    public AnimationData_SO[] animationData_SOs;
    public AnimationData_SO currentData;
    public Stack<AnimationDatas> animationDataStack;

    private bool ifPlay;
    public int index;
    private void Start()
    {
        index = 0;
        animationManager=AnimationManager.Instance;
    }

    public void FillAnimationStack()
    {
        /*dialogueEmptyStack = new Stack<DialogueData>();
            for (int i = dialogueEmpty.dialogueList.Count - 1; i > -1; i--)
            {
                dialogueEmptyStack.Push(dialogueEmpty.dialogueList[i]);
            }*/
        animationDataStack=new Stack<AnimationDatas>();

        for (int i = currentData.animDatas.Count - 1; i > -1; i--)
        {
            animationDataStack.Push(currentData.animDatas[i]);
        }
    }

    public void ShowAnimation()
    {
        if (!ifPlay)
            StartCoroutine(AnimatoinRoutine(animationDataStack));
    }

    private IEnumerator AnimatoinRoutine(Stack<AnimationDatas> animationDatas)
    {
        ifPlay=true;
        if(animationDatas.TryPop(out var result))
        {
            EventHandler.CallGameStateChangerEvent(GameState.Pause);
            EventHandler.CallAnimationEvent(result);
            animationManager.controller = this.gameObject;
            yield return null;  
            ifPlay=false;
        }
        else
        {
            EventHandler.CallAnimationEvent(new AnimationDatas());
            if (index < (animationData_SOs.Length - 1))
            {
                index++;
                currentData = animationData_SOs[index];
            }
            FillAnimationStack();
            ifPlay = false;
        }
    }
}
