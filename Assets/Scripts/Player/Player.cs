using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{

    public Vector2 inputVec; 
    public float speed;
    public Scaner scaner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;


    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator animator;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        scaner = GetComponent<Scaner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    private void OnEnable()
    {
        speed *= Character.Speed;

        animator.runtimeAnimatorController = animCon[GameManager.instance.playerId]; 
    }

    private void Update()
    {
        if(!GameManager.instance.isLive)
        {
            return;
        }

    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        animator.SetFloat("Speed", inputVec.magnitude);
        //animator.SetTrigger("Dead");


        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }


    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!GameManager.instance.isLive)
            return;

        GameManager.instance.health -= Time.deltaTime * 10;

        if(GameManager.instance.health < 0)
        {
            for(int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);  
            }
            animator.SetTrigger("Dead");
            GameManager.instance.GameOver();


        }

        //Debug.Log(GameManager.instance.health);  // 데미지 측정 확인 용 디버그
    }



}
