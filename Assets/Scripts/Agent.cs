using Assets.Scripts.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Team
{
    RED, BLUE
}

public class Agent : MonoBehaviour
{
    public delegate void EvaluationEvent(EvaluatedActionWithScore[] evaluatedActions);
    public event EvaluationEvent OnEvaluatedActions;
    public Team team;
    public string agentName;
    public GameObject thisBase;

    [Header("How many units this agent can move in one tick")]
    public float movementInATick = 1f;

    [Header("Text for the UI that pops up above tha player")]
    public Text nameText;
    public Text actionText;

    [Header("Model to use")]
    public UtilityAIModel model;
    public UtilityAction nextAction;
    public EvaluatedActionWithScore[] lastEvaluatedActions;

    [Header("How close this agent needs to be before they can pick up the flag")]
    public float closenessBeforePickingUpFlag;

    [Header("Where this agent holds the flag")]
    public Transform flagHoldPoint;

    [Header("Probablity of hitting enemy in the open")]
    public float probablityOfHittingEnemy;

    [Header("The bullet prefab")]
    public GameObject bulletPrefab;

    [Header("Where the bullet spawns")]
    public Transform bulletSpawnPoint;

    [Header("The material used for the flash effect when damaged")]
    public Material flashMaterial;

    [Header("MeshRenderer that will have material changed")]
    public MeshRenderer meshRenderer;

    [Header("How much the bullet deviates from target when it is a miss")]
    public float missOffset;

    public bool hasFlag = false;

    [Header("Health bar")]
    public Image healthBarImage;
    public float maxHealth = 1f;
    public float health = 1f;
    public float damageFromBullet;
    public GameObject canvas;
    public SphereCollider sphereCollider;
    public Rigidbody rb;

    Coroutine hitFlashCoroutine;

    public void Start()
    {
        nameText.text = agentName;
    }


    // Picks up the flag if it's close enough to this agent
    // Sets the parent to this flagHoldPoint
    public void GetFlag(World world)
    {
        // Don't do anything if we already have the flag or another agent has it
        if (this.hasFlag) return;
        if (world.agentWithFlag != null) return;

        if ((world.flag.transform.position - this.transform.position).magnitude <= closenessBeforePickingUpFlag)
        {
            world.agentWithFlag = this;
            this.hasFlag = true;
            world.flag.transform.SetParent(flagHoldPoint, false);
        }
    }

    public void FireAtAgent(World world, Agent target, float executionTime)
    {
        if (target == null) return;

        this.transform.rotation = Quaternion.LookRotation(target.transform.position - this.transform.position);

        bool hasHit = Random.Range(0f, 1f) <= probablityOfHittingEnemy;
        Vector3 offset = Vector3.zero;

        if (!hasHit) offset = new Vector3(Random.Range(-missOffset, missOffset), Random.Range(-missOffset, missOffset), Random.Range(-missOffset, missOffset));

        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = bulletSpawnPoint.transform.position;


        StartCoroutine(FireBulletTowardsAgent(world, bullet, target, offset, executionTime));
    }

    IEnumerator FireBulletTowardsAgent(World world, GameObject bullet, Agent target, Vector3 offset, float executionTime)
    {
        float time = 0f;
        Vector3 startPosition = bullet.transform.position;
        Vector3 endPosition = target.transform.position + offset;

        while (time < executionTime)
        {
            // update the end position because the enemy may have moved
            endPosition = target.transform.position + offset;

            Vector3 lerpedPosition = Vector3.Lerp(startPosition, endPosition, time / executionTime);

            bullet.transform.position = lerpedPosition;
            bullet.transform.up = endPosition - startPosition;

            time += Time.deltaTime;

            yield return null;
        }

        Destroy(bullet);
        if (offset.magnitude == 0) target.Hit(world);
    }

    public void DropFlagAtBase(World world)
    {
        if (!this.hasFlag) return;

        if ((thisBase.transform.position - this.transform.position).magnitude <= closenessBeforePickingUpFlag)
        {
            world.ResetFlag();
        }
    }

    public void Die(World world)
    {
        canvas.SetActive(false);
        sphereCollider.enabled = false;
        rb.isKinematic = false;
        rb.AddForce(new Vector3(Random.Range(-2f, 2f), Random.Range(-1f, 1f), Random.Range(-2, 2f)), ForceMode.Impulse);
        DropFlag(world);
    }

    public void Hit(World world)
    {
        health -= damageFromBullet;
        healthBarImage.fillAmount = health;
        if (health <= 0)
        {
            Die(world);
            return;
        }

        if (hitFlashCoroutine != null) return;

        hitFlashCoroutine = StartCoroutine(HitFlash());
    }

    IEnumerator HitFlash()
    {
        Material originalMaterial = meshRenderer.material;
        meshRenderer.material = flashMaterial;

        yield return new WaitForSeconds(0.1f);

        meshRenderer.material = originalMaterial;

        hitFlashCoroutine = null;
    }

    // Drops the flag at this position
    public void DropFlag(World world)
    {
        if (!this.hasFlag) return;

        this.hasFlag = false;
        world.agentWithFlag = null;
        world.flag.transform.SetParent(null, true);
        world.flag.transform.position = new Vector3(world.flag.transform.position.x, world.flagResetPosition.y, world.flag.transform.position.z);
    }

    public void ChooseAction(World world)
    {
        EvaluatedActionWithScore[] evaluatedActions = model.EvaluateActions(this, world);
        
        nextAction = evaluatedActions[0].action;

        actionText.text = nextAction.ToString();

        lastEvaluatedActions = evaluatedActions;

        OnEvaluatedActions?.Invoke(evaluatedActions);
    }
}
