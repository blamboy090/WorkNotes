using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Timers;
using System.Media;
using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;
using System.ComponentModel.DataAnnotations;

namespace WorkNotes
{
    class Program
    {
        //Specify the file path and name
        static string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "WorkNotes.txt");
        static int noteIdCounter;
       
      
        static void Main(string[] args)
        {

            int noteIdCounter = LoadNoteIdCounter(filePath) + 1;
            


            string url = "https://youtu.be/BHkyhVre5V4";

            // Display instructions
            DisplayInstructions();

            // Wait for key press to continue
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            // Clear the console
            Console.Clear();


            // Create a timer that triggers every hour
            var timer = new System.Timers.Timer(60 * 60 * 1000); // 1 hour = 60 minutes * 60 seconds * 1000 milliseconds

            // Set up the event handler for the timer
            timer.Elapsed += TimerElapsed;

            // Start the timer
            timer.Start();

            try
            {
                //run the main program loop
                while (true) { 
                // Get user input
                Console.WriteLine("Enter notes or command:");
                string userInput = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(userInput))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: Please add a note.");
                        Console.ResetColor();
                        Console.WriteLine();
                        Console.ReadKey();
                    }

                    else if (userInput.ToLower() == "exit")
                    {
                        break;
                    }
                    else if (userInput.ToLower() == "read")
                    {
                        // Read the contents of the file
                        string fileContents = File.ReadAllText(filePath);

                        Console.Clear();

                        //Display the file contents
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("File Contents:");
                        Console.ResetColor();
                        Console.WriteLine(fileContents);
                    }
                    else if (userInput.ToLower() == "loafboi")
                    {
                        Console.Clear();
                        Console.WriteLine("FOUND ME!");
                        LaunchUrl(url);
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else if (userInput.ToLower() == "edit")
                    {
                        Console.WriteLine("Enter the ID of the entry to edit");
                        string idInput = Console.ReadLine();
                        if (int.TryParse(idInput, out int id))
                        {
                            EditEntryById(id);
                        }
                        else
                        {
                            Console.WriteLine("Invalid ID. Please enter a valid ID.");
                        }
                    }
                    else if (userInput.ToLower() == "delete")
                    {
                        Console.WriteLine("Enter the ID of the entry to delete:");
                        string idInput = Console.ReadLine();
                        if (int.TryParse(idInput, out int id))
                        {
                            DeleteEntryById(id);
                        }
                        else
                        {
                            Console.WriteLine("Invalid ID. Please enter a valid ID.");
                        }
                    }
                    else
                    {

                        //Create a timestamp with the current date and time
                        string timeStamp = DateTime.Now.ToString("G");

                        // Format the entry with the timestamp and user input
                        string entry = $"{noteIdCounter}: {timeStamp}: {userInput}{Environment.NewLine}";
                        noteIdCounter++;

                        string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                        

                       

                        // Append user input to file
                        File.AppendAllText(filePath, entry);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Text successfully written to the file.");
                        Console.ResetColor();
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            //Stop the timer when the program is finished
            timer.Stop();

            Console.ForegroundColor = ConsoleColor.Red;

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
            Console.Write("We");
            HighlightFirstInstance("l", "l");
            Console.Write("come to the W");
            HighlightFirstInstance("o", "o");
            Console.WriteLine("rkNotes program!");
            Console.Write("This program ");
            HighlightFirstInstance("a", "a");
            Console.Write("llows you to enter notes on what you worked on for the last hour and record them to a ");
            HighlightFirstInstance("f", "f");
            Console.WriteLine("ile.");
            Console.WriteLine("Instructions:");
            Console.WriteLine("1. Enter your notes and press Enter to record it to the file.");
            Console.WriteLine("2. To exit the program, type 'exit' and press Enter.");
            Console.WriteLine("3. To read the contents of the file, type 'read' and press Enter.");
            Console.WriteLine();
            Console.Write("The file will ");
            HighlightFirstInstance("b", "b");
            Console.Write("e located on your deskt");
            HighlightFirstInstance("o", "o");
            Console.Write("p as a .txt f");
            HighlightFirstInstance("i", "i");
            Console.WriteLine("le.");
            Console.WriteLine("Use it as reference while filling out your timesheet.");
            Console.WriteLine();
        }

        static void HighlightFirstInstance(string targetLetter, string text)
        {
            int index = text.IndexOf(targetLetter);
            if (index >= 0)
            {
                Console.Write(text.Substring(0, index));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(targetLetter);
                Console.ResetColor();
                Console.Write(text.Substring(index + 1));
            }
            else
            {
                Console.Write(text);
            }
        }


        static int LoadNoteIdCounter(string filePath)
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                if(lines.Length > 0)
                {
                    string lastLine = lines[lines.Length - 1];
                    string[] parts = lastLine.Split(':');
                    if (parts.Length >= 2 && int.TryParse(parts[0].Trim(), out int noteIdCounter))
                    {
                        return noteIdCounter;
                    }
                }
            }
            return 0;
        }

        static void LaunchUrl(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while launching the URL: " + ex.Message);
            }
        }

        static void EditEntryById(int id)
        {
            string[] lines = File.ReadAllLines(filePath);
            bool entryFound = false;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] parts = line.Split(':');
                if (parts.Length >= 2 && int.TryParse(parts[0].Trim(), out int noteId) && noteId == id)
                {
                    Console.WriteLine($"Current entry with ID {id}:");
                    Console.WriteLine(line);

                    Console.WriteLine("Enter the new note:");
                    string newNote = Console.ReadLine();

                    lines[i] = $"{id}: {parts[1].Trim()}: {newNote}";

                    entryFound = true;
                    break;
                }
            }

            if (entryFound)
            {
                File.WriteAllLines(filePath, lines);
                Console.WriteLine("Entry updated successfully.");
            }
            else
            {
                Console.WriteLine($"Entry with ID {id} not found.");
            }
        }
        static void DeleteEntryById(int id)
        {
            string[] lines = File.ReadAllLines(filePath);
            bool entryFound = false;
            string entryToDelete = null;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] parts = line.Split(':');
                if (parts.Length >= 2 && int.TryParse(parts[0].Trim(), out int noteId) && noteId == id)
                {
                    Console.WriteLine($"Deleting entry with ID {id}:");
                    Console.WriteLine(line);

                    entryFound = true;
                    entryToDelete = line;

                    break;
                }
            }

            if (entryFound)
            {
                Console.WriteLine("Are you sure you want to delete this entry? (Y/N)");
                string confirmation = Console.ReadLine();
                if (confirmation.ToLower() == "y")
                {
                    // Remove the entry from the array
                    lines = lines.Where(line => line != entryToDelete).ToArray();

                    File.WriteAllLines(filePath, lines);
                    Console.WriteLine("Entry deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Deletion canceled.");
                }
            }
            else
            {
                Console.WriteLine($"Entry with ID {id} not found.");
            }
        }


    }
}