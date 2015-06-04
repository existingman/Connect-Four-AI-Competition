using System;
using System.Collections.Generic;

class LogicTemplate: PlayerLogic
{
    //Please update this file and I will include it in the poject.
    //Change the class name and file name to something unique.

    public LogicTemplate()
    {
        // Please enter your real name as the string to be used to declare the winner
        Name = "Anomynous";
    }

    public override int Move(colour[,] board)
    {
        //You must return an integer from 0 to 6 representing the column you wish to drop down from left to right.
        //board is a 2 dimensional array of enums 'colour'. 
        //colour{ blank, red, yellow }
        //Your players colour can be referenced using the keyword 'MyColour'
        //Returning an invalid value (ie: less than 0, more thn 6 or the column is full) will cause you to lose automatically.

        // the virtual class is a terrible player. Delete this.
        return base.Move(board);
    }
}

