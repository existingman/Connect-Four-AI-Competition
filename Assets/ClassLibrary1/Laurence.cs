using System;
using System.Collections.Generic;


class Laurence: PlayerLogic
{
    System.Random rnd = new Random();

    public Laurence()
    {
        // Please enter your real name as the string to be used to declare the winner
        Name = "Laurence";
    }

    public override int Move(colour[,] board)
    {
        float[] value = new float[7];
        for (int a = 0; a < 7; a++)
        {
            value[a] = 0;
        }

        for (int column = 0; column < 7; column++)
        {
            int row = 7;
            for (int y = 6; y >= 0; y--)
            {
                if (board[column, y] == colour.blank)
                {
                    row = y;
                }
            }
            if (row > 6)
            {
                value[column] = -100000000f;
            }
            else
            {
                colour[][] stripes = new colour[8][];
                for(int a = 0; a < 8; a++)
                {
                    stripes[a] = new colour[7];
                }

                for (int b = 0; b < 2; b++)
                {
                    for (int a = 0; a < 7; a++)
                    {
                        int x = a + column - 3;
                        int y = row +b;
                        if (x >= 0 && x < 7 && y < 7)
                        {
                            stripes[0 +b*4][a] = board[x, y];
                        }
                        else
                        {
                            stripes[0 + b * 4][a] = colour.invalid;
                        }
                    }

                    for (int a = 0; a < 7; a++)
                    {
                        int x = column;
                        int y = a + row + b - 3;
                        if (y >= 0 && y < 7)
                        {
                            stripes[1 + b * 4][a] = board[x, y];
                        }
                        else
                        {
                            stripes[1 + b * 4][a] = colour.invalid;
                        }
                    }

                    for (int a = 0; a < 7; a++)
                    {
                        int x = a + column - 3;
                        int y = a + row - 3+b;
                        if (y >= 0 && y < 7 && x >= 0 && x < 7)
                        {
                            stripes[2 + b * 4][a] = board[x, y];
                        }
                        else
                        {
                            stripes[2 + b * 4][a] = colour.invalid;
                        }
                    }

                    for (int a = 0; a < 7; a++)
                    {
                        int x = a + column - 3;
                        int y = -a + row + 3 + b;
                        if (y >= 0 && y < 7 && x >= 0 && x < 7)
                        {
                            stripes[3 + b * 4][a] = board[x, y];
                        }
                        else
                        {
                            stripes[3 + b * 4][a] = colour.invalid;
                        }
                    }
                }

                for (int stripe = 0; stripe < 4; stripe++)
                {
                    stripeCounts stripCounts = getValues(stripes[stripe]);
                    if (stripCounts.playerHardConnects > 2 || stripCounts.opponentHardConnects > 2)
                    {
                        if (stripCounts.playerHardConnects > 2)
                        {
                            value[column] += 1000000;
                        }
                        else
                        {
                            value[column] += 1000;
                        }
                    }
                    else
                    {
                        if (stripCounts.playerHardConnects + stripCounts.playerSoftConnects + stripCounts.playercontinues > 2)
                        {
                            value[column] += stripCounts.playerHardConnects + stripCounts.playerSoftConnects * 0.4f + stripCounts.playercontinues * 0.1f;
                        }
                        if (stripCounts.opponentHardConnects + stripCounts.opponentSoftConnects + stripCounts.opponentcontinues > 2)
                        {
                            value[column] += stripCounts.opponentHardConnects + stripCounts.opponentSoftConnects * 0.4f + stripCounts.opponentcontinues * 0.1f;
                        }
                    }
                }

                for (int stripe = 4; stripe < 8; stripe++)
                {
                    stripeCounts stripCounts = getValues(stripes[stripe]);
                    if (stripCounts.playerHardConnects > 2 || stripCounts.opponentHardConnects > 2)
                    {
                        if (stripCounts.playerHardConnects > 2)
                        {
                            value[column] -= 100;
                        }
                        else
                        {
                            value[column] -= 10000;
                        }
                    }
                    else
                    {
                        if (stripCounts.playerHardConnects + stripCounts.playerSoftConnects + stripCounts.playercontinues > 2)
                        {
                            value[column] -= stripCounts.playerHardConnects + stripCounts.playerSoftConnects * 0.4f + stripCounts.playercontinues * 0.1f;
                        }
                        if (stripCounts.opponentHardConnects + stripCounts.opponentSoftConnects + stripCounts.opponentcontinues > 2)
                        {
                            value[column] -= stripCounts.opponentHardConnects + stripCounts.opponentSoftConnects * 0.4f + stripCounts.opponentcontinues * 0.1f;
                        }
                    }
                }

            }
        }
        List<int> candidates = new List<int>();
        float best = -1000000000;
        int choice = 3;

        for(int a = 0; a < 7; a++)
        {
            if(value[a] > best)
            {
                candidates = new List<int>();
                candidates.Add(a);
                best = value[a];
            }
            if(value[a] == best)
            {
                candidates.Add(a);
            }
        }
        if (candidates.Count == 1)
        {
            choice = candidates[0];
        }
        else
        {
            double random = rnd.NextDouble() * candidates.Count;
            int index = (int)(System.Math.Floor(random));
            choice = candidates[index];
        }

        return choice;
    }


    struct stripeCounts
    {
        public int playerHardConnects;
        public int opponentHardConnects;
        public int playerSoftConnects;
        public int opponentSoftConnects;
        public int playercontinues;
        public int opponentcontinues;
        
    }

    private stripeCounts getValues(colour[] stripe)
    {
        stripeCounts counts = new stripeCounts();

        bool PlayerGapped = false;
        bool PlayerBlocked = false;
        bool opponentGapped = false;
        bool opponentBlocked = false;
        for (int a = 2; a >= 0; a--)
        {
            if (stripe[a] == colour.invalid)
            {
                PlayerBlocked = true;
                opponentBlocked = true;
            }

            if (stripe[a] == colour.red || stripe[a] == colour.yellow)
            {
                if (stripe[a] == MyColour)
                {
                    opponentBlocked = true;
                    if (!PlayerBlocked)
                    {
                        if (!PlayerGapped)
                        {
                            counts.playerHardConnects++;
                        }
                        else
                        {
                            counts.playerSoftConnects++;
                        }
                    }
                }
                else
                {
                    PlayerBlocked = true;
                    if (!opponentBlocked)
                    {
                        if (!opponentGapped)
                        {
                            counts.opponentHardConnects++;
                        }
                        else
                        {
                            counts.opponentSoftConnects++;
                        }
                    }
                }
            }

            if (stripe[a] == colour.blank)
            {
                if (!PlayerBlocked)
                {
                    counts.playercontinues++;
                }
                if (!opponentBlocked)
                {
                    counts.opponentcontinues++;
                }
            }
        }

        PlayerGapped = false;
        PlayerBlocked = false;
        opponentGapped = false;
        opponentBlocked = false;
        for (int a = 4; a < 7; a++)
        {
            if (stripe[a] == colour.invalid)
            {
                PlayerBlocked = true;
                opponentBlocked = true;
            }

            if (stripe[a] == colour.red || stripe[a] == colour.yellow)
            {
                if (stripe[a] == MyColour)
                {
                    opponentBlocked = true;
                    if (!PlayerBlocked)
                    {
                        if (!PlayerGapped)
                        {
                            counts.playerHardConnects++;
                        }
                        else
                        {
                            counts.playerSoftConnects++;
                        }
                    }
                }
                else
                {
                    PlayerBlocked = true;
                    if (!opponentBlocked)
                    {
                        if (!opponentGapped)
                        {
                            counts.opponentHardConnects++;
                        }
                        else
                        {
                            counts.opponentSoftConnects++;
                        }
                    }
                }
            }

            if (stripe[a] == colour.blank)
            {
                if (!PlayerBlocked)
                {
                    counts.playercontinues++;
                }
                if (!opponentBlocked)
                {
                    counts.opponentcontinues++;
                }
            }
        }

        return counts;
    }

}

