using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterForbbid : MonoBehaviour
{
    /*public float velocity;
    public Vector3 Transform;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Transform=collision.transform.position;
        Debug.Log("ccccccccccccccccccccccccccccccccccxxxxxxxxxxxxxxxkkkkkkkkkkkkkkk");
        TopDownCharacterController play = collision.gameObject.GetComponent<TopDownCharacterController>();
        if(play!=null)
        {
            Debug.Log("warning");
            if(!play.alienation)
            {
                //InvokeRepeating("Check", 0, Time.fixedDeltaTime);
                StartCoroutine(PlayerMove());
            }
            else
            {
                gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }
    }
    void Check()
    {
        float distance = (Transform - DataManager.instance.player.transform.position).sqrMagnitude;
        Debug.Log(distance);
        if(distance>5f)
        {
            StopCoroutine(PlayerMove());
        }

    }
    public IEnumerator  PlayerMove()
    {
        while (true)
        {
            float distance = (Transform - DataManager.instance.player.transform.position).sqrMagnitude;
            Debug.Log(distance);
            if (distance > 5f)
            {
                StopCoroutine(PlayerMove());
                break;
            }
            DataManager.instance.player.transform.position = Vector3.MoveTowards(DataManager.instance.player.transform.position, DataManager.instance.player.transform.position - new Vector3(5, 0, 0), velocity);
            yield return new WaitForFixedUpdate();
        }
    }*/
}
