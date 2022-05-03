string target = Argument<string>("target", "ExecuteBuild");
string config = Argument<string>("config", "Release");
bool VSBuilt = Argument<bool>("vsbuilt", false);

// Cake API Reference: https://cakebuild.net/dsl/
// setup variables
var buildDir = "./Build";
var csprojPaths = GetFiles("./**/Aki.*(Launcher|Launcher.CLI).csproj");
var delPaths = GetDirectories("./**/*(obj|bin)");
var akiData = "./Aki.Launcher/Aki_Data";
var resourceData = "./Resources";
var licenseFile = "LICENSE.md";
var publishRuntime = "win10-x64";
var launcherDebugFolder = "./Aki.Launcher/bin/Debug/net6.0/win10-x64";

// Clean build directory and remove obj / bin folder from projects
Task("Clean")
    .WithCriteria(!VSBuilt) //building from VS will lock the files and fail to clean the project directories. Post-Build event on Aki.Build sets this switch to true to avoid this.
    .Does(() => 
    {
        CleanDirectory(buildDir);
    })
    .DoesForEach(delPaths, (directoryPath) => 
    {
        DeleteDirectory(directoryPath, new DeleteDirectorySettings 
        {
            Recursive = true,
            Force = true
        });
    });

// Restore, build, and publish selected csproj files
Task("Publish")
    .IsDependentOn("Clean")
    .DoesForEach(csprojPaths, (csprojFile) => 
    {
        DotNetPublish(csprojFile.FullPath, new DotNetPublishSettings 
        {
            NoLogo = true,
            Configuration = config,
            Runtime = publishRuntime,
            PublishSingleFile = true,
            SelfContained = false,
            OutputDirectory = buildDir
        });
    });

// Copy Aki_Data folder and license to build directory
Task("CopyBuildData")
    .IsDependentOn("Publish")
    .Does(() => 
    {
        CopyDirectory(akiData, $"{buildDir}/Aki_Data");
        CopyDirectory(resourceData, $"{buildDir}/");
        CopyFile(licenseFile, $"{buildDir}/LICENSE-Launcher.txt");
    });

// Copy Aki_Data to the launcher's debug directory so you can run the launcher with debugging from VS
Task("CopyDebugData")
    .WithCriteria(config == "Debug")
    .Does(() => 
    {
        EnsureDirectoryDoesNotExist($"{launcherDebugFolder}/Aki_Data");

        CopyDirectory(akiData, $"{launcherDebugFolder}/Aki_Data");
    });

// Remove pdb files from build if running in release configuration
Task("RemovePDBs")
    .WithCriteria(config == "Release")
    .IsDependentOn("CopyBuildData")
    .Does(() => 
    {
        DeleteFiles($"{buildDir}/*.pdb");
    });

// Runs all build tasks based on dependency and configuration
Task("ExecuteBuild")
    .IsDependentOn("CopyBuildData")
    .IsDependentOn("RemovePDBs")
    .IsDependentOn("CopyDebugData");

// Runs target task
RunTarget(target);