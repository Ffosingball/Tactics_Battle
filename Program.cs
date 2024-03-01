using System;

namespace tactics_battle
{
    class Record 
    {
        public string name, shortName, info;
        public int totalPoints;
    }




    class Program
    {
        static public Record[] tactics = new Record[19]
        {
            new Record { name = "Random", shortName = "R", totalPoints = 0, info = "Рандомно співпрацює або зраджує" },
            new Record { name = "Tit for Tat", shortName = "TfT", totalPoints = 0, info = "Зазвича співпрацює, але якщо його зрадили у минулому ході то він зрадить у наступному" },
            new Record { name = "AlwaysC", shortName = "C", totalPoints = 0, info = "Завжди співпрацює" },
            new Record { name = "AlwaysT", shortName = "T", totalPoints = 0, info = "Завжди зраджує" },
            new Record { name = "Tester", shortName = "Te", totalPoints = 0, info = "У першому ході співпрацює, у другому зраджує і якщо у наступному ході супротивник також зраджує, то далі всю гру співпрацює. Якщо супротивник на третьому ході співпрацює, то він усі наступні ходи буде зраджувати" },
            new Record { name = "User", shortName = "U", totalPoints = 0, info = "Якщо ви хочете пограти проти інших тактик" },
            new Record { name = "Cycled", shortName = "Cy", totalPoints = 0, info = "Циклічно п'ять ходів зраджує потім п'ять ходів співпрацює" },
            new Record { name = "Cycled Long", shortName = "CyL", totalPoints = 0, info = "Циклічно двадцять ходів зраджує потім двадцять ходів співпрацює" },
            new Record { name = "Cycled Super Long", shortName = "CySL", totalPoints = 0, info = "Циклічно п'ятьдесят ходів зраджує потім п'ятьдесят ходів співпрацює" },
            new Record { name = "Cycled Single Shot", shortName = "CSS", totalPoints = 0, info = "Зраджує через кожні п'ять ходів" },
            new Record { name = "Cycled Single Shot Long", shortName = "CSSL", totalPoints = 0, info = "Зраджує через кожні двадцять ходів" },
            new Record { name = "Cycled Single Shot Super Long", shortName = "CSSSL", totalPoints = 0, info = "Зраджує через кожні п'ятьдесят ходів" },
            new Record { name = "Forgiving Tit for Tat", shortName = "FTfT", totalPoints = 0, info = "Зазвича співпрацює, але якщо його зрадили у минулому ході то він зрадить у наступному з шансом 90%" },
            new Record { name = "Reversed Single Shot", shortName = "RSS", totalPoints = 0, info = "Співпрацює лише кожен п'ятий хід" },
            new Record { name = "Reversed Single Shot Long", shortName = "RSSL", totalPoints = 0, info = "Співпрацює лише кожен двадцятий хід" },
            new Record { name = "Reversed Single Shot Super Long", shortName = "RSSSL", totalPoints = 0, info = "Співпрацює лише кожен п'ятьдесятий хід" },
            new Record { name = "Soft Majority", shortName = "SM", totalPoints = 0, info = "Співпрацює якщо супротивник більше половини ходів співпрацював, інакше буде зраджувати" },
            new Record { name = "Hard Majority", shortName = "HM", totalPoints = 0, info = "Співпрацює якщо супротивник більше 75% ходів співпрацював, інакше буде зраджувати" },
            new Record { name = "Proverka", shortName = "P", totalPoints = 0, info = "Перевірка" }
        };



        static void start_game(string t1, string t2, int rounds, out int p1, out int p2, out string winner, out Boolean[] lastAns1, out Boolean[] lastAns2) 
        {
            int tempP1, tempP2;

            Boolean ans1, ans2;
            lastAns1 = new Boolean[rounds];
            lastAns2 = new Boolean[rounds];

            p1 = 0;
            p2 = 0;

            for (int i = 0; i < rounds; i++)
            {
                ans1 = check_tactics(t1,lastAns1,lastAns2,i);
                ans2 = check_tactics(t2, lastAns2, lastAns1, i);

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
                winner = t1+" та "+t2;
            }
            else
            {
                winner = t2;
            }
        }



        static Boolean check_tactics(string t, Boolean[] lastAnsMy, Boolean[] lastAnsEnemy, int curTurn) 
        {
            Boolean ans=false;

            switch (t)
            {
                case "TfT":
                    ans = Tit_for_Tat_T(lastAnsEnemy, curTurn);
                    break;
                case "C":
                    ans = AlwaysC_C();
                    break;
                case "T":
                    ans = AlwaysC_T();
                    break;
                case "Te":
                    ans = Tester_T(lastAnsEnemy, curTurn);
                    break;
                case "U":
                    ans = User_T(lastAnsEnemy, curTurn);
                    break;
                case "FTfT":
                    ans = Forgiving_Tit_for_Tat_T(lastAnsEnemy, curTurn);
                    break;
                case "Cy":
                    ans = Cycled_T(curTurn,5);
                    break;
                case "CyL":
                    ans = Cycled_T(curTurn,20);
                    break;
                case "CySL":
                    ans = Cycled_T(curTurn, 50);
                    break;
                case "CSS":
                    ans = Cycled_Single_Shot(curTurn, 5);
                    break;
                case "CSSL":
                    ans = Cycled_Single_Shot(curTurn, 20);
                    break;
                case "CSSSL":
                    ans = Cycled_Single_Shot(curTurn, 50);
                    break;
                case "RSS":
                    ans = Reversed_Single_Shot(curTurn, 5);
                    break;
                case "RSSL":
                    ans = Reversed_Single_Shot(curTurn, 20);
                    break;
                case "RSSSL":
                    ans = Reversed_Single_Shot(curTurn, 50);
                    break;
                case "SM":
                    ans = Majority(lastAnsEnemy, 0.5f, curTurn);
                    break;
                case "HM":
                    ans = Majority(lastAnsEnemy, 0.75f, curTurn);
                    break;
                case "P":
                    ans = Proverka();
                    break;
                default:
                    ans=Random_T();
                    break;
            }

            return ans;
        }



        static void get_points(Boolean ans1, Boolean ans2, out int p1, out int p2) 
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



        static Boolean Random_T() 
        {
            Random r = new Random();
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



        static Boolean Proverka()
        {
            Random r = new Random();
            int n = r.Next(1, 101);

            if (n < 10)
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        static Boolean AlwaysC_T()
        {
            return false;
        }



        static Boolean AlwaysC_C()
        {
            return true;
        }



        static Boolean User_T(Boolean[] lastAnsEnemy, int curTurn)
        {
            if (curTurn!=0)
            {
                if (lastAnsEnemy[curTurn-1])
                {
                    Console.WriteLine("Рішення супротивника: C");
                }
                else
                {
                    Console.WriteLine("Рішення супротивника: T");
                }
            }
            else
            {
                Console.WriteLine("Гра розпочалася!");
            }

            Console.WriteLine("Раунд "+curTurn);

            Console.WriteLine("Співпрацювати(C) чи зрадити(T):");
            string a = Console.ReadLine();
            while (a!= "C" && a!= "T")
            {
                Console.WriteLine("Введіть на англійській розкладці або C(співпрацювати) або T(зрадити):");
                a = Console.ReadLine();
            }

            if (a=="C")
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        static Boolean Tit_for_Tat_T(Boolean[] lastAnsEnemy, int curTurn) 
        {
            if (curTurn==0)
            {
                return true;
            }
            else if (lastAnsEnemy[curTurn-1]==false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        static Boolean Cycled_Single_Shot(int curTurn, int cycle)
        {
            if (curTurn % cycle == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        static Boolean Reversed_Single_Shot(int curTurn, int cycle)
        {
            if (curTurn % cycle == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        static Boolean Majority(Boolean[] lastAnsEnemy, float percentage, int curTurn) 
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
            else if (coop/curTurn >= percentage)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        static Boolean Cycled_T(int curTurn, int period) 
        {
            int p = curTurn / period;

            if (p % 2 == 1)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }



        static Boolean Forgiving_Tit_for_Tat_T(Boolean[] lastAnsEnemy, int curTurn)
        {
            if (curTurn == 0)
            {
                return true;
            }
            else if (lastAnsEnemy[curTurn - 1] == false)
            {
                Random r = new Random();

                int chance = r.Next(1, 11);
                if (chance == 1)
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



        static Boolean Tester_T(Boolean[] lastAnsEnemy, int curTurn)
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



        static void outputResult(string tactica, int rounds, Boolean[] res) 
        {
            Console.Write(tactica + ": \t");
            for (int i = 0; i < rounds; i++)
            {
                if (res[i])
                {
                    Console.Write("C");
                }
                else
                {
                    Console.Write("T");
                }
            }
            Console.Write("\n");
        }



        static void Main(string[] args)
        {
            int rounds=0, points1, points2;
            string tactica1, tactica2, winner, ans;
            Boolean con = true, wrong=false;

            Console.WriteLine("Щоб провести турнір тактик введіть С");
            Console.WriteLine("Щоб покинути програму введіть E");
            Console.WriteLine("Щоб показати інформацію про тактики введіть I");
            Console.WriteLine("Щоб показати лише скорочені тактик введіть S");
            Console.WriteLine("Щоб ще раз побачити команди введіть H");
            Console.WriteLine("Введіть команду: ");
            ans = Console.ReadLine();
            Console.WriteLine(" ");

            while (con)
            {
                switch (ans)
                {
                    case "C":
                        Console.WriteLine("Скільки раундів ви хочете провести?");
                        while (rounds == 0)
                        {
                            try
                            {
                                rounds = int.Parse(Console.ReadLine());
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Помилка!");
                            }
                        }

                        Boolean[] res1 = new Boolean[rounds];
                        Boolean[] res2 = new Boolean[rounds];

                        Console.WriteLine("Перша тактика: ");
                        tactica1 = Console.ReadLine();
                        Console.WriteLine("Друга тактика: ");
                        tactica2 = Console.ReadLine();
                        Console.WriteLine(" ");

                        start_game(tactica1, tactica2, rounds, out points1, out points2, out winner, out res1, out res2);

                        outputResult(tactica1, rounds, res1);
                        outputResult(tactica2, rounds, res2);
                        Console.WriteLine(" ");

                        Console.WriteLine("Тактика " + tactica1 + " набрала:\t " + points1);
                        Console.WriteLine("Тактика " + tactica2 + " набрала:\t " + points2);
                        Console.WriteLine("Переможець:\t " + winner);
                        Console.WriteLine(" ");

                        rounds = 0;
                        break;

                    case "E":
                        con = false;
                        break;

                    case "H":
                        Console.WriteLine("Щоб провести турнір тактик введіть С");
                        Console.WriteLine("Щоб покинути програму введіть E");
                        Console.WriteLine("Щоб показати інформацію про тактики введіть I");
                        Console.WriteLine("Щоб показати лише скорочені тактик введіть S");
                        Console.WriteLine("Щоб ще раз побачити команди введіть H");
                        break;

                    case "I":
                        Console.WriteLine("Список всіх доступних тактик: ");
                        Console.WriteLine(" ");

                        for (int i = 0; i < tactics.Length; i++)
                        {
                            Console.WriteLine("Назва: "+tactics[i].name);
                            Console.WriteLine("Скорочена назва: " + tactics[i].shortName);
                            Console.WriteLine("Пояснення: " + tactics[i].info);
                            Console.WriteLine(" ");
                        }
                        break;

                    case "S":
                        Console.WriteLine("Скорочені назви всіх тактик: ");
                        Console.WriteLine(" ");

                        for (int i = 0; i < tactics.Length; i++)
                        {
                            Console.WriteLine(tactics[i].shortName);
                        }
                        Console.WriteLine(" ");
                        break;

                    default:
                        wrong = true;
                        break;
                }

                if (wrong)
                {
                    Console.WriteLine("Введена невідома команда. Введіть ще раз: ");
                    ans = Console.ReadLine();
                    wrong = false;
                    Console.WriteLine(" ");
                }
                else if (con)
                {
                    Console.WriteLine("Введіть подальші команди: ");
                    ans = Console.ReadLine();
                    Console.WriteLine(" ");
                }
            }
        }
    }
}
