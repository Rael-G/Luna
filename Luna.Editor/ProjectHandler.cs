using System.Diagnostics;
using System.Reflection;
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
            //TODO Nuget packages for Luna
            //RunDotnetCommand($"add package {package}", projectDir);

            var tempReferencePath = "C:\\Users\\israe\\OneDrive\\Documentos\\Projetos\\C#\\Luna";
            var tempPackage = Path.Combine(tempReferencePath, package);
            RunDotnetCommand($"add reference {tempPackage}", projectDir);
        }

        AddProjectInfo(projectDir, projectName);
        CreateProgramClass(usings, services, projectDir);
        CreateClass(projectName, "Root", "Node", projectDir);
        RunDotnetCommand("build", projectDir);
        var assembly = GetAssembly(projectDir, projectName);
        var root = InstatiateClassAsNode(assembly, projectName + ".Root");
        SaveRootFile(projectDir, root);
        var config = new LunaConfig();
        var configFilePath = Path.Combine(projectDir, "config.yaml");
        ConfigSerializer.WriteConfig(configFilePath, config);
    }

    public static Node OpenProject(string path)
    {
        var projectName = Path.GetFileName(path)!;
        var assembly = GetAssembly(path, projectName);
        var type = GetTypeFromAssembly(assembly, projectName + ".Root");

        return NodeSerializer.LoadFromFile(Path.Combine(path, "rootfile"), type, assembly) as Node??
            throw new LunaException($"Can't get root node from rootfile at: {path}");
    }

    public void ApplyConfig()
    {
        var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.yaml");
        var config = ConfigSerializer.ReadConfig(configFilePath);

        if (config is null)
        {
            return;
        }
        
        Window.Title = config.Title;
        Window.Size = config.Resolution;
        Window.Flags |= config.Vsync? WindowFlags.Vsync : WindowFlags.None;
    }

    public static void SaveRootFile(string projectPath, Node root)
        => NodeSerializer.SaveToFile(root, Path.Combine(projectPath, "rootfile"), Assembly.GetAssembly(root.GetType()));

    public T ReadRootFile<T>() where T : Node
    {
        return NodeSerializer.LoadFromFile(Path.Combine(Directory.GetCurrentDirectory(), "rootfile"), typeof(T), Assembly.GetAssembly(typeof(T))) as T 
            ?? throw new LunaException("Failed to read rootfile.");
    }

    private static void CreateClass(string nameSpace, string className, string baseClassName, string directory)
    {
        string filePath = Path.Combine(directory, $"{className}.cs");        
        string classContent = 
$@"using Luna;

namespace {nameSpace};

public class {className} : {baseClassName}
{{

}}
";
        File.WriteAllText(filePath, classContent);
    }

    private static void CreateProgramClass(string[] usings, string[] services, string projectDirectory)
    {
        string usingsContent = string.Empty;
        foreach(var u in usings)
        {
            usingsContent += $"using {u};\n";
        }

        string servicesContent = string.Empty;
        foreach(var s in services)
        {
            servicesContent += $"{s}.AddServices();\n";
        }

        string programContent = 
$@"using Luna;
{usingsContent}
{servicesContent}
var editor = Injector.Get<IEditor>();
editor.ApplyConfig();
Host.CreateWindow();
var root = editor.ReadRootFile<Root>();
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
            ),
            new XElement("None",
                new XAttribute("Update", "config.yaml"),
                new XElement("CopyToOutputDirectory", "PreserveNewest")
            )
        );

        projectXml.Root?.Add(itemGroup);

        projectXml?.Save(csprojPath);
        Console.WriteLine($"Updated {csprojPath} with assets info.");
    }

    private static Assembly GetAssembly(string projectPath, string projectName) 
    {
        var dll = Path.Combine(projectPath, "bin", "Debug", "net8.0",  projectName + ".dll");
        return Assembly.LoadFrom(dll);
    }

    private static Type GetTypeFromAssembly(Assembly assembly, string className)
        => assembly.GetType(className)??
            throw new LunaException($"Failed to get node type of {className} from assembly : {assembly.GetName()}.");

    private static Node InstatiateClassAsNode(Assembly assembly, string className)
    {
        var nodeType = GetTypeFromAssembly(assembly, className);

        return Activator.CreateInstance(nodeType) as Node??
            throw new LunaException($"Failed to create an instance of {nodeType} as a {nameof(Node)}.");
    }
}
