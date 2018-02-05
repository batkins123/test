call SetVars.bat

runas /user:EINSTEIN\administrator /savecred "%out_dir%\%exe_name% stop"
pause

runas /user:EINSTEIN\administrator /savecred "%out_dir%\%exe_name% uninstall"
pause
