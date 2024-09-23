using System.Diagnostics;
using System.Xml.Linq;

namespace Luna.Editor;

public class ProjectHandler : IEditor
{
    public static void CreateProject(string projectName, string slnPath, string[] usings, string[] packages, string[] services)
    {
        string projectDir = Path.Combine(slnPath, projectName);
        if (!Directory.Exists(projectDir))
        {
            Directory.CreateDirectory(projectDir);
        }

        RunDotnetCommand($"new sln", slnPath);
        RunDotnetCommand($"new console -o {projectName}", slnPath);
        RunDotnetCommand($"sln add {projectDir}\\{projectName}.csproj", slnPath);

        foreach (var package in packages)
        {
            RunDotnetCommand($"add package {package}", projectDir);
        }

        AddProjectInfo(projectDir, projectName);
        CreateProgramClass(usings, services, projectDir);
        SaveRootFile(projectDir, new Node());
    }

    public static void SaveRootFile(string projectPath, Node root)
        => NodeSerializer.SaveToFile(root, Path.Combine(projectPath, "rootfile"));

    public Node ReadRootFile()
        => NodeSerializer.LoadFromFile<Node>(Path.Combine(Directory.GetCurrentDirectory(), "rootfile"))?? new Node();

    public static void CreateClass(string nameSpace, string className, string baseClassName, string directory)
    {
        string filePath = Path.Combine(directory, $"{className}.cs");        
        string classContent = $@"
using Luna;

namespace {nameSpace}
{{
    public class {className} : {baseClassName}
    {{

        public override void Start()
        {{
            
        }}

        public override void Update()
        {{
            
        }}
    }}
}}
";
        File.WriteAllText(filePath, classContent);
    }

    public static void CreateProgramClass(string[] usings, string[] services, string projectDirectory)
    {
        string usingsContent = string.Empty;
        foreach(var u in usings)
        {
            usingsContent += $"using {u};\n";
        }

        string servicesContent = string.Empty;
        foreach(var s in usings)
        {
            servicesContent += $"{s}.AddServices();\n";
        }

        string programContent = $@"
using Luna;
{usingsContent}

{servicesContent}

var root = Injector.Get<IEditor>().ReadRootFile();
Host.Run(root);

";
        File.WriteAllText(Path.Combine(projectDirectory, "Program.cs"), programContent);
        Console.WriteLine("Created Program.cs.");
    }

    private static void RunDotnetCommand(string arguments, string workingDirectory = "")
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = workingDirectory
            }
        };

        process.Start();
        process.OutputDataReceived += (sender, data) => Console.WriteLine(data.Data);
        process.ErrorDataReceived += (sender, data) => Console.WriteLine(data.Data);
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();
    }
    
    private static void AddProjectInfo(string projectDir, string projectName)
    {
        string csprojPath = Path.Combine(projectDir, $"{projectName}.csproj");
        
        var projectXml = XDocument.Load(csprojPath);

        var itemGroup = new XElement("ItemGroup",
            new XElement("None",
                new XAttribute("Update", "Assets\\**\\*"),
                new XElement("CopyToOutputDirectory", "PreserveNewest")
            ),
            new XElement("None",
                new XAttribute("Update", "rootfile"),
                new XElement("CopyToOutputDirectory", "PreserveNewest")
            )
        );

        projectXml.Root?.Add(itemGroup);

        projectXml?.Save(csprojPath);
        Console.WriteLine($"Updated {csprojPath} with assets info.");
    }
}
