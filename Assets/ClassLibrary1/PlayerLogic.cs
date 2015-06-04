using System;
using System.Collections;
using System.Collections.Generic;


public class PlayerLogic
{
    public virtual int Move(colour[,] board)
    {
        System.Random rnd = new Random();
        double random;
        int temp;

        bool looking = true;
        do
        {
            random = rnd.NextDouble();
            temp = (int)(System.Math.Floor(random * 7));
            if(board[temp,6] == colour.blank)
            {
                return temp;
                looking = false;
            }
        }
        while (looking);
        return 0;
    }

    public colour MyColour;

    public string Name = "Anomynous";
}

