using System.Globalization;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
class Game{
      //array list of available batters. I'm using an array list here because the user will have an option to add a custom batter later on 
        
        public void Start(){
        
        ArrayList batters = new ArrayList();

        batters.Add(new Batter
        ("Aaron Judge", 95, 55, 33, 33));
        batters.Add(new Batter
        ("Mike Trout", 75, 75, 66, 1));
        batters.Add(new Batter
        ("Vladimir Guerrero Jr.", 90, 60, 60, 5));
        batters.Add(new Batter
        ("Shohei Ohtani", 70, 80, 15, 50));
        batters.Add(new Batter
        ("Bryce Harper", 65, 85, 30, 65));

        /*now we're creating an array of fields. This won't be an array list because the user will not have the option
        to create their own field*/
        Ballpark[] parkArray = new Ballpark[3];
        parkArray[0] = (new Ballpark
        ("Rogers Center", "Toronto", 329, 328, 400));

        parkArray[1] = (new Ballpark
        ("Dodger Stadium", "Los Angeles", 330, 375, 395));

        parkArray[2] = (new Ballpark
        ("PNC Park", "Pittsburgh", 325, 320, 399));

        //array of weather options
        Weather[] weatherArray = new Weather[3];
        weatherArray[0] = (new Weather
        ("rainy", "It's starting to rain out there, but we're going to play ball anyways!"));

        weatherArray[1] = (new Weather
       ("sunny", "It's a beautiful sunny day for some baseball. Hopefully the sun doesn't get in the batters' eyes out there!"));

        weatherArray[2] = (new Weather
       ("overcast", "The sun is trying to peek through the clouds but it remains overcast out there."));    
        bool gameOver = false;
        //main game loop       
        while (!gameOver)
        {
            //startup menu
            mainMenu(batters);

            //accepting user input here
            int index = 0;
            string input = Console.ReadLine();
            if (input.ToLower() == "add")
            {
                Console.WriteLine(" ");
                Console.WriteLine("Enter a custom batter in the format 'name, power, contact, Ltendency, Rtendency', or enter 'back' to return to the main menu:");
                input = Console.ReadLine().Trim();

                if (input.ToLower() == "back")
                {
                    mainMenu(batters);
                }

                //here I've created a string array of user input
                //split(' , ') splits the elements wherever the user inputs a comma
                string[] parts = input.Split(',');

                //the array needs 5 elements or else the console will return the invalid message and return the user to the main menu
                if (parts.Length != 5)
                {
                    Console.WriteLine("Invalid input: must be in the format 'name, power, contact, Ltendency, Rtendency'");
                    Console.WriteLine(" ");
                    continue;
                }

                //element [0] will be the name of the new batter
                //string value can be anything, so the name will just be whatever is before the first comma
                string name = parts[0];

                //we need to create a double value for our other four elements
                //we will attempt to parse the remaining four user inputs to derive a double
                if (double.TryParse(parts[1], out double power) && double.TryParse(parts[2], out double contact) && double.TryParse(parts[3], out double leftTendency) && double.TryParse(parts[4], out double rightTendency))
                {
                    //the batter will only be added if it fulfills these requirements
                    //the math is dependent upon the batter attributes being between the specified values
                    if (power >= 1.0 && power <= 99.0 && contact >= 1.0 && contact <= 99.0 && leftTendency + rightTendency >= 1.0 && leftTendency + rightTendency <= 99.0 && leftTendency >= 0 && rightTendency >= 0)
                    {
                        //requirements are fulfilled, batter will now be added
                        batters.Add(new Batter(name, power, contact, leftTendency, rightTendency));
                    }
                    //the created batter did not have values matching the specifications. User returned to menu
                    else
                    {
                        Console.WriteLine("Invalid input: power, contact, and tendencies must be between 1 and 99. Tendencies must also not exceed 99 when added.");
                        Console.WriteLine();
                        continue;
                    }
                }

                //the user input for the last 4 elements were not able to parsed. User returned to main menu
                else
                {
                    Console.WriteLine("Invalid input: power and contact must be numeric values");
                    continue;
                }

                // Print out the contents of the list
                Console.WriteLine(" ");
                Console.WriteLine("New batter successfully created!");
                for (int i = batters.Count - 1; i <= batters.Count - 1; i++)
                {
                    Console.WriteLine($"{name}: power= {power}, contact= {contact}");
                }
                Console.WriteLine(" ");
                continue;
            }

            //user entered a character that did not correspond to a batter in the array list 
            else if (!int.TryParse(input, out index))
            {
                Console.WriteLine("Invalid selection. Please enter a number or 'add'.");
                Console.WriteLine(" ");
                continue;
            }

            //appropriate input and it will now be processed
            else if (index > 0 && index < batters.Count + 1)
            {
                index = int.Parse(input) - 1;
            }
        

            //user entered a number outside of the array list
            else
            {
                Console.WriteLine("invalid selection");
                Console.WriteLine(" ");
                continue;
            }

            //applying the selected batter's attributes
            Batter selectedBatter = (Batter)batters[index];
            string batterSelectionName = selectedBatter.name;
            double batterSelectionPower = selectedBatter.power;
            double batterSelectionContact = selectedBatter.contact;
            double batterSelectionLTendency = selectedBatter.leftTendency;
            double batterSelectionRTendency = selectedBatter.rightTendency;
            Console.WriteLine(" ");


            //Displaying the user's batter choice
            Console.WriteLine($"Player Selected: {batterSelectionName.ToString()}");
            Console.WriteLine(" ");

            //these variables are to keep track of stats/apply difficulty
            byte outCounter = 0;
            double maxDistance = 0;
            double homeRunCounter = 0;
            double swingCount = 0;
            double userDifficulty = 0;

            //prompting the user to input a difficulty level
            difficultySelectorScreen(ref userDifficulty, batters);

            //and finally it's time to select the field you will be playing on
            //field selection will have an impact on the distance of the fence in L/C/R
            int k = 1;
            int fieldIndex=0;

            Console.WriteLine("Select a field:");
            foreach (Ballpark park in parkArray)
            {
                Console.WriteLine(@$"Select {k} for {park.name} in {park.city}
    Left field distance= {park.leftFieldDistance}
    Center field distance= {park.centerFieldDistance}
    Right field distance= {park.rightFieldDistance}");
                k++;
                Console.WriteLine(" ");
            }
            Console.WriteLine("Or select any other key for a random field");

            //selecting a random field
            Random randomField = new Random();
            int randomFieldSelector = (randomField.Next(0, 3));

            string fieldInput = Console.ReadLine().Trim();
            if (!int.TryParse(fieldInput, out fieldIndex))
            {
                Console.WriteLine("Random field selected.");
                Console.WriteLine(" ");
                fieldIndex = randomFieldSelector;
            }

            //taking the input and selecting the corresponding field
            else if (fieldIndex > 0 && fieldIndex < parkArray.Length + 1)
            {
                fieldIndex = int.Parse(fieldInput) - 1;
            }
            else
            {
                Console.WriteLine("Random field selected");
                Console.WriteLine(" ");
                fieldIndex = randomFieldSelector;
            }

            //applying the fence distances of the ballpark
            Ballpark selectedField = (Ballpark)parkArray[fieldIndex];
            string selectedFieldName = selectedField.name;
            string selectedFieldCity = selectedField.city;
            int selectedLeftField = selectedField.leftFieldDistance;
            int selectedRightField = selectedField.rightFieldDistance;
            int selectedCenterField = selectedField.centerFieldDistance;

            //weather will be applied at random
            Random randomWeather = new Random();
            int randomWeatherSelector = (randomWeather.Next(0, 3));

            Weather weatherSelection = (Weather)weatherArray[randomWeatherSelector];
            string selectedWeatherType = weatherSelection.type;
            string selectedWeatherDescription = weatherSelection.description;


            //we're ready to play
            Console.WriteLine(" ");
            Console.WriteLine($@"Welcome to {selectedFieldName} in {selectedFieldCity}. {selectedWeatherDescription} {batterSelectionName.ToString()} steps up to the plate, settles into the box, and you know he's looking to do some damage.");

            while (outCounter < 3)
            {

                Random random = new Random();

                //randomly generating a number to determine contact, power, and direction
                double contactGenerator = (random.Next(300, 1001) / userDifficulty);
                double powerGenerator = (random.Next(2300, 4751) / userDifficulty);
                int directionLGenerator = random.Next(1, 11);
                int directionRGenerator = random.Next(1, 11);
                bool contactMade = true;
                double contactOdds = (batterSelectionContact * (contactGenerator / 75));
                double powerOdds = (batterSelectionPower * (powerGenerator / 800));

                //the weather will have an impact on the result of the at bat
                WeatherEffects(selectedWeatherType, ref contactOdds, ref powerOdds);

                //applying the fence distances of the selected field
                int fenceDistanceL = selectedField.leftFieldDistance;
                int fenceDistanceC = selectedField.centerFieldDistance;
                int fenceDistanceR = selectedField.rightFieldDistance;
                int fenceDistance = 0;

                double directionLOdds = batterSelectionLTendency * directionLGenerator;
                double directionROdds = batterSelectionRTendency * directionRGenerator;


                Console.WriteLine(" ");
                Console.WriteLine("Press any key to swing");
                string swingOption = Console.ReadLine().Trim();

                //press any key to swing
                if (swingOption != null)
                {

                    //this will determine which direction the ball was hit
                    HitDirectionCalculator(ref fenceDistance, fenceDistanceL, fenceDistanceC, fenceDistanceR, directionLOdds, directionROdds);

                    //if the contactOdds are over 300, the user made contact with the ball, otherwise it's an out
                    ContactResults(ref contactMade, ref outCounter, contactOdds, fenceDistance, fenceDistanceC, fenceDistanceL, fenceDistanceR, homeRunCounter, ref swingCount);

                    //Since the ball can only travel a distance if contact is made, distance only applies if the batter made contact
                    if (contactMade == true)
                    {
                        DistanceCalculator(powerOdds, fenceDistance, ref homeRunCounter, ref outCounter, ref maxDistance);
                    }
                }
            }


            //user turn is over when user records 3 outs

            Console.WriteLine(" ");
            Console.WriteLine("Your stats:");
            Console.WriteLine(@$"Your score = {homeRunCounter.ToString()}
Batting average = {Math.Round(homeRunCounter / swingCount, 3)}
Longest home run= {Math.Round(maxDistance, 2)} feet!");
            Console.WriteLine(" ");


            byte cpuOutCounter = 0;

            Console.WriteLine("Computer's Turn!");
            Console.WriteLine(" ");

            Random cpuPlayerGenerator = new Random();
            int cpuPlayerSelector = (cpuPlayerGenerator.Next(1, batters.Count));

            Batter cpuBatter = (Batter)batters[cpuPlayerSelector];
            string cpuSelectionName = cpuBatter.name;
            double cpuSelectionPower = cpuBatter.power;
            double cpuSelectionContact = cpuBatter.contact;
            double cpuSelectionLT = cpuBatter.leftTendency;
            double cpuSelectionRT = cpuBatter.rightTendency;

            Console.WriteLine($@"Computer has selected: {cpuBatter.name}
Power: {cpuBatter.power}
Contact: {cpuBatter.contact}");
            Console.WriteLine(" ");
            Console.WriteLine($"And now here comes {cpuSelectionName} to the plate. He needs to beat {homeRunCounter} dingers to win.");
            Console.WriteLine(" ");

            double cpuHomeRunCounter = 0;
            double cpuMaxDistance = 0;
            double cpuSwingCount = 0;

            //the cpu will now go through their turn automatically until they reach 3 outs
            //the math is similar to the user turn
            //the same format is followed as with the user turn but no prompt is needed to swing
            while (cpuOutCounter < 3)
            {
                Random random1 = new Random();
                double cpuContactGenerator = (random1.Next(300, 1001) * userDifficulty);
                double cpuPowerGenerator = (random1.Next(2300, 4751) * userDifficulty);
                int cpuDirectionLGenerator = random1.Next(1, 11);
                int cpuDirectionRGenerator = random1.Next(1, 11);

                bool cpuContactMade = true;
                double cpuContactOdds = (cpuSelectionContact * (cpuContactGenerator / 75));
                double cpuPowerOdds = (cpuSelectionPower * (cpuPowerGenerator / 800));

                WeatherEffects(selectedWeatherType, ref cpuContactOdds, ref cpuPowerOdds);

                int fenceDistanceL = selectedField.leftFieldDistance;
                int fenceDistanceC = selectedField.centerFieldDistance;
                int fenceDistanceR = selectedField.rightFieldDistance;


                double cpuDirectionLOdds = cpuSelectionLT * cpuDirectionLGenerator;
                double cpuDirectionROdds = cpuSelectionRT * cpuDirectionRGenerator;
                int fenceDistance = 0;

                HitDirectionCalculator(ref fenceDistance, fenceDistanceL, fenceDistanceC, fenceDistanceR, cpuDirectionLOdds, cpuDirectionROdds);
                ContactResults(ref cpuContactMade, ref cpuOutCounter, cpuContactOdds, fenceDistance, fenceDistanceC, fenceDistanceL, fenceDistanceR, cpuHomeRunCounter, ref cpuSwingCount);
                if (cpuContactMade == true)
                {
                    DistanceCalculator(cpuPowerOdds, fenceDistance, ref cpuHomeRunCounter, ref cpuOutCounter, ref cpuMaxDistance);
                }

            }
            //the cpu turn is over. The stats of the cpu are printed
            Console.WriteLine(" ");
            Console.WriteLine("CPU stats:");
            Console.WriteLine(@$"CPU score = {cpuHomeRunCounter.ToString()}
Batting average = {Math.Round(cpuHomeRunCounter / cpuSwingCount, 3)}
Longest home run= {Math.Round(cpuMaxDistance, 2)} feet!");
            Console.WriteLine(" ");

            //declaring a winner
            WinnerDeclaration(batterSelectionName, cpuSelectionName, homeRunCounter, cpuHomeRunCounter);

            bool exitStatus = false;

            while (!exitStatus){
            try{
            Console.WriteLine(" ");
            Console.WriteLine("Press Y to play again or N to exit.");
            Console.WriteLine(" ");
            string playAgain = Console.ReadLine().ToLower().Trim();
            if (playAgain == "y"){
                break;
                mainMenu(batters);
            }
            if (playAgain == "n")
            {
                exitStatus=true;
                Console.WriteLine("Thanks for playing. Goodbye!");
                gameOver = true;
            }
            } 
            catch (Exception ex){
            Console.WriteLine("Invalid Selection. Press 'N' to exit or 'Y' to play again");
                continue;
            }  
            }
        
        }
    }


    public static void HitDirectionCalculator(ref int fenceDistance, int fenceDistanceL, int fenceDistanceC, int fenceDistanceR, double hitterDirectionLOdds, double hitterDirectionROdds)
    {
        if (hitterDirectionLOdds >= 165 && hitterDirectionROdds < 165)
        {
            fenceDistance = fenceDistanceL;
        }
        else if (hitterDirectionROdds >= 165 && hitterDirectionLOdds < 165)
        {
            fenceDistance = fenceDistanceR;
        }
        else if (hitterDirectionLOdds >= 165 && hitterDirectionROdds >= 165)
        {
            if (hitterDirectionLOdds > hitterDirectionROdds)
            {
                fenceDistance = fenceDistanceL;
            }
            else if (hitterDirectionLOdds < hitterDirectionROdds)
            {
                fenceDistance = fenceDistanceR;
            }
            else
            {
                fenceDistance = fenceDistanceC;
            }
        }
        else
        {
            fenceDistance = fenceDistanceC;
        }
    }
    //prompting the user to input a difficulty level
    private static void difficultySelectorScreen(ref double userDifficulty, ArrayList batters)
    {
        Console.WriteLine(@"Please select your desired difficulty level
1= easy
2= medium
3= hard");
        string selectedDifficulty =
        Console.ReadLine().Trim();

        //the user difficulty variable will influence the math of the game to make it easier or harder
        switch (selectedDifficulty)
        {

            case "1":
                {
                    userDifficulty = 0.90;
                    Console.WriteLine(" ");
                    Console.WriteLine("You have selected easy.");
                    Console.WriteLine(" ");
                    break;
                }

            case "2":
                {
                    userDifficulty = 1;
                    Console.WriteLine(" ");
                    Console.WriteLine("You have selected medium.");
                    Console.WriteLine(" ");
                    break;
                }

            case "3":
                {
                    userDifficulty = 1.10;
                    Console.WriteLine(" ");
                    Console.WriteLine("You have selected hard. Good luck with that buddy.");
                    Console.WriteLine(" ");
                    break;
                }

            default:
                {
                    userDifficulty = 0.90;
                    Console.WriteLine(" ");
                    Console.WriteLine("Alright well since you can't follow basic instructions, we're just going to default the difficulty to EASY.");
                    Console.WriteLine(" ");
                    break;
                }
        }
    }
    private static void mainMenu(ArrayList batters)
    {
        int j = 0;
        Console.WriteLine("Welcome to the home run derby!");
        Console.WriteLine(" ");
        Console.WriteLine("Select a Batter or use 'add' to create your own batter!");
        Console.WriteLine(" ");
        //This foreach loop will iterate through each batter in the array list and tell the user which number to input to select their desired character
        foreach (Batter batter in batters)
        {
            Console.WriteLine(@$"Select {j + 1} for: {batter.name}  
    Power: {batter.power}
    Contact: {batter.contact}");
            Console.WriteLine(" ");
            j++;
        }
    }

    private static void WeatherEffects(string selectedWeatherType, ref double hitterContactOdds, ref double hitterPowerOdds)
    {
        if (selectedWeatherType == "rainy")
        {
            hitterPowerOdds = hitterPowerOdds - 5;
            hitterContactOdds = hitterContactOdds + 5;
        }

        if (selectedWeatherType == "sunny")
        {
            hitterPowerOdds = hitterPowerOdds + 5;
            hitterContactOdds = hitterContactOdds - 5;
        }
    }
    private static void ContactResults(ref bool hitterContactMade, ref byte hitterOutCounter, double hitterContactOdds, int fenceDistance, int fenceDistanceC, int fenceDistanceL, int fenceDistanceR, double hitterHomeRunCounter, ref double hitterSwingCount)
    {
        if (hitterContactOdds > 300)
        {
            hitterContactMade = true;
            hitterSwingCount++;
            Console.WriteLine(" ");
            Console.Write("Swung on and drilled ");
            if (fenceDistance == fenceDistanceC)
            {
                Console.WriteLine("to center field...");
            }
            if (fenceDistance == fenceDistanceL)
            {
                Console.WriteLine("to left field...");
            }
            if (fenceDistance == fenceDistanceR)
            {
                Console.WriteLine("to right field...");
            }
        }
        else
        {
            hitterContactMade = false;
            hitterOutCounter++;
            hitterSwingCount++;
            Console.WriteLine(" ");
            Console.WriteLine("Swung on and missed. He cannot be happy about that one.");
            Console.WriteLine($"That is out number {hitterOutCounter.ToString()}.");
            Console.WriteLine($"Score: {hitterHomeRunCounter.ToString()}");
            Console.WriteLine(" ");

        }
    }
    public static void DistanceCalculator(double hitterPowerOdds, int fenceDistance, ref double hitterHomeRunCounter, ref byte hitterOutCounter, ref double hitterMaxDistance)
    {
        if (hitterPowerOdds >= fenceDistance)
        {
            hitterHomeRunCounter++;
            Console.WriteLine("AND IT IS OUT OF HERE!");
            Console.WriteLine($"That went {Math.Round(hitterPowerOdds, 2)} feet!");

            if (hitterPowerOdds > 450 && hitterPowerOdds < 550)
            {
                Console.WriteLine("Wow! That was an absolute nuke!");
            }

            if (hitterPowerOdds > 550)
            {
                Console.WriteLine("I don't think that's humanly possible. You might have a random drug test coming your way.");
            }

            Console.WriteLine(" ");
            Console.WriteLine($"Score: {hitterHomeRunCounter.ToString()}");

            if (hitterOutCounter == 1)
            {
                Console.WriteLine($"And there is still only {hitterOutCounter.ToString()} out.");
            }
            else
            {
                Console.WriteLine($"And there are still {hitterOutCounter.ToString()} outs.");
            }

            Console.WriteLine(" ");
            if (hitterPowerOdds > hitterMaxDistance)
            {
                hitterMaxDistance = hitterPowerOdds;
            }
        }


        else if (hitterPowerOdds < fenceDistance)
        {
            Console.WriteLine("... and it dies out before the wall. That's an out.");
            hitterOutCounter++;
            Console.WriteLine($"That is out number {hitterOutCounter.ToString()}.");
            Console.WriteLine($"Score: {hitterHomeRunCounter.ToString()}");
        }
    }
    private static void WinnerDeclaration(string batterSelectionName, string cpuSelectionName, double homeRunCounter, double cpuHomeRunCounter)
    {
        Console.WriteLine("And that's the game!");
        Console.WriteLine(@$"And the final score is:
(user){batterSelectionName}:{homeRunCounter}
(computer){cpuSelectionName}: {cpuHomeRunCounter}");

        if (cpuHomeRunCounter == homeRunCounter)
        {
            Console.WriteLine(" ");
            Console.WriteLine("Tie game!");
        }

        else if (cpuHomeRunCounter > homeRunCounter)
        {
            Console.WriteLine(" ");
            Console.WriteLine("You lose!");
        }

        else if (cpuHomeRunCounter < homeRunCounter)
        {
            Console.WriteLine(" ");
            Console.WriteLine("You Win!");
        }
    }
}
