using System;
using System.IO;
using System.Collections.Generic;

namespace tactics_battle
{
    class Record 
    {
        public string name, shortName, info;
        public int totalPoints;

        public Record(string name, string shortName, string info) 
        {
            totalPoints = 0;
            this.name = name;
            this.shortName = shortName;
            this.info = info;
        }
    }




    class Program
    {
        static Record[] tactics;

        static Dictionary<string, Func<bool[],int,bool>> actionsToDo;

        static Random r;
        static int cycle,period1,period2,c,p1,p2;
        static int majorCycle;
        static bool redefine=false,forGrudger=true, ansWSLS;



        static void start_game(string t1, string t2, int rounds, out int p1, out int p2, out string winner, out bool[] lastAns1, out bool[] lastAns2) 
        {
            int tempP1, tempP2;

            forGrudger = true;
            ansWSLS = true;

            bool ans1, ans2;
            lastAns1 = new bool[rounds];
            lastAns2 = new bool[rounds];

            p1 = 0;
            p2 = 0;

            for (int i = 0; i < rounds; i++)
            {
                if (i == 0)
                    redefine = true;
                else
                    redefine = false;

                ans1 = actionsToDo[t1](lastAns2,i);
                ans2 = actionsToDo[t2](lastAns1, i);

                get_points(ans1, ans2, out tempP1, out tempP2);
                p1 = p1 + tempP1;
                p2 = p2 + tempP2;

                lastAns1[i] = ans1;
                lastAns2[i] = ans2;
            }

            if (p1>p2)
            {
                winner = t1;
            }
            else if (p1 == p2)
            {
                winner = t1+" and "+t2;
            }
            else
            {
                winner = t2;
            }
        }



        static void get_points(bool ans1, bool ans2, out int p1, out int p2) 
        {
            if (ans1 && ans2)
            {
                p1 = 3;
                p2 = 3;
            }
            else if (ans1)
            {
                p2 = 5;
                p1 = 0;
            }
            else if (ans2)
            {
                p2 = 0;
                p1 = 5;
            }
            else
            {
                p1 = 1;
                p2 = 1;
            }
        }



        static bool Random_T(bool[] lastAnsEnemy, int curTurn) 
        {
            int n = r.Next(1, 101);

            if (n<51)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        static bool Proverka(bool[] lastAnsEnemy, int curTurn)
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



        static bool AlwaysC_T(bool[] lastAnsEnemy, int curTurn)
        {
            return false;
        }



        static bool AlwaysC_C(bool[] lastAnsEnemy, int curTurn)
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
                    Console.WriteLine("Opponent betrayed");
                }
            }
            else
            {
                Console.WriteLine("Game started!");
            }

            Console.WriteLine("Round "+curTurn);

            string a = Console.ReadLine();
            while (a!= "c" && a!= "t")
            {
                Console.WriteLine("Enter c - cooperate or t - betray:");
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



        static bool Cycled_Single_Shot(bool[] lastAnsEnemy, int curTurn)
        {
            if (period1 == -1)
            {
                Console.WriteLine("Input length of the period for the CSS: ");
                period1 = int.Parse(Console.ReadLine());
            }

            if (redefine)
                p1 = r.Next(0, period1);

            if (curTurn % period1 == p1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        static bool Reversed_Single_Shot(bool[] lastAnsEnemy, int curTurn)
        {
            if (period2 == -1)
            {
                Console.WriteLine("Input length of the period for RSS: ");
                period2 = int.Parse(Console.ReadLine());
            }

            if(redefine)
                p2 = r.Next(0, period2);

            if (curTurn % period2 == p2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        static bool S_Majority(bool[] lastAnsEnemy, int curTurn) 
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


        static bool H_Majority(bool[] lastAnsEnemy, int curTurn)
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
            if (cycle == -1)
            {
                Console.WriteLine("Input length of the cycle for Cy: ");
                cycle = int.Parse(Console.ReadLine());
            }

            if (redefine)
                c = r.Next(0, cycle * 2);

            int p = (curTurn+(cycle*2)-c) / cycle;

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
                int chance = r.Next(1, 101);
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



        static bool Grudger(bool[] lastAnsEnemy, int curTurn) 
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


        static bool Win_Stay_Loose_Shift(bool[] lastAnsEnemy, int curTurn) 
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



        static void outputResult(string tactica, int rounds, bool[] res, out string ans) 
        {
            Console.Write(tactica + ": \t");
            ans = "";
            for (int i = 0; i < rounds; i++)
            {
                if (res[i])
                {
                    Console.Write("C");
                    ans = ans + "C";
                }
                else
                {
                    Console.Write("T");
                    ans = ans + "T";
                }
            }
            Console.Write("\n");
        }


        static void initializeTournament() 
        {
            int rounds = 0, points1, points2;

            string[] writeInFile = new string[3];
            writeInFile[0] = "singleTournament,"+majorCycle;

            Console.WriteLine("How many rounds do you want to make?");
            while (rounds == 0)
            {
                try
                {
                    rounds = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Error!");
                }
            }

            bool[] res1 = new bool[rounds];
            bool[] res2 = new bool[rounds];

            Console.WriteLine("Input short name of the first tactics: ");
            string tactica1 = Console.ReadLine();
            Console.WriteLine("Input short name of the second tactics: ");
            string tactica2 = Console.ReadLine();
            Console.WriteLine(" ");

            string winner;
            start_game(tactica1, tactica2, rounds, out points1, out points2, out winner, out res1, out res2);

            string ans1, ans2;
            outputResult(tactica1, rounds, res1, out ans1);
            outputResult(tactica2, rounds, res2, out ans2);
            Console.WriteLine(" ");

            writeInFile[1] = tactica1+","+points1+","+ans1;
            writeInFile[2] = tactica2 + "," + points2 + "," + ans2;
            File.AppendAllLines("dataResults.csv", writeInFile);

            Console.WriteLine("Tactics " + tactica1 + " got: " + points1);
            Console.WriteLine("Tactics " + tactica2 + " got: " + points2);
            Console.WriteLine("Winner " + winner);
            Console.WriteLine(" ");

            rounds = 0;
        }


        static void initializeMultipleTournaments() 
        {
            int rounds = 0, tournaments=0, avg1=0, avg2=0, won1=0, won2=0;

            File.AppendAllText("dataResults.csv","multipleTournament," + majorCycle+"\n");

            Console.WriteLine("How many tournaments do you want: ");
            tournaments = int.Parse(Console.ReadLine());

            Console.WriteLine("How many rounds do you want to make?");
            while (rounds == 0)
            {
                try
                {
                    rounds = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Error!");
                }
            }

            Console.WriteLine("Input short name of the first tactics: ");
            string tactica1 = Console.ReadLine();
            Console.WriteLine("Input short name of the second tactics: ");
            string tactica2 = Console.ReadLine();
            Console.WriteLine(" ");

            for (int i = 1; i < tournaments + 1; i++)
            {
                bool[] res1 = new bool[rounds];
                bool[] res2 = new bool[rounds];
                string winner;
                int points1, points2;

                string[] writeInFile = new string[3];
                writeInFile[0] = ""+i;

                start_game(tactica1, tactica2, rounds, out points1, out points2, out winner, out res1, out res2);

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
                Console.WriteLine("-------");
                Console.WriteLine(" ");
                Console.WriteLine("Tournament " + i);

                string ans1, ans2;
                outputResult(tactica1, rounds, res1, out ans1);
                outputResult(tactica2, rounds, res2, out ans2);
                Console.WriteLine(" ");

                writeInFile[1] = tactica1 + "," + points1 + "," + ans1;
                writeInFile[2] = tactica2 + "," + points2 + "," + ans2;
                File.AppendAllLines("dataResults.csv", writeInFile);

                Console.WriteLine("Tactics " + tactica1 + " got: " + points1);
                Console.WriteLine("Tactics " + tactica2 + " got: " + points2);
                Console.WriteLine("Winner " + winner);
            }

            avg1 = avg1 / tournaments;
            avg2 = avg2 / tournaments;

            Console.WriteLine(" ");
            Console.WriteLine("Tactics " + tactica1 + " average points is: " + avg1);
            Console.WriteLine("Tactics " + tactica2 + " average points is: " + avg2);
            Console.WriteLine("Tactics " + tactica1 + " won " + won1+" times!");
            Console.WriteLine("Tactics " + tactica2 + " won " + won2+" times!");
        }


        static void outputTactics() 
        {
            Console.WriteLine("List of all tactics: ");
            Console.WriteLine(" ");

            for (int i = 0; i < tactics.Length; i++)
            {
                Console.WriteLine(i + ") Name: " + tactics[i].name + "; short name: " + tactics[i].shortName);
                Console.WriteLine("Explanation: " + tactics[i].info);
            }
        }


        //Добавить функцию с помощью которой можно просматривать данные с файла
        //Добавить больше тактик. Найти где я спрашивал у них чатаГПТ и спросить ещё чтобы было штук 40(January, Strat models in Axelrod)
        //Добавить функцию турнира где все тактики против друг друга соревнуються и выводиться отсортированый список тактик с общим кол-вом очков
        //Если раундов больше чем 1000 то спросить записывать ли данные или нет, также не выводить все результат на экран


        static void Main(string[] args)
        {
            tactics= new Record[18]
            {
                new Record("Random","R","Randomly cooperate or betray" ),
                new Record ("Tit for Tat(Mirror)","TfT","It usually cooperates, but if it was betrayed in the last turn, it will betray in the next"),
                new Record ("AlwaysC","C","Always cooperate"),
                new Record ("AlwaysT","T","Always betray"),
                new Record ("Tester","Te","In the first turn, it cooperates, in the second he betrays, and if the opponent also betrays on the third turn, it cooperates for the rest of the game. Otherwise, it will betray all subsequent turns" ),
                new Record ("User","U","If you want to play against other tactics"),
                new Record ("Cycled","Cy","Cyclically given number of moves betrays then the same number of moves cooperates" ),
                new Record ("Cycled Single Shot","CSS","Betrays after every specified number of moves" ),
                new Record ("Forgiving Tit for Tat","FTfT","It usually cooperates, but if it was betrayed in the last turn, it will betray in the next move with 85% chance" ),
                new Record ("Nice Tit for Tat(Tit for Two Tats)","NTfT","Similar to Tit for Tat, but it begins by cooperating and continues to cooperate as long as the opponent does not betray twice in a row. If the opponent betrays twice in a row, Nice Tit for Tat betray permanently" ),
                new Record ("Suspicious Tit for Tat","STfT","A variation of Tit for Tat, where it starts by betraying and then mirrors the opponent's previous move. This strategy is designed to initially avoid exploitation by betraying opponents" ),
                new Record ("Two Tits for Tat","TTfT","Cooperates unless the opponent betray, in which case it betrays twice in a row" ),
                new Record ("Reversed Single Shot","RSS","Cooperate after every specified number of moves" ),
                new Record ("Soft Majority","SM","Cooperates if the opponent has cooperated for more than half of the moves, otherwise he will betray" ),
                new Record ("Hard Majority","HM","Cooperates if the opponent has cooperated for more than 75% of the moves, otherwise he will betray" ),
                new Record ("Grudger","G","This strategy cooperates until the opponent betray, after which it betrays for the remainder of the game" ),
                new Record ("Win-Stay, Loose-Shift(Pavlov)","WSLS","This strategy (also known as Pavlov) starts with cooperation, then keeps cooperating as long as it receives the same outcome (win or lose). If the outcome changes, it switches to the opposite action." ),
                new Record ("Check","Ch","Check something" )
            };

            actionsToDo = new Dictionary<string, Func<bool[], int, bool>>
            {
                { "R", Random_T },
                { "TfT", Tit_for_Tat_T },
                { "C", AlwaysC_C },
                { "T", AlwaysC_T },
                { "Te", Tester_T },
                { "U", User_T },
                { "FTfT", Forgiving_Tit_for_Tat_T },
                { "NTfT", Nice_Tit_for_Tat_T },
                { "STfT", Suspicious_Tit_for_Tat_T },
                { "TTfT", Tit_for_Two_Tats_T },
                { "Cy", Cycled_T },
                { "Ch", Proverka },
                { "CSS", Cycled_Single_Shot },
                { "RSS", Reversed_Single_Shot },
                { "SM", S_Majority },
                { "HM", H_Majority },
                { "G", Grudger },
                { "WSLS", Win_Stay_Loose_Shift }
            };

            string ans="";
            r = new Random();
            majorCycle = 0;

            int curSeans = 0;
            if (File.Exists("dataResults.csv"))
            {
                string[] lines = File.ReadAllLines("dataResults.csv");

                int pos = lines.Length - 1;
                while (curSeans==0) 
                {
                    string[] line = lines[pos].Split(',');

                    if (line.Length == 2) 
                    {
                        if (line[0] == "session") 
                        {
                            curSeans = int.Parse(line[1]);
                            curSeans++;
                        }
                    }

                    pos--;
                }
            }
            else
            {
                File.Create("dataResults.csv").Close();
                curSeans = 1;
            }

            File.AppendAllText("dataResults.csv", "session,"+curSeans+"\n");

            Console.WriteLine("To make a single tournament between two tactics - t");
            Console.WriteLine("To make a multiple tournaments between two tactics - m");
            Console.WriteLine("To exit a program - x");
            Console.WriteLine("To show info about all tactics - i");
            Console.WriteLine("Show commands available - h");

            while (ans!="x")
            {
                cycle = -1;
                period1 = -1;
                period2 = -1;

                Console.WriteLine(" ");
                Console.WriteLine("------");
                Console.WriteLine(" ");
                Console.WriteLine("Input command: ");
                ans = Console.ReadLine();
                Console.WriteLine(" ");

                switch (ans)
                {
                    case "t":
                        majorCycle++;
                        initializeTournament();

                        break;

                    case "m":
                        majorCycle++;
                        initializeMultipleTournaments();

                        break;

                    case "h":
                        Console.WriteLine("To make a single tournament between two tactics - t");
                        Console.WriteLine("To exit a program - x");
                        Console.WriteLine("To show info about tactics - i");
                        Console.WriteLine("Show commands available - h");

                        break;

                    case "i":
                        outputTactics();

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
