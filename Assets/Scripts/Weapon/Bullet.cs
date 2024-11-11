using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();    

    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if(per >= 0)
        {
            rigid.velocity = dir * 15f;
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Enemy") || per == -100) // per -100은 근거리 무기
        {
            return;
        }

        per--;

        if(per < 0 )
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)      // 원거리 투사체가 밖으로 나갈 시 없어지는 로직
    {
        if (!collision.CompareTag("Enemy") || per == -100)
        {
            return;
        }
        gameObject.SetActive(false);

    }



}
