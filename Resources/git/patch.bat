@echo off
powershell write-host -fore Yellow -back DarkBlue "SPO Setup ..."
powershell write-host -fore Yellow -back DarkBlue "-----------------------------------------"
taskkill /F /IM Aki.Launcher.exe > nul 2>&1
taskkill /F /IM Server.exe > nul 2>&1
IF EXIST "git\git.exe" (
    attrib +h git > nul 2>&1
    powershell write-host -fore Green "Extracting Setup Files ..."
    git\git.exe -y -gm2 -InstallPath="git" > nul 2>&1
    del git\git.exe > nul 2>&1
    del git\git.7z.001 > nul 2>&1
)
FOR /F "USEBACKQ" %%F IN (`powershell -NoLogo -NoProfile -Command ^(Get-Item "EscapeFromTarkov.exe"^).VersionInfo.FileVersion`) DO (SET fileVersion=%%F)
powershell write-host -fore Green "Downloading Downgrade Patch %fileVersion% ..."
IF "%fileVersion%"=="0.12.12.17975" (
    call :DOWNLOAD
    xcopy /Y /E git\\patch\\12.12.17975\\. > nul 2>&1
    call :DOWNGRADE
    goto :EOF
)
IF "%fileVersion%"=="0.12.12.17861" (
    call :DOWNLOAD
    xcopy /Y /E git\\patch\\12.12.17681\\. > nul 2>&1
    call :DOWNGRADE
    goto :EOF
)
IF "%fileVersion%"=="0.12.12.17686" (
    call :DOWNLOAD
    echo Installing Downgrade Patch 17686 ...
    xcopy /Y /E git\\patch\\12.12.17686\\. > nul 2>&1
    call :DOWNGRADE
    goto :EOF
)
IF "%fileVersion%"=="0.12.12.17639" (
    call :DOWNLOAD
    xcopy /Y /E git\\patch\\12.12.17639\\. > nul 2>&1
    call :DOWNGRADE
    goto :EOF
)
IF "%fileVersion%"=="0.12.12.17566" (
    call :DOWNLOAD
    xcopy /Y /E git\\patch\\12.12.17566\\. > nul 2>&1
    call :DOWNGRADE
    goto :EOF
)
IF "%fileVersion%"=="0.12.12.17349" (
    call :INSTALL
    goto :EOF
) else (
    powershell write-host -fore Red "Update Tarkov or Wait for Patcher Release"
    echo Current Tarkov Version : %fileVersion%
    pause
    goto :EOF
)

:DOWNGRADE
powershell write-host -fore Green "Downgrading Tarkov to Correct Version ..."
start "" "patcher.exe"
ping 127.0.0.1 -n 16 > nul
:WAIT
ping 127.0.0.1 -n 3 > nul
IF EXIST "PATCHER_TEMP" (
  goto :WAIT
)
taskkill /F /IM patcher.exe > nul 2>&1
rmdir /Q /S .\git\patch > nul 2>&1
del patcher.exe > nul 2>&1
goto :INSTALL

:INSTALL
mkdir .\git\aki > nul 2>&1
powershell write-host -fore Green "Downloading AKI ..."
.\git\mingw64\bin\curl.exe -LJO https://raw.githubusercontent.com/r1ft4469/spo-updater/update/aki.dat.001 > nul 2>&1
.\git\mingw64\bin\curl.exe -LJO https://raw.githubusercontent.com/r1ft4469/spo-updater/update/aki.dat.002 > nul 2>&1
move .\aki.dat.001 .\git\aki\aki.dat.001 > nul 2>&1
move .\aki.dat.002 .\git\aki\aki.dat.002 > nul 2>&1
powershell write-host -fore Green "Setting Up AKI ..."
.\git\bin\7za.exe x -ogit\aki .\git\aki\aki.dat.001 > nul 2>&1
del /F /Q .\git\aki\aki.dat.001 > nul 2>&1
del /F /Q .\git\aki\aki.dat.002 > nul 2>&1
powershell write-host -fore Green "Checking Launcher for Newest Version ..."
.\git\mingw64\bin\curl.exe -LJO https://raw.githubusercontent.com/r1ft4469/spo-updater/update/update.dat > nul 2>&1
move /Y .\update.dat .\git\update.dat > nul 2>&1
copy /Y .\git\update.dat .\git\aki\update.exe > nul 2>&1
.\git\aki\update.exe -y -gm2 > nul 2>&1
del /F /Q .\git\aki\update.exe > nul 2>&1
powershell write-host -fore Green "Downloading SPO ..."
git\cmd\git.exe clone --recursive https://github.com/kobrakon/SPO_DEV.git .\git\spo > nul 2>&1
xcopy /Y /E git\\spo\\. git\\aki\\. > nul 2>&1
xcopy /Y /E .\git\spo\user\mods\r1ft-DynamicTimeCycle\r1ft.DynamicTimeCyle.dll .\git\aki\BepInEx\plugins\r1ft.DynamicTimeCyle.dll* > nul 2>&1
xcopy /Y /E .\git\spo\user\mods\.SPO\mods\Headlamps\r1ft.Headlamps.dll .\git\aki\BepInEx\plugins\r1ft.Headlamps.dll* > nul 2>&1
rmdir /Q /S .\git\spo > nul 2>&1
powershell write-host -fore Green "Installing SPO ..."
if NOT EXIST ".\Logs" (
   mkdir .\Logs > nul 2>&1
)
robocopy .\git\aki\ .\ /E /FFT /LOG:Logs\Install.log > nul 2>&1
rmdir /Q /S .\git\aki > nul 2>&1
if NOT EXIST "user\profiles" (
    mkdir .\user\profiles > nul 2>&1
)
powershell write-host -fore Yellow -back DarkBlue "Finished."
powershell write-host -fore Yellow -back DarkBlue "Starting Launcher ..."
powershell write-host -fore Yellow -back DarkBlue "-----------------------------------------"
start "" "Aki.Launcher.exe"
.\Server.exe
goto :EOF

:DOWNLOAD
mkdir .\git\patch > nul 2>&1
.\git\mingw64\bin\curl.exe -LJ -o .\git\patch\patch.dat.001 https://raw.githubusercontent.com/r1ft4469/spo-updater/update/patch.dat.001 > nul 2>&1
.\git\mingw64\bin\curl.exe -LJ -o .\git\patch\patch.dat.002 https://raw.githubusercontent.com/r1ft4469/spo-updater/update/patch.dat.002 > nul 2>&1
.\git\bin\7za.exe x -ogit\patch .\git\patch\patch.dat.001 > nul 2>&1
del .\git\patch\patch.dat.001 > nul 2>&1
del .\git\patch\patch.dat.002 > nul 2>&1
goto :EOF
