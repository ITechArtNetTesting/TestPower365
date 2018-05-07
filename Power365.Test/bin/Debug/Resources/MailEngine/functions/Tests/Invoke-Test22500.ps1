function Invoke-Test22500{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox,
		[Parameter(Position=2, Mandatory=$true)] [String]$SourcePermission,
		[Parameter(Position=4, Mandatory=$true)] [String]$TargetPermission

    )  
 	Begin
	 {
		$Data = "" | Select OrginalId,MovedId,NewId
		if($TargetMailbox.IsPresent){
			$service = $Script:TargetService
			$MailboxName = $Script:TargetMailbox
		}
		else{
			$service = $Script:SourceService
			$MailboxName = $Script:SourceMailbox
		}
		Import-Module ($script:ModuleRoot + '\engine\btT2TPSModule.psd1') -Force
		##Create Message
		$folderid= new-object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::Inbox,$MailboxName)   
 		$InboxFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service,$folderid)
		#Move Contact to New folder
		$NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
		$FolderName = "Test22500-" + (Get-Date).ToString()
		$NewFolder.DisplayName = $FolderName
		$NewFolder.FolderClass = "IPF.Note"
		#Define Folder Veiw Really only want to return one object  
		$fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
		#Define a Search folder that is going to do a search based on the DisplayName of the folder  
		$SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName,$NewFolder.DisplayName)  
		#Do the Search  
		$EntryIdVal = $null
		$data = "" | Select Folder1,SourceId,TargetPermission
		$findFolderResults = $InboxFolder.FindFolders($SfSearchFilter,$fvFolderView)  
		if ($findFolderResults.TotalCount -eq 0){  
			Write-host ("Folder Doesn't Exist") 
			$PermissiontoAdd = [Microsoft.Exchange.WebServices.Data.FolderPermissionLevel]::Reviewer  
			$newfp = new-object Microsoft.Exchange.WebServices.Data.FolderPermission($SourcePermission,$PermissiontoAdd)  
            $NewFolder.Permissions.Add($newfp)  
  			$NewFolder.Save($InboxFolder.Id)  
			Write-host ("Folder Created")  
			$PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF,[Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
			$psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
			$psPropertySet.Add($PR_ENTRYID)
			$NewFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service,$NewFolder.Id,$psPropertySet)
			$NewFolder.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)
			$data.Folder1 = $EntryIdVal
			$data.SourceId = $NewFolder.Id
			$data.TargetPermission = $TargetPermission
			
		}  
		else{  
		    throw ("Folder already Exist with that Name")  
		
		} 	
		$tfile = New-P365TranslationFile -SourceAddress $SourcePermission -TargetAddress $TargetPermission
		Write-host "Part 1 - Message Created"
		Invoke-P365MailboxCopy	-mappingfile $tfile	
		$Folder  = Invoke-P365FindFolder -TargetMailbox -FolderId $EntryIdVal -ParentFolderPath \Inbox
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
		$TestResults.TestCase = "22500"
		$TestResults.Description = "Folder Permission"
		$TestResults.TestLastRun = (Get-Date)		
		if($folder -ne $Null){
			$TestResults.TestResult = "Failed"
			#test permissions
			$psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)
			$psPropertySet.Add([Microsoft.Exchange.WebServices.Data.FolderSchema]::Permissions)
			$folder.Load($psPropertySet)
			foreach($fperm in $folder.Permissions){  
    		if($fperm.UserId.PrimarySmtpAddress -ne $null){  
				write-host ($fperm.UserId.PrimarySmtpAddress)
				if($fperm.UserId.PrimarySmtpAddress.ToLower() -eq $TargetPermission.ToLower()){
						$NewFolder.Load($psPropertySet)  
						$existingperm = $null  
						foreach($fperm1 in $NewFolder.Permissions){  
							if($fperm1.UserId.PrimarySmtpAddress -ne $null){  
								if($fperm1.UserId.PrimarySmtpAddress.ToLower() -eq $SourcePermission.ToLower()){  
										$existingperm = $fperm1  
								}  
							}  
						}  
						if($existingperm -ne $null){  
							$NewFolder.Permissions.Remove($existingperm)  
						}   
						$PermissiontoAdd = [Microsoft.Exchange.WebServices.Data.FolderPermissionLevel]::Editor  
						$newfp = new-object Microsoft.Exchange.WebServices.Data.FolderPermission($SourcePermission,$PermissiontoAdd)  
						$NewFolder.Permissions.Add($newfp) 
						$NewFolder.Update() 
                		$TestResults.Data = $data
						$TestResults.TestResult = "Succeeded"
        			}  
    			}  
			}  

		}
		else{
			$TestResults.TestResult = "Failed"
		}
		$Script:TestResults.OverAllResult = "InComplete"
		if($Script:TestResults.ContainsKey("Test22500")){
			$Script:TestResults["Test22500"] = $TestResults
		}
		else{
			$Script:TestResults.Add("Test22500",$TestResults)
		}
		
		Write-Host "Done" -ForegroundColor Green
     }
}