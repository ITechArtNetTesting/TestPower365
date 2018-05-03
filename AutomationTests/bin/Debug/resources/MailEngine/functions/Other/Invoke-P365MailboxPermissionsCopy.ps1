function Invoke-P365MailboxPermissionsCopy{
    param( 
		[Parameter(Position=3, Mandatory=$false)] [String]$MappingFile
    )  
 	Begin
	 {
		 if([String]::IsNullOrEmpty($MappingFile)){
			 $MappingFile = ($script:ModuleRoot + '\engine\mapping.csv')
		 }
		Import-Module ($script:ModuleRoot + '\engine\btT2TPSModule.psd1') -Force
		Copy-T2TMailboxPermissions -SourceMailbox $Script:SourceMailbox -SourceCredentials $Script:SourcePSCreds -TargetMailbox $Script:TargetMailbox -TargetCredentials $Script:TargetPSCreds -mappingfile $MappingFile -ProcessingPath c:\temp -SourceOnPremise $Script:SourceOnPrem -TargetOnPremise $Script:TargetOnPrem -SourceAutoDiscoverOverRide $Script:SourceAutoDiscoverOverRide -TargetAutoDiscoverOverRide $Script:TargetAutoDiscoverOverRide -SourcePowerShellOverRide $Script:SourcePowerShellOverRide -TargetPowerShellOverRide $Script:TargetPowerShellOverRide 
		
	}
}

