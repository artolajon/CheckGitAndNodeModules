
using System.Diagnostics;

string baseDir = AppDomain.CurrentDomain.BaseDirectory;
List<string> withGit = new List<string>();
List<string> withNodeModules = new List<string>();

foreach (string subDir in Directory.GetDirectories(baseDir))
{
    string dirName = Path.GetFileNameWithoutExtension(subDir);

    bool hasGit = Directory.Exists(Path.Combine(subDir, ".git"));
    bool hasNodeModules = Directory.Exists(Path.Combine(subDir, "node_modules"));

    if (hasGit)
    {
        // Check for pending Git changes
        bool hasChanges = HasPendingGitChanges(subDir);

        if (hasChanges)
        {
            withGit.Add(dirName);
        }
    }

    if (hasNodeModules)
    {
        withNodeModules.Add(dirName);
    }
}

Console.WriteLine("Directories with changes:");
foreach(string dir in withGit)
{
    Console.WriteLine($"-> {dir}");
}

Console.WriteLine("Directories with node_modules:");
foreach (string dir in withNodeModules)
{
    Console.WriteLine($"-> {dir}");
}




static bool HasPendingGitChanges(string directory)
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = "status --porcelain",
            WorkingDirectory = directory,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process process = Process.Start(psi))
        {
            string output = process.StandardOutput.ReadToEnd();
            return !string.IsNullOrEmpty(output);
        }
    }
