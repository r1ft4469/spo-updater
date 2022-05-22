@echo off
powershell write-host -fore Cyan "SPO Launcher Update ..."
powershell write-host -fore Cyan "-----------------------------------------"
taskkill /F /IM Server.exe > nul 2>&1
taskkill /F /IM Aki.Launcher.exe > nul 2>&1
powershell write-host -fore Yellow "Installing new Launcher ..."
move .\git\update.dat.new .\update.exe > nul 2>&1
update.exe -y -gm2 > nul 2>&1
del /F /Q update.exe > nul 2>&1
powershell write-host -fore Cyan "Finished."
powershell write-host -fore Cyan "Starting Launcher ..."
powershell write-host -fore Cyan "-----------------------------------------"
start "" "Aki.Launcher.exe"
.\Server.exe