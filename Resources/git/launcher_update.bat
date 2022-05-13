@echo off
taskkill /F /IM Server.exe
taskkill /F /IM Aki.Launcher.exe
move launcherupdate.dat launcherupdate.exe
launcherupdate.exe -y -gm2
del /F /Q launcherupdate.exe
start "" "Server.exe"
start "" "Aki.Launcher.exe"
