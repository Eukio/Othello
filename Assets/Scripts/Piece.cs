using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Piece : MonoBehaviour
{
    // Start is called before the first frame update
    public static int EMPTY = -1;
   public static int WHITE = 0;
    public static int BLACK = 1;
    int color;
    bool clicked;
    Animator animator;
    SpriteRenderer sr;
  [SerializeField] OnHand h;
    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
       ChangeColor();
    }
    public int GetColor()
    {
        return color;
    }
    public void SetColor(int color)
    {
        this.color = color;
    }
    public void OnMouseDown()
    {
        clicked = true;

        Debug.Log(color);
    }
    public bool Clicked()
    {
        return clicked;
    }
    public void SetClicked(bool clicked)
    {
        this.clicked = clicked;
    }
    public int GetR()
    {
        return Convert.ToInt32(name.Substring(1, 1));    }
    public int GetC()
    {
       return Convert.ToInt32(name.Substring(3, 1));
    }
   public void ChangeColor()
    {
        if (color == EMPTY)
            sr.enabled = false;
        else if (color == WHITE)
        {
            animator.SetInteger("State", 0);
            sr.enabled = true;
        }
        else if (color == BLACK)
        {
            animator.SetInteger("State", 1);
            sr.enabled = true;
        }
    }
}
