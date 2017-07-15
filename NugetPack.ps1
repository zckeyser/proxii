Function UpdateAssemblyVersions($rootFolder) {
  # matches things that look like AssemblyInfo("1.2.3.4")
  $regex = 'AssemblyVersion\("(\d+\.){2}\d+"\)'

  # retrieve assembly version from nuspec
  [xml] $spec = Get-Content $rootFolder\Proxii\Proxii.nuspec
  $version = $spec.package.metadata.version

  Write-Host "Setting assembly version to $version"

  $assemblyVersion = "AssemblyVersion(`"$version`")"

  $assemblyInfoFile = "$rootFolder\Proxii\Properties\AssemblyInfo.cs"

  $assemblyInfo = Get-Content $assemblyInfoFile

  $assemblyInfo -replace $regex, $assemblyVersion | Out-File $assemblyInfoFile
}

try {
    pushd $PSScriptRoot

    $rootFolder = $PSScriptRoot

    Write-Host "Updating assembly versions..."
    UpdateAssemblyVersions $rootFolder

    # nuget pack ./Proxii/proxii.csproj
} finally {
    popd
}