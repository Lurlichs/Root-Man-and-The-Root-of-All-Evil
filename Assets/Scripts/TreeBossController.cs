using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBossController : MonoBehaviour
{

    public GameObject spike, root;

    enum States
    {
        IDLE,
        IDLE_SPECIAL,
        LAUGH,
        SPIKE,
        SPIT,
        MELEE
    }

    [Tooltip("Chance per idle animation cycle of doing the special idle animation")]
    public float chanceOfSpecialIdle = 0.05f;
    [Tooltip("Chance per idle animation cycle of doing a laugh")]
    public float chanceOfLaugh = 0.05f;
    [Tooltip("Chance per idle animation cycle of doing a spike attack")]
    public float chanceOfSpike = 0.05f;
    [Tooltip("Chance per idle animation cycle of doing a spit attack")]
    public float chanceOfSpit = 0.05f;
    [Tooltip("Chance per idle animation cycle of doing a melee attack")]
    public float chanceOfMelee = 0.05f;
    [Tooltip("Seconds to lock out spike or root attacks after a spike or root attack")]
    public float spikeLockout = 5.0f;

    private Animator animator;
    private RootWaveSpawn rootWave;

    private States currentState;
    private float lastSpike;

    [Header("Leave Blank if not a boss enemy")]
    [SerializeField] private Bosses_ScriptableObj BossScObj;

    // Start is called before the first frame update
    void Start()
    {
        // The animator is in a child object so we need to use GetComponentInChildren instead of GetComponent
        animator = GetComponentInChildren<Animator>();
        rootWave = GetComponent<RootWaveSpawn>();
        animator.SetTrigger("StartLaugh");
        // Because the animator is in a child object, we can't get it to call a function here,
        // instead, use AnimationEventsHandler to forward the calls to our AnimationClipEnded method
        GetComponentInChildren<AnimationEventsHandler>().AddAnimationClipEndCallback(AnimationClipEnded);

        if (BossScObj != null)
        {
            //Uncomment when hooked up to enemy class
            //UI_Manager.Instance.SetBossHealthBar(BossScObj, health);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Returns true if a state was chosen, otherwise returns false if you should
    // try again e.g. due to lockout
    private bool chooseNewState()
    {
        float randomVar = Random.value;

        if (randomVar < chanceOfSpecialIdle)
        {
            animator.SetTrigger("StartSpecial");
            return true;
        }
        else
        {
            randomVar -= chanceOfSpecialIdle;
            if (randomVar < chanceOfLaugh)
            {
                if ((Time.fixedTime - lastSpike) < spikeLockout)
                {
                    return false; // lockout
                }
                animator.SetTrigger("StartLaugh");
                lastSpike = Time.fixedTime;
            }
            else
            {
                randomVar -= chanceOfLaugh;
                if (randomVar < chanceOfSpike)
                {
                    if ((Time.fixedTime - lastSpike) < spikeLockout)
                    {
                        return false; // lockout
                    }
                    animator.SetTrigger("StartSpike");
                    rootWave.Spawn();
                    lastSpike = Time.fixedTime;
                }
                else
                {
                    randomVar -= chanceOfSpike;
                    if (randomVar < chanceOfSpit)
                    {
                        animator.SetTrigger("StartSpit");
                    }
                    else
                    {
                        randomVar -= chanceOfSpit;
                        if (randomVar < chanceOfMelee)
                        {
                            animator.SetTrigger("StartMelee");
                        }
                    }
                }
            }
        }
        return true;
    }

    private void AnimationClipEnded(string clipName)
    {
        bool successful;

        do
        {
            successful = chooseNewState();
        } while (!successful);
    }

    public void SpikeWave()
    {
        print("hello");
    }
        
    }

  
