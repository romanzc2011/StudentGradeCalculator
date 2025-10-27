using System;
using System.Collections;
using System.Linq;
using System.IO;
using System.Text;
using GradeList = System.Collections.Generic.List<float>;

int totalCreditHrs = 0;
int totalEarnedCredits = 0;
float gradePointAvg = 0;
float gradesSum = 0;
string letterGrade = "";
string sName = "";

var studentGrades = new Dictionary<string, (GradeList Grades, float GradesSum, float GradeAvg, string LetterGrade)>();
var gradeScale = new Dictionary<string, IEnumerable<float>>
{
    { "A+", Enumerable.Range(97, 4).Select(i => (float)i) },  // 97–100
    { "A",  Enumerable.Range(93, 4).Select(i => (float)i) },  // 93–96
    { "A-", Enumerable.Range(90, 3).Select(i => (float)i) },  // 90–92
    { "B+", Enumerable.Range(87, 3).Select(i => (float)i) },  // 87–89
    { "B",  Enumerable.Range(83, 4).Select(i => (float)i) },  // 83–86
    { "B-", Enumerable.Range(80, 3).Select(i => (float)i) },  // 80–82
    { "C+", Enumerable.Range(77, 3).Select(i => (float)i) },  // 77–79
    { "C",  Enumerable.Range(73, 4).Select(i => (float)i) },  // 73–76
    { "C-", Enumerable.Range(70, 3).Select(i => (float)i) },  // 70–72
    { "D+", Enumerable.Range(67, 3).Select(i => (float)i) },  // 67–69
    { "D",  Enumerable.Range(63, 4).Select(i => (float)i) },  // 63–66
    { "D-", Enumerable.Range(60, 3).Select(i => (float)i) },  // 60–62
    { "F",  Enumerable.Range(0, 60).Select(i => (float)i) }   // 0–59
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

                // Separate parts of student records
                var parts = line.Split(':');
                string studentName = parts[0].Trim();
                string[] gradesStr = parts[1].Trim().Split(delimiters, StringSplitOptions.RemoveEmptyEntries); // These are the individual grades to be converted to float

                var grades = new List<float>();

                // Convert string numbers in float if possible
                foreach (string grade in gradesStr)
                {
                    
                    if (float.TryParse(grade, out float value))
                    {
                        grades.Add(value);
                    }
                }

                float gSum = grades.Sum();
                float avg = gSum / grades.Count();

                // Get proper letter grade corresponding with numerical value
                var match = gradeScale.FirstOrDefault(g =>
                {
                    float min = g.Value.Min();
                    float max = g.Value.Max();
                    return avg >= min && avg <= max;
                });

                letterGrade = match.Key != null ? match.Key : "Unknown";
                Console.WriteLine($"Name: {studentName} : {letterGrade}");
            }
        }
        
        
    }
} catch (FileNotFoundException ex)
{
    Console.WriteLine(ex.Message);
    return;
}
