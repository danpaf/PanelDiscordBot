

using System.Drawing;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace MainBot.Services;

public class BingoBitmapService
{
    public static int[] GenerateRandomNumbers()
    {
        Random random = new Random();
        HashSet<int> generatedNumbers = new HashSet<int>();
        int[] randomNumbers = new int[20];

        for (int i = 0; i < 20; i++)
        {
            int number;
            do
            {
                number = random.Next(1, 61);
            } while (generatedNumbers.Contains(number));
            generatedNumbers.Add(number);
            randomNumbers[i] = number;
        }
       
        return randomNumbers;
    }
    public static int[] GenerateRandomCard()
    {
        Random random = new Random();
        HashSet<int> generatedNumbers = new HashSet<int>();
        int[] randomNumbers = new int[25];

        for (int i = 0; i < 25; i++)
        {
            int number;
            do
            {
                number = random.Next(1, 61);
            } while (generatedNumbers.Contains(number));
            generatedNumbers.Add(number);
            randomNumbers[i] = number;
        }
       
        return randomNumbers;
    }


    public static async Task<DiscordMessageBuilder> GenerateBingoImage( CommandContext ctx,int[] cardNumbers)
    {
        // Create a new bitmap with dimensions 500x500
        var bitmap = new Bitmap(500, 500);

        // Create a new graphics object
        var graphics = Graphics.FromImage(bitmap);

        // Set the background color to white
        graphics.Clear(Color.White);

        // Create a new font object with a size of 30
        var font = new Font("Arial", 30);

        // Set the brush color to black
        var blackBrush = new SolidBrush(Color.Black);
        var redBrush = new SolidBrush(Color.Red);

        // Draw the numbers on the bitmap
        for (int i = 0; i < cardNumbers.Length; i++)
        {
            int x = (i % 5) * 100;
            int y = (i / 5) * 100;
            
            graphics.DrawString(cardNumbers[i].ToString(), font,blackBrush, x + 40, y + 40);
        }

        // Save the bitmap to a memory stream
        var stream = new MemoryStream();
        bitmap.Save(stream, ImageFormat.Png);
        stream.Position = 0;

        // Create a new Discord message with the bitmap as an attachment
        var message = new DiscordMessageBuilder().WithContent("").AddFile("bingo.png", stream);

        await ctx.Channel.SendMessageAsync(message);

        return message;
    }
    public static Bitmap GetColoredBingoBitmap(int[] cardNumbers, int[] calledNumbers,int bet)
    {
        // Create a new bitmap with dimensions 500x500
        var bitmap = new Bitmap(500, 500);

        // Create a new graphics object
        var graphics = Graphics.FromImage(bitmap);

        // Set the background color to white
        graphics.Clear(Color.White);

        // Create a new font object with a size of 30
        var font = new Font("Arial", 30);

        // Set the brush color to black
        var brush = new SolidBrush(Color.Black);

        // Draw the numbers on the bitmap
        for (int i = 0; i < cardNumbers.Length; i++)
        {
            int x = (i % 5) * 100;
            int y = (i / 5) * 100;
            var number = cardNumbers[i];
            if (Array.Exists(calledNumbers, num => num == number))
            {
                // Number has been called, so draw it in red
                var redBrush = new SolidBrush(Color.Red);
                graphics.FillEllipse(redBrush, x + 20, y + 20, 60, 60);
                brush = redBrush;
            }
            graphics.DrawString(bet.ToString(),font,Brushes.Green,250,0);
            graphics.DrawString(number.ToString(), font, brush, x + 40, y + 40);
            brush = new SolidBrush(Color.Black);
        }

        return bitmap;
    }
    public static Point GetNumberPositionOnBitmap(Bitmap bitmap,int[] cardNumbers, int number)
    {
        
        // Find the index of the number in the cardNumbers array
        int index = Array.IndexOf(cardNumbers, number);
        if (index < 0)
        {
            // Number not found in the array, return an invalid point
            return new Point(-1, -1);
        }

// Calculate the row and column that the number appears in on the bingo card
        int row = index / 5;
        int col = index % 5;

// Calculate the x and y pixel coordinates of the center of the square
        int x = (col * 100) + 68;
        int y = (row * 100) + 60;
        var graphics = Graphics.FromImage(bitmap);
        var redBrush = new SolidBrush(Color.Red);
        graphics.FillEllipse(redBrush, x-25 , y-25 , 50, 50);
        return new Point(x, y);

    }
    /*public static Point PutHouses(int[] cardNumbers, int multiplayer)
    {
        
        // Find the index of the number in the cardNumbers array
        int index = Array.IndexOf(cardNumbers, multiplayer);
        if (index < 0)
        {
            // Number not found in the array, return an invalid point
            return new Point(-1, -1);
        }

// Calculate the row and column that the number appears in on the bingo card
        int row = index / 5;
        int col = index % 5;

// Calculate the x and y pixel coordinates of the center of the square
        int x = (col * 100) + 68;
        int y = (row * 100) + 60;
        var graphics = Graphics.FromImage(bitmap);
        var redBrush = new SolidBrush(Color.Red);
        graphics.FillEllipse(redBrush, x , y , 60, 60);
        return new Point(x, y);

    }*/
    public static void HighlightRandomNumbers(Bitmap bitmap, int[] cardNumbers, int count)
    {
       
        var graphics = Graphics.FromImage(bitmap);
        var random = new Random();
        var indexes = Enumerable.Range(0, cardNumbers.Length).OrderBy(x => random.Next()).Take(count);
        foreach (var index in indexes)
        {
            int row = index / 5;
            int col = index % 5;
            int x = (col * 100) + 50;
            int y = (row * 100) + 38;
            var brush = new SolidBrush(Color.Blue);
            graphics.FillEllipse(brush,x-5, y-5, 50, 50);
            
        }
    }
    public static Bitmap DrawNumberOnBitmap(Bitmap bitmap, int[] cardNumbers, int[] calledNumbers, int number)
    {
        // Find the index of the number in the cardNumbers array
        int index = Array.IndexOf(cardNumbers, number);
        if (index < 0)
        {
            // Number not found in the array, return the original bitmap
            return bitmap;
        }

        // Calculate the row and column that the number appears in on the bingo card
        int row = index / 5;
        int col = index % 5;

        // Calculate the x and y pixel coordinates of the center of the square
        int x = col * 100 + 50;
        int y = row * 100 + 50;

        // Create a new graphics object from the bitmap
        var graphics = Graphics.FromImage(bitmap);

        // Create a new font object with a size of 12
        var font = new Font("Arial", 12);

        // Set the brush color to black
        var brush = new SolidBrush(Color.Black);

        // Draw the called numbers on the bitmap
        foreach (int calledNumber in calledNumbers)
        {
            int calledIndex = Array.IndexOf(cardNumbers, calledNumber);
            if (calledIndex >= 0)
            {
                int calledRow = calledIndex / 5;
                int calledCol = calledIndex % 5;
                int calledX = calledCol * 100 + 50;
                int calledY = calledRow * 100 + 50;
                var calledBrush = new SolidBrush(Color.Red);
                graphics.FillEllipse(calledBrush, calledX - 20, calledY - 20, 60, 60);
                brush = new SolidBrush(Color.Black);
            }
        }

        // Draw the random number with x2 symbol
        string numberString = number.ToString();
        if (new Random().Next(2) == 1) // 50% chance of doubling the number
        {
            numberString += " x2";
        }
        graphics.DrawString(numberString, font, brush, x, y - 20);

        return bitmap;
    }



}

