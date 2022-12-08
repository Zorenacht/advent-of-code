using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace AoC_2022;

public sealed partial class Day07 : Day
{
    [Test]
    public void Example()
    {
        var result = 0;
        var root = ParseInput(InputExample);
        SetFolderSizes(root);
        FindSizesGreaterThan100000(root, ref result);
        result.Should().Be(95437);
    }

    [Test]
    public void Part1()
    {
        var result = 0;
        var root = ParseInput(InputPart1);
        SetFolderSizes(root);
        FindSizesGreaterThan100000(root, ref result);
        result.Should().Be(1_778_099);
    }

    [Test]
    public void Part2()
    {
        var maxSize = 4_0000_000;
        var root = ParseInput(InputPart2);
        SetFolderSizes(root);
        int sizeToBeDeleted = root.Size - maxSize;
        var minSize = root.Size;
        FindSmallestToBeDeleted(root, sizeToBeDeleted, ref minSize);
        minSize.Should().Be(1_623_571);
    }

    File ParseInput(string[] input)
    {
        var root = new File() { Parent = null };
        File current = null;
        var cmd = "";
        var cmdArg = "";
        foreach (string line in input)
        {
            var command = new Regex("\\$ ([a-z]*).?(.*)?").Match(line).Groups;
            var arg = new Regex("(.*) (.*)").Match(line).Groups;
            var type = "";
            var name = "";
            if (command.Count > 1)
            {
                cmd = command[1].Value;
                cmdArg = command[2].Value;
                type = "";
                name = "";
            }
            else
            {
                type = arg[1].Value;
                name = arg[2].Value;
            }
            if (cmd == "cd")
            {
                if (cmdArg == "/")
                {
                    current = root;
                }
                else if (cmdArg == "..")
                {
                    current = current.Parent;
                }
                else
                {
                    current = current.Children.First(child => child.Name == cmdArg);
                }
            }
            else if (cmd == "ls" && name != "")
            {
                if (!current.Children.Any(child => child.Name == name))
                {
                    if (type == "dir")
                    {
                        current.Children.Add(new File() { Parent = current, Name = name, Type = "dir" });
                    }
                    else
                    {
                        current.Children.Add(new File() { Parent = current, Name = name, Size = Int32.Parse(type), Type = "file" });
                    }
                }
            }

        }
        return root;
    }

    int SetFolderSizes(File current)
    {
        if(current.Size > 0)
        {
            return current.Size;
        }
        foreach(var child in current.Children)
        {
            current.Size += SetFolderSizes(child);
        }
        return current.Size;
    }

    void FindSizesGreaterThan100000(File current, ref int sizeCount)
    {
        if(current.Type == "dir" && current.Size <= 100000)
        {
            sizeCount += current.Size;
        }
        foreach(var child in current.Children)
        {
            FindSizesGreaterThan100000(child, ref sizeCount);
        }
    }
    
    void FindSmallestToBeDeleted(File current, int sizeToBeDeleted, ref int currentMinSize)
    {
        if (current.Type == "dir" && current.Size >= sizeToBeDeleted && current.Size <= currentMinSize)
        {
            currentMinSize = current.Size;
        }
        foreach (var child in current.Children)
        {
            FindSmallestToBeDeleted(child, sizeToBeDeleted, ref currentMinSize);
        }
    }



    public class File
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public List<File> Children { get; set; } = new List<File>();
        public File? Parent { get; set; }

    }
}