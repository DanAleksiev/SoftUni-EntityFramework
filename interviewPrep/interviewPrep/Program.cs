using System.Diagnostics;
using System.Text;

namespace interviewPrep
    {
    internal class Program
        {
        static void Main(string[] args)
            {
            ////capitalise the first letter in every word
            //string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
            //Console.WriteLine($"Capitalise: {lorem}");
            //Console.WriteLine(Capitalise(lorem));

            ////find the common numbers between the 2 arrays
            //int[] firstArrWithCommonInt = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, };
            //int[] secondArrWithCommonInt = { 1, 7, 8, 9, 10, 11, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, };
            //int[] commonElements = GetComonElements(firstArrWithCommonInt, secondArrWithCommonInt);
            //Console.WriteLine(string.Join(" ", commonElements));


            //TODO: sorting alorytms !!!
            int[] sortTheArray = { 10, 4, 6, 1, 2, 5, 7, 8, 3, 9, };
            int[] sortedArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            Console.WriteLine(SortList(sortedArray, sortTheArray));
            Console.WriteLine(SortedList(sortedArray, sortTheArray));
            Console.WriteLine(QuickSortAlgorithm(sortedArray, sortTheArray));

            }

        private static bool SortedList(int[] sortedArray, int[] sortTheArray)
            {
            int[] unsortedArray = sortTheArray;
            StringBuilder sb = new StringBuilder();

            Stopwatch speed = new Stopwatch();
            speed.Start();

            List<int> sortedList = sortTheArray.ToList();
            sortedList.Sort();

            speed.Stop();

            sb.AppendLine("The array is sorted with Sort List algorithm:");
            sb.AppendLine($"    It was :{string.Join("", unsortedArray)}");
            sb.AppendLine($"    Output :{string.Join("", sortTheArray)}");
            sb.AppendLine($"    Expected :{string.Join("", sortedArray)}");
            sb.AppendLine($"{speed.ElapsedTicks}");
            sb.AppendLine($"....................................................");
            return sb.ToString();
            }

        private static string SortList(int[] sortedArray, int[] sortTheArray)
            {
            int[] unsortedArray = sortTheArray;
            StringBuilder sb = new StringBuilder();

            Stopwatch speed = new Stopwatch();
            speed.Start();
            
            List<int> sortedList = sortTheArray.ToList();
            sortedList.Sort();

            speed.Stop();

            sb.AppendLine("The array is sorted with Sort List algorithm:");
            sb.AppendLine($"    It was :{string.Join("", unsortedArray)}");
            sb.AppendLine($"    Output :{string.Join("", sortTheArray)}");
            sb.AppendLine($"    Expected :{string.Join("", sortedArray)}");
            sb.AppendLine($"{speed.ElapsedTicks}");
            sb.AppendLine($"....................................................");
            return sb.ToString();
            }

        private static string QuickSortAlgorithm(int[] sortedArray, int[] sortTheArray)
            {
            StringBuilder sb = new StringBuilder();
            int[] unsortedArray = sortTheArray;

            Stopwatch speed = new Stopwatch();
            speed.Start();

            for (int i = 0; i < unsortedArray.Length; i++)
                {
                for (int j = unsortedArray.Length-1; j > i; j--)
                    {
                    if (unsortedArray[i] > unsortedArray[j])
                        {
                        int tempJ = unsortedArray[j];
                        int tempI = unsortedArray[i];
                        unsortedArray[i] = tempJ;
                        unsortedArray[j] = tempI;
                        
                        }
                    }
                }
            speed.Stop();
            sb.AppendLine("The array is sorted with Quicksort algorithm:");
            sb.AppendLine($"    It was :{string.Join("", unsortedArray)}");
            sb.AppendLine($"    Output :{string.Join("", sortTheArray)}");
            sb.AppendLine($"    Expected :{string.Join("", sortedArray)}");
            sb.AppendLine($"{speed.ElapsedTicks}");
            sb.AppendLine($"....................................................");
            return sb.ToString();
            }

        public static string Capitalise(string input)
            {
            string[] capitalise = input.Split(' ');

            StringBuilder sb = new StringBuilder();

            foreach (string s in capitalise)
                {
                char[] word = s.ToCharArray();

                word[0] = char.Parse(word[0].ToString().ToUpper());
                string output = new string(word);
                sb.Append(output + " ");
                }

            return sb.ToString().Trim();
            }

        private static int[] GetComonElements(int[] firstArr, int[] secondArr)
            {
            int[] thirdArr = firstArr.Intersect(secondArr).ToArray();
            return thirdArr;
            }
        }
    }