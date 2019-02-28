using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coding_bots
{
    class Startup
    {
        // Wszystkie pliki wejściowe odczytywane są katalogu readFilePath. Odczytywane są tylko pliki z rozszerzeniem .in 
        static readonly string readFilePath = "C:\\coding_bots";

        // Wszystkie pliki wyjściowe zapisywane są do katalogu readFilePath\writeFileDirectory. Zapisywane są z rozszerzeniem .out
        static readonly string writeFileDirectory = "outputs";

        // Jeśli jest generowany plik, który już istnieje to jest on przenoszony do katalogu readFilePath\writeFileDirectory\archiveFileDirectory
        static readonly string archiveFileDirectory = "archive";

        // Nazwa pliku wejściowego. Podawana po uruchomieniu programu.
        static string _fileName { get; set; }
        static List<string> _fileContent { get; set; }

        static void Main(string[] args)
        {
            // Wybieramy plik, który ma zostać wczytany.
            if (!ChooseInputFile())
            {
                Console.ReadKey();
                return;
            }
            
            // Odczytujemy dane z pliku do jakiś zmiennych globalnych czy czegoś tam. Jeśli true to wszystko jest OK.
            if (!ReadInputData())
            {
                Console.ReadKey();
                return;
            }

            // Klasa Challenge to tam będziemy implementować algorytm itp.
            Challenge task = new Challenge();

            // Przekazujemy odczytane dane do tej klasy Challenge. Jeśli true to wszystko jest OK.
            if(task.PrepareData(_fileContent))
            {
                try
                {
                    // Uruchamiamy algorytm. Wszystkie wyjątki zostaną tutaj obsłużone, więc tam nie trzeba się tym martwić.
                    Console.WriteLine("\nSolving challenge...");
                    var watch = Stopwatch.StartNew();
                    task.Solve();
                    watch.Stop();
                    Console.WriteLine($"Challenge solved in {GetSolvingTime(watch.ElapsedMilliseconds)}\n");
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Something went wrong: {e.Message}");
                    Console.ReadKey();
                    return;
                }
            }

            // Generujemy dane do zapisu.
            var dataToSave = task.GetSaveData();

            // Zapisujemy dane do pliku
            SaveData(dataToSave);
        }

        static bool ReadInputData()
        {
            string filePath = Path.Combine(readFilePath, $"{_fileName}.in");

            if(!File.Exists(filePath))
            {
                Console.WriteLine($"File \"{filePath}\" does not exists.");
                return false;
            }

            try
            {
                Console.WriteLine($"Reading file {filePath}");
                _fileContent = File.ReadAllLines(filePath).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while reading the input file: {e.Message}");
                return false;
            }

            return true;
        }

        static bool SaveData(List<string> lines)
        {
            string newfileName = Path.Combine(readFilePath, writeFileDirectory, $"{_fileName}.out");
            string archiveFileName = Path.Combine(readFilePath, writeFileDirectory, archiveFileDirectory, $"{_fileName}-{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.out");

            if (File.Exists(newfileName))
            {
                Console.WriteLine($"Moving an existing file \"{_fileName}.out\" to archive directory.");

                try
                {
                    File.Move(newfileName, archiveFileName);
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Error while moving existing file. Exception:\n{e.Message}");
                    return false;
                }
            }

            try
            {
                Console.WriteLine($"Saving results to file \"{newfileName}\"");
                File.WriteAllLines(newfileName, lines);
            }
            catch(Exception e)
            {
                Console.WriteLine($"The data could not be saved to the file. Exception:\n {e.Message}");
                return false;
            }

            return true;
        }

        static bool ChooseInputFile()
        {
            var directory = Path.Combine(readFilePath, writeFileDirectory, archiveFileDirectory);

            if (!Directory.Exists(directory)) 
            {
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Directory creation process failed: {e.Message}");
                    return false;
                }
            }

            var files = Directory.GetFiles(readFilePath, "*.in").Select(file => Path.GetFileNameWithoutExtension(file));

            if(files.Count() == 0)
            {
                Console.WriteLine("Input files not found.");
                return false;
            }

            var parsedFiles = string.Join(", ", files);
            string choosedFile = "";

            do
            {
                Console.Clear();
                Console.WriteLine($"Choose one of possible input files\n{parsedFiles}\n");
                Console.Write("Choose: ");
                choosedFile = Console.ReadLine();
            } while (files.Contains(choosedFile) == false);
            
            Console.WriteLine($"Choosed file: \"{choosedFile}.in\"\n");
            _fileName = choosedFile;

            return true;
        }

        static string GetSolvingTime(long duration)
        {
            long miliseconds = duration % 1000;
            long seconds = (duration / 1000) % 60;

            string result = $"{miliseconds}ms";

            if(seconds == 0)
            {
                return result;
            }

            result = $"{seconds}sec {result}";
            long minutes = (duration / (1000 * 60)) % 60;

            if (minutes == 0)
            {
                return result;
            }

            result = $"{minutes}min {result}";
            long hours = (duration / (1000 * 60 * 60)) % 60;

            if (hours == 0)
            {
                return result;
            }

            return $"{hours}h {result}";
        }
    }
}
