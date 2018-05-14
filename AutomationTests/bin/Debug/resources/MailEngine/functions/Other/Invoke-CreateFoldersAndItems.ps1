function Invoke-CreateFoldersAndItems{
    param( 
        [Parameter(Position=0, Mandatory=$false)] [Microsoft.Exchange.WebServices.Data.Folder]$RootFolder,
        [Parameter(Position=1, Mandatory=$false)] [String]$RootFolderPath,
        [Parameter(Position=1, Mandatory=$false)] [String]$TestNumber
    )  
 	Begin
	{
         ##Create Message

        #Move Contact to New folder
        $NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($RootFolder.service)  
        $FolderName = "Test" + $TestNumber + "-" + (Get-Date).ToString("s")
        $NewFolder.DisplayName = $FolderName
        $NewFolder.FolderClass = "IPF.Note"
        #Define Folder Veiw Really only want to return one object  
        $fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
        #Define a Search folder that is going to do a search based on the DisplayName of the folder  
        $SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName, $NewFolder.DisplayName)  
        #Do the Search  
        $EntryIdVal = $null
        $data = "" | Select Folder1, Folder2, Folder3, ItemIds,TargetMessage
        $data.Folder1 = $RootFolderPath + "\" + $FolderName
        $data.TargetMessage = ""
        $data.ItemIds = New-Object "System.Collections.Generic.List[Byte[]]"
        $findFolderResults = $RootFolder.FindFolders($SfSearchFilter, $fvFolderView)  
        if ($findFolderResults.TotalCount -eq 0) {  
            Write-host ("Folder Doesn't Exist") 
            $NewFolder.Save($RootFolder.Id)  
            $PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
            $psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
            $psPropertySet.Add($PR_ENTRYID)
            $NewFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service, $NewFolder.Id, $psPropertySet)
            $NewFolder.TryGetProperty($PR_ENTRYID, [ref]$EntryIdVal)
            $NewFolder1 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $FolderName1 = "Test" + $TestNumber + "-" + (Get-Date).ToString("s")
            $NewFolder1.DisplayName = $FolderName1
            $NewFolder1.FolderClass = "IPF.Note"
            $NewFolder1.Save($NewFolder.Id) 
            $data.Folder2 = $RootFolderPath + "\" + $FolderName + "\" + $FolderName1
            $NewFolder2 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $FolderName2 = "Test2" + $TestNumber + "-" + (Get-Date).ToString("s")
            $NewFolder2.DisplayName = $FolderName2
            $NewFolder2.FolderClass = "IPF.Note"
            $NewFolder2.Save($NewFolder.Id) 
            $data.Folder3 = $RootFolderPath + "\" + $FolderName + "\" + $FolderName2
            for ($Im = 0; $Im -lt 10; $Im++) {
                    $Email = New-Object Microsoft.Exchange.WebServices.Data.EmailMessage -ArgumentList $service
                    $Email.Subject = "Email test - " + $Im
                    $Email.ToRecipients.Add("Fake@binarytree.com") 
                    $Email.Body = New-Object Microsoft.Exchange.WebServices.Data.MessageBody  
                    $Email.Body.BodyType = [Microsoft.Exchange.WebServices.Data.BodyType]::HTML  
                    $Email.Body.Text = "Body" 
                    $Email.Save($NewFolder2.Id)            
                    $PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
                    $psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
                    $psPropertySet.Add($PR_ENTRYID)
                    $Email.Load($psPropertySet)
                    $EntryIdVal = $null		
                    [Void]$Email.TryGetProperty($PR_ENTRYID, [ref]$EntryIdVal)  
                    $data.ItemIds.Add($EntryIdVal)      
            }
        }
        return $data
	}
}

