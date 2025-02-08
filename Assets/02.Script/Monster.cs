using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    private Transform door;
    [SerializeField] private new Renderer renderer;

    public int hp;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        door = GameManager.instance.door;
        renderer = gameObject.GetComponent<Renderer>();
    }

    private void Update()
    {
        agent.SetDestination(door.transform.position);
        if (hp <= 0) Destroy(gameObject);
    }

    IEnumerator TakeDamage()
    {
        hp--;
        renderer.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        renderer.material.color = Color.white;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            StartCoroutine(TakeDamage());
        }
    }
}
