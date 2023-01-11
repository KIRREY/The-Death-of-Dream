using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : MonoBehaviour
    {
       /* public bool alienation;
        public float speed;
        public float originalspeed;
        private Animator animator;
        //public GameObject Dialog;
        Rigidbody2D rigidbody2D;
        //Vector2 lookDirection = new Vector2(1, 0);
        float horizontal;
        float vertical;
        //bool isopen;
        public Light2D Light2D;
        [Range(-1, 1)]
        public float randomup;
        [Range(-1, 1)]
        public float randomdown;
        [Range(-1, 1)]
        public float randomright;
        [Range(-1, 1)]
        public float randomleft;
        public GameObject menu;
        private void Start()
        {
            Light2D.gameObject.SetActive(false);
            animator = GetComponent<Animator>();
            rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        }
        public bool isOpenMenu;
        public void RunMenu()
        {
            if(Input.GetKeyDown(KeyCode.X))
            {
                Time.timeScale=1-Time.timeScale;
                menu.SetActive(!isOpenMenu);
                isOpenMenu = !isOpenMenu;
            }
        }
        public bool lightRun;
        *//*[Range(0, 1)]
        public float hitdelta;
        [Range(0, 5)]
        public float hitdelta1;*//*
        private void Update()
        {
            speed = originalspeed;
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            RunMenu();
            Vector2 move = new Vector2(horizontal, vertical);
            if (Input.GetKeyDown(KeyCode.Q))
            {
                lightRun = !lightRun;
                Light2D.gameObject.SetActive(lightRun);
            }
            *//*if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                lookDirection.Set(move.x, move.y);
                lookDirection.Normalize();
            }
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
            {
                int layerMask = 1 << 2;
                layerMask = ~layerMask;
                RaycastHit2D hit = Physics2D.Raycast(rigidbody2D.position + Vector2.up * hitdelta, lookDirection, hitdelta1, layerMask);
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider != null && hit.collider.gameObject.tag == "Interactive")
                {
                    Dialog.transform.position = Camera.main.WorldToScreenPoint(transform.position);
                    isopen = !isopen;
                    Color color = Dialog.GetComponent<Image>().color;
                    color.a = 1 - color.a;
                    Dialog.GetComponent<Image>().color = color;
                    Dialog.transform.GetChild(0).gameObject.GetComponent<Text>().text = hit.transform.gameObject.name;
                }
            }
            if (Dialog.GetComponent<Image>().color.a == 0f)
            {
                Dialog.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
            }
            else
            {
                return;
            }*//*
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = 1.5f * speed;
            }
            Vector2 dir = Vector2.zero;
            if (Input.GetKey(KeyCode.A))
            {
                dir.x = -1;
                Light2D.gameObject.GetComponent<LightController>().randomy = randomleft;
                Light2D.transform.rotation = Quaternion.Euler(0, 0, 90);
                animator.SetInteger("Direction", 3);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                dir.x = 1;
                Light2D.gameObject.GetComponent<LightController>().randomy = randomright;
                Light2D.transform.rotation = Quaternion.Euler(0, 0, 270);
                animator.SetInteger("Direction", 2);
            }

            if (Input.GetKey(KeyCode.W))
            {
                dir.y = 1;
                Light2D.gameObject.GetComponent<LightController>().randomy = randomup;
                Light2D.transform.rotation = Quaternion.Euler(0, 0, 0);
                animator.SetInteger("Direction", 1);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                dir.y = -1;
                Light2D.gameObject.GetComponent<LightController>().randomy = randomdown;
                Light2D.transform.rotation = Quaternion.Euler(0, 0, 180);
                animator.SetInteger("Direction", 0);
            }

            dir.Normalize();
            animator.SetBool("IsMoving", dir.magnitude > 0);

            rigidbody2D.velocity = speed * dir;
        }*/
    }
}
