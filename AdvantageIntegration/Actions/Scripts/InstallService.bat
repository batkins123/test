call SetVars.bat

runas /user:EINSTEIN\administrator /savecred "%out_dir%\%exe_name% install"
pause

runas /user:EINSTEIN\administrator /savecred "%out_dir%\%exe_name% start"
pause
