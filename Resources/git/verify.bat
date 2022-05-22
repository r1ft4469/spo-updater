@echo off
powershell write-host -fore Cyan "SPO Verification ..."
powershell write-host -fore Cyan "-----------------------------------------"
taskkill /F /IM Aki.Launcher.exe > nul 2>&1
taskkill /F /IM Server.exe > nul 2>&1
mkdir .\git\aki > nul 2>&1
powershell write-host -fore Yellow "Getting AKI File Checks ..."
.\git\mingw64\bin\curl.exe -LJO https://raw.githubusercontent.com/r1ft4469/spo-updater/update/aki.dat.001 > nul 2>&1
.\git\mingw64\bin\curl.exe -LJO https://raw.githubusercontent.com/r1ft4469/spo-updater/update/aki.dat.002 > nul 2>&1
move .\aki.dat.001 .\git\aki\aki.dat.001 > nul 2>&1
move .\aki.dat.002 .\git\aki\aki.dat.002 > nul 2>&1
powershell write-host -fore Yellow "Verifying AKI ..."
.\git\bin\7za.exe x -ogit\aki .\git\aki\aki.dat.001 > nul 2>&1
del /F /Q .\git\aki\aki.dat.001 > nul 2>&1
del /F /Q .\git\aki\aki.dat.002 > nul 2>&1
powershell write-host -fore Yellow "Checking Launcher for Newest Version ..."
.\git\mingw64\bin\curl.exe -LJO https://raw.githubusercontent.com/r1ft4469/spo-updater/update/update.dat > nul 2>&1
move /Y .\update.dat .\git\update.dat > nul 2>&1
copy /Y .\git\update.dat .\git\aki\update.exe > nul 2>&1
.\git\aki\update.exe -y -gm2 > nul 2>&1
del /F /Q .\git\aki\update.exe > nul 2>&1
powershell write-host -fore Yellow "Getting SPO File Checks ..."
git\cmd\git.exe clone --recursive https://github.com/kobrakon/SPO_DEV.git .\git\spo > nul 2>&1
xcopy /Y /E git\\spo\\. git\\aki\\. > nul 2>&1
xcopy /Y /E .\git\spo\user\mods\r1ft-DynamicTimeCycle\r1ft.DynamicTimeCyle.dll .\git\aki\BepInEx\plugins\r1ft.DynamicTimeCyle.dll* > nul 2>&1
xcopy /Y /E .\git\spo\user\mods\.SPO\mods\Headlamps\r1ft.Headlamps.dll .\git\aki\BepInEx\plugins\r1ft.Headlamps.dll* > nul 2>&1
rmdir /Q /S .\git\spo > nul 2>&1
powershell write-host -fore Yellow "Verifying SPO ..."
robocopy .\git\aki\ .\ /E /FFT /LOG:Logs\Verify.log > nul 2>&1
rmdir /Q /S .\git\aki > nul 2>&1
if NOT EXIST "user\profiles" (
    mkdir .\user\profiles > nul 2>&1
)
powershell write-host -fore Cyan "Finished."
powershell write-host -fore Cyan "Starting Launcher ..."
powershell write-host -fore Cyan "-----------------------------------------"
start "" "Aki.Launcher.exe"
.\Server.exe