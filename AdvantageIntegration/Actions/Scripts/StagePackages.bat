Call SetVars.bat

echo Deleting packages

del /f /q /s %pkg_dir%

echo Installing packages

Bin\nuget install Nancy.Hosting.Self -OutputDirectory %pkg_dir%
Bin\nuget install Topshelf -OutputDirectory %pkg_dir%
Bin\nuget install Topshelf.Linux -OutputDirectory %pkg_dir%

echo Staging package assemblies for build process

copy /y %pkg_dir%\Nancy.1.4.1\lib\net40\Nancy.dll %ref_dir%
copy /y %pkg_dir%\Nancy.Hosting.Self.1.4.1\lib\net40\Nancy.Hosting.Self.dll %ref_dir%
copy /y %pkg_dir%\Topshelf.4.0.3\lib\net452\Topshelf.dll %ref_dir%
copy /y %pkg_dir%\Topshelf.Linux.1.0.16.15\lib\Topshelf.Linux.dll %ref_dir%

pause
