using UnityEngine;
using System.Collections;

public enum colour { red, yellow, blank, invalid };

public class StartBehaviour : MonoBehaviour {
    
    private colour[,] _board = new colour[7, 7];
    private GameObject[,] _cubes = new GameObject[7, 7];

    private PlayerLogic _player1;
    private PlayerLogic _player2;

    bool go = false;
    bool turn = false;
    bool display = false;
    

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

                _player1 = new Laurence( colour.yellow );
                _player2 = new Laurence( colour.red );
            }
        }
	}


    public void Reset()
    {
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 7; y++)
            {
                _board[x, y] = colour.blank;
                if (display)  _cubes[x, y].gameObject.GetComponent<Renderer>().material.color = Color.gray;
            }
        }
    }


    public void Play()
    {
        StartCoroutine(doMatch());
    }



    private IEnumerator doMatch()
    {
        go = true;
        turn = false;
        display = true;

        while(go)
        {
            yield return new WaitForSeconds(0.5f);
            switch (oneTurn())
            {
                case Result.Player1Win:
                    Debug.Log(_player1.Name + " (Yellow) Wins!");
                    go = false;
                    break;

                case Result.Player2Win:
                     Debug.Log(_player2.Name + " (Red) Wins!");
                     go = false;
                     break;

                case Result.tie:
                    Debug.Log("No spaces left!");
                    Debug.Log("It's a tie...");
                    go = false;
                    break;

                case Result.nothing:
                    turn = !turn;
                    break;
            }
        }
    }


    public void LogOneThousand()
    {
        int player1Wins = 0;
        int player2Wins = 0;
        int ties = 0;
        bool switchPlayers = false;
        display = false;

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

            while(go)
            {
                switch (oneTurn())
                {
                    case Result.Player1Win:
                        player1Wins++;
                        go = false;
                        break;

                    case Result.Player2Win:
                        player2Wins++;
                        go = false;
                        break;

                    case Result.tie:
                        ties++;
                        go = false;
                        break;

                    case Result.nothing:
                        turn = !turn;
                        break;
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
        Reset();
    }


    struct Direction
    {
        public Direction(int X, int Y)
        {
            x = X;
            y = Y;
        }
        public int x;
        public int y;
    }
    private Direction[] directions =
    {
        new Direction(0,1),
        new Direction(1,0),
        new Direction(1,1),
        new Direction(1,-1)
    };

    enum Result { Player1Win, Player2Win, tie, nothing }

    private Result oneTurn()
    {
        PlayerLogic currentPlayer;
        colour playerColour;

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

        if (column < 0 || column > 6)
        {
            if(display) Debug.Log("AI column selection is out of range!");
            if (turn)
            {
                return Result.Player1Win;
            }
            else
            {
                return Result.Player2Win;
            }
        }
        else
        {
            if (_board[column, 6] != colour.blank)
            {
                if (display) Debug.Log("Selected column is full. Selection Invalid.");
                if (turn)
                {
                    return Result.Player1Win;
                }
                else
                {
                    return Result.Player2Win;
                }
            }
            else
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
                if (display)
                {
                    if (playerColour == colour.red)
                    {
                        _cubes[column, row].gameObject.GetComponent<Renderer>().material.color = Color.red;
                    }
                    else
                    {
                        _cubes[column, row].gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                    }
                }
                foreach (Direction d in directions)
                {
                    int count = 1;
                    int xPos = column;
                    int yPos = row;
                    for (; ; )
                    {
                        xPos += d.x;
                        yPos += d.y;
                        if (xPos >= 0 && xPos <= 6 && yPos >= 0 && yPos <= 6)
                        {
                            if (_board[xPos, yPos] == playerColour)
                            {
                                count++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                    xPos = column;
                    yPos = row;
                    for (; ; )
                    {
                        xPos -= d.x;
                        yPos -= d.y;
                        if (xPos >= 0 && xPos <= 6 && yPos >= 0 && yPos <= 6)
                        {
                            if (_board[xPos, yPos] == playerColour)
                            {
                                count++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (count > 3)
                    {
                        if(turn)
                        {
                            return Result.Player2Win;
                        }
                        else
                        {
                            return Result.Player1Win;
                        }
                    }
                }

                for (int a = 0; a < 7; a++)
                {
                    if (_board[a, 6] == colour.blank)
                    {
                        return Result.nothing;
                    }
                }
                return Result.tie;
            }
        }
    }
}


