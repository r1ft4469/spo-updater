@echo off
taskkill /F /IM Server.exe
git\cmd\git.exe clone --recursive https://github.com/kobrakon/SPO_DEV.git update
xcopy /Y /E update\\. .
xcopy /Y /E .\user\mods\r1ft-DynamicTimeCycle\r1ft.DynamicTimeCyle.dll .\BepInEx\plugins\r1ft.DynamicTimeCyle.dll
xcopy /Y /E .\user\mods\.SPO\mods\Headlamps\r1ft.Headlamps.dll .\BepInEx\plugins\r1ft.Headlamps.dll
rmdir /Q /S update
Server.exe