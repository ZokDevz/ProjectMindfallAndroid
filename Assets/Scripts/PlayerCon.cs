


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    public float moveSpeed;

    public bool Left;
    public bool Right;
    public bool Up;
    public bool Down;
    public bool Moving;
    private Animator animator;
    public LayerMask Obstacles;
    public LayerMask grassLayer;

    public event Action OnEncountered;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Left)
        {
            animator.SetFloat("moveX", -1f);
            animator.SetFloat("moveY", 0f);
            moveX = -1f;
        }
        if (Right)
        {
            animator.SetFloat("moveX", 1f);
            animator.SetFloat("moveY", 0f);
            moveX = 1f;
        }

        if (Up)
        {
            animator.SetFloat("moveY", 1f);
            animator.SetFloat("moveX", 0f);
            moveY = 1f;
        }
        if (Down)
        {
            animator.SetFloat("moveX", 0f);
            animator.SetFloat("moveY", -1f);
            moveY = -1f;
        }

      

        // Move the player
        Vector3 targetPos = transform.position + new Vector3(moveX, moveY, 0f) * moveSpeed * Time.deltaTime;
        if (IsWalkable(targetPos))
        {
            transform.Translate(new Vector3(moveX, moveY, 0f) * moveSpeed * Time.deltaTime);
        }

    }
   
    private void CheckForEncounters()
    {
        
        if (Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
            if (UnityEngine.Random.Range(1, 101) <= 15)
            {
                animator.SetBool("Moving", false);
                OnEncountered();
            }
        }
    }
   
//boolean
 public void RightBtnDown()
    {
        Right = true;
        animator.SetBool("Moving", true);

        Debug.Log("walkright");
    }

    public void RightBtnUp()
    {
        Right = false;

        if (!Left && !Up && !Down)
        {
            animator.SetFloat("moveX", 1); // No horizontal movement, set to idle
            Debug.Log("idleright top");
        }
        else if (!Left)
        {
            animator.SetFloat("moveY", -1); // Switch to idle left animation if not moving left
            Debug.Log("idleright bottom");
        }
        animator.SetBool("Moving", false);
        CheckForEncounters();
    }

    public void LeftBtnDown()
    {
        Left = true;
        animator.SetBool("Moving", true);

    }

    public void LeftBtnUp()
    {
        Left = false;

        if (!Right && !Up && !Down)
        {
            animator.SetFloat("moveX", -1); // No horizontal movement, set to idle facing left
            animator.SetFloat("moveY", 0);
        }
        else if (!Right)
        {
            animator.SetFloat("moveY", 1); // Switch to idle right animation if not moving right
        }
        animator.SetBool("Moving", false);
        CheckForEncounters();
    }

    public void UpBtnDown()
    {
        Up = true;
        animator.SetBool("Moving", true);

    }

    public void UpBtnUp()
    {
        Up = false;

        if (!Right && !Left && !Down)
        {
            animator.SetFloat("moveY", 1);
            animator.SetFloat("moveX", 0);// No vertical movement, set to idle facing up
            Debug.Log("idleUP top");
        }
        else if (!Down)
        {
            animator.SetFloat("moveX", -1); // Switch to idle facing up animation if not moving down
            Debug.Log("idleUP bottom");
        }
        animator.SetBool("Moving", false);
        CheckForEncounters();
    }

    public void DownBtnDown()
    {
        Down = true;
        animator.SetBool("Moving", true);

    }

    public void DownBtnUp()
    {
        Down = false;

        if (!Right && !Left && !Up)
        {
            animator.SetFloat("moveY", -1); // No vertical movement, set to idle facing down
            animator.SetFloat("moveX", 0);
            Debug.Log("idleDown Top");
        }
        else if (!Up)
        {
            animator.SetFloat("moveX", 1); // Switch to idle facing down animation if not moving up
            Debug.Log("idleDown bottom");
        }
        animator.SetBool("Moving", false);
        CheckForEncounters();
    }
//boolean end here

    private bool IsWalkable(Vector3 targetPos)
    {
        // Cast a ray from the current position to the target position
        RaycastHit2D hit = Physics2D.Linecast(transform.position, targetPos, Obstacles);

        // If the ray hits an obstacle, return false
        if (hit.collider != null)
        {
            return false;
        }
     
        return true;
    }
}

