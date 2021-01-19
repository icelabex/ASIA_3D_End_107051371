using UnityEngine;
using UnityEngine.AI;// use aicoode
public class Enemy : MonoBehaviour
{

    [Header("MoveSpeed"), Range(0, 50)]
    public float speed = 3;
    [Header("StopDistance"), Range(0, 50)]
    public float stopDistance = 2.5f;
    [Header("AttackCoolTime"), Range(0, 50)]
    public float ct = 2f;
    [Header("AttackCenter")]
    public Transform atkPoint;
    [Header("Atklength"), Range(0f, 5f)]
    public float atkLength;
    [Header("Strength"), Range(0f, 500)]
    public float atk = 30;

    
    private Transform player;
    private NavMeshAgent nav;
    private Animator ani;
    //計測器
    private float timer;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();

        ani = GetComponent<Animator>();

        player = GameObject.Find("Player").transform;


        nav.speed = speed;
        nav.stoppingDistance = stopDistance;
    }

    private void Update()
    {
        Track();
        Attack();
    }

    //線を描く
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(atkPoint.position, atkPoint.forward * atkLength);
    }

    private RaycastHit hit;

    public float hp = 100;

    private void Attack()
    {
        if(nav.remainingDistance < stopDistance)
        {
            timer += Time.deltaTime;
            Vector3 pos = player.position;
            pos.y = transform.position.y;

            transform.LookAt(pos);
            
            if (timer >= ct)
            {
                ani.SetTrigger("attacksys");
                timer = 0;
                //物理の判断式
                if (Physics.Raycast(atkPoint.position, atkPoint.forward,out hit,atkLength,1 << 8))
                {
                    //コードを呼び出してダメージのアニメーションを行う
                    hit.collider.GetComponent<Player>().Damage(atk);
                }
            }
        }
    }

    public void Damage(float damage)
    {
        hp -= damage;
        ani.SetTrigger("damagedsys");

        if (hp <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        nav.isStopped = true;
        enabled = false;
        ani.SetBool("deathysys",true);
      
    }

    //ここでプレーヤーの位置をトラック
    private void Track()
    {
        nav.SetDestination(player.position);
        //距離でアニメーションをコントロール
        ani.SetBool("runsys", nav.remainingDistance > stopDistance);
    }
}
