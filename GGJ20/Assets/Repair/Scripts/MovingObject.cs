using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour {

    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private float movementSpeed;
    private BoxCollider2D myBC;
    private Rigidbody2D myRB;

    protected virtual void Awake()
    {
        myBC = GetComponent<BoxCollider2D>();
        myRB = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        movementSpeed = 1f / moveTime;
    }

    //Se comprueba que no hay nada entre el objeto y el destino y se mueve el objeto en una direccion.
    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end; //= start + new Vector2(xDir, xDir);
        end.x = start.x + xDir;
        end.y = start.y + yDir;

        myBC.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        myBC.enabled = true;

        //Si no hay nada entre medias nos movemos
        if(hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }

    //Comprueba si el objeto se puede mover
    protected virtual bool AttemptMove(int xDir, int yDir)
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if (!canMove)
        {
            OnCantMove(hit.transform.gameObject);
        }

        return canMove;
    }

    protected abstract void OnCantMove(GameObject go);

    //Corrutina que mueve al objeto a una posicion
    protected IEnumerator SmoothMovement(Vector2 end)
    {
        float remainingDistance = Vector2.Distance(myRB.position, end);
        while(remainingDistance > float.Epsilon)
        {
            Vector2 newPosition = Vector2.MoveTowards(myRB.position, end, movementSpeed * Time.deltaTime);
            myRB.MovePosition(newPosition);
            remainingDistance = Vector2.Distance(myRB.position, end);
            yield return null;
        }
    }
}
