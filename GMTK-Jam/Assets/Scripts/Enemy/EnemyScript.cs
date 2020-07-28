using UnityEngine;
using UnityEngine.AI;
public class EnemyScript : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    public bool withinRange;
    public State state;
    Vector3 goal;
    float walkTimer;
    float damageTimer;

    public GameObject enemyMesh;
    Animator anim;

    [SerializeField]
    private int _enemyHealth = 5;

    public enum State
    {
        Idle,
        Walking,
        Combat
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = enemyMesh.GetComponent<Animator>();

        walkTimer = 0;
        damageTimer = 0;
        withinRange = false;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player_Object");
    }

    // Update is called once per frame
    void Update()
    {

        print(Vector3.Distance(player.transform.position, transform.position));

        if (state == State.Idle)
        {
            agent.speed = 0;

        }
        else if (state == State.Walking)
        {
            anim.SetBool("Walk", true);
            agent.speed = 3;
            if (walkTimer > 0)
            {
                moveToGoal();
                walkTimer -= Time.deltaTime;
            }
            else
            {                
                setNewGoal();
                walkTimer = Random.Range(1, 5);
            }
        }
        else if (state == State.Combat)
        {
            agent.speed = 4.5f;
            agent.SetDestination(player.transform.position);
            anim.SetBool("Walk", true);

            if (Vector3.Distance(player.transform.position, transform.position) < 2)
            {
                anim.SetBool("Attack", true);
            }
            else
            {
                anim.SetBool("Attack", false);
            }
        }
    }

    public void Damage(int damageAmount)
    {
        //Subtract damage amount wehn Damage function is called
        _enemyHealth -= damageAmount;
        //Check if health has fallen below zero
        if (_enemyHealth < 0)
        {
            player.GetComponent<StressManager>().setEnemyCount(-1);
            //If health has fallen below zero, deactivate it
            gameObject.SetActive(false);
        }
    }

    private void setNewGoal()
    {
        goal = transform.position + new Vector3(Random.insideUnitSphere.x * 3, 0, Random.insideUnitSphere.z * 3);
    }
    private void moveToGoal()
    {
        agent.SetDestination(goal);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "PlayerBody")
        {
            if (damageTimer <= 0)
            {
                state = State.Combat;
                if (Vector3.Distance(player.transform.position, transform.position) < 2)
                {
                    Player_Controller player = other.GetComponentInParent<Player_Controller>();
                    player.Damage();
                }
                damageTimer = 1;
            }
            else
            {
                damageTimer -= Time.deltaTime;
            }

        }

        if (other.tag == "ShockWave")
        {
            state = State.Combat;
        }
    }
}
