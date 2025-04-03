using System;
using System.Collections.Generic;
using System.Text;
using System.Media;

namespace HangmanAppTest
{
    internal class Program
    {
        private static void PrintHangman(int wrong)
        {
            string[] stages = {
                "\n+---+\n    |\n    |\n    |\n   ===",
                "\n+---+\nO   |\n    |\n    |\n   ===",
                "\n+---+\nO   |\n|   |\n    |\n   ===",
                "\n+---+\n O  |\n/|  |\n    |\n   ===",
                "\n+---+\n O  |\n/|\\ |\n    |\n   ===",
                "\n+---+\n O  |\n/|\\ |\n/   |\n   ===",
                "\n+---+\n O  |\n/|\\ |\n/ \\ |\n   ==="
            };
            Console.WriteLine(stages[Math.Min(wrong, stages.Length - 1)]);
        }

        private static int PrintWord(List<char> guessedLetters, string randomWord)
        {
            int correctLetters = 0;
            Console.Write("\r\n");

            foreach (char c in randomWord)
            {
                if (guessedLetters.Contains(c))
                {
                    Console.Write(c + " ");
                    correctLetters++;
                }
                else
                {
                    Console.Write("_ ");
                }
            }

            return correctLetters;
        }

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Willkommen bei Hangman!");
            Console.WriteLine("-----------------------------------------");

            // Kategorien und Wörter
            Dictionary<string, List<string>> categories = new Dictionary<string, List<string>>
            {
                { "Wissenschaft", new List<string>
                    { 
                        "atommüll", "bunsenbrenner", "elektronen", "gravitationskraft",
                        "magnetismus", "photosynthese", "quantenphysik", "reaktionsgleichung",
                        "sonnenfinsternis", "treibhauseffekt"
                    }
                },
                { "Tiere", new List<string>
                    { 
                        "honigbiene", "kängurubeutel", "wüstenfuchs", "renaissance", "tiger", "giraffe", "delfin", "schmetterling", "leopard", "känguru", "panda", "zebra", "pinguin", "elefant"
                    }
                },
                { "Sport", new List<string>
                    { 
                        "basketballfeld", "eiskunstlauf", "fallschirmspringen", "fußballtorwart" ,  "tennis", "basketball", "schwimmen", "fußball", "handball", "rugby", "leichtathletik", "skifahren", "snowboarden", "volleyball"
                    }
                }
            };

            // Kategorieauswahl
            Console.WriteLine("Wähle eine Kategorie:");
            int index = 1;
            foreach (var category in categories.Keys)
            {
                Console.WriteLine($"{index++}. {category}");
            }

            int choice;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= categories.Count)
                {
                    break;
                }
                Console.WriteLine("Ungültige Eingabe. Wähle eine gültige Kategorie.");
            }

            string selectedCategory = new List<string>(categories.Keys)[choice - 1];
            List<string> selectedWords = categories[selectedCategory];
            Random random = new Random();
            string randomWord = selectedWords[random.Next(selectedWords.Count)];

            int wrongGuesses = 0;
            List<char> guessedLetters = new List<char>();
            int correctLetters = 0;

            while (wrongGuesses < 6 && correctLetters < randomWord.Length)
            {
                Console.Write("\nBisher geratene Buchstaben: ");
                foreach (char letter in guessedLetters)
                {
                    Console.Write(letter + " ");
                }

                Console.Write("\nRate einen Buchstaben: ");
                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Bitte gib einen Buchstaben ein!");
                    continue;
                }

                char letterGuessed = char.ToLower(input[0]);

                if (guessedLetters.Contains(letterGuessed))
                {
                    Console.WriteLine("\r\nDiesen Buchstaben hast du schon geraten.");
                    PrintHangman(wrongGuesses);
                    correctLetters = PrintWord(guessedLetters, randomWord);
                    continue;
                }

                bool correct = randomWord.Contains(letterGuessed);

                guessedLetters.Add(letterGuessed);

                if (correct)
                {
                    Console.Beep(1000, 200);  // Richtig geraten → hoher Ton
                    correctLetters = PrintWord(guessedLetters, randomWord);
                }
                else
                {
                    Console.Beep(500, 200);  // Falsch geraten → tiefer Ton
                    wrongGuesses++;
                    PrintHangman(wrongGuesses);
                    correctLetters = PrintWord(guessedLetters, randomWord);
                }
            }

            Console.WriteLine(wrongGuesses < 6
                ? "\nGlückwunsch! Du hast das Wort erraten! 🎉"
                : $"\nGame Over! Das Wort war: {randomWord}");

            Console.WriteLine("\nDanke fürs Spielen!");
        }
    }
}
