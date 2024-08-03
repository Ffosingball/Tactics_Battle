using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;

namespace strategies_battle
{
    class Record 
    {
        public string name, shortName, info;
        public int totalPoints, won, lost, minGot, maxGot, tie;

        public Record(string name, string shortName, string info) 
        {
            totalPoints = 0;
            won = 0;
            lost = 0;
            tie = 0;
            minGot = 999999999;
            maxGot = 0;
            this.name = name;
            this.shortName = shortName;
            this.info = info;
        }
    }




    class Program
    {
        static Record[] strategies;

        static Dictionary<string, Func<bool[],int,bool>> actionsToDo;

        static Random random;
        static int forCycles,forCSS,forRSS, forSCSS, forSRSS, forLCSS, forLRSS;
        static bool redefine=false,misunderstanding=false,forGrudger=true, forFriedmanNice = true, forFriedmanSus = true, ansWSLS, forFriedmanVeryNice=true;



        static void start_game(string strategy1, string strategy2, int rounds, out int points1, out int points2, out string winner, out bool[] lastAns1, out bool[] lastAns2) 
        {
            int tempPoints1, tempPoints2;

            forGrudger = true;
            forFriedmanNice = true;
            forFriedmanSus=true;
            forFriedmanVeryNice = true;
            ansWSLS = true;

            bool ans1, ans2;
            lastAns1 = new bool[rounds];
            lastAns2 = new bool[rounds];

            points1 = 0;
            points2 = 0;

            for (int i = 0; i < rounds; i++)
            {
                if (i == 0)
                    redefine = true;
                else
                    redefine = false;

                ans1 = actionsToDo[strategy1](lastAns2,i);
                ans2 = actionsToDo[strategy2](lastAns1, i);

                if (misunderstanding) 
                {
                    int n = random.Next(1,101);

                    if (n == 1) 
                    {
                        if (ans1) ans1 = false;
                        else ans1 = true;
                    }

                    if (n == 100)
                    {
                        if (ans2) ans2 = false;
                        else ans2 = true;
                    }
                }

                get_points(ans1, ans2, out tempPoints1, out tempPoints2);
                points1 = points1 + tempPoints1;
                points2 = points2 + tempPoints2;

                lastAns1[i] = ans1;
                lastAns2[i] = ans2;
            }

            if (points1>points2)
            {
                winner = strategy1;
            }
            else if (points1 == points2)
            {
                winner = strategy1+" and "+strategy2;
            }
            else
            {
                winner = strategy2;
            }
        }



        static void get_points(bool ans1, bool ans2, out int points1, out int points2) 
        {
            if (ans1 && ans2)
            {
                points1 = 3;
                points2 = 3;
            }
            else if (ans1)
            {
                points2 = 5;
                points1 = 0;
            }
            else if (ans2)
            {
                points2 = 0;
                points1 = 5;
            }
            else
            {
                points1 = 1;
                points2 = 1;
            }
        }



        static bool Random_T(bool[] lastAnsEnemy, int curTurn) 
        {
            int n = random.Next(1, 101);

            if (n<51)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        static bool Proverka_T(bool[] lastAnsEnemy, int curTurn)
        {
            if (curTurn < 10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        static bool AlwaysD_T(bool[] lastAnsEnemy, int curTurn)
        {
            return false;
        }



        static bool AlwaysC_T(bool[] lastAnsEnemy, int curTurn)
        {
            return true;
        }



        static bool User_T(bool[] lastAnsEnemy, int curTurn)
        {
            if (curTurn!=0)
            {
                if (lastAnsEnemy[curTurn-1])
                {
                    Console.WriteLine("Opponent cooperated");
                }
                else
                {
                    Console.WriteLine("Opponent deflects");
                }
            }
            else
            {
                Console.WriteLine("Game started!");
            }

            Console.WriteLine("Round "+curTurn);

            string a = Console.ReadLine();
            while (a!= "c" && a!= "d")
            {
                Console.WriteLine("Enter c - cooperate or d - deflect:");
                a = Console.ReadLine();
            }

            if (a=="c")
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        static bool Tit_for_Tat_T(bool[] lastAnsEnemy, int curTurn) 
        {
            if (curTurn==0)
            {
                return true;
            }
            else if (lastAnsEnemy[curTurn-1])
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        static bool Joss_T(bool[] lastAnsEnemy, int curTurn)
        {
            if (curTurn == 0)
            {
                return true;
            }
            else if (lastAnsEnemy[curTurn - 1])
            {
                if(random.Next(1,101)<16)
                    return false;
                else
                    return true;
            }
            else
            {
                return false;
            }
        }



        static bool Suspicious_Joss_T(bool[] lastAnsEnemy, int curTurn)
        {
            if (curTurn == 0)
            {
                return false;
            }
            else if (lastAnsEnemy[curTurn - 1])
            {
                if (random.Next(1, 101) < 16)
                    return false;
                else
                    return true;
            }
            else
            {
                return false;
            }
        }



        static bool Suspicious_Tit_for_Tat_T(bool[] lastAnsEnemy, int curTurn)
        {
            if (curTurn == 0)
            {
                return false;
            }
            else if (lastAnsEnemy[curTurn - 1])
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        static bool Nice_Tit_for_Tat_T(bool[] lastAnsEnemy, int curTurn)
        {
            if (curTurn == 0 || curTurn==1)
            {
                return true;
            }
            else if (!lastAnsEnemy[curTurn - 1] && !lastAnsEnemy[curTurn - 2])
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        static bool Tit_for_Two_Tats_T(bool[] lastAnsEnemy, int curTurn)
        {
            if (curTurn == 0)
            {
                return true;
            }
            else if (curTurn>1)
            {
                if (!lastAnsEnemy[curTurn - 1] || !lastAnsEnemy[curTurn - 2])
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (!lastAnsEnemy[curTurn - 1])
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }



        static bool Suspicious_Tit_for_Two_Tats_T(bool[] lastAnsEnemy, int curTurn)
        {
            if (curTurn == 0)
            {
                return false;
            }
            else if (curTurn > 1)
            {
                if (!lastAnsEnemy[curTurn - 1] || !lastAnsEnemy[curTurn - 2])
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (!lastAnsEnemy[curTurn - 1])
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }



        static bool Cycled_Single_Shot_T(bool[] lastAnsEnemy, int curTurn)
        {
            if (redefine)
                forCSS = random.Next(0, 5);

            if (curTurn % 5 == forCSS)
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        static bool Short_Cycled_Single_Shot_T(bool[] lastAnsEnemy, int curTurn)
        {
            if (redefine)
                forSCSS = random.Next(0, 3);

            if (curTurn % 3 == forSCSS)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        static bool Long_Cycled_Single_Shot_T(bool[] lastAnsEnemy, int curTurn)
        {
            if (redefine)
                forLCSS = random.Next(0, 10);

            if (curTurn % 10 == forLCSS)
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        static bool Reversed_Single_Shot_T(bool[] lastAnsEnemy, int curTurn)
        {
            if(redefine)
                forRSS = random.Next(0, 5);

            if (curTurn % 5 == forRSS)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        static bool Short_Reversed_Single_Shot_T(bool[] lastAnsEnemy, int curTurn)
        {
            if (redefine)
                forSRSS = random.Next(0, 3);

            if (curTurn % 3 == forSRSS)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        static bool Long_Reversed_Single_Shot_T(bool[] lastAnsEnemy, int curTurn)
        {
            if (redefine)
                forLRSS = random.Next(0, 10);

            if (curTurn % 10 == forLRSS)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        static bool Soft_Majority_T(bool[] lastAnsEnemy, int curTurn) 
        {
            float coop = 0f;

            for (int i = 0; i < curTurn+1; i++)
            {
                if (lastAnsEnemy[i]==true)
                {
                    coop++;
                }
            }

            if (curTurn==0)
            {
                return true;
            }
            else if (coop/curTurn >= 0.5f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        static bool Hard_Majority_T(bool[] lastAnsEnemy, int curTurn)
        {
            float coop = 0f;

            for (int i = 0; i < curTurn + 1; i++)
            {
                if (lastAnsEnemy[i] == true)
                {
                    coop++;
                }
            }

            if (curTurn == 0)
            {
                return true;
            }
            else if (coop / curTurn >= 0.75f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        static bool Cycled_T(bool[] lastAnsEnemy, int curTurn) 
        {
            if (redefine)
                forCycles = random.Next(0, 10 * 2);

            int p = (curTurn + (10 * 2) - forCycles) / 10;
            if (p % 2 == 1)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }



        static bool Forgiving_Tit_for_Tat_T(bool[] lastAnsEnemy, int curTurn)
        {
            if (curTurn == 0)
            {
                return true;
            }
            else if (lastAnsEnemy[curTurn - 1] == false)
            {
                int chance = random.Next(1, 101);
                if (chance < 16)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }



        static bool Tester_T(bool[] lastAnsEnemy, int curTurn)
        {
            if (curTurn == 0)
            {
                return true;
            }
            else if (curTurn == 1)
            {
                return false;
            }
            else if (curTurn == 2)
            {
                return true;
            }
            else if (lastAnsEnemy[2] == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        static bool Another_Tester_T(bool[] lastAnsEnemy, int curTurn)
        {
            if (curTurn == 0)
            {
                return false;
            }
            else if (curTurn == 1)
            {
                return true;
            }
            else if (curTurn == 2)
            {
                return false;
            }
            else if (lastAnsEnemy[2] == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        static bool Grudger_T(bool[] lastAnsEnemy, int curTurn) 
        {
            if (curTurn == 0)
                return true;
            else if (lastAnsEnemy[curTurn-1] && forGrudger)
                return true;
            else 
            {
                forGrudger = false;
                return false;
            }
        }


        static bool Friedman_T(bool[] lastAnsEnemy, int curTurn)
        {
            if (curTurn == 0)
                return true;
            else if (lastAnsEnemy[curTurn - 1] && forFriedmanSus)
                return true;
            else
            {
                forFriedmanSus = false;

                if (random.Next(1, 101) < 16)
                    return true;
                else
                    return false;
            }
        }


        static bool Nice_Friedman_T(bool[] lastAnsEnemy, int curTurn)
        {
            if (curTurn == 0)
                return true;
            else if (lastAnsEnemy[curTurn - 1] && forFriedmanNice)
                return true;
            else
            {
                forFriedmanNice = false;

                if (random.Next(1, 101) < 51)
                    return true;
                else
                    return false;
            }
        }


        static bool Very_Nice_Friedman_T(bool[] lastAnsEnemy, int curTurn)
        {
            if (curTurn == 0)
                return true;
            else if (lastAnsEnemy[curTurn - 1] && forFriedmanVeryNice)
                return true;
            else
            {
                forFriedmanVeryNice = false;

                if (random.Next(1, 101) < 81)
                    return true;
                else
                    return false;
            }
        }


        static bool Win_Stay_Loose_Shift_T(bool[] lastAnsEnemy, int curTurn) 
        {
            if (curTurn == 0)
            {
                ansWSLS = true;
                return true;
            }
            else
            {
                if (lastAnsEnemy[curTurn - 1] != ansWSLS)
                {
                    ansWSLS = lastAnsEnemy[curTurn - 1];
                }

                return ansWSLS;
            }
        }



        static void output_result(string strategy, int rounds, bool[] answers) 
        {
            Console.Write(strategy + ": \t");
            for (int i = 0; i < rounds; i++)
            {
                if (rounds < 1000)
                {
                    if (answers[i])
                    {
                        Console.Write("C");
                    }
                    else
                    {
                        Console.Write("D");
                    }
                }
            }
            Console.Write("\n");
        }



        static int input_integer(string text) 
        {
            int n=-1;

            Console.WriteLine(text);
            while (n < 0)
            {
                try
                {
                    n = int.Parse(Console.ReadLine());

                    if(n<0)
                        Console.WriteLine("Wrong input! Input number more than 0");
                }
                catch (Exception)
                {
                    Console.WriteLine("Wrong input! Input number more than 0");
                }
            }

            return n;
        }



        static string input_name_of_strategy(string text)
        {
            string ans="";
            bool exist = false;

            Console.WriteLine(text);
            while (!exist)
            {
                ans=Console.ReadLine();

                for (int i = 0; i < strategies.Length; i++) 
                {
                    if (strategies[i].shortName == ans) 
                    {
                        exist = true;
                        break;
                    }
                }

                if (!exist)
                    Console.WriteLine("This strategy does not exist!");
            }

            return ans;
        }



        static void initialize_tournament() 
        {
            int points1, points2;
            string winner;

            Console.WriteLine("Turn on small chance of misunderstanding (y - yes, everything else - no)?");
            string s = Console.ReadLine();
            misunderstanding = s == "y";

            int rounds = input_integer("How many rounds do you want to make?");

            bool[] answers1 = new bool[rounds];
            bool[] answers2 = new bool[rounds];

            string strategy1 = input_name_of_strategy("Input short name of the first strategy");
            string strategy2 = input_name_of_strategy("Input short name of the second strategy");
            Console.WriteLine(" ");

            start_game(strategy1, strategy2, rounds, out points1, out points2, out winner, out answers1, out answers2);

            output_result(strategy1, rounds, answers1);
            output_result(strategy2, rounds, answers2);
            Console.WriteLine(" ");

            Console.WriteLine("Strategy " + strategy1 + " got: " + points1);
            Console.WriteLine("Strategy " + strategy2 + " got: " + points2);
            Console.WriteLine("Winner " + winner);
            Console.WriteLine(" ");
        }



        static void initialize_multiple_tournaments() 
        {
            int won1=0, won2=0, avg1=0, avg2=0;

            Console.WriteLine("Turn on small chance of misunderstanding (y - yes, everything else - no)?");
            string s = Console.ReadLine();
            misunderstanding = s == "y";

            Console.WriteLine("Do you want to see all answers or not (y - yes, everything else - no)?");
            string show = Console.ReadLine();
            bool show2 = show == "y";

            int tournaments = input_integer("How many tournaments do you want to make?");

            int rounds = input_integer("How many rounds do you want to make?");

            string strategy1 = input_name_of_strategy("Input short name of the first strategy");
            string strategy2 = input_name_of_strategy("Input short name of the second strategy");
            Console.WriteLine(" ");

            for (int i = 1; i < tournaments + 1; i++)
            {
                bool[] answers1 = new bool[rounds];
                bool[] answers2 = new bool[rounds];
                string winner;
                int points1, points2;

                start_game(strategy1, strategy2, rounds, out points1, out points2, out winner, out answers1, out answers2);

                avg1 = avg1 + points1;
                avg2 = avg2 + points2;

                if (points1 > points2)
                {
                    won1++;
                }
                else if (points2 > points1)
                {
                    won2++;
                }

                Console.WriteLine(" ");
                Console.WriteLine("Tournament " + i);

                if (show2)
                {
                    output_result(strategy1, rounds, answers1);
                    output_result(strategy2, rounds, answers2);
                }

                Console.WriteLine("Strategy " + strategy1 + " got: " + points1);
                Console.WriteLine("Strategy " + strategy2 + " got: " + points2);
                Console.WriteLine("Winner " + winner);
            }

            avg1 = avg1 / tournaments;
            avg2 = avg2 / tournaments;

            Console.WriteLine(" ");
            Console.WriteLine("strategy " + strategy1 + "'s average points is: " + avg1);
            Console.WriteLine("strategy " + strategy2 + "'s average points is: " + avg2);
            Console.WriteLine("strategy " + strategy1 + " won " + won1+" times!");
            Console.WriteLine("strategy " + strategy2 + " won " + won2+" times!");
        }




        static void grand_multiple_tournaments() 
        {
            Console.WriteLine("Turn on small chance of misunderstanding (y - yes, everything else - no)?");
            string s = Console.ReadLine();
            misunderstanding = s == "y";

            int tournaments = input_integer("How many tournaments do you want to make?");

            int rounds = input_integer("How many rounds do you want to make?");

            for (int i = 1; i < strategies.Length; i++)
            {
                strategies[i].totalPoints = 0;
                strategies[i].won = 0;
                strategies[i].lost = 0;
                strategies[i].minGot = 999999999;
                strategies[i].maxGot = 0;
            }

            for (int k = 1; k < strategies.Length-1; k++)
            {
                string strategy1 = strategies[k].shortName;

                Console.WriteLine(" ");
                Console.WriteLine("---------");
                Console.WriteLine(" ");
                Console.WriteLine("Tournaments of "+strategy1);

                for (int j = k+1; j < strategies.Length-1; j++)
                {
                    string strategy2 = strategies[j].shortName;
                    for (int i = 1; i < tournaments + 1; i++)
                    {
                        bool[] answers1 = new bool[rounds];
                        bool[] answers2 = new bool[rounds];
                        string winner;
                        int points1, points2;

                        start_game(strategy1, strategy2, rounds, out points1, out points2, out winner, out answers1, out answers2);


                        strategies[k].totalPoints = strategies[k].totalPoints + points1;
                        strategies[j].totalPoints = strategies[j].totalPoints + points2;

                        if (strategies[k].minGot > points1)
                            strategies[k].minGot = points1;

                        if (strategies[k].maxGot < points1)
                            strategies[k].maxGot = points1;

                        if (strategies[j].minGot > points2)
                            strategies[j].minGot = points2;

                        if (strategies[j].maxGot < points2)
                            strategies[j].maxGot = points2;

                        if (points1 > points2)
                        {
                            strategies[k].won++;
                            strategies[j].lost++;
                        }
                        else if (points2 > points1)
                        {
                            strategies[j].won++;
                            strategies[k].lost++;
                        }
                        else if (points1 == points2) 
                        {
                            strategies[j].tie++;
                            strategies[k].tie++;
                        }

                        Console.WriteLine(" ");
                        Console.WriteLine("Tournament " + i);

                        Console.WriteLine("Startegy " + strategy1 + " got: " + points1);
                        Console.WriteLine("Strategy " + strategy2 + " got: " + points2);
                        Console.WriteLine("Winner " + winner);
                    }
                }
            }

            Console.WriteLine(" ");
            Console.WriteLine("---------");
            Console.WriteLine(" ");
            Console.WriteLine("Results!");

            for (int i = 1; i < strategies.Length-1; i++)
            {
                int avgPoints = strategies[i].totalPoints / (tournaments * (strategies.Length-1));
                Console.Write("Strategy " + strategies[i].name + "; total points: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(strategies[i].totalPoints);
                Console.ResetColor();
                Console.Write("; avarage points: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(avgPoints);
                Console.ResetColor();
                Console.WriteLine(" won: "+ strategies[i].won+"; lost: "+strategies[i].lost + "; ties: " + strategies[i].tie + "; minimum points got: " + strategies[i].minGot + "; maximum points got: " + strategies[i].maxGot);
            }
        }




        static void one_against_all_multiple_tournaments()
        {
            Console.WriteLine("Turn on small chance of misunderstanding (y - yes, everything else - no)?");
            string s = Console.ReadLine();
            misunderstanding = s == "y";

            int tournaments = input_integer("How many tournaments do you want to make?");

            int rounds = input_integer("How many rounds do you want to make?");

            Console.WriteLine("Do you want to see all answers or not (y - yes, everything else - no): ");
            string show= Console.ReadLine();
            bool show2 = show=="y";

            string strategy1 = input_name_of_strategy("Input short name of the strategy");

            for (int i = 1; i < strategies.Length; i++)
            {
                strategies[i].totalPoints = 0;
                strategies[i].won = 0;
                strategies[i].lost = 0;
                strategies[i].minGot = 999999999;
                strategies[i].maxGot = 0;
            }

            int k = -1;
            for (int i = 1; i < strategies.Length; i++)
            {
                if (strategies[i].shortName==strategy1)
                    k = i;
            }

            for (int j = 1; j < strategies.Length-1; j++)
            {
                if (k != j)
                {
                    string tactica2 = strategies[j].shortName;
                    for (int i = 1; i < tournaments + 1; i++)
                    {
                        bool[] res1 = new bool[rounds];
                        bool[] res2 = new bool[rounds];
                        string winner;
                        int points1, points2;

                        start_game(strategy1, tactica2, rounds, out points1, out points2, out winner, out res1, out res2);

                        strategies[k].totalPoints = strategies[k].totalPoints + points1;

                        if (strategies[k].minGot > points1)
                            strategies[k].minGot = points1;

                        if (strategies[k].maxGot < points1)
                            strategies[k].maxGot = points1;

                        if (points1 > points2)
                        {
                            strategies[k].won++;
                        }
                        else if (points2 > points1)
                        {
                            strategies[k].lost++;
                        }
                        else if (points1 == points2)
                        {
                            strategies[k].tie++;
                        }

                        Console.WriteLine(" ");
                        Console.WriteLine("Tournament " + i);

                        if (show2) 
                        {
                            output_result(strategy1, rounds, res1);
                            output_result(tactica2, rounds, res2);
                        }

                        Console.WriteLine("Strategy " + strategy1 + " got: " + points1);
                        Console.WriteLine("Strategy " + tactica2 + " got: " + points2);
                        Console.WriteLine("Winner " + winner);
                    }
                }
                
            }

            Console.WriteLine(" ");
            Console.WriteLine("---------");
            Console.WriteLine(" ");
            Console.WriteLine("Results!");

            int avgPoints = strategies[k].totalPoints / (tournaments * (strategies.Length - 2));
            Console.Write("Strategy " + strategies[k].name + "; total points: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(strategies[k].totalPoints);
            Console.ResetColor();
            Console.Write("; avarage points: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(avgPoints);
            Console.ResetColor();
            Console.WriteLine(" won: " + strategies[k].won + "; lost: " + strategies[k].lost + "; ties: " + strategies[k].tie + "; minimum points got: " + strategies[k].minGot + "; maximum points got: " + strategies[k].maxGot);
        }



        static void output_tactics() 
        {
            Console.WriteLine("List of all stategies: ");
            Console.WriteLine(" ");

            for (int i = 0; i < strategies.Length; i++)
            {
                Console.WriteLine(i + ") Name: " + strategies[i].name + "; short name: " + strategies[i].shortName);
                Console.WriteLine("Explanation: " + strategies[i].info);
            }
        }


        //Добавить больше тактик. Найти где я спрашивал у них чатаГПТ и спросить ещё чтобы было штук 40(January, Strat models in Axelrod)


        static void Main(string[] args)
        {
            strategies= new Record[29]
            {
                new Record ("User","U","If you want to play against other strategies"),
                new Record("Random","R","Randomly cooperate or deflects" ),
                new Record ("Tit for Tat(Mirror)","TfT","It usually cooperates, but if opponent deflects in the last turn, it will deflect in the next"),
                new Record ("AlwaysC","C","Always cooperate"),
                new Record ("AlwaysD","D","Always deflects"),
                new Record ("Tester","Te","In the first turn, it cooperates, in the second it deflects to test the opponent. If the opponent deflects on the third turn, it cooperates for the rest of the game. Otherwise, it will deflects all subsequent turns" ),
                new Record ("Another Tester","ATe","In the first turn, it deflects, in the second it cooperates to test the opponent. If the opponent cooperate on the third turn, it cooperates for the rest of the game. Otherwise, it will deflects all subsequent turns" ),
                new Record ("Cycled","Cy","Cyclically given number of turns deflects then the same number of turns cooperates" ),
                new Record ("Cycled Single Shot","CSS","Deflectss after every 4 steps" ),
                new Record ("Short Cycled Single Shot","SCSS","Deflectss after every 2 steps" ),
                new Record ("Long Cycled Single Shot","LCSS","Deflectss after every 9 steps" ),
                new Record ("Forgiving Tit for Tat","FTfT","It usually cooperates, but if opponent deflects in the last turn, it will deflect in the next turn with 85% chance" ),
                new Record ("Nice Tit for Tat(Tit for Two Tats)","NTfT","Similar to Tit for Tat, but it begins by cooperating and continues to cooperate as long as the opponent does not deflect twice in a row. If the opponent do that, Nice Tit for Tat deflects permanently" ),
                new Record ("Suspicious Tit for Tat","STfT","A variation of Tit for Tat, where it starts by deflecting and then mirrors the opponent's previous move. This strategy is designed to initially avoid exploitation by deflecting opponents" ),
                new Record ("Two Tits for Tat","TTfT","Cooperates unless the opponent deflects, in which case it deflects twice in a row" ),
                new Record ("Suspicious Two Tits for Tat","STTfT","The same as usual TTfT but deflects at the first turn" ),
                new Record ("Reversed Single Shot","RSS","Cooperate after every 4 turns" ),
                new Record ("Short Reversed Single Shot","SRSS","Cooperate after every 2 turns" ),
                new Record ("Long Reversed Single Shot","LRSS","Cooperate after every 9 turns" ),
                new Record ("Soft Majority","SM","Cooperates if the opponent has cooperated for more than half of the moves, otherwise it will deflect" ),
                new Record ("Hard Majority","HM","Cooperates if the opponent has cooperated for more than 75% of the moves, otherwise it will deflect" ),
                new Record ("Grudger","G","This strategy cooperates until the opponent deflects, after which it deflectss for the remainder of the game" ),
                new Record ("Win-Stay, Loose-Shift(Pavlov)","WSLS","This strategy (also known as Pavlov) starts with cooperation, then keeps cooperating as long as it receives the same outcome (win or lose). If the outcome changes, it switches to the opposite action." ),
                new Record ("Joss","J","Similar to Tit for Tat but occasionally defects randomly, even when the opponent cooperates" ),
                new Record ("Suspicious Joss","J","The same as usual Joss but deflects at the first turn" ),
                new Record ("Friedman","F","Cooperates until the opponent defects; then, it cooperates with a 15% chance." ),
                new Record ("Nice Friedman","NF","Cooperates until the opponent defects; then, it cooperates with a 50% chance." ),
                new Record ("Very Nice Friedman","VNF","Cooperates until the opponent defects; then, it cooperates with a 80% chance." ),
                new Record ("Check","Ch","Check something" )
            };

            actionsToDo = new Dictionary<string, Func<bool[], int, bool>>
            {
                { "R", Random_T },
                { "TfT", Tit_for_Tat_T },
                { "C", AlwaysC_T },
                { "D", AlwaysD_T },
                { "Te", Tester_T },
                { "ATe", Another_Tester_T },
                { "U", User_T },
                { "FTfT", Forgiving_Tit_for_Tat_T },
                { "NTfT", Nice_Tit_for_Tat_T },
                { "STfT", Suspicious_Tit_for_Tat_T },
                { "TTfT", Tit_for_Two_Tats_T },
                { "STTfT", Suspicious_Tit_for_Two_Tats_T },
                { "Cy", Cycled_T },
                { "Ch", Proverka_T },
                { "CSS", Cycled_Single_Shot_T },
                { "SCSS", Short_Cycled_Single_Shot_T },
                { "LCSS", Long_Cycled_Single_Shot_T },
                { "RSS", Reversed_Single_Shot_T },
                { "SRSS", Short_Reversed_Single_Shot_T },
                { "LRSS", Long_Reversed_Single_Shot_T },
                { "SM", Soft_Majority_T },
                { "HM", Hard_Majority_T },
                { "G", Grudger_T },
                { "J", Joss_T },
                { "SJ", Suspicious_Joss_T },
                { "F", Friedman_T },
                { "NF", Nice_Friedman_T },
                { "VNF", Very_Nice_Friedman_T },
                { "WSLS", Win_Stay_Loose_Shift_T }
            };

            string ans="";
            random = new Random();

            Console.WriteLine("To make a single tournament between two strategies - t");
            Console.WriteLine("To make a multiple tournaments between two strategies - m");
            Console.WriteLine("To make a multiple tournaments between all strategies - g");
            Console.WriteLine("To make a multiple tournaments between one against all others - d");
            Console.WriteLine("To exit a program - x");
            Console.WriteLine("To show info about all strategies - i");
            Console.WriteLine("Show commands available - h");

            while (ans!="x")
            {

                Console.WriteLine(" ");
                Console.WriteLine("------");
                Console.WriteLine(" ");
                Console.WriteLine("Input command: ");
                ans = Console.ReadLine();
                Console.WriteLine(" ");
                misunderstanding = false;

                switch (ans)
                {
                    case "t":
                        initialize_tournament();

                        break;

                    case "m":
                        initialize_multiple_tournaments();

                        break;

                    case "g":
                        grand_multiple_tournaments();

                        break;

                    case "d":
                        one_against_all_multiple_tournaments();

                        break;

                    case "h":
                        Console.WriteLine("To make a single tournament between two strategies - t");
                        Console.WriteLine("To make a multiple tournaments between two strategies - m");
                        Console.WriteLine("To make a multiple tournaments between all strategies - g");
                        Console.WriteLine("To make a multiple tournaments between one against all others - d");
                        Console.WriteLine("To exit a program - x");
                        Console.WriteLine("To show info about all strategies - i");
                        Console.WriteLine("Show commands available - h");

                        break;

                    case "i":
                        output_tactics();

                        break;

                    case "x":
                        break;

                    default:
                        Console.WriteLine("Unknown command entered!");
                        break;
                }
            }
        }
    }
}
