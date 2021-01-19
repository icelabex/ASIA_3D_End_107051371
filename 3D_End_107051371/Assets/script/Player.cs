using UnityEngine;
using Invector.vCharacterController;
public class Player : MonoBehaviour
{

    private float hp = 100;
    private Animator ani;

    private int atkCount;

    private float timer;

    [Header("ComboCT"), Range(0, 3)]
    public float interval = 1;
    [Header("AttackCenter")]
    public Transform atkPoint;
    [Header("Atklength"), Range(0f, 5f)]
    public float atkLength;
    [Header("Strength"), Range(0f, 500)]
    public float atk = 30;


    private void Awake()
    {
        ani = GetComponent<Animator>();

    }

    private void Update()
    {
        Attack();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(atkPoint.position, atkPoint.forward * atkLength);
    }

    private RaycastHit hit;

    private void Attack()
    {
        if (atkCount < 3)
        {
            if (timer < interval)
            {
                timer += Time.deltaTime;

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    atkCount++;
                    timer = 0;
                    ani.SetInteger("Combo", atkCount);

                    if (Physics.Raycast(atkPoint.position, atkPoint.forward, out hit, atkLength, 1 << 9))
                    {
                        //コードを呼び出してダメージのアニメーションを行う
                        hit.collider.GetComponent<Enemy>().Damage(atk);
                    }
                }
            }
            else
            {
                atkCount = 0;
                timer = 0;
            }
        }

        if (atkCount == 3) atkCount = 0;
        ani.SetInteger("Combo", atkCount);
    }

    public void Damage(float damage)
    {
        hp -= damage;
        ani.SetTrigger("Damagedsys");

        if (hp <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        ani.SetTrigger("Diesys");
        vThirdPersonController vt = GetComponent<vThirdPersonController>();
        vt.lockMovement = true;
        vt.lockRotation = true;
    }
}