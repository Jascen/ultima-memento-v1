echo "Purging existing files"
rmdir /s /q "src\Server\Source"
rmdir /s /q "src\Scripts\DataScripts"
rmdir /s /q "src\Scripts\InfoScripts"
del /F "src\World.exe - Shortcut" 
pause

echo "Setting up symbolic existing links"
mklink /d src\Server\Source ..\..\World\Data\System\Source
mklink /d src\Scripts\DataScripts ..\..\World\Data\Scripts
mklink /d src\Scripts\InfoScripts ..\..\World\Info\Scripts
mklink "src\World.exe - Shortcut" ..\World\World.exe
pause

echo Compiling World.exe file
cd src\tools
.\compile-world-win.bat