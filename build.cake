#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#tool "nuget:?package=GitVersion.CommandLine"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// CONSTANTS, VARS
///////////////////////////////////////////////////////////////////////////////

string version = "1.0.0";

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir1 = Directory("./src/AlsekLib/bin") + Directory(configuration);
var buildDir2 = Directory("./src/AlsekLibServer/bin") + Directory(configuration);


//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir1);
	CleanDirectory(buildDir2);
}); 

Task("Version")
   .Does(() =>
{
    GitVersion(new GitVersionSettings {OutputType = GitVersionOutput.BuildServer});

	var symVer = GitVersion();
	Information($"SemVer: {symVer.SemVer}");
	version = symVer.SemVer;
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore("./src/CakeTest.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
	.IsDependentOn("Version")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild("./src/CakeTest.sln", settings =>
        settings.SetConfiguration(configuration));
    }
    else
    {
      // Use XBuild
      XBuild("./src/CakeTest.sln", settings =>
        settings.SetConfiguration(configuration));
    }
});

Task("Copy-And-Publish-Artifacts")
	.IsDependentOn("Build")
    .WithCriteria(BuildSystem.IsRunningOnAzurePipelinesHosted)
    .Does(() =>
{
        Information($"Creating directory ./drop");
        CreateDirectory($"./drop");
        Information($"Copying all files from {buildDir1} to ./drop");
        CopyDirectory($"{buildDir1}", $"./drop");
		Information($"Copying all files from {buildDir2} to ./drop");
        CopyDirectory($"{buildDir2}", $"./drop");
        Information($"Uploading files from artifact directory: ./drop to TFS");
        TFBuild.Commands.UploadArtifactDirectory($"./drop");
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Copy-And-Publish-Artifacts")
    .Does(() => 
{
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);