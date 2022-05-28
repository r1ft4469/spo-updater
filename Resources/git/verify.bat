@echo off
powershell write-host -fore Blue "SPO Verification ..."
powershell write-host -fore Blue "-----------------------------------------"
taskkill /F /IM Aki.Launcher.exe > nul 2>&1
taskkill /F /IM Server.exe > nul 2>&1
mkdir .\git\aki > nul 2>&1
powershell write-host -fore DarkYellow "Getting AKI File Checks ..."
.\git\mingw64\bin\curl.exe -LJO --connect-timeout 5 --max-time 10 --retry 5 --retry-delay 0 --retry-max-time 40 https://anonfiles.com/Ne75Ockey8/aki.dat_001 > nul 2>&1
.\git\mingw64\bin\curl.exe -LJO --connect-timeout 5 --max-time 10 --retry 5 --retry-delay 0 --retry-max-time 40 https://anonfiles.com/7579Ockcy4/aki.dat_002 > nul 2>&1
move .\aki.dat.001 .\git\aki\aki.dat.001 > nul 2>&1
move .\aki.dat.002 .\git\aki\aki.dat.002 > nul 2>&1
powershell write-host -fore DarkYellow "Verifying AKI ..."
.\git\bin\7za.exe x -ogit\aki .\git\aki\aki.dat.001 > nul 2>&1
del /F /Q .\git\aki\aki.dat.001 > nul 2>&1
del /F /Q .\git\aki\aki.dat.002 > nul 2>&1
powershell write-host -fore DarkYellow "Checking Launcher for Newest Version ..."
.\git\mingw64\bin\curl.exe -LJO --connect-timeout 5 --max-time 10 --retry 5 --retry-delay 0 --retry-max-time 40 https://github.com/r1ft4469/spo-updater/releases/download/Beta12/update.dat > nul 2>&1
move /Y .\update.dat .\git\update.dat > nul 2>&1
copy /Y .\git\update.dat .\git\aki\update.exe > nul 2>&1
.\git\aki\update.exe -y -gm2 > nul 2>&1
del /F /Q .\git\aki\update.exe > nul 2>&1
powershell write-host -fore DarkYellow "Getting SPO File Checks ..."
git\cmd\git.exe clone --recursive https://github.com/kobrakon/SPO_DEV.git .\git\spo > nul 2>&1
xcopy /Y /E git\\spo\\. git\\aki\\. > nul 2>&1
xcopy /Y /E .\git\spo\user\mods\r1ft-DynamicTimeCycle\r1ft.DynamicTimeCyle.dll .\git\aki\BepInEx\plugins\r1ft.DynamicTimeCyle.dll* > nul 2>&1
xcopy /Y /E .\git\spo\user\mods\.SPO\mods\Headlamps\r1ft.Headlamps.dll .\git\aki\BepInEx\plugins\r1ft.Headlamps.dll* > nul 2>&1
rmdir /Q /S .\git\spo > nul 2>&1
powershell write-host -fore DarkYellow "Verifying SPO ..."
robocopy .\git\aki\ .\ /E /FFT /LOG:Logs\Verify.log > nul 2>&1
rmdir /Q /S .\git\aki > nul 2>&1
if NOT EXIST "user\profiles" (
    mkdir .\user\profiles > nul 2>&1
)
powershell write-host -fore Blue "Finished."
powershell write-host -fore Blue "Starting Launcher ..."
powershell write-host -fore Blue "-----------------------------------------"
start "" "Aki.Launcher.exe"
.\Server.exe