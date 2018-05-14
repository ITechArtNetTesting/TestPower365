function Invoke-Validate32366 {
    param( 
        [Parameter(Position = 0, Mandatory = $false)] [switch]$SourceMailbox,
        [Parameter(Position = 1, Mandatory = $false)] [switch]$TargetMailbox
    )  
    Begin {
		
        if ($Script:TestResults.ContainsKey("Test32366")) {
            $Script:TestResults["Test32366"].ValidationResult = "Failed"	
            $Script:TestResults["Test32366"].OverAllResult = "Failed"	
            $Script:TestResults["Test32366"].Data.Folder3
            $LogStart = New-Object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition([Guid]::Parse("{0006200A-0000-0000-C000-000000000046}"),0x8706, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::SystemTime);
            $LogEnd = New-Object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition([Guid]::Parse("{0006200A-0000-0000-C000-000000000046}"),0x8708, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::SystemTime);
            $psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
            $psPropertySet.Add($LogStart)
            $psPropertySet.Add($LogEnd)
            $Message = Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test32366"].Data.Folder -MessageId $Script:TestResults["Test32366"].Data.MessageId -TargetMailbox
            if ($Message -ne $null) {
                $Message.Load($psPropertySet)
                $LogStartValue = $null
                if($Message.TryGetProperty($LogStart,[ref]$LogStartValue)){
                    write-host $LogStartValue.ToString()
                    Write-Host $Script:TestResults["Test32366"].Data.LogStart.ToUniversalTime().ToString()
                    if($LogStartValue.ToString() -eq $Script:TestResults["Test32366"].Data.LogStart.ToUniversalTime().ToString()){
                         $Script:TestResults["Test32366"].ValidationResult = "Succeeded"
                        $Script:TestResults["Test32366"].OverAllResult = "Succeeded"  	
                    }
                }
            }
            else {
                $Script:TestResults["Test32366"].ValidationResult = "Failed"	
                $Script:TestResults["Test32366"].OverAllResult = "Failed"		
            }
            Write-Host "Done" -ForegroundColor Green
        }
        else {
            Write-Host "No Test exists run test first" -ForegroundColor Red
        }
		
    }
}