using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBossController : MonoBehaviour
{
    enum States
    {
        IDLE,
        IDLE_SPECIAL,
        LAUGH,
        SPIKE,
        SPIT
    }

    [Tooltip("Chance per idle animation cycle of doing the special idle animation")]
    public float chanceOfSpecialIdle = 0.05f;
    [Tooltip("Chance per idle animation cycle of doing a laugh")]
    public float chanceOfLaugh = 0.05f;
    [Tooltip("Chance per idle animation cycle of doing a spike attack")]
    public float chanceOfSpike = 0.05f;
    [Tooltip("Chance per idle animation cycle of doing a spit attack")]
    public float chanceOfSpit = 0.05f;

    private Animator animator;

    private States currentState;

    [Header("Leave Blank if not a boss enemy")]
    [SerializeField] private Bosses_ScriptableObj BossScObj;

    // Start is called before the first frame update
    void Start()
    {
        // The animator is in a child object so we need to use GetComponentInChildren instead of GetComponent
        animator = GetComponentInChildren<Animator>();
        animator.SetTrigger("StartLaugh");
        // Because the animator is in a child object, we can't get it to call a function here,
        // instead, use AnimationEventsHandler to forward the calls to our AnimationClipEnded method
        GetComponentInChildren<AnimationEventsHandler>().AddAnimationClipEndCallback(AnimationClipEnded);

        if (BossScObj != null)
        {
            //health = BossScObj.baseHealth;
            UI_Manager.Instance.SetBossHealthBar(BossScObj);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void AnimationClipEnded(string clipName)
    {
        float randomVar = Random.value;

        if (randomVar < chanceOfSpecialIdle)
        {
            animator.SetTrigger("StartSpecial");
        }
        else
        {
            randomVar -= chanceOfSpecialIdle;
            if (randomVar < chanceOfLaugh)
            {
                animator.SetTrigger("StartLaugh");
            }
            else
            {
                randomVar -= chanceOfLaugh;
                if (randomVar < chanceOfSpike)
                {
                    animator.SetTrigger("StartSpike");
                }
                else
                {
                    randomVar -= chanceOfSpit;
                    if (randomVar < chanceOfSpit)
                    {
                        animator.SetTrigger("StartSpit");
                    }
                }
            }
        }
    }
}
