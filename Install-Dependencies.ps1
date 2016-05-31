$AzureStorageEmulator = "${env:ProgramFiles(x86)}\Microsoft SDKs\Azure\Storage Emulator\AzureStorageEmulator.exe"

if (!(Test-Path $AzureStorageEmulator)) {
    $AzureStorageEmulatorInstallerPath = "$env:temp\AzureEmulatorInstaller.msi"
    if (!(Test-Path $AzureStorageEmulatorInstallerPath)) {
        Write-Host "Downloading the Azure Storage Emulator..." -ForegroundColor Green
        Invoke-WebRequest -Uri "https://go.microsoft.com/fwlink/?linkid=717179&clcid=0x409" -OutFile $AzureStorageEmulatorInstallerPath
        Unblock-File $AzureStorageEmulatorInstallerPath
    }

    Write-Host "Installing the Azure Storage Emulator..." -ForegroundColor Green
    Start-Process -FilePath msiexec -ArgumentList '/i',"$AzureStorageEmulatorInstallerPath","/passive","/norestart" -Wait
    Write-Host "Initializing the Azure Storage Emulator..." -ForegroundColor Green
    & $AzureStorageEmulator init -inprocess
}

Write-Host "Starting the Azure Storage Emulator..." -ForegroundColor Green
& $AzureStorageEmulator start
