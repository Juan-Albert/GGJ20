using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MovingObject
{
    public int damage;

    public AudioClip enemyAttackSound1, enemyAttackSound2, moveSound1, moveSound2;

    private Animator animator;
    private Transform target;
    private bool skipMove;


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

    //controla que el enemigo se mueva cada 2 turnos, genera el sonido del movimiento, se mueve y devuelve si se ha podido mover.
    protected override bool AttemptMove(int xDir, int yDir)
    {
        if(skipMove)
        {
            skipMove = false;
            return false;
        }
        bool canMove = base.AttemptMove(xDir, yDir);

        skipMove = true;
        return canMove;
    }

    //Calcula la posicion del jugador 
    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if(Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }
        else
        {
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }
        AttemptMove(xDir, yDir);
    }

    protected override void OnCantMove(GameObject go)
    {
        
        
    }
}
