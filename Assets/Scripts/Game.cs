using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Text text;
    [SerializeField] Piece[] pieces;
    Piece[,] board = new Piece[8, 8];
    bool win;
    int BPoints;
    int WPoints;
    public static int WHITE = 0;
    public static int BLACK = 1;
    [SerializeField] OnHand h; 
    Piece temp;
    List<Piece> flipList = new List<Piece>();

    void Start()
    {
        for(int x = 0; x < pieces.Length; x++)
        {
            //for loop through board, if i and j equals, then add piece to that location

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (i == pieces[x].GetR() && j == pieces[x].GetC())
                    {
                        board[i, j] = pieces[x];
                    }
                }
            }
        }


     
        Reset();


    }
    void Update()
    {

        if (!win)
        {
          
                //when piece is clicked, return position
                //with position, check row, check col, check diag
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        if (board[i, j].Clicked() && board[i, j].GetColor() == Piece.EMPTY)// && board[i,j].GetColor() == Piece.EMPTY
                    {
                            temp = board[i, j];
                            temp.SetClicked(false);
                        }
                    }
                }
            if (temp != null && isValidMove(temp))
            {
                board[temp.GetR(), temp.GetC()].SetColor(h.GetTurn());
              //  Debug.Log("Valid Move: " + flipList.Count);
                for (int x = 0; x < flipList.Count; x++)
                {

                    if (h.GetTurn() == BLACK)
                    {
                   //     Debug.Log("Color " + flipList[x].GetR() + " " + flipList[x].GetC() + " WHITE");

                        board[flipList[x].GetR(), flipList[x].GetC()].GetComponent<Animator>().SetBool("FliptoBlack", true);
                        board[flipList[x].GetR(), flipList[x].GetC()].GetComponent<Animator>().SetBool("FliptoWhite", false);
                        board[flipList[x].GetR(), flipList[x].GetC()].GetComponent<Animator>().SetInteger("State", 1);
                        board[flipList[x].GetR(), flipList[x].GetC()].SetColor(BLACK);


                    }
                    if (h.GetTurn() == WHITE)
                    {
                  //      Debug.Log("Color " + flipList[x].GetR() + " " + flipList[x].GetC() + " BLACK");


                        board[flipList[x].GetR(), flipList[x].GetC()].GetComponent<Animator>().SetBool("FliptoWhite", true);
                        board[flipList[x].GetR(), flipList[x].GetC()].GetComponent<Animator>().SetBool("FliptoBlack", false);
                        board[flipList[x].GetR(), flipList[x].GetC()].GetComponent<Animator>().SetInteger("State", 0);
                        board[flipList[x].GetR(), flipList[x].GetC()].SetColor(WHITE);


                    }


                }
                GetComponent<AudioSource>().Play();

                board[temp.GetR(), temp.GetC()].SetColor(h.GetTurn());


                if (h.GetTurn() == OnHand.WHITE)
                {
                    board[temp.GetR(), temp.GetC()].GetComponent<Animator>().SetInteger("State", 0);

                    h.SetTurn(BLACK);
                }
                else {
                    board[temp.GetR(), temp.GetC()].GetComponent<Animator>().SetInteger("State", 1);

                    h.SetTurn(WHITE);
            }
            }
            CheckWin();
            if (win && Input.GetKeyUp(KeyCode.R))
            {
                Reset();
                Debug.Log("Reset");
            }
           
            temp = null;
            /*    for (int x = 0; x < flipList.Count; x++)
                {
                    board[flipList[x].GetR(), flipList[x].GetC()].GetComponent<Animator>().SetBool("FliptoBlack", false);
                    board[flipList[x].GetR(), flipList[x].GetC()].GetComponent<Animator>().SetBool("FliptoWhite", false);

                }
            */
                flipList.Clear();
      

        }

    }
    public void Reset()
    {
        board[3, 3].GetComponent<Animator>().SetInteger("State", 1);
        board[4, 4].GetComponent<Animator>().SetInteger("State", 1);
        board[3, 4].GetComponent<Animator>().SetInteger("State", 0);
        board[4, 3].GetComponent<Animator>().SetInteger("State", 0);
        for (int x = 0; x < pieces.Length; x++)
        {
            //for loop through board, if i and j equals, then add piece to that location

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (i == pieces[x].GetR() && j == pieces[x].GetC())
                    {
                        board[i, j].SetColor(Piece.EMPTY);
                    }
                }
            }
        }
       
        board[3, 3].SetColor(Piece.BLACK);
        board[4, 4].SetColor(Piece.BLACK);
        board[3, 4].SetColor(Piece.WHITE);
        board[4, 3].SetColor(Piece.WHITE);

        win = false;
        BPoints = 0;
        WPoints = 0;
        text.text = "";
    }
    // Update is called once per frame
 
    public void CheckWin()
    {
        bool forward = false;
        int count = 0;
        int count2 = 0;
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j].GetColor() == Piece.BLACK)
                {
                    BPoints++;
                }
                else if (board[i, j].GetColor() == Piece.WHITE)
                {
                    WPoints++;
                }
                else
                {
                   if(!isValidMove(board[i, j]))
                    forward = true;
                    break;
                }
            }
        }
        if(!forward)
        if (BPoints > WPoints)//Black Win
        {
            win = true;
            text.text = "Black Wins!";
            
        }
        else if (WPoints > BPoints)
        { //White Win
            win = true;
            text.text = "White Wins!";
        }
        else
        {
            win = true;
            text.text = "Tie Game!";
        }
    }
    public bool isValidMove(Piece temp)
    {
        List<Piece> tempList = new List<Piece>();
        bool check = false;
        //move left and count opposite color of OnHand until color
        //move right and count opposite color until color
        //flip everything in flipList
        for (int x = temp.GetC()+1; x < board.GetLength(0); x++) //right
        {
            if ( board[temp.GetR(), x].GetColor() == h.GetTurn()) // valid flip
            {
                if (x == temp.GetC() + 1)
                {
                    tempList.Clear();
                    break;
                }
                for (int i = 0; i < tempList.Count; i++)
                    flipList.Add(tempList[i]);
                tempList.Clear();
                check = true;
                //Debug.Log("Can left");

                break;
            }
            else if (board[temp.GetR(), x].GetColor() != h.GetTurn() && board[temp.GetR(), x].GetColor() != Piece.EMPTY) //can flip
            {
                tempList.Add(board[temp.GetR(), x]);
                continue;
            }
           
           else if ((x == board.GetLength(0) - 1 && board[temp.GetR(), x].GetColor() != h.GetTurn()) || board[temp.GetR(), x].GetColor() == Piece.EMPTY) //cannot flip
            {
                tempList.Clear();
               // Debug.Log("Cannot left");
                break;

            }
        }

        for (int x = temp.GetC()-1; x >= 0; x--) //left
        {
             if (board[temp.GetR(), x].GetColor() == h.GetTurn()) // valid flip
            {// && board[temp.GetR(), x].GetColor() == h.GetTurn()
                if(x == temp.GetC() - 1)
                {
                    tempList.Clear();
                    break;
                }

                for (int i = 0; i < tempList.Count; i++)
                    flipList.Add(tempList[i]);
                tempList.Clear();
                check = true;
                //Debug.Log("Can right " + x);
                break;
            }
            else if (board[temp.GetR(), x].GetColor() != h.GetTurn() && board[temp.GetR(), x].GetColor() != Piece.EMPTY) //can flip
            {
                tempList.Add(board[temp.GetR(), x]);
                continue;
            }
            
            else if ((x == 0 && board[temp.GetR(), x].GetColor() != h.GetTurn()) || board[temp.GetR(), x].GetColor() == Piece.EMPTY) //cannot flip
            {
                tempList.Clear();
                //Debug.Log("Cannot right");
                break;
            }
        }

        for (int y = temp.GetR()+1; y < board.GetLength(1); y++) //up
        {
            if (board[y, temp.GetC()].GetColor() == h.GetTurn()) // valid flip
            {
                if (y == temp.GetR() + 1) { 
                tempList.Clear();
                    break;
            }

                for (int i = 0; i < tempList.Count; i++)
                    flipList.Add(tempList[i]);
               //  Debug.Log(temp.GetR()+" Here: " + (tempList.Count)+"[R,C] "+ y+","+(temp.GetC()));

                tempList.Clear();
                check = true;
             //   Debug.Log("Can Up");
                break;
            }
            else if (board[y, temp.GetC()].GetColor() != h.GetTurn() && board[y, temp.GetC()].GetColor() != Piece.EMPTY) //can flip
            {
                tempList.Add(board[y, temp.GetC()]);

                continue;
            }
            else if ((y == board.GetLength(1) - 1 && board[y, temp.GetC()].GetColor() != h.GetTurn()) || board[y, temp.GetC()].GetColor() == Piece.EMPTY) //cannot flip
            {
                tempList.Clear();
           //     Debug.Log("Cannot Up");
                break;


            }
        }



        for (int y = temp.GetR()-1; y >= 0; y--) //placed below, checking if theres a piece above me
        {
             if ( board[y, temp.GetC()].GetColor() == h.GetTurn()) // valid flip
            {//&& board[y, temp.GetC()].GetColor() == h.GetTurn()

                if (y == temp.GetR() - 1)
                {
                    tempList.Clear();
                    break;
                }
                for (int i = 0; i < tempList.Count; i++)
                    flipList.Add(tempList[i]);
                tempList.Clear();
                check = true;
               /// Debug.Log("Can Down ");
                break;
            }
           else if (board[y, temp.GetC()].GetColor() != h.GetTurn() && board[y, temp.GetC()].GetColor() != Piece.EMPTY) //can flip
            {
                tempList.Add(board[y, temp.GetC()]);
                continue;
            }
            else if ((y == 0 && board[y, temp.GetC()].GetColor() != h.GetTurn()) || board[y, temp.GetC()].GetColor() == Piece.EMPTY) //cannot flip
            {
                tempList.Clear();
               // Debug.Log("Cannot Down");
                break;


            }
        }

        int add = 1;
          for (int y = temp.GetR() - 1; y >= 0; y--) //placed below, checking if theres a piece above me and right
        {          
            if (add+temp.GetC()<8 &&  board[y, temp.GetC()+ add].GetColor() == h.GetTurn()) // valid flip
            { 
                if (y == temp.GetR() - 1 || add + temp.GetC() == temp.GetC() + 1)
                {
                    tempList.Clear();
                    break;
                }
                for (int i = 0; i < tempList.Count; i++)
                    flipList.Add(tempList[i]);
                tempList.Clear();
                check = true;
                //Debug.Log("Can Down Left");
                break;
            }
           else if (add + temp.GetC() < 8 && board[y, temp.GetC() + add].GetColor() != h.GetTurn() && board[y, temp.GetC() + add].GetColor() != Piece.EMPTY) //can flip
            {
                tempList.Add(board[y, temp.GetC()+add]); //Not adding? I don't know why
                add++;
                continue;
            }
            else if (add+ temp.GetC() <8 && (((y == 0 || ((add+ temp.GetC()) == board.GetLength(0) - 1)) && board[y, temp.GetC()+ add].GetColor() != h.GetTurn()) || board[y, temp.GetC() + add].GetColor() == Piece.EMPTY)) //cannot flip
            {
              //  Debug.Log(temp.GetR()+" Here: " + (tempList.Count)+"[R,C] "+ y+","+(add+temp.GetC()));
                if(tempList.Count > 0)
                //Debug.Log(tempList[0].GetR() + " and: " + tempList[0].GetC());

                tempList.Clear();
              //  Debug.Log("Cannot Down Left ");
                break;
            }
            add++;
         
        }




        add = 1;


    if(temp.GetC() != 0)
        for (int y = temp.GetR() - 1; y >= 0; y--) //placed below, checking if theres a piece above me and Left
        {

            if (board[y, temp.GetC() - add].GetColor() == h.GetTurn()) // valid flip
            {
                if (y == temp.GetR() - 1 || temp.GetC()-add == temp.GetC() - 1) { 
                    tempList.Clear();               
                    break;
                }
                for (int i = 0; i < tempList.Count; i++)
                    flipList.Add(tempList[i]);
                tempList.Clear();
                check = true;
            //    Debug.Log("Can Down Right");
                break;
            }
            else if (temp.GetC()-add > 0 && (board[y, temp.GetC() - add].GetColor() != h.GetTurn() && board[y, temp.GetC() - add].GetColor() != Piece.EMPTY)) //can flip
            {

                        tempList.Add(board[y, temp.GetC() - add]); //Not adding? I don't know why
                add++;
                continue;
            }
            else if (((y == 0 || (( temp.GetC()-add) == 0)) && board[y, temp.GetC() - add].GetColor() != h.GetTurn()) || board[y, temp.GetC() - add].GetColor() == Piece.EMPTY) //cannot flip
            {


          //              Debug.Log(temp.GetR() + " Here: " + (tempList.Count) + "[R,C] " + y + "," + (add + temp.GetC()));
                if (tempList.Count > 0)
        //            Debug.Log(tempList[0].GetR() + " and: " + tempList[0].GetC());

                tempList.Clear();
      //          Debug.Log("Cannot Down Right ");
                break;
            }
            add++;

       }

         add = 1;
        for (int y = temp.GetR() + 1; y < board.GetLength(1); y++) //placed below, checking if theres a piece below me and right
        {
            if (add + temp.GetC() < 8 && board[y, temp.GetC() + add].GetColor() == h.GetTurn()) // valid flip
            {
                if (y == temp.GetR() + 1 || add + temp.GetC() == temp.GetC() + 1)
                {
                    tempList.Clear();
                    break;
                }
                for (int i = 0; i < tempList.Count; i++)
                    flipList.Add(tempList[i]);
                tempList.Clear();
                check = true;
               // Debug.Log("Can Up Left");
                break;
            }
            else if (add + temp.GetC() < 8 && board[y, temp.GetC() + add].GetColor() != h.GetTurn() && board[y, temp.GetC() + add].GetColor() != Piece.EMPTY) //can flip
            {
                tempList.Add(board[y, temp.GetC() + add]); //Not adding? I don't know why
                add++;
                continue;
            }
            else if (add + temp.GetC() < 8 && ((( y == board.GetLength(1) - 1 || ((add + temp.GetC()) == board.GetLength(0) - 1)) && board[y, temp.GetC() + add].GetColor() != h.GetTurn()) || board[y, temp.GetC() + add].GetColor() == Piece.EMPTY)) //cannot flip
            {
                //  Debug.Log(temp.GetR()+" Here: " + (tempList.Count)+"[R,C] "+ y+","+(add+temp.GetC()));
                if (tempList.Count > 0)
                    //Debug.Log(tempList[0].GetR() + " and: " + tempList[0].GetC());

                    tempList.Clear();
               // Debug.Log("Cannot Up Left");
                break;
            }
            add++;

        }
        add = 1;

        if (temp.GetC() != 0)
            for (int y = temp.GetR() + 1; y < board.GetLength(1); y++)//placed below, checking if theres a piece below me and Left
            {

                if (board[y, temp.GetC() - add].GetColor() == h.GetTurn()) // valid flip
                {
                    if (y == temp.GetR() +1 || temp.GetC() - add == temp.GetC() - 1)
                    {
                        tempList.Clear();
                        break;
                    }
                    for (int i = 0; i < tempList.Count; i++)
                        flipList.Add(tempList[i]);
                    tempList.Clear();
                    check = true;
                   // Debug.Log("Can Up Right");
                    break;
                }
                else if (temp.GetC() - add > 0 && (board[y, temp.GetC() - add].GetColor() != h.GetTurn() && board[y, temp.GetC() - add].GetColor() != Piece.EMPTY)) //can flip
                {

                    tempList.Add(board[y, temp.GetC() - add]); //Not adding? I don't know why
                    add++;
                    continue;
                }
                else if (((y == board.GetLength(1) - 1 || ((temp.GetC() - add) == 0)) && board[y, temp.GetC() - add].GetColor() != h.GetTurn()) || board[y, temp.GetC() - add].GetColor() == Piece.EMPTY) //cannot flip
                {


              //      Debug.Log(temp.GetR() + " Here: " + (tempList.Count) + "[R,C] " + y + "," + (add + temp.GetC()));
                  //  if (tempList.Count > 0)
                //        Debug.Log(tempList[0].GetR() + " and: " + tempList[0].GetC());

                    tempList.Clear();
            //        Debug.Log("Cannot Up Right ");
                    break;
                }
                add++;

            }


     


        if (check && flipList.Count != 0) { //Check is not working, or fliplist has 0 and still passes through the loop
          //  Debug.Log("Yes");
            return true;

    }
        else {
            tempList.Clear();
        return false; }
    }
}
