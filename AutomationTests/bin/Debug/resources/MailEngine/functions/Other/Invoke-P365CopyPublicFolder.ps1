function Invoke-P365CopyPublicFolder{
    param( 
		[Parameter(Position=3, Mandatory=$false)] [String]$MappingFile,
		[Parameter(Position=4, Mandatory=$true)] [String]$SourceFolderPath,
		[Parameter(Position=5, Mandatory=$true)] [String]$TargetCopyPath
    )  
 	Begin
	 {
		 if([String]::IsNullOrEmpty($MappingFile)){
			 $MappingFile = ($script:ModuleRoot + '\engine\mapping.csv')
		 }
		Import-Module ($script:ModuleRoot + '\engine\btT2TPSModule.psd1') -Force
		$DomainMappings = New-Object "System.Collections.Generic.Dictionary``2[System.String,System.String]"
		$SourceDomain = new-object System.net.mail.mailaddress($Script:SourceMailbox)
		$TargetDomain =  new-object System.net.mail.mailaddress($Script:TargetMailbox)
		$DomainMappings.Add(($SourceDomain.Host).ToLower(), ($TargetDomain.Host).ToLower())
		$DomainMappings.Add("_default_", ($TargetDomain.Host).ToLower())
		Copy-T2TPublicFolders -SourceMailbox $Script:SourceMailbox -SourceCredentials $Script:SourcePSCreds -TargetMailbox $Script:TargetMailbox -TargetCredentials $Script:TargetPSCreds  -mappingfile $MappingFile  -ProcessingPath c:\temp -SourcePublicFolderPath $SourceFolderPath -TargetPublicFolderPath $TargetCopyPath -Delta:$true -SourceOnPremise $Script:SourceOnPrem -TargetOnPremise $Script:TargetOnPrem -SourceAutoDiscoverOverRide $Script:SourceAutoDiscoverOverRide -TargetAutoDiscoverOverRide $Script:TargetAutoDiscoverOverRide
	}
}

