@echo off
For /f "tokens=2-4 delims=/ " %%a in ('date /t') do (set mydate=%%c-%%a-%%b)
For /f "tokens=1-2 delims=/:" %%a in ('time /t') do (set mytime=%%a%%b)
call :sub >Logs\CDSC2CProbe_%mydate%_%mytime%.txt 2>&1
exit /b

:sub
ProbeTests.exe cdsc2c