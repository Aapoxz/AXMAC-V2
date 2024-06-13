using System;
using System.Drawing;
using System.Management;
using Console2 = Colorful.Console;
namespace AXMACV2
{
    // Created by aapoxz
    // Goofy ass mac address changer V2

    // Changelogs
    // [+] Added gui
    // [+] Added discord server joining
    // [?] (BUG) Exitting application doesnt work properly ?

    // Todo:
    // Add HWID Changer? Or something like that
    // Fix exit bug
    // Make GUI to advanced mac changer's selection

    class Program
    {
        static void Main(string[] args)
        {
            ConsoleMenu menu = new ConsoleMenu(new string[]
            {

                "Advanced mac changer",
                "Fast mac changer",
                "Discord server",
                "Exit"
            });
        
            menu.Run();
        }
    }

    class ConsoleMenu
    {
        private string[] options;
        private int selectedIndex;

        public ConsoleMenu(string[] options)
        {
            this.options = options;
            this.selectedIndex = 0;
        }

        public void DisplayMenu()
        {
            

           

            Console.Clear();
            Console2.Title = "AXMAC - V2";
            Console2.WriteLine("           __   ____  __          _____  __      _____  \r\n     /\\    \\ \\ / /  \\/  |   /\\   / ____| \\ \\    / /__ \\ \r\n    /  \\    \\ V /| \\  / |  /  \\ | |       \\ \\  / /   ) |\r\n   / /\\ \\    > < | |\\/| | / /\\ \\| |        \\ \\/ /   / / \r\n  / ____ \\  / . \\| |  | |/ ____ \\ |____     \\  /   / /_ \r\n /_/    \\_\\/_/ \\_\\_|  |_/_/    \\_\\_____|     \\/   |____|\r\n                                                        \r\n                                                        ", Color.BlueViolet);
        
            for (int i = 0; i < options.Length; i++)
            {
                if (i == selectedIndex)
                {
                    // Highlight selected option with different colors
                    Console2.ForegroundColor = Color.BlueViolet;
                    Console.WriteLine($" >> {options[i]} << ");
                   
                    Console.ResetColor();
                }
                else
                {
                    // Regular option display
                   
                    Console.WriteLine($"    {options[i]}    ");
                }
            }

            Console.ResetColor();
        }

        public void Run()
        {
            int frequency = 400; // Frequency in Hertz (between 37 and 32767)
            int duration = 50;   // Duration in milliseconds
            int frequency2 = 100; // Frequency in Hertz (between 37 and 32767)
       
            ConsoleKey keyPressed;
            do
            {
                DisplayMenu();
                keyPressed = Console.ReadKey(true).Key;

                switch (keyPressed)
                {
                    case ConsoleKey.UpArrow:
                        Console.Beep(frequency, duration);
                        selectedIndex = (selectedIndex == 0) ? options.Length - 1 : selectedIndex - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        Console.Beep(frequency, duration);
                        selectedIndex = (selectedIndex == options.Length - 1) ? 0 : selectedIndex + 1;
                        break;
                    case ConsoleKey.Enter:
                        Console.Beep(frequency2, duration);
                        ExecuteOption();
                        break;
                }
            } 
            while (keyPressed != ConsoleKey.Escape && options[selectedIndex] != "Enviroment.Exit(0)");
        }

        private void ExecuteOption()
        {
            string newMacAddress = GenerateRandomMacAddress();
            Console.Clear();
            Console2    .BackgroundColor = Color.Cyan;
            Console2.ForegroundColor = Color.Black;
   
            Console.ResetColor();

            switch (selectedIndex)
            {
                case 0:
                    // Option 1 action
                  
                    ChangeMacAddress(newMacAddress);
                    break;
                case 1:
                    // Option 2 action
                 
                    FastChangeMacAddress(newMacAddress);
                    break;
                case 2:
                    // Option 3 action
                    Console.Write("https://discord.gg/9um4EhpGQz",Color.BlueViolet);
                    break;
                case 3:
                    // Exit action
                    Environment.Exit(1);
                    break;
            }
            static void FastChangeMacAddress(string newMacAddress)
            {
                try
                {
                    var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionID != NULL");
                    var adapters = searcher.Get().Cast<ManagementObject>();

                    var firstAdapter = adapters.FirstOrDefault();

                    if (firstAdapter != null)
                    {
                        Console.WriteLine($"");
                        firstAdapter.InvokeMethod("Disable", null);
                        firstAdapter["MACAddress"] = newMacAddress.Replace(":", "");
                        firstAdapter.Put();
                        firstAdapter.InvokeMethod("Enable", null);


                        Console2.ForegroundColor = Color.BlueViolet;
                        Console.WriteLine($"MAC address changed. (Computer restart is needed)\n");

                  
                     
                        Console.ReadLine();
                    }
                    else
                    {
                
                        Console.WriteLine("No adapters found.");
                        Console.ReadLine();
                    }
                
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            static string GenerateRandomMacAddress()
            {
                Random rand = new Random();
                byte[] macAddr = new byte[6];
                rand.NextBytes(macAddr);
                // Set the local admin bit. The second least significant bit of the first byte
                macAddr[0] = (byte)(macAddr[0] & (byte)254);
                // Ensure it is not multicast
                macAddr[0] = (byte)(macAddr[0] | (byte)2);

                return string.Join(":", macAddr.Select(b => b.ToString("X2")));
            }
            static void ChangeMacAddress(string newMacAddress)
            {
                int frequency = 400; // Frequency in Hertz (between 37 and 32767)
                int duration = 50;   // Duration in milliseconds
                int frequency2 = 100; // Frequency in Hertz (between 37 and 32767)
                try
                {
                    var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionID != NULL");
                    var adapters = searcher.Get().Cast<ManagementObject>().ToList();

                    if (adapters.Count == 0)
                    {
                        Console.WriteLine("No network adapters found.");
                        return;
                    }

                    int currentSelection = 0;
                    ConsoleKeyInfo key;

                    do
                    {
                        Console.Clear();
                       
                        Console.WriteLine("Select an adapter where you want your mac to be changed in\n");    
                        for (int i = 0; i < adapters.Count; i++)
                        {
                            if (i == currentSelection)
                            {
                                Console.BackgroundColor = ConsoleColor.Gray;
                                Console2.WriteLine($"> {adapters[i]["NetConnectionID"]}",Color.BlueViolet);
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.WriteLine($"  {adapters[i]["NetConnectionID"]}");
                            }
                        }

                        key = Console.ReadKey(true);

                        if (key.Key == ConsoleKey.UpArrow)
                        {
                            Console.Beep(frequency, duration);
                            currentSelection = (currentSelection > 0) ? currentSelection - 1 : adapters.Count - 1;
                        }
                        else if (key.Key == ConsoleKey.DownArrow)
                        {
                            Console.Beep(frequency, duration);
                            currentSelection = (currentSelection < adapters.Count - 1) ? currentSelection + 1 : 0;
                        }
                    

                    } 
                    while
                          
                    (key.Key != ConsoleKey.Enter);
                    
                    var selectedAdapter = adapters[currentSelection];

                    selectedAdapter.InvokeMethod("Disable", null);
                    selectedAdapter["MACAddress"] = newMacAddress.Replace(":", "");
                    selectedAdapter.Put();
                    selectedAdapter.InvokeMethod("Enable", null);
                    Console.WriteLine($"");

                    Console2.WriteLine($"MAC address changed. (Computer restart is needed)\n", Color.BlueViolet);
                    Console.ResetColor();

                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            Console.ReadKey();
        }
    }
}
