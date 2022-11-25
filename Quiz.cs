using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ILSQuiz
{
    internal class Quiz
    {

        //VARIABLES
        int TOTAL_QUESTIONS = 100;
        int MAX_QUESTIONS = 20;
        int PASSING_SCORE = 10;
        string CATEGORY = "science";
        string DIFFICULTY = "easy";
        string QUIZ_TYPE = "ILS Based";
        //List of all Categories Multidimensional Array
        string[][] details_list = {
            new[] { "science", "arts_and_literature", "film_and_tv", "food_and_drink", "general_knowledge", "geography", "history", "music", "society_and_culture", "sports_and_leisure", "random" },
            new[] { "easy", "medium", "hard", "random" }};

        //Program to Run on Main Class
        public void run()
        {
            mainMenu();
        }

        // START QUIZ SCREEN
        public void simpleQuiz()
        {
            Console.Clear();
            List<Question> quizQuestions = generateQuestions();
            string[] answerStorage = new string[MAX_QUESTIONS];
            string response;
            int score = 0;
            for (int i = 0; i < MAX_QUESTIONS;)
            {

                Console.Write("  Score: ");
                if (score < PASSING_SCORE)
                {
                    writeColored("" + score, ConsoleColor.Yellow);
                }
                else
                {
                    writeColored("" + score, ConsoleColor.Green);
                }

                List<string> choices = showQuestion(i, quizQuestions[i]);
                Console.WriteLine("");
                Console.WriteLine("  Type 'help' for list of Commands");
                response = getResponse(ConsoleColor.Magenta);
                switch (response)
                {
                    case "help":
                        Console.Clear();
                        Console.WriteLine("                           HELP COMMAND");
                        Console.WriteLine("  ---------------------------------------------------------------");
                        Console.WriteLine("  1 - 4            Choose an Answer from the choices");
                        Console.WriteLine("  Finish           Instantly Finishes the Quiz");
                        Console.WriteLine("  ---------------------------------------------------------------");
                        Console.WriteLine("  PRESS ENTER TO CONTINUE. . .");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case "finish":
                        Console.Clear();
                        Console.WriteLine("  -------------------------------------------------");
                        Console.WriteLine("     Are you sure you wanna end the quiz? Y / N");
                        Console.WriteLine("  -------------------------------------------------");
                        string reply = getResponse(ConsoleColor.Magenta).ToLower();
                        if (reply == "yes" || reply == "y")
                        {
                            Console.Clear();
                            finalizeScore(quizQuestions, answerStorage);
                        }
                        Console.Clear();
                        break;
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                        answerStorage[i] = choices[Convert.ToInt16(response) - 1];
                        if (quizQuestions[i].correctAnswer == answerStorage[i])
                        {
                            score++;
                            Console.Clear();
                            writeColored("  Correct Answer!\n", ConsoleColor.Green);

                        }
                        else
                        {
                            Console.Clear();
                            writeColored("  Incorrect Answer!\n", ConsoleColor.Red);
                        }
                        i++;
                        break;
                    default:
                        Console.Clear();
                        writeColored("  Invalid Input! Please enter a number between 1 - 4 only\n", ConsoleColor.Red);
                        break;
                }
            }
            Console.Clear();
            finalizeScore(quizQuestions, answerStorage);
        }
        public void complexQuiz()
        {
            Console.Clear();
            List<Question> quizQuestions = generateQuestions();
            string[] answerStorage = new string[MAX_QUESTIONS];
            string response;


            for (int i = 0; i < MAX_QUESTIONS;)
            {
                Console.Write("\n  Your Answer: ");
                if (answerStorage[i] == null || answerStorage[i] == "")
                {
                    writeColored("", ConsoleColor.Black, ConsoleColor.Gray);
                }
                else
                {
                    writeColored(" " + answerStorage[i] + " ", ConsoleColor.Black, ConsoleColor.Gray);
                }
                List<string> choices = showQuestion(i, quizQuestions[i]);

                writeColored("   << Jump 1  |  < Back        ", ConsoleColor.DarkYellow);
                Console.Write("| 1 | 2 | 3 | 4 |         ");
                writeColored("Next >  |  Jump " + MAX_QUESTIONS + " >>\n", ConsoleColor.DarkYellow);
                Console.WriteLine("  --------------------------------------------------------------------------------");
                Console.WriteLine("");
                Console.WriteLine("  Type 'help' for list of Commands");
                response = getResponse(ConsoleColor.Magenta);

                //Checking Responses 
                switch (response)
                {
                    case "help":
                        Console.Clear();
                        Console.WriteLine("                           HELP COMMAND");
                        Console.WriteLine("  ---------------------------------------------------------------");
                        Console.WriteLine("  NEXT             Move to the next question");
                        Console.WriteLine("  BACK             Returns to the previous question");
                        Console.WriteLine("  JUMP [number]    Skips to the question with the given number");
                        Console.WriteLine("  FINISH           Finishes the Quiz");
                        Console.WriteLine("  1 - 4            Choose an Answer from the choices");
                        Console.WriteLine("  ---------------------------------------------------------------");
                        Console.WriteLine("  PRESS ENTER TO CONTINUE. . .");
                        Console.ReadLine();
                        break;
                    case "back":
                        if (i > 0)
                        {
                            i--;
                        }
                        else
                        {
                            Console.Clear();
                            writeColored("  This is the first question!! You can't go back\n", ConsoleColor.Red);
                            continue;
                        }
                        break;
                    case "next":
                        if (i < MAX_QUESTIONS - 1)
                        {
                            i++;
                        }
                        else
                        {
                            Console.Clear();
                            writeColored("  This is the last question!! You can't go next\n", ConsoleColor.Red);
                            continue;

                        }
                        break;
                    case "finish":
                        Console.Clear();
                        finishQuiz(answerStorage, quizQuestions);
                        break;
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                        Console.Clear();
                        answerStorage[i] = choices[Convert.ToInt32(response) - 1];
                        break;
                    default:
                        string[] args = response.Split(' ');
                        if (args[0] == "jump")
                        {
                            if (args.Length < 2)
                            {
                                Console.Clear();
                                Console.WriteLine("  Enter a Jump Value");
                                continue;
                            }
                            string jump = args[1];
                            try
                            {
                                if (Convert.ToInt16(jump) > 0 && Convert.ToInt16(jump) <= MAX_QUESTIONS)
                                {
                                    Console.Clear();
                                    try
                                    {
                                        i = Convert.ToInt16(jump) - 1;
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Could not jump");
                                    }

                                    continue;
                                }
                                else
                                {
                                    Console.Clear();
                                    writeColored("  Invalid Jump Value!\n", ConsoleColor.Red);
                                    continue;
                                }
                            }
                            catch
                            {
                                Console.Clear();
                                writeColored("  Invalid Jump Value!\n", ConsoleColor.Red);
                                continue;
                            }

                        }

                        Console.Clear();
                        writeColored("  Please enter only the Numbers 1 - 4 when answering!\n", ConsoleColor.Red);
                        continue;

                }
                Console.Clear();

            }
        }

        // FINISH QUIZ SCREEEN
        public void finishQuiz(string[] a, List<Question> questions)
        {
            int unanswered = 0;

            Console.WriteLine("   ============== DOUBLE CHECK YOUR ANSWER =================");
            for (int i = 0; i < MAX_QUESTIONS; i++)
            {
                Console.Write("  " + (i + 1) + "). ");
                writeColored(" " + questions[i].question + "\n", ConsoleColor.Cyan);
                if (a[i] == null || a[i] == "")
                {
                    Console.WriteLine("     NONE", ConsoleColor.Red, ConsoleColor.Gray);
                    unanswered += 1;
                }
                else
                {
                    Console.WriteLine("     " + a[i], ConsoleColor.Black, ConsoleColor.Gray);
                }

                Console.WriteLine();

            }

            Console.WriteLine("\n");
            Console.WriteLine("              -------------------------------------------------\n");
            Console.WriteLine("                       Are you sure you wanna finish?\n          ");
            if (unanswered > 0)
            {
                Console.WriteLine("                  You still have " + unanswered + " Questions to Answer         ");
            }

            Console.WriteLine("              -------------------------------------------------  ");
            Console.WriteLine();
            Console.WriteLine("                      Yes / Y                 No / N");
            Console.WriteLine();

            string response = getResponse(ConsoleColor.Magenta);
            if (response == "yes" || response == "y")
            {
                Console.Clear();
                finalizeScore(questions, a);

            }
            else
            {
                Console.Clear();
                return;
            }
        }

        // MAIN MENU SCREEN
        public void mainMenu()
        {
            Console.Clear();
            while (true)
            {

                Console.WriteLine();
                Console.WriteLine("  ======================= MAIN MENU =========================\n");
                Console.WriteLine("       QUIZ TYPE    :  " + QUIZ_TYPE.ToUpper());
                Console.WriteLine("       CATEGORY     :  " + CATEGORY.ToUpper().Replace('_', ' '));
                Console.WriteLine("       DIFFICULTY   :  " + DIFFICULTY.ToUpper());
                Console.WriteLine("       MAX QUESTIONS:  " + MAX_QUESTIONS);
                Console.WriteLine("       PASSING SCORE:  " + PASSING_SCORE + "\n");

                Console.WriteLine("  ------------------------------------------------------------");
                Console.WriteLine("            Start    |    Settings    |     Exit     ");
                Console.WriteLine("  ------------------------------------------------------------");
                Console.WriteLine("");
                Console.WriteLine("  Type 'help' for list of Commands");
                string response = getResponse(ConsoleColor.Magenta);

                switch (response)
                {
                    case "help":
                        Console.Clear();
                        Console.WriteLine("                         MENU HELP COMMAND");
                        Console.WriteLine("  ---------------------------------------------------------------");
                        Console.WriteLine("  START            Starts the Quiz");
                        Console.WriteLine("  SETTINGS         Opens a menu for changing settings");
                        Console.WriteLine("  EXIT             Exits the Application");
                        Console.WriteLine(" ");
                        Console.WriteLine("                           TERMINOLOGIES");
                        Console.WriteLine("  ---------------------------------------------------------------");
                        Console.WriteLine("  Quiz Type        The Type of Quiz.");
                        Console.WriteLine("  - Experimental   The score of the user will be shown at the end of the quiz");
                        Console.WriteLine("  - ILS Based      The user cannot repeat questions but their score is always shown.");
                        Console.WriteLine();
                        Console.WriteLine("  Category         This is the Quiz Topic, Default: Science");
                        Console.WriteLine();
                        Console.WriteLine("  Difficulty       How hard are the questions. Default: Easy");
                        Console.WriteLine();
                        Console.WriteLine("  Max Questions    How many questions will be shown, Default: 20");
                        Console.WriteLine();
                        Console.WriteLine("  Passing Score    What is the passing score, Default: 10");
                        Console.WriteLine("  ---------------------------------------------------------------");
                        Console.WriteLine("  PRESS ENTER TO CONTINUE. . .");
                        Console.ReadLine();
                        break;
                    case "start":
                        if (QUIZ_TYPE == "ILS Based")
                        {
                            simpleQuiz();
                        }
                        else
                        {
                            complexQuiz();
                        }

                        break;
                    case "settings":
                        settings();
                        break;
                    case "exit":
                        Console.Clear();
                        Console.WriteLine("  ----------------------------------------");
                        Console.WriteLine("     Are you sure you wanna quit? Y / N");
                        Console.WriteLine("  ----------------------------------------");
                        response = getResponse(ConsoleColor.Magenta).ToLower();
                        if (response == "yes" || response == "y")
                        {
                            Environment.Exit(0);
                        }
                        Console.WriteLine("  Quitting Application. . .");

                        break;
                    default:
                        Console.Clear();
                        writeColored("  Invalid Option!", ConsoleColor.Red);
                        continue;
                }
                Console.Clear();
            }

        }

        // SETTINGS SCREEN
        public void settings()
        {
            Console.Clear();
            while (true)
            {

                Console.WriteLine("  ======================= SETTINGS ========================= \n");


                Console.WriteLine("  |  1.) Change CATEGORY");
                Console.WriteLine("  |  2.) Change DIFFICULTY");
                Console.WriteLine("  |  3.) Change MAX QUESTIONS");
                Console.WriteLine("  |  4.) Change PASSING SCORE");
                Console.WriteLine("  |  5.) Switch Quiz Type");
                Console.WriteLine("  |  6.) BACK\n");
                Console.WriteLine("  -----------------------------------------------------------");

                string response = getResponse(ConsoleColor.Magenta);

                switch (response)
                {

                    case "1":
                        Console.Clear();
                        changeValues("CATEGORY");
                        break;
                    case "2":
                        Console.Clear();
                        changeValues("DIFFICULTY");
                        break;

                    case "3":
                        Console.Clear();
                        changeValues("MAX QUESTIONS");
                        break;
                    case "4":
                        Console.Clear();
                        changeValues("PASSING SCORE");
                        break;
                    case "5":
                        Console.Clear();
                        changeValues("QUIZ TYPE");
                        break;
                    case "6":
                        Console.Clear();
                        mainMenu();
                        break;
                    default:
                        Console.Clear();
                        writeColored("  Invalid Input! Please enter 1 - 5 Only\n", ConsoleColor.Red);
                        break;
                }
            }
        }



        // SETTINGS CHANGE VALUES 
        public void changeValues(string variable)
        {
            Console.WriteLine("  ======================= " + variable + " ========================= \n");

            if (variable == "CATEGORY")
            {
                Console.WriteLine("  Change Category");

                Console.WriteLine("  -----------------------------------------------------------");
                for (int i = 0; i < details_list[0].Length; i++)
                {
                    Console.WriteLine("  " + (i + 1) + "). " + details_list[0][i].Replace('_', ' ').ToUpper());
                }
                Console.WriteLine("  12). BACK TO MENU");
                string response = getResponse(ConsoleColor.Magenta);
                if (response == "12")
                {
                    mainMenu();
                }
                try
                {
                    CATEGORY = details_list[0][Convert.ToInt16(response) - 1];
                    writeColored("  Successfully Changed Category to " + CATEGORY + "\n", ConsoleColor.Green);
                    Console.WriteLine("  PRESS ENTER TO CONTINUE. . .");
                    Console.ReadLine();
                    mainMenu();
                }
                catch
                {
                    Console.Clear();
                    writeColored("  Invalid Category. . . Please choose between 1 - 10 \n", ConsoleColor.Red);

                    changeValues("CATEGORY");

                }
            }
            else if (variable == "DIFFICULTY")
            {
                Console.WriteLine("  Change Difficulty");

                Console.WriteLine("  -----------------------------------------------------------");
                for (int i = 0; i < details_list[1].Length; i++)
                {
                    Console.WriteLine("  " + (i + 1) + "). " + details_list[1][i]);
                }
                Console.WriteLine("  5). BACK TO MENU");
                string response = getResponse(ConsoleColor.Magenta);
                if (response == "5")
                {
                    mainMenu();
                }
                try
                {
                    DIFFICULTY = details_list[1][Convert.ToInt16(response) - 1];
                    writeColored("  Successfully Changed Difficulty to " + DIFFICULTY + "\n", ConsoleColor.Green);
                    Console.WriteLine("  PRESS ENTER TO CONTINUE. . .");
                    Console.ReadLine();
                    mainMenu();
                }
                catch
                {
                    Console.Clear();
                    writeColored("  Invalid Difficulty. . . Please choose between 1 - 3 \n", ConsoleColor.Red);
                    changeValues("DIFFICULTY");

                }
            }
            else if (variable == "QUIZ TYPE")
            {
                if (QUIZ_TYPE == "ILS Based")
                {
                    QUIZ_TYPE = "Experimental";
                }
                else
                {
                    QUIZ_TYPE = "ILS Based";
                }
                writeColored("  Successfully Changed Quiz type to " + QUIZ_TYPE + "\n", ConsoleColor.Green);
                Console.WriteLine("  PRESS ENTER TO CONTINUE. . .");
                Console.ReadLine();
                mainMenu();

            }
            else if (variable == "MAX QUESTIONS")
            {
                Console.WriteLine("  Change Max Questions");

                Console.WriteLine("  -----------------------------------------------------------");
                Console.WriteLine("  Current Max Number of Questions:  " + MAX_QUESTIONS);
                Console.Write("  New Max Number of Questions:");
                try
                {
                    int response = Convert.ToInt16(getResponse(ConsoleColor.Magenta));
                    if (response > 0 && response <= 100)
                    {
                        MAX_QUESTIONS = response;
                        PASSING_SCORE = MAX_QUESTIONS / 2;
                        writeColored("  Successfully Changed Max Number of Questions to " + MAX_QUESTIONS + "\n", ConsoleColor.Green);
                    }
                    else
                    {
                        Console.Clear();
                        writeColored("  Amount must be between 1 - 100 \n", ConsoleColor.Red);
                        changeValues("MAX QUESTIONS");
                    }
                }
                catch
                {
                    Console.Clear();
                    writeColored("  Invalid Amount \n", ConsoleColor.Red);
                    changeValues("MAX QUESTIONS");
                }

                Console.WriteLine("  PRESS ENTER TO CONTINUE. . .");
                Console.ReadLine();
                mainMenu();
            }
            else if (variable == "PASSING SCORE")
            {
                Console.WriteLine("  Change Passing Score");

                Console.WriteLine("  -----------------------------------------------------------");
                Console.WriteLine("  Current Minimum Passing Score:  " + PASSING_SCORE);


                try
                {
                    Console.Write("  New Passing Score:");
                    int response = Convert.ToInt16(getResponse(ConsoleColor.Magenta));
                    if (response > 0 && response <= MAX_QUESTIONS)
                    {
                        PASSING_SCORE = response;
                        writeColored("  Successfully Changed Passing Score to " + PASSING_SCORE + "\n", ConsoleColor.Green);
                    }
                    else
                    {
                        Console.Clear();
                        writeColored("  Amount must be between 1 - " + MAX_QUESTIONS + " \n", ConsoleColor.Red);
                        changeValues("PASSING SCORE");
                    }

                }
                catch
                {
                    Console.Clear();
                    writeColored("  Invalid Amount \n", ConsoleColor.Red);
                    changeValues("PASSING SCORE");

                }
                Console.WriteLine("  PRESS ENTER TO CONTINUE. . .");
                Console.ReadLine();
                mainMenu();
            }
        }

        //GETTING 100 QUESTIONS FROM A DATABASE

        public List<Question> generateQuestions()
        {
            List<Question> questions = new List<Question>();
            string url = "https://the-trivia-api.com/api/questions?limit=" + 50;
            if (CATEGORY == "random" || DIFFICULTY == "random")
            {
                if (DIFFICULTY != "random")
                {
                    url += "&difficulty=" + DIFFICULTY;
                }
                else if (CATEGORY != "random")
                {
                    url += "&categories=" + CATEGORY;
                }
            }
            else
            {
                url += "&categories=" + CATEGORY + "&difficulty=" + DIFFICULTY;
            }
            using (WebClient wc = new WebClient())
            {
                dynamic json;
                json = wc.DownloadString(url);
                questions.AddRange(JsonConvert.DeserializeObject<List<Question>>(json));
            }

            using (WebClient wc = new WebClient())
            {
                dynamic json;
                json = wc.DownloadString(url);
                questions.AddRange(JsonConvert.DeserializeObject<List<Question>>(json));
            }
            return questions;
        }

        //UTILITIES I MADE FOR MYSELF
        public void writeColored(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }
        public void writeColored(string text, ConsoleColor color, ConsoleColor bg)
        {
            Console.BackgroundColor = bg;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }
        public string getResponse(ConsoleColor c)
        {
            Console.ForegroundColor = c;
            Console.Write("  ");
            string response = Console.ReadLine().ToLower();
            Console.ResetColor();
            return response;
        }

        //Format the Question here!
        public List<string> showQuestion(int i, Question q)
        {

            List<string> choices = new List<string>();
            foreach (string choice in q.incorrectAnswers)
            {
                choices.Add(choice);
            }
            choices.Add(q.correctAnswer);
            //Shuffle Question
            choices = choices.OrderBy(k => Guid.NewGuid()).ToList();




            Console.Write("\n\n  Category:  ");
            writeColored(q.category, ConsoleColor.Blue);
            Console.Write("       Difficulty:  ");
            writeColored(q.difficulty, ConsoleColor.Blue);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("  Question " + (i + 1) + ". ");
            writeColored("    " + q.question + "\n", ConsoleColor.Cyan);
            Console.WriteLine();
            //Print the Choices
            Console.WriteLine("  --------------------------------------------------------------------------------");
            for (int j = 0; j < 4; j++)
            {
                Console.WriteLine("      " + (j + 1) + "). " + choices[j] + " ");
            }

            Console.WriteLine("  --------------------------------------------------------------------------------");


            return choices;


        }

        public void finalizeScore(List<Question> question, string[] answers)
        {
            int score = 0;
            for (int i = 0; i < MAX_QUESTIONS; i++)
            {
                Console.WriteLine("  " + question[i].question);

                if (question[i].correctAnswer == answers[i])
                {
                    writeColored("   " + answers[i] + "\n", ConsoleColor.Green);
                    score++;
                }
                else
                {
                    if (answers[i] == null || answers[i] == "") writeColored("   " + "N/A" + "\n", ConsoleColor.Red);
                    else writeColored("   " + answers[i] + "\n", ConsoleColor.Red);


                    writeColored("   " + question[i].correctAnswer + "\n", ConsoleColor.Yellow);
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n\n___________________________________________________\n");
            if (score >= PASSING_SCORE)
            {
                writeColored("   You have Passed the Quiz! ", ConsoleColor.Green);
            }
            else
            {
                writeColored("   You have Failed the Quiz! ", ConsoleColor.Red);

            }

            Console.WriteLine("   You scored " + score + " / " + MAX_QUESTIONS + "\n");

            Console.WriteLine("   PRESS ENTER TO CONTINUE TO MAIN MENU. . .");
            Console.ReadLine();
            Console.Clear();
            mainMenu();
        }
    }

    // QUESTION CLASS
    public class Question
    {
        public string category { get; set; }
        public string id { get; set; }
        public string correctAnswer { get; set; }
        public List<string> incorrectAnswers { get; set; }
        public string question { get; set; }
        public List<string> tags { get; set; }
        public string type { get; set; }
        public string difficulty { get; set; }
        public List<object> regions { get; set; }
    }
}
