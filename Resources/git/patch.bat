@echo off
taskkill /F /IM Aki.Launcher.exe
taskkill /F /IM Server.exe
IF EXIST "git\git.exe" (
    git\git.exe -y -gm2 -InstallPath="git"
    del git\git.exe
    del git\git.7z.001
)
FOR /F "USEBACKQ" %%F IN (`powershell -NoLogo -NoProfile -Command ^(Get-Item "EscapeFromTarkov.exe"^).VersionInfo.FileVersion`) DO (SET fileVersion=%%F)
IF "%fileVersion%"=="0.12.12.17975" (
    call :DOWNLOAD
    xcopy /Y /E git\\patch\\12.12.17975\\.
    call :INSTALL
    goto :EOF
)
IF "%fileVersion%"=="0.12.12.17861" (
    call :DOWNLOAD
    xcopy /Y /E git\\patch\\12.12.17681\\.
    call :INSTALL
    goto :EOF
)
IF "%fileVersion%"=="0.12.12.17686" (
    call :DOWNLOAD
    xcopy /Y /E git\\patch\\12.12.17686\\.
    call :INSTALL
    goto :EOF
)
IF "%fileVersion%"=="0.12.12.17639" (
    call :DOWNLOAD
    xcopy /Y /E git\\patch\\12.12.17639\\.
    call :INSTALL
    goto :EOF
)
IF "%fileVersion%"=="0.12.12.17566" (
    call :DOWNLOAD
    xcopy /Y /E git\\patch\\12.12.17566\\.
    call :INSTALL
    goto :EOF
)
IF "%fileVersion%"=="0.12.12.17349" (
    start "" "cmd.exe" /c git\verify.bat
    goto :EOF
) else (
    echo Update Tarkov or Wait for Patcher Release
    echo Current Releases Available
    echo ------------------
    echo 0.12.12.17686
    echo 0.12.12.17639
    echo 0.12.12.17566
    echo 0.12.12.17349 < AKI VERSION
    pause
    goto :EOF
)

:INSTALL
patcher.exe
rmdir /Q /S .\git\patch
del patcher.exe
start "" "cmd.exe" /c git\verify.bat
goto :EOF

:DOWNLOAD
mkdir .\git\patch
.\git\mingw64\bin\curl.exe -LJ -o .\git\patch\patch.dat.001 https://raw.githubusercontent.com/r1ft4469/spo-updater/update/patch.dat.001
.\git\mingw64\bin\curl.exe -LJ -o .\git\patch\patch.dat.002 https://raw.githubusercontent.com/r1ft4469/spo-updater/update/patch.dat.002
.\git\bin\7za.exe x -ogit\patch .\git\patch\patch.dat.001
del .\git\patch\patch.dat.001
del .\git\patch\patch.dat.002
goto :EOF
