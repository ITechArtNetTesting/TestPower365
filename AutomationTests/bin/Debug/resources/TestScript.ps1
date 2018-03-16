function Pause
	{
	    Write-Host -NoNewLine "Press any key to continue . . . "
	    [Console]::ReadKey($true) | Out-Null
	    Write-Host
	}
Write-Host "A"
Write-Host "B"
Write-Host "C"
Write-Host "Press any key to continue ..."
#$x= Read-Host
#$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeydown")
#$x = $Host.UI.RawUI.ReadKey()
#Pause
#$x= Read-Host
Start-Sleep -Seconds 5
Write-Host "D"
Write-Host "E"
Write-Host "F"