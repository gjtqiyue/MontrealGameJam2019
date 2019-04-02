using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MonsterScript : MonoBehaviour
{
    public Animator anim;
    public float chaseDistance;

    [SerializeField]
    Transform player;
    NavMeshAgent nav;
    Vector3 spawnPoint;

    public bool enableChasing;
    
    void Start()
    {
        enableChasing = false;
        spawnPoint = transform.position;
        anim.SetBool("Awake", true);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        MonsterManager.Instance.RegisterMonster(this);
    }

    void Update()
    {
        if (enableChasing)
        {
            Debug.Log("chase distance " + (player.transform.position - transform.position).sqrMagnitude);
            if ((player.transform.position - transform.position).sqrMagnitude < chaseDistance)
            {
                anim.SetBool("HasTarget", true);
                nav.SetDestination(player.transform.position);
            }
            else
            {
                anim.SetBool("HasTarget", false);
                nav.SetDestination(spawnPoint);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // close enough to attack
            anim.SetBool("TargetInRange", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // far enough to stop
        anim.SetBool("TargetInRange", false);
    }

    public void KillMummy()
    {
        anim.SetBool("Dead", true);
        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<CharacterScript>().GetHitByMonster();
        }
    }
}
