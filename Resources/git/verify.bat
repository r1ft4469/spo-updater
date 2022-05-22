@echo off
taskkill /F /IM Aki.Launcher.exe
taskkill /F /IM Server.exe
mkdir .\git\aki
.\git\mingw64\bin\curl.exe -LJO https://raw.githubusercontent.com/r1ft4469/spo-updater/update/aki.dat.001
.\git\mingw64\bin\curl.exe -LJO https://raw.githubusercontent.com/r1ft4469/spo-updater/update/aki.dat.002
move .\aki.dat.001 .\git\aki\aki.dat.001
move .\aki.dat.002 .\git\aki\aki.dat.002
.\git\bin\7za.exe x -ogit\aki .\git\aki\aki.dat.001
del /F /Q .\git\aki\aki.dat.001
del /F /Q .\git\aki\aki.dat.002
.\git\mingw64\bin\curl.exe -LJO https://raw.githubusercontent.com/r1ft4469/spo-updater/update/update.dat
move /Y .\update.dat .\git\update.dat
copy /Y .\git\update.dat .\git\aki\update.exe
.\git\aki\update.exe -y -gm2
del /F /Q .\git\aki\update.exe
git\cmd\git.exe clone --recursive https://github.com/kobrakon/SPO_DEV.git .\git\spo
xcopy /Y /E git\\spo\\. git\\aki\\.
xcopy /Y /E .\git\spo\user\mods\r1ft-DynamicTimeCycle\r1ft.DynamicTimeCyle.dll .\git\aki\BepInEx\plugins\r1ft.DynamicTimeCyle.dll*
xcopy /Y /E .\git\spo\user\mods\.SPO\mods\Headlamps\r1ft.Headlamps.dll .\git\aki\BepInEx\plugins\r1ft.Headlamps.dll*
rmdir /Q /S .\git\spo
robocopy .\git\aki\ .\ /E
rmdir /Q /S .\git\aki
if NOT EXIST "user\profiles" (
    mkdir .\user\profiles
)
start "" "Aki.Launcher.exe"
start "" "Server.exe"