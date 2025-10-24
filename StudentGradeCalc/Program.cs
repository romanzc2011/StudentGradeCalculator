using System;
using System.Collections;
using System.Linq;
using System.IO;
using System.Text;
using GradeList = System.Collections.Generic.List<float>;


int totalCreditHrs = 0;
int totalEarnedCredits = 0;
float gradePointAvg = 0;

var studentGrades = new Dictionary<string, GradeList>();
var gradeScale = new Dictionary<string, IEnumerable<int>>
{
    { "A+", Enumerable.Range(97, 4) },  // 97–100
    { "A",  Enumerable.Range(93, 4) },  // 93–96
    { "A-", Enumerable.Range(90, 3) },  // 90–92
    { "B+", Enumerable.Range(87, 3) },  // 87–89
    { "B",  Enumerable.Range(83, 4) },  // 83–86
    { "B-", Enumerable.Range(80, 3) },  // 80–82
    { "C+", Enumerable.Range(77, 3) },  // 77–79
    { "C",  Enumerable.Range(73, 4) },  // 73–76
    { "C-", Enumerable.Range(70, 3) },  // 70–72
    { "D+", Enumerable.Range(67, 3) },  // 67–69
    { "D",  Enumerable.Range(63, 4) },  // 63–66
    { "D-", Enumerable.Range(60, 3) },  // 60–62
    { "F",  Enumerable.Range(0, 60) }   // 0–59
};


try
{
    if (!File.Exists("../../../../students.txt"))
    {
        throw new FileNotFoundException("The students.txt file was not found.");
    }

    char[] delimiters = { ' ', ',' };

    // *Read student names from file */
    const Int32 BufferSize = 512;
    using(var fs = File.OpenRead("../../../../students.txt"))
    {
        using(var sr = new StreamReader(fs, Encoding.UTF8, true, BufferSize))
        {
            String line;
            while((line = sr.ReadLine()) != null)
            {
                // Skipping blank lines
                if(string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var parts = line.Split(':');
                string studentName = parts[0].Trim();
                string[] gradesStr = parts[1].Trim().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                var grades = new List<float>();

                // Convert string numbers in float if possible
                foreach (string grade in gradesStr)
                {
                    
                    if (float.TryParse(grade, out float value))
                    {
                        grades.Add(value);
                    }
                }
                // Now add all the grades to their respective student
                studentGrades[studentName] = grades;
            }
        }
        
        foreach (var (name, g) in studentGrades)
        {
            Console.WriteLine($"{name}: {string.Join(", ", g)}");
        }
    }
} catch (FileNotFoundException ex)
{
    Console.WriteLine(ex.Message);
    return;
}

// ******************************************************************************
// ** MAIN PROGRAM LOGIC
// ******************************************************************************
bool keepRunning = true;

while(keepRunning)
{
    Console.WriteLine("Continue running... ?");
    string decision = Console.ReadLine().Trim().ToLower();

    if (decision == "exit") break;
}

