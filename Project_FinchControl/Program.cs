using FinchAPI;
using System;
using System.Collections.Generic;
using System.IO;

namespace Project_FinchControl
{

    //_____________________________________________________
    // Title: Finch Control
    // Description: 
    // Application Type: Console
    // Author: Kendrick
    // Dated Created: 
    // Last Modified: 
    // ____________________________________________________

    class Program
    {

        private enum FinchCommand
        {
            NONE,
            MOVEFORWARD,
            MOVEBACKWARD,
            STOPMOTORS, 
            WAIT,
            TURNRIGHT,
            TURNLEFT,
            LEDON,
            LEDOFF,
            DONE

        }
        static void Main(string[] args)
        {
            
            setTheme();
            displayWouldYouWantToUpdate();
            DisplayWelcomeScreen();
            DisplayMainMenu();
            DisplayClosingScreen();
        }


        static void displayWouldYouWantToUpdate()
        {
            string userResponse;
            bool ChangeTheme = false;
            Console.Clear();
            Console.WriteLine("would you like to update your theme? [yes, no]");
            userResponse = Console.ReadLine().ToLower().Trim();
            Console.WriteLine();
            do
            {
                switch (userResponse)
                {
                    case "yes":
                        updateTheam();
                        Console.WriteLine("would you like to update your theme again? [yes, no]");
                        userResponse = Console.ReadLine().ToLower().Trim();
                        if (userResponse == "no")
                        {
                            ChangeTheme = true;
                        }
                        break;

                    case "no":
                        setTheme();
                        ChangeTheme = true;
                        DisplayContinuePrompt();
                        break;

                    default:
                        Console.WriteLine("Please say yes or no");
                        Console.WriteLine("would you like to update your theme again? [yes, no]");
                        userResponse = Console.ReadLine().ToLower().Trim();
                        break;
                }
            } while (!ChangeTheme);
     

        }

        static void updateTheam()
        {
            //
            //Trying to update the theme
            //
            string updatedbackground;
            string updatedForeground;
            string dataPath = @"Data\Theme.txt";

            Console.WriteLine("what would you like the color of the foreground to be?");
            Console.WriteLine();
            Console.WriteLine("Color Options: Red, Green,");
            Console.WriteLine("Yellow, White,");
            Console.WriteLine("Cyan, Black,");
            Console.WriteLine("DarkBlue, DarkCyan,");
            Console.WriteLine("DarkGrey, DarkGreen,");
            Console.WriteLine("DarkMagenta, DarkRed,");
            Console.WriteLine("DarkYellow, Gray,");
            Console.WriteLine("Magenta, Blue");
            Console.WriteLine();

            updatedForeground = Console.ReadLine();

            Console.WriteLine("what would you like the color of the background to be?");
            Console.WriteLine();
            Console.WriteLine("Color Options: Red, Green,");
            Console.WriteLine("Yellow, White,");
            Console.WriteLine("Cyan, Black,");
            Console.WriteLine("DarkBlue, DarkCyan,");
            Console.WriteLine("DarkGrey, DarkGreen,");
            Console.WriteLine("DarkMagenta, DarkRed,");
            Console.WriteLine("DarkYellow, Gray,");
            Console.WriteLine("Magenta, Blue");
            Console.WriteLine();

            updatedbackground = Console.ReadLine();


            File.WriteAllText(dataPath, updatedForeground + "\n");
            ConsoleColor foregroundColor;

            Enum.TryParse(updatedForeground, out foregroundColor);
            Console.ForegroundColor = foregroundColor;



            File.AppendAllText(dataPath, updatedbackground);
            ConsoleColor backgroundColor;

            Enum.TryParse(updatedbackground, out backgroundColor);
            Console.BackgroundColor = backgroundColor;



        }

        static void setTheme()
        {
            //
            //saved Theme
            //
            Console.Clear();
            string dataPath = "Data\\Theme.txt";
            string[] theme;

            ConsoleColor foregroundColor;
            ConsoleColor background;
            theme = File.ReadAllLines(dataPath);

            Enum.TryParse(theme[0], out foregroundColor);
            Console.ForegroundColor = foregroundColor;

            Enum.TryParse(theme[1], out background);
            Console.BackgroundColor = background;
        }

        static void DisplayMainMenu()
        {
            Finch finchRobot = new Finch();

            //
            //Varaibles
            //
            bool finchRobotConnected = false;
            bool quitApplication = false;
            string menuChoice;

            //
            //Menu Selection
            //
            do
            {
                DisplayScreenHeader("Main Menu");
                Console.WriteLine("a.) Connect Finch Robot");
                Console.WriteLine("b.) Talent Show");
                Console.WriteLine("c.) Data Recorder");
                Console.WriteLine("d.) Alarm System");
                Console.WriteLine("e.) User Programing");
                Console.WriteLine("f.) Disconnect Finch Robot");
                Console.WriteLine("q.) Quit");
                Console.Write("Enter Choice: ");
                menuChoice = Console.ReadLine().ToUpper();


                switch (menuChoice)
                {
                    case "A":

                        if (finchRobotConnected)
                        {
                            Console.Clear();
                            Console.WriteLine("Finch robot already connected. Returning to main menu.");

                            DisplayContinuePrompt();

                        }
                        else
                        {
                            finchRobotConnected = DisplayConnectFinchRobot(finchRobot);
                        }
                        break;

                    case "B":
                        if (finchRobotConnected)
                        {
                            DisplayTalentShow(finchRobot);
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Finch robot is not connected, please go back and connect it.");
                            DisplayContinuePrompt();

                        }

                        break;

                    case "C":

                        displayDataRecorder(finchRobot);


                        break;

                    case "D":

                        if (finchRobotConnected)
                        {
                            DisplayAlarmSystem(finchRobot);
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Finch Robot not connected. Return to main menu and connect.");
                            DisplayContinuePrompt();
                        }
                        break;

                    case "E":

                        DisplayUserProgramming(finchRobot);

                        break;

                    case "F":
                        DisplayDisconnectFinchRobot(finchRobot);

                        break;

                    case "Q":
                        quitApplication = true;
                        break;

                    default:

                        Console.WriteLine();
                        Console.WriteLine("\t===================================");
                        Console.WriteLine("\tSorry, please enter a menu choice");
                        Console.WriteLine("\t===================================");
                        DisplayContinuePrompt();

                        break;
                }


            } while (!quitApplication);
        }
        #region User programming
        static void DisplayFinchCommands(List<FinchCommand> commands)
        {

            DisplayScreenHeader("display Finch Commands");
            foreach (FinchCommand command in commands)
            {

                Console.WriteLine(command);

            }
            DisplayContinuePrompt();
        }

        static void DisplayGetFinchCommands(List<FinchCommand> commands)
        {
            FinchCommand command = FinchCommand.NONE;
            string userResponse;
            bool done = false;

            //
            //Users selection
            //
            DisplayScreenHeader("Finch Robot Commands");
            Console.WriteLine("all of the commands: ");
            Console.WriteLine("Moveforward, Movebackward"); 
            Console.WriteLine("Stopmotors, Wait");
            Console.WriteLine("Turnright,Turnleft");
            Console.WriteLine("LEDon, LEDoff ");
            Console.WriteLine("When you are all done with your commands just add [Done]");

            //
            //Commands
            //
            do
            {
                Console.Write("Enter Command:");
                userResponse = Console.ReadLine().ToUpper();
                switch (userResponse)
                {
                    case "MOVEFORWARD":
                        Enum.TryParse(userResponse, out command);
                        commands.Add(command);
                        break;

                    case "MOVEBACKWARD":
                        Enum.TryParse(userResponse, out command);
                        commands.Add(command);
                        break;

                    case "STOPMOTORS":
                        Enum.TryParse(userResponse, out command);
                        commands.Add(command);
                        break;

                    case "WAIT":
                        Enum.TryParse(userResponse, out command);
                        commands.Add(command);
                        break;

                    case "TURNRIGHT":
                        Enum.TryParse(userResponse, out command);
                        commands.Add(command);
                        break;

                    case "TURNLEFT":
                        Enum.TryParse(userResponse, out command);
                        commands.Add(command);
                        break;

                    case "LEDON":
                        Enum.TryParse(userResponse, out command);
                        commands.Add(command);
                        break;

                    case "LEDOFF":
                        Enum.TryParse(userResponse, out command);
                        commands.Add(command);
                        break;
                    case "GETLIGHT":
                        Enum.TryParse(userResponse, out command);
                        commands.Add(command);
                        break;
                    case "GETTEMP":
                        Enum.TryParse(userResponse, out command);
                        commands.Add(command);
                        break;
                    case "DONE":
                        done = true;
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("This is not a valid Command.");
                        DisplayContinuePrompt();
                        Console.WriteLine();
                        break;
                }
            } while (!done);


            DisplayContinuePrompt();
        }

        static void DisplayUserProgramming(Finch finchRobot)
        {
            //
            //Varibles
            //
            string menuChoice;
            bool quitApplication = false;

            (int motorSpeed, int ledBrightness, int waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;



            List<FinchCommand> commands = new List<FinchCommand>();

            do
            {

                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("a) Set Command Parameters");
                Console.WriteLine("b) Add Commands");
                Console.WriteLine("c) View Commands");
                Console.WriteLine("d) Execute Commands");
                Console.WriteLine("e) save Commands to Text File");
                Console.WriteLine("q) Quit");
                Console.WriteLine();
                Console.Write("Enter Choice:");

                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":

                        commandParameters = DisplayGetCommandParameters();

                        break;

                    case "b":

                        DisplayGetFinchCommands(commands);

                        break;

                    case "c":

                        DisplayFinchCommands(commands);

                        break;

                    case "d":

                        DisplayExecuteCommands(finchRobot, commands, commandParameters);

                        break;


                    case "e":
                        DisplayWriteUserProgramingData(commands);

                        break;

                    case "f":

                        commands = DisplayReadUserProgrammingData();

                        break;

                    case "q":

                        quitApplication = true;

                        break;



                    default:

                        Console.WriteLine();
                        Console.WriteLine("Please enter a letter for the menu choice.");
                        DisplayContinuePrompt();

                        break;

                }



            } while (!quitApplication);

        }
        static void DisplayWriteUserProgramingData(List<FinchCommand> commands)
        {

            string dataPath = @"Data\Data.txt";
            List<string> commandsString = new List<string>();

            DisplayScreenHeader("Write Commands to the data file");

            foreach (FinchCommand command in commands)
            {
                commandsString.Add(command.ToString());
            }

            Console.WriteLine("All ready to save");
            File.WriteAllLines(dataPath, commandsString.ToArray());

            DisplayContinuePrompt();

        }

        static void DisplayExecuteCommands(Finch finchrobot, List<FinchCommand> commands, (int motorSpeed, int ledBrightness, int waitSeconds) commandParameters)
        {

            int motorSpeed = commandParameters.motorSpeed;
            int ledBrightness = commandParameters.ledBrightness;
            int waitSeconds = commandParameters.waitSeconds * 1000;

            DisplayScreenHeader("Execte Finch Commands");

            //
            // info and pause
            //
            foreach (FinchCommand command in commands)
            {
                Console.WriteLine(command);
                switch (command)
                {
                    case FinchCommand.NONE:
                        break;
                    case FinchCommand.MOVEFORWARD:
                        finchrobot.setMotors(motorSpeed, motorSpeed);
                        finchrobot.wait(waitSeconds);
                        finchrobot.setMotors(0, 0);
                        break;
                    case FinchCommand.MOVEBACKWARD:
                        finchrobot.setMotors(-motorSpeed, -motorSpeed);
                        finchrobot.wait(waitSeconds);
                        finchrobot.setMotors(0, 0);
                        break;
                    case FinchCommand.STOPMOTORS:
                        break;
                    case FinchCommand.WAIT:
                        finchrobot.wait(waitSeconds);
                        break;
                    case FinchCommand.TURNRIGHT:
                        finchrobot.setMotors(motorSpeed, -motorSpeed);
                        finchrobot.wait(waitSeconds);
                        finchrobot.setMotors(0, 0);
                        break;
                    case FinchCommand.TURNLEFT:
                        finchrobot.setMotors(-motorSpeed, motorSpeed);
                        finchrobot.wait(waitSeconds);
                        finchrobot.setMotors(0, 0);
                        break;
                    case FinchCommand.LEDON:
                        finchrobot.setLED(ledBrightness, ledBrightness, ledBrightness);
                        finchrobot.wait(waitSeconds);
                        break;
                    case FinchCommand.LEDOFF:
                        finchrobot.setLED(0, 0, 0);
                        break;
                    case FinchCommand.DONE:
                        finchrobot.setLED(0, 0, 0);
                        finchrobot.setMotors(0, 0);
                        break;
                    default:
                        break;
                }


            }

            DisplayContinuePrompt();
        }
        static List<FinchCommand> DisplayReadUserProgrammingData()
        {

            string dataPath = @"Data\Data.txt";
            List<FinchCommand> commands = new List<FinchCommand>();
            string[] commandsString;

            DisplayScreenHeader("Read commands from the data file");
            Console.WriteLine("Ready to read commands from the data file");
            Console.WriteLine();

            commandsString = File.ReadAllLines(dataPath);
            FinchCommand command;

            foreach (string commandString in commandsString)
            {
                Enum.TryParse(commandString, out command);
                commands.Add(command);
            }

            Console.WriteLine();
            Console.WriteLine("Commands read from data file done");

            DisplayContinuePrompt();

            return commands;

        }

        static (int motorSpeed, int ledBrightness, int waitSeconds) DisplayGetCommandParameters()
        {
            (int motorSpeed, int ledBrightness, int waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;
            string userResponse;

            DisplayScreenHeader("Command Paramaters");

            //
            //GEtting paramaters
            //
            bool ValidResponse = false;
            do
            {
                Console.Write("Enter Motor Speed [1 - 255]: ");
                userResponse = Console.ReadLine();
                ValidResponse = int.TryParse(userResponse, out commandParameters.motorSpeed);
                
                if (!ValidResponse)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please Enter a Valid Value");
                }

            } while (!ValidResponse);

            do
            {
                Console.Write("Enter LED brightness [1-255]");
                userResponse = Console.ReadLine();
                ValidResponse = int.TryParse(userResponse, out commandParameters.ledBrightness);
               

                  if (!ValidResponse)
                  {
                    Console.WriteLine();
                    Console.WriteLine("Please Enter a Valid Value");
                   }

            } while (!ValidResponse);

            do
            {
                Console.Write("Enter wait time ");
                userResponse = Console.ReadLine();
               ValidResponse = int.TryParse(userResponse, out commandParameters.waitSeconds);
              

                if (!ValidResponse)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please Enter a Valid Value");
                }

            } while (!ValidResponse);


            DisplayContinuePrompt();
            return commandParameters;
        }
        #endregion

        #region Temp monitoring/and Light
        static bool MonitorLightLevel(Finch finchrobot, double threshold, double minThreshold, int maxseconds)
        {
            bool thresholdExceeded = false;
            int currentLightLevel;
            double seconds = 0;

            while (!thresholdExceeded && seconds <= maxseconds)
            {
                finchrobot.setLED(0, 255, 0);
                DisplayScreenHeader("Monitoring Light Levels");
                currentLightLevel = finchrobot.getLeftLightSensor();
                Console.WriteLine($"Max Light Level: {threshold}");
                Console.WriteLine($"Current Light Level: {currentLightLevel}");

                if (currentLightLevel > threshold)
                {
                    thresholdExceeded = true;
                }

                finchrobot.wait(500);
                seconds += 0.5;
            }

            return thresholdExceeded;
        }
        static bool MonitorTemperature(Finch finchrobot, double threshold, double minThreshold, int maxseconds)
        {
            bool thresholdExceeded = false;
            double currentTemperature;
            double seconds = 0;

            while (!thresholdExceeded && seconds <= maxseconds)
            {
                finchrobot.setLED(0, 255, 0);
                DisplayScreenHeader("Monitoring Temperature");
                currentTemperature = finchrobot.getTemperature() * (9 / 5) + 32;
                Console.Write($"What is The Max Temperature:  {threshold}\u00B0F");
                Console.Write($"What is The min Temperature:  {minThreshold}\u00B0F");
                Console.WriteLine($"Current Temperature: {currentTemperature}\u00B0F");

                if (currentTemperature < minThreshold)
                {
                    thresholdExceeded = true;
                }

                if (currentTemperature > threshold)
                {
                    thresholdExceeded = true;
                }

                finchrobot.wait(1000);
                seconds += 0.5;
            }

            return thresholdExceeded;
        }

        static void DisplayAlarmSystem(Finch finchrobot)
        {
            DisplayScreenHeader("Alarm System");

            int maxseconds;
            double maxThreshold;
            double minThreshold;
            bool thresholdExceeded;
            string alarmType;

            alarmType = DisplayGetAlarmType();
            maxseconds = DisplayGetMaxSeconds();
            maxThreshold = DisplayGetMaxThresholdTemp(finchrobot, alarmType);
            minThreshold = DisplayGetMinThresholdTemp(finchrobot, alarmType);


            if (alarmType == "light")
            {
                thresholdExceeded = MonitorLightLevel(finchrobot, maxThreshold, minThreshold, maxseconds);
            }
            else
            {
                thresholdExceeded = MonitorTemperature(finchrobot, maxThreshold, minThreshold, maxseconds);
            }



            if (thresholdExceeded)
            {
                for (int i = 0; i < 5; i++)
                {
                    finchrobot.setLED(255, 0, 0);
                    finchrobot.noteOn(255);
                    finchrobot.wait(800);
                    finchrobot.setLED(0, 0, 0);
                    finchrobot.noteOff();
                    finchrobot.wait(800);
                }

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("threshold Exceeded");
            }
            else
            {
                Console.WriteLine("Ran out of time");
            }


            DisplayContinuePrompt();
        }
        static int DisplayGetMaxSeconds()
        {

            int maxSeconds;
            bool ValidInput;

            do
            {
                DisplayScreenHeader("Monitoring Time");
                Console.Write("In seconds, how long do you want to Monitor: ");
                ValidInput = int.TryParse(Console.ReadLine(), out maxSeconds);
                if (!ValidInput)
                {
                    Console.WriteLine("Please Enter a Valid Input.");
                    DisplayContinuePrompt();
                }

            } while (!ValidInput);

            DisplayContinuePrompt();

            return maxSeconds;

        }

        static string DisplayGetAlarmType()
        {
            bool ValidInput = false;
            string AlarmType;
            do
            {
                DisplayScreenHeader("Alarm Type");


                Console.WriteLine("Alarm Type light or temperature");
                AlarmType = Console.ReadLine().ToLower().Trim();
                switch (AlarmType)
                {
                    case "light":
                        ValidInput = true;
                        DisplayContinuePrompt();
                        break;
                    case "temperature":
                        ValidInput = true;
                        DisplayContinuePrompt();
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("Invaldi response, Try again");
                        DisplayContinuePrompt();
                        break;
                }
            } while (!ValidInput);

            return AlarmType;
        }

        static double DisplayGetMaxThresholdTemp(Finch finchRobot, string alarmType)
        {
            double maxthreshold = 0;
            bool ValidInput = false;


            do
            {
                DisplayScreenHeader("Threshold Value");

                switch (alarmType)
                {

                    case "temperature":
                        Console.WriteLine($"Current Temperature: {finchRobot.getTemperature() * (9 / 5) + 32}\u00B0F");
                        Console.Write("Enter Maximum Temperature:");
                        ValidInput = double.TryParse(Console.ReadLine(), out maxthreshold);
                        if (!ValidInput)
                        {
                            Console.WriteLine("Invalid input, Try agian.");
                            DisplayContinuePrompt();
                        }

                        break;

                    case "light":

                        Console.WriteLine($"Current Light Level: {finchRobot.getLeftLightSensor()+ finchRobot.getRightLightSensor()/2}");
                        Console.Write("Enter Max light level 0 - 255:");
                        ValidInput = double.TryParse(Console.ReadLine(), out maxthreshold);
                        if (!ValidInput)
                        {
                            Console.WriteLine("Invalid input, Try again.");
                            DisplayContinuePrompt();
                        }
                        break;
                    default:
                        break;
                }
            } while (!ValidInput);

            DisplayContinuePrompt();

            return maxthreshold;
        }

        static double DisplayGetMinThresholdTemp(Finch finchRobot, string alarmType)
        {
            double minThreshold = 0;

            switch (alarmType)
            {
                case "light":
                    DisplayScreenHeader("min light");
                    Console.WriteLine("Enter min light: ");
                    minThreshold = double.Parse(Console.ReadLine());
                    break;
                case "temperature":
                    DisplayScreenHeader("Min temp");

                    Console.Write("Enter Min Temperature:");
                    minThreshold = double.Parse(Console.ReadLine());
                    break;
                default:
                    break;
            }



            DisplayContinuePrompt();

            return minThreshold;
        }

        static bool MonitorLightLevel(Finch finchrobot, double threshold, int maxseconds)
        {
            bool thresholdExceeded = false;
            int currentLightLevel;
            double seconds = 0;

            while (!thresholdExceeded && seconds <= maxseconds)
            {
                finchrobot.setLED(0, 255, 0);
                DisplayScreenHeader("Monitoring Light Levels");
                currentLightLevel = finchrobot.getLeftLightSensor();
                Console.WriteLine($"Maximum Light Level: {threshold}");
                Console.WriteLine($"Current Light Level: {currentLightLevel}");

                if (currentLightLevel > threshold)
                {
                    thresholdExceeded = true;
                }

                finchrobot.wait(500);
                seconds += 0.5;
            }

            return thresholdExceeded;
        }
        #endregion

        #region DATA REC
        static double DisplayGetDataPointFrequency()
        {
            DisplayScreenHeader("Data point frequency");

            double dataPointFrequency;

            Console.Write("Enter data point frequency: ");
            double.TryParse(Console.ReadLine(), out dataPointFrequency);
            DisplayContinuePrompt();



            return dataPointFrequency;
        }

        static int DisplayGetNumberOfDataPoints()
        {
            int numberOfDataPoints;

            DisplayScreenHeader("Number of data points");

            Console.Write("Enter number of data points: ");
            int.TryParse(Console.ReadLine(), out numberOfDataPoints);

            DisplayContinuePrompt();

            return numberOfDataPoints;


        }


        static void displayDataRecorder(Finch finchRobot)
        {

            double dataPointFrequency;
            int numberOfDataPoints;
            string DataType;
            DisplayScreenHeader("Data Recorder");




            //tell the user whats going to happen 

            Console.WriteLine("we are recourding the data");

            DataType = DisplayDataTypeChoice();
            dataPointFrequency = DisplayGetDataPointFrequency();
            numberOfDataPoints = DisplayGetNumberOfDataPoints();



            //
            // Create the array
            //



            switch (DataType)
            {
                case "temperature":
                    double[] Temperatures = new double[numberOfDataPoints];

                    DisplayGetTempData(numberOfDataPoints, dataPointFrequency, Temperatures, finchRobot);

                    displayDataTemp(Temperatures);
                    break;
                case "light":
                    double[] LightLevels = new double[numberOfDataPoints];

                    DisplayGetLightData(numberOfDataPoints, dataPointFrequency, LightLevels, finchRobot);

                    displayDataLight(LightLevels);
                    break;
                default:
                    DisplayDataTypeChoice();
                    break;
            }



        }
        #region Temp
        static void displayDataTemp(double[] temperature)
        {
            DisplayScreenHeader("temperature data");



            for (int index = 0; index < temperature.Length; index++)
            {
                Console.WriteLine($"temperature {index + 1}: {temperature[index]}");
            }



            DisplayContinuePrompt();
        }



        static void displayDataLight(double[] light)
        {
            DisplayScreenHeader("Light data");



            for (int index = 0; index < light.Length; index++)
            {
                Console.WriteLine($"Light {index + 1}: {light[index]}");
            }



            DisplayContinuePrompt();
        }




        static void DisplayGetTempData(int numberOfDataPoints, double DataPointFrequency, double[] temperatures, Finch finchRobot)
        {
            Console.WriteLine("================");
            DisplayScreenHeader("Get data set");
            Console.WriteLine("================");
            //
            // Provide the user info and prompt
            //
            for (int Index = 0; Index < numberOfDataPoints; Index++)
            {

                //
                //recored data 
                //
                temperatures[Index] = (finchRobot.getTemperature() * (9 / 5)) + (32);
                int milliseconds = (int)(DataPointFrequency * 1000);
                finchRobot.wait(milliseconds);



                //
                // echo
                //
                Console.WriteLine($"Temperature {Index + 1}: {temperatures[Index]}\u00B0F");

            }

            DisplayContinuePrompt();

        }
        static void DisplayGetLightData(int numberofDataPoints, double DataPointFrequency, double[] Lightlevels, Finch finchRobot)
        {
            DisplayScreenHeader("Get Data");


            for (int index = 0; index < numberofDataPoints; index++)
            {
                Lightlevels[index] = finchRobot.getLeftLightSensor() + finchRobot.getRightLightSensor() / (2);
                int milliSeconds = ((int)(DataPointFrequency * 1000));
                finchRobot.wait(milliSeconds);


                //
                //echo
                //
                Console.WriteLine($"Light Level {index + 1}: {Lightlevels[index]}");
            }
        }

        static string DisplayDataTypeChoice()
        {
            string menuChoice;
            bool vaildInput = false;

            do
            {
                Console.WriteLine("what would you light to record? Pick light or temperature");
                menuChoice = Console.ReadLine().ToLower().Trim();

                switch (menuChoice)
                {
                    case "temperature":

                        vaildInput = true;
                        DisplayContinuePrompt();

                        break;

                    case "light":

                        vaildInput = true;
                        DisplayContinuePrompt();

                        break;

                    default:
                        Console.WriteLine("Please enter a vailed responce");
                        break;
                }


            } while (!vaildInput);

            return menuChoice;
        }
        #endregion
        #endregion

        #region Talent Show
        static void DisplayTalentShow(Finch finchRobot)
        {
            string userResponse;


            DisplayScreenHeader("Talent Show");
            Console.WriteLine("do you want a dance or a song?");
            userResponse = Console.ReadLine().Trim().ToLower();

            if (userResponse == "dance")
            {
                Console.WriteLine("are you READY FOR THIS");
                lightShow(finchRobot);
                DisplayContinuePrompt();
                Dance(finchRobot);
            }
            if (userResponse == "song")
            {
                Console.WriteLine("are you READY FOR THIS");
                lightShow(finchRobot);
                DisplayContinuePrompt();
                song(finchRobot);
            }
            else
                Console.WriteLine();
            Console.WriteLine("Please enter a vaild responce");
            DisplayContinuePrompt();





        }

        static void Dance(Finch finchRobot)
        {
            bool Dance = false;
            do
            {
                for (int i = 0; i < 5; i++)
                {
                    finchRobot.setMotors(left: -100, right: 100);
                    finchRobot.wait(1000);
                    finchRobot.setMotors(left: 0, right: 0);
                    finchRobot.setMotors(left: 100, right: -100);
                    finchRobot.wait(1000);
                    finchRobot.setMotors(left: 0, right: 0);
                }
                finchRobot.setMotors(left: 100, right: 100);
                finchRobot.wait(1000);
                finchRobot.setMotors(left: 0, right: 0);

                for (int i = 0; i < 5; i++)
                {
                    finchRobot.setMotors(left: -100, right: 100);
                    finchRobot.wait(1000);
                    finchRobot.setMotors(left: 0, right: 0);
                    finchRobot.setMotors(left: 100, right: -100);
                    finchRobot.wait(1000);
                    finchRobot.setMotors(left: 0, right: 0);
                }
                finchRobot.setMotors(left: -100, right: -100);
                finchRobot.wait(1000);
                finchRobot.setMotors(left: 0, right: 0);

                Dance = true;

            } while (Dance);

        }

        static void lightShow(Finch finchRobot)
        {
            finchRobot.setLED(255, 0, 0);
            finchRobot.wait(500);
            finchRobot.setLED(0, 255, 0);
            finchRobot.wait(500);
            finchRobot.setLED(0, 0, 255);
            finchRobot.wait(500);
            finchRobot.setLED(255, 0, 300);
        }

        static void song(Finch finchRobot)
        {

            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(167); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(400); finchRobot.wait(375); finchRobot.noteOff();
            finchRobot.setLED(255, 0, 0);
            finchRobot.noteOn(392); finchRobot.wait(375); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(392); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(330); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(440); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(494); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(466); finchRobot.wait(42); finchRobot.noteOff();
            finchRobot.noteOn(440); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(392); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(400); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(698); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(400); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(587); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(494); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(392); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(330); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(440); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(494); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(440); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(392); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(400); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(698); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(400); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(167); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(400); finchRobot.wait(375); finchRobot.noteOff();
            finchRobot.noteOn(392); finchRobot.wait(375); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(392); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(330); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(440); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(494); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(466); finchRobot.wait(42); finchRobot.noteOff();
            finchRobot.noteOn(440); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(392); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(400); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(698); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(400); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(587); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(494); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(392); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(330); finchRobot.wait(325); finchRobot.noteOff();
            finchRobot.noteOn(440); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(494); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(466); finchRobot.wait(42); finchRobot.noteOff();
            finchRobot.noteOn(440); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(392); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(400); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(698); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(400); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(550); finchRobot.wait(300); finchRobot.noteOff();
            finchRobot.noteOn(300); finchRobot.wait(300); finchRobot.noteOff();

        }

        #endregion

        #region HELPER METHODS
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("Ready to disconnect the finch robot");
            DisplayContinuePrompt();

            finchRobot.disConnect();
            Console.WriteLine();
            Console.WriteLine("Finch bot is now disconnected");

            DisplayContinuePrompt();
        }

        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            bool finchRobotConnected;

            DisplayScreenHeader("Connect to Finch Robot");

            Console.WriteLine("Ready to connect to Finch robot. Be sure to connect the USB cable to the robot and the computer.");
            DisplayContinuePrompt();

            finchRobotConnected = finchRobot.connect();

            if (finchRobotConnected)
            {
                finchRobot.setLED(0, 255, 0);
                finchRobot.noteOn(15000);
                finchRobot.wait(1000);
                finchRobot.setLED(0, 0, 0);
                finchRobot.noteOff();
                Console.WriteLine();
                Console.WriteLine("Finch robot is now connected.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Unable to connect to the Finch robot.");
            }
            return finchRobotConnected;

        }

        /// <summary>
        /// display welcome screen
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display closing screen
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }
        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion

    }


}
