using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;

    Player player;

    void Awake()
    {
        player = GameManager.instance.player;    
    }




    void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        WeaponRotate();


        // 테스트코드
        if(Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);
        }

    }
    

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
            Place();

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);

    }


    public void Init(ItemData data)
    {
        // 기본 세팅 (Base)
        name = "Weapon" + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // 속성 세팅 (Property)
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for(int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if(data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }


        switch(id)
        {
            case 0:
                speed = 150f * Character.WeaponSpeed;
                Place();
                break;

            default:
                speed = 0.3f;
                break;
        }
        // Hand set
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);


        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Fire()
    {
        if (!player.scaner.nearTarget)
            return;

        Vector3 targetPos = player.scaner.nearTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);


        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }


    void Place()    // 배치 상태
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;
            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count; 
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World); // 1.5f는 Y방향이 플레이어와 무기의 거리가 1.5f이다.

            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); //  -1 는 무한 관통(per) 이다.

        }
    }

    void WeaponRotate()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);

                break;

            default:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;

        }
    }






}
