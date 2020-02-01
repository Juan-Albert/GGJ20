using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MovingObject
{
    public int damage;

    public AudioClip enemyAttackSound1, enemyAttackSound2, moveSound1, moveSound2;

    private Animator animator;
    private Transform target;


    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        base.Awake();
    }

    protected override void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        GameManager.instance.AddMinionToLIst(this);
        base.Start();
    }

    protected override bool AttemptMove(int xDir, int yDir)
    {

        bool canMove = base.AttemptMove(xDir, yDir);

        return canMove;
    }

    public void MoveMinion(MovementDirection direction)
    {
        int xDir = 0, yDir = 0;

        switch (direction)
        {
            case MovementDirection.UP:
                yDir = 1;
                break;
            case MovementDirection.DOWN:
                yDir = -1;
                break;
            case MovementDirection.RIGHT:
                xDir = 1;
                break;
            case MovementDirection.LEFT:
                xDir = -1;
                break;
        }

        AttemptMove(xDir, yDir);
    }

    protected override void OnCantMove(GameObject go)
    {
        
        
    }
}
