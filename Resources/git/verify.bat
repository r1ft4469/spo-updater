@echo off
taskkill /F /IM Aki.Launcher.exe
taskkill /F /IM Server.exe
mkdir .\git\aki
.\git\mingw64\bin\curl.exe -O https://dev.sp-tarkov.com/attachments/0b99e60d-8b5b-4d09-a64e-d03b6083bff6
move .\0b99e60d-8b5b-4d09-a64e-d03b6083bff6 .\git\aki\aki.zip
.\git\bin\7za.exe x -ogit\aki .\git\aki\aki.zip
del /F /Q .\git\aki\aki.zip
if NOT EXIST "git\update.dat" (
    .\git\mingw64\bin\curl.exe -LJO https://raw.githubusercontent.com/r1ft4469/spo-updater/update/update.dat
    move /Y .\update.dat .\git\update.dat
)
copy /Y .\git\update.dat .\git\aki\update.exe
.\git\aki\update.exe -y -gm2
del /F /Q .\git\aki\update.exe
git\cmd\git.exe clone --recursive https://github.com/kobrakon/SPO_DEV.git .\git\spo
xcopy /Y /E git\\spo\\. git\\aki\\.
del .\git\spo\BepInEx\plugins\r1ft.DynamicTimeCyle.dll
del .\git\spo\BepInEx\plugins\r1ft.Headlamps.dll
xcopy /Y /E .\git\spo\user\mods\r1ft-DynamicTimeCycle\r1ft.DynamicTimeCyle.dll .\git\spo\BepInEx\plugins\r1ft.DynamicTimeCyle.dll*
xcopy /Y /E .\git\spo\user\mods\.SPO\mods\Headlamps\r1ft.Headlamps.dll .\git\spo\BepInEx\plugins\r1ft.Headlamps.dll*
rmdir /Q /S .\git\spo
robocopy .\git\aki\ .\ /E
rmdir /Q /S .\git\aki
if NOT EXIST "user\profiles" (
    mkdir .\user\profiles
)
start "" "Aki.Launcher.exe"
Server.exe