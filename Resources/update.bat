@echo off
taskkill /F /IM Server.exe
bin\\git.exe clone --recursive https://github.com/kobrakon/SPO_DEV.git update
xcopy /Y /E update\\. .
rmdir /Q /S update
Server.exe