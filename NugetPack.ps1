Function UpdateAssemblyVersions($rootFolder) {
  # matches things that look like AssemblyInfo("1.2.3.4")
  $regex = 'AssemblyVersion\("(\d+\.){3}\d+"\)'

  # retrieve assembly version from nuspec
  $assemblyVersion = "AssemblyVersion(`"$major.$minor.$revision.$build`")"

  $assemblyInfoFile = "$rootFolder\SharedAssemblyInfo.cs"

  (Get-Content $assemblyInfoFile) -replace $regex, $assemblyVersion | Out-File $assemblyInfoFile
}

$rootFolder = $PSScriptRoot

UpdateAssemblyVersions $rootFolder

