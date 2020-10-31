using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAgent : MonoBehaviour
{
    Player player = null;
    NavMeshAgent navAgent = null;
    [SerializeField] private float maxHealth = 10f;
    private float currentHealth = 0;

    [Header("Attack Sequence")]
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private int attackDamage = 5;
    [SerializeField] private GameObject eyeLocation = null;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Chase Sequence")]
    [SerializeField] private float chaseRange = 10f;
    [SerializeField] private float chaseSpeed = 7f;

    [Header("Audio")]
    [SerializeField] private AudioSource enemyAudio = null;
    [SerializeField] private AudioClip angryClip = null;
    [SerializeField] private AudioClip breathingClip = null;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        if ((navAgent = GetComponent<NavMeshAgent>()) == null)
            gameObject.AddComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

        float targetSqrDistance = (transform.position - player.transform.position).sqrMagnitude;
        if (targetSqrDistance <= attackRange * attackRange)
        {
            AttackSequence();
        }
        else if (targetSqrDistance <= chaseRange * chaseRange && targetSqrDistance >= attackRange * attackRange)
        {
            ChaseSequence(player.transform.position);
        }
        else
        {
            WanderSequence();
        }
    }

    public void TakeDamge (float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void AttackSequence()
    {
        navAgent.ResetPath();
        transform.LookAt(player.transform);
    }

    private void ChaseSequence(Vector3 targetDestination)
    {
        navAgent.SetDestination(targetDestination);
        navAgent.speed = chaseSpeed;
        transform.LookAt(player.transform);
    }

    private void WanderSequence()
    {

    }
}
