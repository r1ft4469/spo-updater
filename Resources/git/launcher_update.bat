@echo off
powershell write-host -fore Blue "SPO Launcher Update ..."
powershell write-host -fore Blue "-----------------------------------------"
taskkill /F /IM Server.exe > nul 2>&1
taskkill /F /IM Aki.Launcher.exe > nul 2>&1
powershell write-host -fore DarkYellow "Installing new Launcher ..."
move .\git\update.dat.new .\update.exe > nul 2>&1
update.exe -y -gm2 > nul 2>&1
del /F /Q update.exe > nul 2>&1
powershell write-host -fore Blue "Finished."
powershell write-host -fore Blue "Starting Launcher ..."
powershell write-host -fore Blue "-----------------------------------------"
start "" "Aki.Launcher.exe"
.\Server.exe