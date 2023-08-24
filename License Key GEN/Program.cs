using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace LicenseKey_Gen
{
    class Program
    {
        /// <summary>
        /// Main entry point of the program.
        /// </summary>
        static void Main(string[] args)
        {
            // Initialize the random number generator.
            Random rand = new Random();

            // Decide the count of random numbers to generate.
            int numberOfTimes = rand.Next(10, 21);
            StringBuilder stage1Output = new StringBuilder("|");
            List<int> numbers = new List<int>();

            // Generate a set of random numbers and build the Stage 1 output.
            for (int i = 0; i < numberOfTimes; i++)
            {
                long number = GenerateRandomNumber(rand);
                numbers.Add((int)number);
                stage1Output.Append($" {number} |");
            }

            // Display the generated random numbers and their subsequent conversion.
            Console.WriteLine($"Stage 1: {stage1Output.ToString()}");
            string stage2Output = ConvertNumbersToLetters(numbers, rand);
            Console.WriteLine($"Stage 2: {stage2Output}");

            // Generate the final key and display it.
            string key = GenerateKey(stage2Output, rand);
            Console.WriteLine(" ");
            Console.WriteLine("[!] Key Generated [!]");
            Console.WriteLine(" ");
            Console.WriteLine($"Key | {key}");
        }

        /// <summary>
        /// Generates a random number after applying a series of operations.
        /// </summary>
        private static long GenerateRandomNumber(Random rand)
        {
            // Start with a random number in the given range.
            long number = rand.Next(1000, 1650001);
            int innerLoopCount = rand.Next(10, 10001);

            // Apply a series of random operations to the number.
            for (int j = 0; j < innerLoopCount; j++)
            {
                int operation = rand.Next(1, 7);
                switch (operation)
                {
                    case 1: number += rand.Next(1, 5); break;  // Random addition.
                    case 2: number -= rand.Next(1, 5); break;  // Random subtraction.
                    case 3: number *= rand.Next(1, 5); break;  // Random multiplication.
                    case 4: number %= (rand.Next(1, 9) + 1); break;  // Random modulo operation.
                    case 5: number /= rand.Next(1, 3); break;  // Random division.
                    case 6: number = (number * 3) + 1; break;  // Specific operation to transform the number.
                }
            }

            // Subtract 7 and ensure the number is non-negative.
            number -= 7;
            number = Math.Abs(number);

            // If the result is zero, replace with a random number between 1 and 9.
            if (number == 0)
                number = rand.Next(1, 10);

            return number;
        }

        /// <summary>
        /// Converts a list of numbers into a combination of numbers and letters.
        /// </summary>
        private static string ConvertNumbersToLetters(List<int> numbers, Random rand)
        {
            // Keeps track of the frequency of each letter.
            Dictionary<char, int> letterCounts = new Dictionary<char, int>();
            StringBuilder result = new StringBuilder();
            int letterCount = 0;

            // Iterate through each number.
            foreach (var number in numbers)
            {
                // Convert numbers between 1-26 to letters, ensuring no letter repeats more than once.
                if (number <= 26 && letterCount < 6)
                {
                    char letter = (char)(64 + number);
                    if (!letterCounts.ContainsKey(letter))
                        letterCounts[letter] = 0;

                    if (letterCounts[letter] < 1)
                    {
                        result.Append(letter);
                        letterCounts[letter]++;
                        letterCount++;
                    }
                    else
                    {
                        result.Append(number);
                    }
                }
                else
                {
                    result.Append(number);
                }
            }

            // If the result is less than 18 characters, pad with random numbers until it reaches the desired length.
            while (result.Length < 18)
            {
                result.Append(rand.Next(0, 10).ToString());
            }

            // Ensure the result is exactly 18 characters.
            return result.ToString().Substring(0, 18);
        }

        /// <summary>
        /// Generates a formatted key from the input string.
        /// </summary>
        private static string GenerateKey(string input, Random rand)
        {
            // Split the input into numbers and letters.
            List<char> chars = input.ToList();
            List<char> numbers = chars.Where(char.IsDigit).ToList();
            List<char> letters = chars.Where(char.IsLetter).ToList();

            // Define the sizes for each block in the final key.
            int[] blockSizes = { 4, 5, 4, 5 };
            int countNumbers = 0;
            StringBuilder result = new StringBuilder();

            // Iterate over each block size and construct the key.
            for (int index = 0; index < blockSizes.Length; index++)
            {
                var blockSize = blockSizes[index];
                StringBuilder block = new StringBuilder();
                while (block.Length < blockSize)
                {
                    char c;
                    // Prioritize numbers until the count threshold is reached.
                    if (numbers.Any() && countNumbers < 3)
                    {
                        c = numbers[0];
                        numbers.RemoveAt(0);
                        countNumbers++;
                    }
                    else
                    {
                        countNumbers = 0;
                        // If letters are available, use them.
                        if (letters.Any())
                        {
                            c = letters[0];
                            letters.RemoveAt(0);
                        }
                        else
                        {
                            // If no letters are left, use a random uppercase letter.
                            c = (char)rand.Next(65, 91);
                        }
                    }
                    block.Append(c);
                }
                result.Append(block.ToString());

                // Add the '-' separator for all blocks except the last one.
                if (index < blockSizes.Length - 1)
                {
                    result.Append('-');
                }
            }
            return result.ToString();
        }
    }
}
