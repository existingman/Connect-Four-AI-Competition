using System;
using System.Collections;
using System.Collections.Generic;


public class PlayerLogic
{
    public PlayerLogic(colour c)
    {
        _myColour = c;
    }

    public virtual int Move(colour[,] board)
    {
        System.Random rnd = new Random();
        for (; ; )
        {
            int temp = rnd.Next(0, 7);
            if (board[temp, 6] == colour.blank)
            {
                return temp;
            }
        }
    }

    private colour _myColour;
    public colour MyColour
    {
        get { return _myColour; }
    }


    public string Name = "Anomynous";
}

