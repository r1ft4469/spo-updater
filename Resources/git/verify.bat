@echo off
taskkill /F /IM Aki.Launcher.exe
taskkill /F /IM Server.exe
IF NOT EXIST "git\aki.zip" (
    mkdir .\git\aki
    .\git\mingw64\bin\curl.exe -O https://dev.sp-tarkov.com/attachments/0b99e60d-8b5b-4d09-a64e-d03b6083bff6
    move .\0b99e60d-8b5b-4d09-a64e-d03b6083bff6 .\git\aki\aki.zip
    .\git\bin\7za.exe x -ogit\aki .\git\aki\aki.zip
    del /F /Q .\git\aki\aki.zip
    if NOT EXIST "git\update.dat" (
        .\git\mingw64\bin\curl.exe -LJO https://github.com/r1ft4469/spo-updater/releases/download/Beta9/update.dat
        move /Y .\update.dat .\git\update.dat
    )
    copy /Y .\git\update.dat .\git\aki\update.exe
    .\git\aki\update.exe -y -gm2
    del /F /Q .\git\aki\update.exe
    .\git\bin\7za.exe a -r .\git\aki.zip .\git\aki
    rmdir /Q /S .\git\aki
)
.\git\bin\7za.exe -ogit x .\git\aki.zip
robocopy .\git\aki\ .\ /
rmdir /Q /S .\git\aki
start "" "Aki.Launcher.exe"
Server.exe