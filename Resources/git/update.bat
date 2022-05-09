@echo off
del git\release.json
move git\release.new.json git\release.json
taskkill /F /IM Server.exe
IF EXIST "git\git.exe" (
    git\git.exe -y -gm2 -InstallPath="git"
    del git\git.exe
    del git\git.7z.001
)
rmdir /Q /S user\mods
mkdir user\mods
move .\BepInEx\plugins\aki-bundles.dll .\BepInEx\
move .\BepInEx\plugins\aki-core.dll .\BepInEx\
move .\BepInEx\plugins\aki-custom.dll .\BepInEx\
move .\BepInEx\plugins\aki-debugging.dll .\BepInEx\
move .\BepInEx\plugins\aki-secret.dll .\BepInEx\
move .\BepInEx\plugins\aki-singleplayer.dll .\BepInEx\
move .\BepInEx\plugins\ConfigurationManager.dll .\BepInEx\
del /Q BepInEx\plugins\*
move .\BepInEx\aki-bundles.dll .\BepInEx\plugins\
move .\BepInEx\aki-core.dll .\BepInEx\plugins\
move .\BepInEx\aki-custom.dll .\BepInEx\plugins\
move .\BepInEx\aki-debugging.dll .\BepInEx\plugins\
move .\BepInEx\aki-secret.dll .\BepInEx\plugins\
move .\BepInEx\aki-singleplayer.dll .\BepInEx\plugins\
move .\BepInEx\ConfigurationManager.dll .\BepInEx\plugins\
git\cmd\git.exe clone --recursive https://github.com/kobrakon/SPO_DEV.git update
xcopy /Y /E update\\. .
del .\BepInEx\plugins\r1ft.DynamicTimeCyle.dll
del .\BepInEx\plugins\r1ft.Headlamps.dll
xcopy /Y /E .\user\mods\r1ft-DynamicTimeCycle\r1ft.DynamicTimeCyle.dll .\BepInEx\plugins\r1ft.DynamicTimeCyle.dll*
xcopy /Y /E .\user\mods\.SPO\mods\Headlamps\r1ft.Headlamps.dll .\BepInEx\plugins\r1ft.Headlamps.dll*
rmdir /Q /S update
Server.exe
