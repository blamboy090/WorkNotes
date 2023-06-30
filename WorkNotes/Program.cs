using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Timers;
using System.Media;

namespace WorkNotes
{
    class Program
    {
      
        static void Main(string[] args)
        {
            //Specify the file path and name
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "WorkNotes.txt");
           

            // Display instructions
            DisplayInstructions();

            // Wait for key press to continue
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            // Clear the console
            Console.Clear();


            // Create a timer that triggers every hour
            var timer = new System.Timers.Timer(60 * 60* 1000); // 1 hour = 60 minutes * 60 seconds * 1000 milliseconds

            // Set up the event handler for the timer
            timer.Elapsed += TimerElapsed;

            // Start the timer
            timer.Start();

            try
            {
                //run the main program loop
                while (true) { 
                // Get user input
                Console.WriteLine("Enter notes:");
                string userInput = Console.ReadLine();

                    if(userInput.ToLower()== "exit")
                    {
                        break;
                    }

                //Create a timestamp with the current date and time
                string timeStamp = DateTime.Now.ToString("G");

                // Format the entry with the timestamp and user input
                string entry = $"{timeStamp}: {userInput}{Environment.NewLine}";

                // Append user input to file
                File.AppendAllText(filePath, entry);

                Console.WriteLine("Text successfully written to the file.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            //Stop the timer when the program is finished
            timer.Stop();

            Console.WriteLine("Program finished. Press any key to exit.");

            Console.ReadKey();

           
        }
        static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Change the console text color to yellow
            Console.ForegroundColor = ConsoleColor.Yellow;

            // Play a chime sound
            PlayChimeSound();

            // Display the message
            Console.WriteLine("Record your time");

            // Reset the console text color
            Console.ResetColor();
        }
        static void PlayChimeSound()
        {
           Console.Beep();
        }

        static void DisplayInstructions()
        {
            string asciiArt = @"
                                _QQ
                              (_)_''>
                             _)    ";

            Console.WriteLine(asciiArt);
            Console.WriteLine();
            Console.WriteLine("Welcome to the WorkNotes program!");
            Console.WriteLine("This program allows you to enter notes on what you worked on for the last hour and record them to a file.");
            Console.WriteLine("Instructions:");
            Console.WriteLine("1. Enter a notes and press Enter to record it to the file.");
            Console.WriteLine("2. To exit the program, type 'exit' and press Enter.");
            Console.WriteLine();
            Console.WriteLine("The file will be located on your desktop as a .txt file.");
            Console.WriteLine("Use it as reference while filling out your timesheet.");
            Console.WriteLine();
        }

    }
}