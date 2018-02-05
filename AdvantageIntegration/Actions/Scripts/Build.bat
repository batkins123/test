call SetVars.bat

rem Delete old output
del /f /q %out_dir%

rem Build library
%csc_dir%\csc -platform:anycpu -target:library -out:%out_dir%\%lib1_name% %src_dir%\Lib\PlanSponsor.cs %src_dir%\Lib\PlanMember.cs > %log_dir%\Build%lib1_name%.log
copy %out_dir% %ref_dir%

rem Build library
%csc_dir%\csc -platform:anycpu -target:library -out:%out_dir%\%lib2_name% -reference:%ref_dir%\%lib1_name% %src_dir%\Lib\Facade.cs > %log_dir%\Build%lib2_name%.log
copy %out_dir% %ref_dir%

rem Build executable
%csc_dir%\csc -platform:anycpu -target:exe -out:%out_dir%\%exe_name% -reference:%ref_dir%\Nancy.Hosting.Self.dll -reference:%ref_dir%\Nancy.dll -reference:%ref_dir%\Topshelf.dll -reference:%ref_dir%\Topshelf.Linux.dll -reference:%ref_dir%\%lib1_name% -reference:%ref_dir%\%lib2_name% %src_dir%\Exe\Host.cs %src_dir%\Exe\Module.cs %src_dir%\Exe\Service.cs > %log_dir%\Build%exe_name%.log
copy %ref_dir% %out_dir%
