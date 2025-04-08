using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class EnemyHandler : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Attack,
        Retreat
    }

    public EnemyState currentState = EnemyState.Patrol;

    public PlayerHandler playerHandler;
    public Rigidbody2D rigidbody;

    [HideInInspector] public float currentHP = 100f;
    [HideInInspector] public float maxHP = 100f;

    [HideInInspector] public float currentTurbo = 0f;
    [HideInInspector] public float maxTurbo = 5f;

    public float topSpeed = 15f;
    public float detectionRange = 20f;
    public float attackRange = 10f;
    public float patrolSpeed = 5f;
    public float safeDistance = 12f;
    public Slider healthSlider;

    public BodyPiece bodyPiece;
    public List<WingPiece> wingPieces;
    public EnginePiece enginePiece;
    public GunPiece gunPiece;

    private void Start()
    {
        if (playerHandler == null)
            playerHandler = GameObject.FindObjectOfType<PlayerHandler>();

        currentHP = maxHP = 100f;
        currentTurbo = 0f; 
        maxTurbo = 5f;

        healthSlider.maxValue = maxHP;

        PullDataFromShipHandler();
    }

    private void FixedUpdate()
    {
        if (currentState == EnemyState.Patrol)
        {
            Patrol();
        }
        else if (currentState == EnemyState.Attack)
        {
            Attack();
        }
        else if (currentState == EnemyState.Retreat)
        {
            Retreat();
        }

        SmartNavigation();
    }

    private void Update()
    {
        healthSlider.value = currentHP;

        DetectPlayer();
    }

    private void DetectPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerHandler.transform.position);
        
        if (distanceToPlayer < attackRange)
        {
            currentState = EnemyState.Attack;
        }
        else if (distanceToPlayer < detectionRange)
        {
            if (currentHP < maxHP * 0.3f)
            {
                currentState = EnemyState.Retreat;
            }
            else
            {
                currentState = EnemyState.Patrol;
            }
        }
        else
        {
            currentState = EnemyState.Patrol;
        }
    }

    private void Patrol()
    {
        Vector2 direction = (Random.insideUnitCircle).normalized;
        rigidbody.linearVelocity = direction * patrolSpeed;

        if (Vector2.Distance(transform.position, playerHandler.transform.position) < attackRange)
        {
            currentState = EnemyState.Attack;
        }
    }

    private void Attack()
    {
        Vector2 direction = (playerHandler.transform.position - transform.position).normalized;

        float distanceToPlayer = Vector2.Distance(transform.position, playerHandler.transform.position);
        if (distanceToPlayer < safeDistance)
        {
            rigidbody.linearVelocity = -direction * topSpeed;
        }
        else
        {
            rigidbody.linearVelocity = direction * topSpeed;
        }

        if (distanceToPlayer < attackRange)
        {
            FireAtPlayer();
        }
    }

    private void FireAtPlayer()
    {
        if (gunPiece != null)
        {
            gunPiece.FireButtonPressed();
        }
    }

    private void Retreat()
    {
        // Retreat by moving in the opposite direction to the player
        Vector2 direction = (transform.position - playerHandler.transform.position).normalized;
        rigidbody.linearVelocity = direction * topSpeed;

        if (Vector2.Distance(transform.position, playerHandler.transform.position) > detectionRange)
        {
            currentState = EnemyState.Patrol; // Once out of danger, resume patrol
        }
    }

    private void SmartNavigation()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 3f);
        if (hit.collider != null && !hit.collider.CompareTag("Player"))
        {
            Vector2 avoidDirection = Vector2.Perpendicular(hit.normal).normalized;
            rigidbody.linearVelocity = avoidDirection * topSpeed;
        }

        if (rigidbody.linearVelocity.magnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(RotateTowards(transform.position, rigidbody.linearVelocity.normalized, wingPieces[0].wingRotationSpeed * Time.fixedDeltaTime, 999f));
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
        }
    }
    
    public static Vector2 RotateTowards(Vector2 current, Vector2 target, float maxRadiansDelta, float maxMagnitudeDelta)
    {
        return Vector3.RotateTowards(current, target, maxRadiansDelta, maxMagnitudeDelta);
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
    }

    private void PullDataFromShipHandler()
    {
        bodyPiece = GetComponentInChildren<BodyPiece>();
        List<WingPiece> wings = GetComponentsInChildren<WingPiece>().ToList();
        
        wingPieces.Clear();
        foreach (WingPiece obj in wings)
        {
            wingPieces.Add(obj);
        }

        enginePiece = GetComponentInChildren<EnginePiece>();
        gunPiece = GetComponentInChildren<GunPiece>();
    }
}

