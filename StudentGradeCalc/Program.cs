using System;
using System.Collections;
using System.Linq;
using System.IO;
using System.Text;
using GradeList = System.Collections.Generic.List<float>;

const int ExamCount = 4;

var studentRecords = new Dictionary<string, (
    GradeList Grades,
    float GradesSum,
    float GradeAvg,     // overall (with weighted extra)
    float ExamAvg,      // average of the first 4 exams
    float ExtraAvg,     // average of extra-credit items (or 0)
    float DeltaPts,     // added points from extra-credit (distributed)
    string LetterGrade)>();

var gradeScale = new Dictionary<string, IEnumerable<float>>
{
    { "A+", Enumerable.Range(97, 5).Select(i => (float)i) },  // 97–100
    { "A",  Enumerable.Range(93, 4).Select(i => (float)i) },  // 93–96
    { "A-", Enumerable.Range(90, 4).Select(i => (float)i) },  // 90–92
    { "B+", Enumerable.Range(87, 4).Select(i => (float)i) },  // 87–89
    { "B",  Enumerable.Range(83, 4).Select(i => (float)i) },  // 83–86
    { "B-", Enumerable.Range(80, 4).Select(i => (float)i) },  // 80–82
    { "C+", Enumerable.Range(77, 4).Select(i => (float)i) },  // 77–79
    { "C",  Enumerable.Range(73, 4).Select(i => (float)i) },  // 73–76
    { "C-", Enumerable.Range(70, 4).Select(i => (float)i) },  // 70–72
    { "D+", Enumerable.Range(67, 4).Select(i => (float)i) },  // 67–69
    { "D",  Enumerable.Range(63, 4).Select(i => (float)i) },  // 63–66
    { "D-", Enumerable.Range(59, 4).Select(i => (float)i) },  // 60–62
    { "F", Enumerable.Range(0, 59).Select(i => (float)i) }    // 0–59
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
            string? line;
            while((line = sr.ReadLine()) != null)
            {
                // Skipping blank lines
                if(string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                // Separate parts of student records
                var parts = line.Split(':');
                if (parts.Length < 2) continue;

                string studentName = parts[0].Trim();
                string[] gradesStr = parts[1].Trim()
                    .Split(delimiters, StringSplitOptions.RemoveEmptyEntries); // These are the individual grades to be converted to float

                var grades = new GradeList();
                foreach (string s in gradesStr)
                {
                    if (float.TryParse(s, out float value)) grades.Add(value);
                }
                if (grades.Count == 0) continue;

                // --- Compute after we have all grades ---
                float examSum   = grades.Take(ExamCount).Sum();
                float examAvg   = examSum / ExamCount;

                var extras      = grades.Skip(ExamCount).ToList();
                float extraSum  = extras.Sum();
                float extraAvg = extras.Count > 0 ? extras.Average() : 0f;

                float weightedExtra = 0.10f * extraSum;             // 10% of extra-credit total
                float finalSum      = examSum + weightedExtra;      // added to exam total
                float finalAvg = finalSum / ExamCount;              // divide by number of exams
                float deltaPts = (extras.Count > 0) ? (weightedExtra / ExamCount) : 0f;  // points added to avg

                // Map finalAvg to a letter
                var match = gradeScale.FirstOrDefault(g =>
                {
                    float min = g.Value.Min();
                    float max = g.Value.Max();
                    return finalAvg >= min && finalAvg <= max;
                });
                string letter = match.Key ?? "Unknown";

                // Store full record
                studentRecords[studentName] = (
                    Grades: grades,
                    GradesSum: grades.Sum(),
                    GradeAvg: finalAvg,
                    ExamAvg: examAvg,
                    ExtraAvg: extraAvg,
                    DeltaPts: deltaPts,
                    LetterGrade: letter
                );
            }
        }
    }

} catch (FileNotFoundException ex)
{
    Console.WriteLine(ex.Message);
    return;
}

// ---- Print report once, after parsing all students ----
Console.WriteLine("Student\t\tExam Score\t\tOverall Grade\tLetter\tExtra Credit");
foreach (var kvp in studentRecords)
{
    string name = kvp.Key;
    var r = kvp.Value;

    Console.WriteLine(
        $"{name,-15}{r.ExamAvg,12:0.##}{r.GradeAvg,16:0.##}   {r.LetterGrade,-3}   {r.ExtraAvg:0.#} ({r.DeltaPts:0.##} pts)");
}
