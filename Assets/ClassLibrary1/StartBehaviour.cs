using UnityEngine;
using System.Collections;

public class StartBehaviour : MonoBehaviour {

    

	// Use this for initialization
	void Start () 
    {
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 7; y++)
            {
                _board[x, y] = colour.blank;
                _cubes[x, y] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                _cubes[x, y].transform.localScale = new Vector3(0.9f, 0.9f, 0.01f);
                _cubes[x, y].transform.position = new Vector3(x, y, 0);
                _cubes[x, y].gameObject.GetComponent<Renderer>().material.color = Color.gray;

                _player1 = new Laurence();
                _player2 = new Laurence();

            }
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void Reset()
    {
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 7; y++)
            {
                _board[x, y] = colour.blank;
                _cubes[x, y].gameObject.GetComponent<Renderer>().material.color = Color.gray;
            }
        }
    }


    private static colour[,] _board = new colour[7, 7];
    private static GameObject[,] _cubes = new GameObject[7, 7];

    private static PlayerLogic _player1;
    private static PlayerLogic _player2;

    bool go = false;
    bool turn = false;

    public void Play()
    {
        _player1.MyColour = colour.yellow;
        _player2.MyColour = colour.red;

        go = true;
        turn = false;

        StartCoroutine(doMatch());
    }

    

    private IEnumerator doMatch()
    {
        while (go)
        {
            yield return new WaitForSeconds(0.5f);
            PlayerLogic currentPlayer;
            colour playerColour;
            bool win = false;
            bool lose = false;
            bool tie = false;
            if (turn)
            {
                currentPlayer = _player2;
                playerColour = colour.red;
            }
            else
            {
                currentPlayer = _player1;
                playerColour = colour.yellow;
            }
            colour[,] temp = new colour[7, 7];
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    temp[x, y] = _board[x, y];
                }
            }
            int column = currentPlayer.Move(temp);
            int row;

            if (column >= 0 && column <= 6)
            {
                if (_board[column, 6] == colour.blank)
                {
                    row = 6;
                    for (int x = 5; x >= 0; x--)
                    {
                        if (_board[column, x] == colour.blank)
                        {
                            row = x;
                        }
                    }
                    _board[column, row] = playerColour;
                    if (playerColour == colour.red)
                    {
                        _cubes[column, row].gameObject.GetComponent<Renderer>().material.color = Color.red;
                    }
                    else
                    {
                        _cubes[column, row].gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                    }

                    //check for win
                    // horisontal
                    int count = 1;
                    int xPos = column;
                    int yPos = row;
                    while (xPos > 0)
                    {
                        xPos--;
                        if (_board[xPos, yPos] == playerColour)
                        {
                            count++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    xPos = column;
                    yPos = row;
                    while (xPos < 6)
                    {
                        xPos++;
                        if (_board[xPos, yPos] == playerColour)
                        {
                            count++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (count >= 4)
                    {
                        win = true;
                    }

                    //down
                    if (row > 2)
                    {
                        count = 1;
                        xPos = column;
                        yPos = row;
                        while (yPos > 0)
                        {
                            yPos--;
                            if (_board[xPos, yPos] == playerColour)
                            {
                                count++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (count >= 4)
                        {
                            win = true;
                        }
                    }

                    //right slant
                    count = 1;
                    xPos = column;
                    yPos = row;
                    while (xPos > 0 && yPos > 0)
                    {
                        xPos--;
                        yPos--;
                        if (_board[xPos, yPos] == playerColour)
                        {
                            count++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    xPos = column;
                    yPos = row;
                    while (xPos < 6 && yPos < 6)
                    {
                        xPos++;
                        yPos++;
                        if (_board[xPos, yPos] == playerColour)
                        {
                            count++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (count >= 4)
                    {
                        win = true;
                    }

                    // left slant
                    count = 1;
                    xPos = column;
                    yPos = row;
                    while (xPos > 0 && yPos < 6)
                    {
                        xPos--;
                        yPos++;
                        if (_board[xPos, yPos] == playerColour)
                        {
                            count++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    xPos = column;
                    yPos = row;
                    while (xPos < 6 && yPos > 0)
                    {
                        xPos++;
                        yPos--;
                        if (_board[xPos, yPos] == playerColour)
                        {
                            count++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (count >= 4)
                    {
                        win = true;
                    }
                }
                else
                {
                    //column full
                    Debug.Log("Invalid Output: Selected Column is full.");
                    lose = true;
                }
            }
            else
            {
                // invalid column
                Debug.Log("Invalid Output: Selected Column is out of range.");
                lose = true;
            }

            if (!win && !lose)
            {
                tie = true;
                for (int x = 0; x < 7; x++)
                {
                    if (_board[x, 6] == colour.blank)
                    {
                        tie = false;
                    }
                }
            }

            if (win)
            {
                Debug.Log(currentPlayer.Name + " (" + playerColour.ToString() + ") Wins!");
            }

            if (lose)
            {
                Debug.Log(currentPlayer.Name + " loses!");
            }

            if (tie)
            {
                Debug.Log("No spaces left!");
                Debug.Log("It's a tie...");
            }


            if (win || lose || tie)
            {
                go = false;
            }
            else
            {
                turn = !turn;
            }
        }
    }





public void LogOneThousand()
    {
        _player1.MyColour = colour.yellow;
        _player2.MyColour = colour.red;
        int player1Wins = 0;
        int player2Wins = 0;
        int ties = 0;
        bool switchPlayers = false;

        for(int g = 0; g < 1000; g++)
        {
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    _board[x, y] = colour.blank;
                }
            }
            go = true;
            turn = switchPlayers;

            while (go)
            {
                PlayerLogic currentPlayer;
                colour playerColour;
                bool win = false;
                bool lose = false;
                bool tie = false;
                if (turn)
                {
                    currentPlayer = _player2;
                    playerColour = colour.red;
                }
                else
                {
                    currentPlayer = _player1;
                    playerColour = colour.yellow;
                }
                colour[,] temp = new colour[7, 7];
                for (int x = 0; x < 7; x++)
                {
                    for (int y = 0; y < 7; y++)
                    {
                        temp[x, y] = _board[x, y];
                    }
                }
                int column = currentPlayer.Move(temp);
                int row;

                if (column >= 0 && column <= 6)
                {
                    if (_board[column, 6] == colour.blank)
                    {
                        row = 6;
                        for (int x = 5; x >= 0; x--)
                        {
                            if (_board[column, x] == colour.blank)
                            {
                                row = x;
                            }
                        }
                        _board[column, row] = playerColour;

                        //check for win
                        // horisontal
                        int count = 1;
                        int xPos = column;
                        int yPos = row;
                        while (xPos > 0)
                        {
                            xPos--;
                            if (_board[xPos, yPos] == playerColour)
                            {
                                count++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        xPos = column;
                        yPos = row;
                        while (xPos < 6)
                        {
                            xPos++;
                            if (_board[xPos, yPos] == playerColour)
                            {
                                count++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (count >= 4)
                        {
                            win = true;
                        }

                        //down
                        if (row > 2)
                        {
                            count = 1;
                            xPos = column;
                            yPos = row;
                            while (yPos > 0)
                            {
                                yPos--;
                                if (_board[xPos, yPos] == playerColour)
                                {
                                    count++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (count >= 4)
                            {
                                win = true;
                            }
                        }

                        //right slant
                        count = 1;
                        xPos = column;
                        yPos = row;
                        while (xPos > 0 && yPos > 0)
                        {
                            xPos--;
                            yPos--;
                            if (_board[xPos, yPos] == playerColour)
                            {
                                count++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        xPos = column;
                        yPos = row;
                        while (xPos < 6 && yPos < 6)
                        {
                            xPos++;
                            yPos++;
                            if (_board[xPos, yPos] == playerColour)
                            {
                                count++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (count >= 4)
                        {
                            win = true;
                        }

                        // left slant
                        count = 1;
                        xPos = column;
                        yPos = row;
                        while (xPos > 0 && yPos < 6)
                        {
                            xPos--;
                            yPos++;
                            if (_board[xPos, yPos] == playerColour)
                            {
                                count++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        xPos = column;
                        yPos = row;
                        while (xPos < 6 && yPos > 0)
                        {
                            xPos++;
                            yPos--;
                            if (_board[xPos, yPos] == playerColour)
                            {
                                count++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (count >= 4)
                        {
                            win = true;
                        }
                    }
                    else
                    {
                        //column full
                        
                        lose = true;
                    }
                }
                else
                {
                    // invalid column
                    
                    lose = true;
                }

                if (!win && !lose)
                {
                    tie = true;
                    for (int x = 0; x < 7; x++)
                    {
                        if (_board[x, 6] == colour.blank)
                        {
                            tie = false;
                        }
                    }
                }

                if (win)
                {
                    if (currentPlayer == _player1)
                    {
                        player1Wins++;
                    }
                    else
                    {
                        player2Wins++;
                    }
                }

                if (lose)
                {
                    if (currentPlayer == _player2)
                    {
                        player1Wins++;
                    }
                    else
                    {
                        player2Wins++;
                    }
                }

                if (tie)
                {
                    ties++;
                }


                if (win || lose || tie)
                {
                    go = false;
                }
                else
                {
                    turn = !turn;
                }
            }
            switchPlayers = !switchPlayers;
        }
        if (player1Wins > player2Wins)
        {
            Debug.Log(_player1.Name + " triumphs over " + _player2.Name + " with " + player1Wins.ToString() + " wins to " + player2Wins.ToString() + " and " + ties.ToString() + " ties.");
        }
        if (player2Wins > player1Wins)
        {
            Debug.Log(_player2.Name + " triumphs over " + _player1.Name + " with " + player2Wins.ToString() + " wins to " + player1Wins.ToString() + " and " + ties.ToString() + " ties.");
        }
        if (player2Wins == player1Wins)
        {
            Debug.Log("Niether " + _player1.Name + " nor " + _player2.Name + " has emerged above the other with " + player1Wins.ToString() + " each and " + ties.ToString() + " ties!");
        }
    }

    private bool CheckWin(int x, int y)
    {

        return false;
    }

}


