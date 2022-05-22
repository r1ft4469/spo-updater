@echo off
powershell write-host -fore Cyan "SPO Update ..."
powershell write-host -fore Cyan "-----------------------------------------"
taskkill /F /IM Aki.Launcher.exe > nul 2>&1
taskkill /F /IM Server.exe > nul 2>&1
del git\release.json > nul 2>&1
move git\release.new.json git\release.json > nul 2>&1
call :CLEAN
powershell write-host -fore Yellow "Getting Newest Version of SPO ..."
git\cmd\git.exe clone --recursive https://github.com/kobrakon/SPO_DEV.git update > nul 2>&1
IF EXIST "git\update.dat" (
    .\git\mingw64\bin\curl.exe -LJ -o .\git\update.dat.new https://raw.githubusercontent.com/r1ft4469/spo-updater/update/update.dat > nul 2>&1
    fc /B .\git\update.dat .\git\update.dat.new > nul > nul 2>&1
    IF errorlevel 0 (
       del /Q update\update.dat.new > nul 2>&1
    ) ELSE (
        xcopy /Y /E .\git\update.dat.new .\git\update.dat* > nul 2>&1
    )
) else (
    .\git\mingw64\bin\curl.exe -O https://raw.githubusercontent.com/r1ft4469/spo-updater/update/update.dat > nul 2>&1
)
powershell write-host -fore Yellow "Installing SPO ..."
xcopy /Y /E .\update\user\mods\r1ft-DynamicTimeCycle\r1ft.DynamicTimeCyle.dll .\update\BepInEx\plugins\r1ft.DynamicTimeCyle.dll* > nul 2>&1
xcopy /Y /E .\update\user\mods\.SPO\mods\Headlamps\r1ft.Headlamps.dll .\update\BepInEx\plugins\r1ft.Headlamps.dll* > nul 2>&1
robocopy .\update\ .\ /E /FFT /LOG:Logs\Update.log > nul 2>&1
rmdir /Q /S update > nul 2>&1
powershell write-host -fore Cyan "Finished."
powershell write-host -fore Cyan "Starting Launcher ..."
powershell write-host -fore Cyan "-----------------------------------------"
start "" "Aki.Launcher.exe"
.\Server.exe
goto :EOF

:CLEAN
powershell write-host -fore Yellow "Cleaning Old Mods ..."
rmdir /Q /S user\mods > nul 2>&1
mkdir user\mods > nul 2>&1
move .\BepInEx\plugins\aki-bundles.dll .\BepInEx\ > nul 2>&1
move .\BepInEx\plugins\aki-core.dll .\BepInEx\ > nul 2>&1
move .\BepInEx\plugins\aki-custom.dll .\BepInEx\ > nul 2>&1
move .\BepInEx\plugins\aki-debugging.dll .\BepInEx\ > nul 2>&1
move .\BepInEx\plugins\aki-secret.dll .\BepInEx\ > nul 2>&1
move .\BepInEx\plugins\aki-singleplayer.dll .\BepInEx\ > nul 2>&1
move .\BepInEx\plugins\ConfigurationManager.dll .\BepInEx\ > nul 2>&1
del /Q BepInEx\plugins\* > nul 2>&1
move .\BepInEx\aki-bundles.dll .\BepInEx\plugins\ > nul 2>&1
move .\BepInEx\aki-core.dll .\BepInEx\plugins\ > nul 2>&1
move .\BepInEx\aki-custom.dll .\BepInEx\plugins\ > nul 2>&1
move .\BepInEx\aki-debugging.dll .\BepInEx\plugins\ > nul 2>&1
move .\BepInEx\aki-secret.dll .\BepInEx\plugins\ > nul 2>&1
move .\BepInEx\aki-singleplayer.dll .\BepInEx\plugins\ > nul 2>&1
move .\BepInEx\ConfigurationManager.dll .\BepInEx\plugins\ > nul 2>&1
goto :EOF