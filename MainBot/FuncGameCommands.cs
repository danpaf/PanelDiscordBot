using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;

namespace MainBot;

public class FuncsBingo
{
    public static bool CheckForWin(int[] cardNumbers, int[] calledNumbers)
    {
        // Check for horizontal win
        bool win = false;
        for (int row = 0; row < 5; row++)
        {
           
            for (int col = 0; col < 5; col++)
            {
                if (!calledNumbers.Contains(cardNumbers[row * 5 + col]))
                {
                    win = false;
                    break;
                }
                
            }
            if (win)
            {
                Console.WriteLine("horz win");
                return true;
            }
        }

        // Check for vertical win
        for (int col = 0; col < 5; col++)
        {
           
            for (int row = 0; row < 5; row++)
            {
                if (!calledNumbers.Contains(cardNumbers[row * 5 + col]))
                {
                    return false;
                   
                }
                if (calledNumbers.Contains(cardNumbers[row * 5 + col]))
                {
                    Console.WriteLine("vert win");
                    return true;
                }
            }
        }
        
        if (calledNumbers.Contains(cardNumbers[0]) && calledNumbers.Contains(cardNumbers[6]) && calledNumbers.Contains(cardNumbers[12]) && calledNumbers.Contains(cardNumbers[18]) && calledNumbers.Contains(cardNumbers[24]))
        {
            Console.WriteLine("diagonal win");
            return true;
        }
        
        if (calledNumbers.Contains(cardNumbers[4]) && calledNumbers.Contains(cardNumbers[8]) && calledNumbers.Contains(cardNumbers[12]) &&
            calledNumbers.Contains(cardNumbers[16]) && calledNumbers.Contains(cardNumbers[20]))
        {
            Console.WriteLine("diagonal win");
            return true;
        }

        // No win found
        return false;
    }


}