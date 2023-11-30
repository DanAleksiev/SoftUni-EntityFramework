using System.Text;

namespace interviewPrep
    {
    internal class Program
        {
        static void Main(string[] args)
            {
            string input = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

            //Console.WriteLine(Capitalise(input));

            int[] firstArr = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, };

            int[] secondArr = { 1, 7, 8, 9, 10, 11, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, };


            int[] commonElements = GetComonElements(firstArr, secondArr);
            Console.WriteLine(string.Join(" ", commonElements));
            //TODO: sorting alorytms !!!

        }

        private static int[] GetComonElements(int[] firstArr, int[] secondArr)
            {
            int[] thirdArr = firstArr.Intersect(secondArr).ToArray();
            return thirdArr;
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
        }
    }