using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    private Transform door;

    [SerializeField]

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        door = GameManager.instance.door;
    }

    private void Update()
    {
        agent.SetDestination(door.transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {

        }
    }
}
