using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Linq;

namespace CcesStatsCalculator
{
    class DialogueFileParser
    {
        public const uint GENERIC_READ = 0x80000000;
        public const uint OPEN_EXISTING = 3;

        // Use interop to call the CreateFile function.
        // For more information about CreateFile,
        // see the unmanaged MSDN reference library.
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern SafeFileHandle CreateFile(string lpFileName, uint dwDesiredAccess,
          uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition,
          uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        string ReadFileContent(string filePath)
        {
            var fileHandle = CreateFile(filePath, GENERIC_READ, 0, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
            using (var fs = new FileStream(fileHandle, FileAccess.Read))
            {
                using (var sr = new StreamReader(fs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        bool AreAllLinePropsSet(DialogueLine line)
        {
            return !string.IsNullOrWhiteSpace(line.Filename) &&
                !string.IsNullOrWhiteSpace(line.Character) &&
                !string.IsNullOrWhiteSpace(line.Expression) &&
                !string.IsNullOrWhiteSpace(line.Line);
        }

        bool IsDialogueLine(string line)
        {
            return line.Contains('>') &&
                line.Contains(':') &&
                !line.StartsWith("condition") &&
                !line.StartsWith("if") &&
                !line.StartsWith("else") &&
                !line.StartsWith("endif") &&
                !line.StartsWith("set") &&
                !line.StartsWith("#") &&
                !line.StartsWith("goto") &&
                !line.StartsWith("label");
        }

        DialogueLine ParseDialogueLine(string rawLine)
        {
            rawLine = rawLine.Trim(); // White spaces can mess with StartsWith condition

            if (!IsDialogueLine(rawLine))
                return new DialogueLine();

            // To lazy to do it properly
            rawLine = rawLine.Replace(@"\c[0]", " ");
            rawLine = rawLine.Replace(@"\c[1]", " ");
            rawLine = rawLine.Replace(@"\c[2]", " ");
            rawLine = rawLine.Replace(@"\c[3]", " ");
            rawLine = rawLine.Replace(@"\c[4]", " ");

            rawLine = rawLine.Replace(@"\.", " ");
            rawLine = rawLine.Replace(@"\!", " ");

            rawLine = rawLine.Split("#", count: 2).ElementAtOrDefault(0); // Exclude comments

            var split = rawLine.Split(new char[] { '>', ':' }, 3);
            return new DialogueLine()
            {
                Character = split.ElementAtOrDefault(0).Trim(),
                Expression = split.ElementAtOrDefault(1).Trim(),
                Line = split.ElementAtOrDefault(2).Trim()
            };
        }

        public IEnumerable<DialogueLine> ParseDialogueFile(string dialogueFilePath)
        {
            var parsedLines = new List<DialogueLine>();
            var rawLines = ReadFileContent(dialogueFilePath)
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            foreach (var rawLine in rawLines)
            {
                var line = ParseDialogueLine(rawLine);

                line.Filename = Path.GetFileNameWithoutExtension(dialogueFilePath);
                
                if (AreAllLinePropsSet(line))
                    parsedLines.Add(line);
            }

            return parsedLines;
        }
    }
}
