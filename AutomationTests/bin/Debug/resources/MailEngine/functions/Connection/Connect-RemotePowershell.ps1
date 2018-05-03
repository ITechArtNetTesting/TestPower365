function Connect-RemotePowershell{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		if($TargetMailbox.IsPresent){
			  if(![String]::IsNullOrEmpty($Script:TargetPowerShellOverRide)){					
						$Session = New-PSSession -ConfigurationName Microsoft.Exchange - -ConnectionUri $Script:TargetPowerShellOverRide  -Credential $Script:TargetPSCreds -Authentication Basic -AllowRedirection -SessionOption(New-PSsessionOption -SkipCACheck -SkipCNCheck)
				}else{
					  $Session = New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://outlook.office365.com/powershell -Credential $Script:TargetPSCreds -Authentication Basic -AllowRedirection
				}
				
		}
		else{
				if(![String]::IsNullOrEmpty($Script:SourcePowerShellOverRide)){
						$Session = New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri $Script:SourcePowerShellOverRide -Credential $Script:SourcePSCreds -Authentication Basic -AllowRedirection	-SessionOption(New-PSsessionOption -SkipCACheck -SkipCNCheck)
				}
				else{
						$Session = New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://outlook.office365.com/powershell -Credential $Script:SourcePSCreds -Authentication Basic -AllowRedirection	
				}				
		}
		return $Session
	 }
	}