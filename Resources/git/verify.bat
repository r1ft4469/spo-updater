@echo off
REM DOWNLOAD AKI

IF NOT EXIST "git\aki.zip" (
    mkdir .\git\aki
    .\git\mingw64\bin\curl.exe -O https://dev.sp-tarkov.com/attachments/0b99e60d-8b5b-4d09-a64e-d03b6083bff6
    move .\0b99e60d-8b5b-4d09-a64e-d03b6083bff6.zip .\git\aki\aki.zip
    .\git\7z\7za.exe x -o .\git\aki\ .\git\aki\aki.zip
    del /F /Q .\git\aki\aki.zip
    if NOT EXIST "git\launcherupdate.dat" (
        REM DOWNLOAD LUANCHER
        REM .\git\mingw64\bin\curl.exe -O 
    )
    REM INSTALL LAUNCHER IN BUILD

    .\git\7z\7za.exe a .\git\aki.zip .\git\aki
    rmdir /Q /S .\git\aki
)

.\git\7z\7za.exe -o .\git\ x .\git\aki\aki.zip
IF EXIST "server.exe" (
    REM VERIFY AKI FILES

    REM ELSE
        REM INSTALL AKI

) ELSE (

    REM GET VERSION OF TARKOV

    REM CHECK AKI VERSION
    
    REM COMPAIRE TO PATCHER

        REM PATCH

    REM INSTALL AKI
)


