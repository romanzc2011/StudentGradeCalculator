using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

int totalCreditHrs = 0;
int totalEarnedCredits = 0;
float gradePointAvg = 0;
var studentName = "";

try
{
    if (!File.Exists("../../../../students.txt"))
    {
        throw new FileNotFoundException("The students.txt file was not found.");
    }

    // *Read student names from file */
    const Int32 BufferSize = 512;
    using(var fs = File.OpenRead("../../../../students.txt"))
    {
        using(var sr = new StreamReader(fs, Encoding.UTF8, true, BufferSize))
        {
            String line;
            while((line = sr.ReadLine()) != null)
            {
                studentName = line.Trim();
                Console.WriteLine($"STUDENT NAME: {studentName}");
            }
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
var studentRecord = new Dictionary<string, (string CourseName, int Credits)>
{
    { "ENG100", ("English 101",         3) },
    { "MATH100", ("Algebra 101",        3) },
    { "BIO100", ("Biology 101",         4) },
    { "CS100", ("Computer Science I",   4) },
    { "PSYC100", ("Psychology 101",     3) }
};

var gradeWeight = new Dictionary<char, int>
{
    { 'A', 4 },
    { 'B', 3 },
    { 'C', 2 },
    { 'D', 1 },
    { 'F', 0 }
};

var studentGrades = new Dictionary<string, char>();
totalCreditHrs = studentRecord.Sum(c => c.Value.Credits);

// Get grades from user, looping thru each course and prompting user for letter grade
foreach (var (code, (name, credits)) in studentRecord)
{
    while (true)
    {
        Console.WriteLine($"Enter letter grade for {name} ({code}): ");
        var earnedGrade = Console.ReadLine()?.Trim().ToUpper();

        // Ensure prompt is not null, reprompt for same course if true
        if (!string.IsNullOrEmpty(earnedGrade) && earnedGrade.Length == 1 &&
            "ABCDF".Contains(earnedGrade))
        {

            studentGrades[code] = earnedGrade[0];
            break;
        }
        Console.WriteLine("Only enter a single letter grade: A, B, C, D, or F.");
    }

    // Grab the course and sum all earned credits from courses
    var course = studentRecord[code];
    totalEarnedCredits += (gradeWeight[studentGrades[code]] * studentRecord[code].Credits);
}

// Calculate the student's GPA
gradePointAvg = (float)totalEarnedCredits / (float)totalCreditHrs;

// Final Print out

Console.WriteLine("\n\n\n============== GRADE REPORT ================");
Console.WriteLine($"Student: {studentName}\n");
Console.WriteLine("Course\t\t\tGrade\tCredit Hours");
Console.WriteLine(new string('-', 44));
foreach (var (code, (cName, credits)) in studentRecord)
{
    Console.WriteLine(
        $"{studentRecord[code].CourseName, -25} " +
        $"{gradeWeight[studentGrades[code]], -8}" +
        $"{studentRecord[code].Credits}"
    );
}
Console.WriteLine($"\nFinal GPA: {gradePointAvg,19:F2}");