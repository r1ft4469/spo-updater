@echo off
taskkill /F /IM Server.exe
taskkill /F /IM Aki.Launcher.exe
move .\git\update.dat.new .\update.exe
update.exe -y -gm2
del /F /Q update.exe
start "" "Server.exe"
start "" "Aki.Launcher.exe"
