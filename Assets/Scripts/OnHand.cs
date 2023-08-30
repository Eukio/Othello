using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class OnHand : MonoBehaviour
{
    // Start is called before the first frame update
    public static int WHITE = 0;
    public static int BLACK = 1;
    int turn;
    Animator animator;
    SpriteRenderer sr;
    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        turn = BLACK;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; 
      transform.position = Vector3.Lerp(transform.position, mousePos, 150 * Time.deltaTime);

         if (turn == WHITE)
        {
            animator.SetInteger("State", 0);
        }
        else if (turn == BLACK)
        {
            animator.SetInteger("State", 1);
        }
    }
    public int GetTurn()
    {
        return turn;
    }
    public void SetTurn(int turn)
    {
        this.turn = turn;
    }
   
        
}
