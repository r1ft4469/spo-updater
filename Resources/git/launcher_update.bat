@echo off
powershell write-host -fore Yellow -back DarkBlue "SPO Launcher Update ..."
powershell write-host -fore Yellow -back DarkBlue "-----------------------------------------"
taskkill /F /IM Server.exe > nul 2>&1
taskkill /F /IM Aki.Launcher.exe > nul 2>&1
powershell write-host -fore Green "Installing new Launcher ..."
move .\git\update.dat.new .\update.exe > nul 2>&1
update.exe -y -gm2 > nul 2>&1
del /F /Q update.exe > nul 2>&1
powershell write-host -fore Yellow -back DarkBlue "Finished."
powershell write-host -fore Yellow -back DarkBlue "Starting Launcher ..."
powershell write-host -fore Yellow -back DarkBlue "-----------------------------------------"
start "" "Aki.Launcher.exe"
.\Server.exe