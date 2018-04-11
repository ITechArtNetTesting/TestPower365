param([string]$sourceUserName = "admin@BTCloud7.Power365.Cloud", [string]$sourcepasswd = "Password31", [string]$TargetUserName = "admin@BTCloud9.Power365.Cloud", [string]$Targetpasswd = "Password32", [string]$StopFilePath1 = "c:\PFTests\stop1.txt", [string]$StopFilePath2 = "c:\PFTests\stop2.txt", [string]$AttachmentPath = "C:\test.txt", [string]$SendAsSourceUserName = "P365PFAutoUser1@corp31.cmtsandbox.com", [string]$TargetAsSourceUserName = "P365PFAutoUser1@corp32.cmtsandbox.com", [string]$Sourcex400Address = "/o=ExchangeLabs/ou=Exchange Administrative Group (FYDIBOHF23SPDLT)/cn=Recipients/cn=c66e95f6b9484417b0dc59918a2783a2-P365PFAutoU", [string]$Targetx400Address = "/o=ExchangeLabs/ou=Exchange Administrative Group (FYDIBOHF23SPDLT)/cn=Recipients/cn=51d6b74a913b4e65bad13c4f464ae875-P365PFAutoU", [string]$SourceProxyAddress = "Test4444@corp31.cmtsandbox.com", [string]$TargetProxyAddress = "Test4444@corp32.cmtsandbox.com")
import-module ($PSScriptRoot + "\..\PSTools\cmdtest\btT2TPSModule.psd1") -Force
import-module ($PSScriptRoot + "\..\PSTools\testutils\btT2TestUtilPSModule.psd1") -Force

#$sourceUserName = "C7O365Admin@btcorp7.onmicrosoft.com"
$sourcepasswdSecure = ConvertTo-SecureString $sourcepasswd -AsPlainText -Force
#
#$TargetUserName = "corp9O365admin@btcorp9.onmicrosoft.com"
$TargetpasswdSecure = ConvertTo-SecureString $Targetpasswd -AsPlainText -Force

$SoruceCreds = New-Object System.Management.Automation.PSCredential ($sourceUserName, $sourcepasswdSecure)
$TargetCreds = New-Object System.Management.Automation.PSCredential ($TargetUserName, $TargetpasswdSecure)

$SourceFolderPath = "\\AutomationTests\Test1"
$TargetFolderPath = "\\AutomationTests\Test1"
$TargetCopyPath = "\\AutomationTests"

#$MappingFile = $PSScriptRoot + "\..\PSTools\mappingC7toC9.csv"

$ProcessingPath = 'c:\unittests\ProcessingPath'

$LogPath = "C:\unittests\logs"
$TestLogFile = $LogPath + "\" + "OverAllLog" + (Get-Date).ToString("yyyyMMddHHmm") + ".log"

$SourceMailbox = $sourceUserName
$TargetMailbox = $TargetUserName 

#$SendAsSourceUserName --'P365Filter1@corp31.cmtsandbox.com'
#$TargetAsSourceUserName --'P365Filter1@corp32.cmtsandbox.com'
#$Sourcex400Address '/o=ExchangeLabs/ou=Exchange Administrative Group (FYDIBOHF23SPDLT)/cn=Recipients/cn=99a7a43ba3ab4ba5bcd80f1941c64ad0-P365Filter1'
#$Targetx400Address '/o=ExchangeLabs/ou=Exchange Administrative Group (FYDIBOHF23SPDLT)/cn=Recipients/cn=e5de27c6a98d496c8ae21490b95f64cb-P365Filter1' 

$SourceSMTPAddresss = $SendAsSourceUserName
$TargetSMTPAddress = $TargetAsSourceUserName

#$SourceProxyAddress = "Mohini111@corp31.cmtsandbox.com"
#$TargetProxyAddress = "Mohini111@corp32.cmtsandbox.com"

$DomainMappings = New-Object "System.Collections.Generic.Dictionary``2[System.String,System.String]"
$DomainMappings.Add(("corp31.cmtsandbox.com").ToLower(), ("corp32.cmtsandbox.com").ToLower())
$DomainMappings.Add("_default_", ("corp32.cmtsandbox.com").ToLower())
##
#names
###########################################################################################################################################################################
$AddFolderTestNameMailEnabled = "NewMailEnable" + (Get-Date).ToString("s");
$AddFolderTree = "NewFolderTree" + (Get-Date).ToString("s");
$AddFolderTestName = "NewMailFolderAdd" + (Get-Date).ToString("s");
$FolderTestName = "NewContactFolderAdd"+ (Get-Date).ToString("s");
$CalFolderTestName = "NewCalendarFolderAdd"+ (Get-Date).ToString("s");
$FolderTestName2 = "NewFolderToDelete"+ (Get-Date).ToString("s");
$FolderTestName3 = "NewFolderToMove"+ (Get-Date).ToString("s");
$FolderTestNameR = "NewFolderToRename"+ (Get-Date).ToString("s");
$ContactAddSubject = "Contact New Item";
$AppointmentAddSubject = "Test Meeting" ;
$CustomItemTest = "Sticky Note Add";
$ItemAddSubject = "Test SMTP Address Transform New Item" ;
$ItemAddSubject400 = "Test x400 Address Transform New Item";
$ItemAddSubjectAttachment = "Test New Item for Attachment Test";

$dlName = "New PDL Link Test " ;
$contactNameDL = "New Contact ";
$ModifiedItemSubject = "Test Modified Message ";
$FolderTestNamer2 = "NewFolderToRename2" + (Get-Date).ToString("s");
############################################## Start the TestSession ###################################################

$TestSession = Start-T2tPublicFolderMigrationTests -SourceMailbox $SourceMailbox -SourceCredentials $SoruceCreds -TargetMailbox $TargetMailbox -TargetCredentials $TargetCreds -LogFilePath $LogPath  -SourcePublicFolderPath $SourceFolderPath -TargetPublicFolderPath $TargetCopyPath  

########################################## PreDeleta Test #############################################################

#######################Test 1 Folder existance Test this test if the folderpath are the same in both Mailboxes #############################

#PreDeleta Test

#Test 1 Folder existance Test this test if the folderpath are the same in both Mailboxes

$Test1 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "folderexits" 
if($Test1.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Folder existance Check succeeded") -ForegroundColor Green 
    ("Folder existance Check succeeded") | Out-File -Append -FilePath $TestLogFile  
}
else
{
  foreach ($err in $Test1.Errors)
    {
        write-host $err
    }   
    Write-Host ("Folder existance Check failed") -ForegroundColor Red       
}

#Test2 Item existance Test test if the Item exists in both mailboxes checks Receipents displayName and attachment count

$Test2 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "sourcetargetitems" 
if($Test2.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Source Target Item existance Check succeeded") -ForegroundColor Green   
    ("Source Target Item existance Check succeeded") | Out-File -Append -FilePath $TestLogFile  
}
else
{
    Write-Host ("Source Target Item existance Check failed") -ForegroundColor Red  
     foreach ($err in $Test2.Errors)
    {
        write-host $err
    }     
}


#AddFolder Tests
#Add a Folder to Mail Enabled

$TestmeAdd = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addfolder" -ParentSourceFolder $SourceFolderPath -FolderName $AddFolderTestNameMailEnabled -FolderClass "IPF.Note"
if($TestmeAdd.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add New MailFolder Check succeeded-1") -ForegroundColor Green   
   ("Add New MailFolder Check succeeded")   | Out-File -Append -FilePath $TestLogFile  
}
else
{
    Write-Host ("Add New MailFolder Check failed") -ForegroundColor Red 
    ("Add New MailFolder Check failed")  | Out-File -Append -FilePath $TestLogFile    

    foreach ($t in $TestmeAdd.Errors){
    Write-Host ($t) -ForegroundColor Red 
    }
}
#Add Folder Tree
$TestmeAddFT = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addfoldertree" -ParentSourceFolder $SourceFolderPath -FolderName $AddFolderTree -FolderClass "IPF.Note"
if($TestmeAddFT.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add New FolderTree Check succeeded-1") -ForegroundColor Green   
   ("Add New FolderTree Check succeeded")   | Out-File -Append -FilePath $TestLogFile  
}
else
{
    Write-Host ("Add New FolderTree Check failed") -ForegroundColor Red 
    foreach ($err in $TestmeAddFT.Errors)
    {
        write-host $err
    }   
}

#Test3 Add New Mail Folder to Source Mailboxes

$Test3 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addfolder" -ParentSourceFolder $SourceFolderPath -FolderName $AddFolderTestName -FolderClass "IPF.Note"
if($Test3.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add New MailFolder Check succeeded-2") -ForegroundColor Green   
   ("Add New MailFolder Check succeeded")   | Out-File -Append -FilePath $TestLogFile  
}
else
{
    Write-Host ("Add New MailFolder Check failed") -ForegroundColor Red 
     foreach ($err in $Test3.Errors)
    {
        write-host $err
    }   
}

#Test4 Add New Contacts Folder to Source Mailboxes
$Test4 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addfolder" -ParentSourceFolder $SourceFolderPath -FolderName $FolderTestName -FolderClass "IPF.Contact"
if($Test4.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add New Contacts Check succeeded") -ForegroundColor Green   
    ("Add New Contacts Check succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add New Contacts Check failed") -ForegroundColor Red
    foreach ($err in $Test4.Errors)
    {
        write-host $err
    }         
}

#Test5 Add New Calendar Folder to Source Mailboxes

$Test5 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addfolder" -ParentSourceFolder $SourceFolderPath -FolderName $CalFolderTestName -FolderClass "IPF.Appointment"
if($Test5.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add New Calendar Check succeeded") -ForegroundColor Green   
    ("Add New Calendar Check succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add New Calendar Check failed") -ForegroundColor Red   
      foreach ($err in $Test5.Errors)
    {
        write-host $err
    }   
}

#Test6 Add Folder to Test Folder Move and Delete



$Test6 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addfolder" -ParentSourceFolder $SourceFolderPath -FolderName $FolderTestName2 -FolderClass "IPF.Note"
if($Test6.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Folder to Test Folder Delete succeeded") -ForegroundColor Green   
    ("Add Folder to Test Folder Delete succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add Folder to Test Folder Delete failed") -ForegroundColor Red
      foreach ($err in $Test6.Errors)
    {
        write-host $err
    }    
}


$Test6M = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addfolder" -ParentSourceFolder $SourceFolderPath  -FolderName $FolderTestName3 -FolderClass "IPF.Note"
if($Test6M.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Folder to Test Folder Move succeeded") -ForegroundColor Green   
    ("Add Folder to Test Folder Move succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add Folder to Test Folder Move failed") -ForegroundColor Red
     foreach ($err in $Test6M.Errors)
    {
        write-host $err
    }    
}


$Test6R = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addfolder" -ParentSourceFolder $SourceFolderPath  -FolderName $FolderTestNameR -FolderClass "IPF.Note"
if($Test6R.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Folder to Test Folder Rename succeeded") -ForegroundColor Green   
    ("Add Folder to Test Folder Rename succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add Folder to Test Folder Rename failed") -ForegroundColor Red
     foreach ($err in $Test6R.Errors)
    {
        write-host $err
    }      
}
#AddItemTests

$ItemAddSubject = "Test New Item " + (Get-Date).ToString("yyyyMMddHHmmss");

$Test7 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addmailitem" -SourceFolder $SourceFolderPath  -ItemSubject $ItemAddSubject
if($Test7.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Item Test succeeded-1") -ForegroundColor Green  
    ("Add Item Test succeeded") | Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Add Item Test failed") -ForegroundColor Red
     foreach ($err in $Test7.Errors)
    {
        write-host $err
    }   
}

$ItemAddSubjectm = "Test New Item Move " + (Get-Date).ToString("yyyyMMddHHmmss");

$Test7M = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addmailitem" -SourceFolder $SourceFolderPath  -ItemSubject $ItemAddSubjectm
if($Test7M.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Item Test succeeded-2") -ForegroundColor Green  
    ("Add Item Test succeeded") | Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Add Item Test failed") -ForegroundColor Red
     foreach ($err in $Test7M.Errors)
    {
        write-host $err
    }    
}

$ItemAddSubjectd = "Test New Item Move " + (Get-Date).ToString("yyyyMMddHHmmss");

$Test7D = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addmailitem" -SourceFolder $SourceFolderPath  -ItemSubject $ItemAddSubjectd
if($Test7D.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Item Test succeeded-3") -ForegroundColor Green  
    ("Add Item Test succeeded") | Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Add Item Test failed") -ForegroundColor Red
     foreach ($err in $Test7D.Errors)
    {
        write-host $err
    }   
}



$Test8 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addcontactitem" -SourceFolder $SourceFolderPath  -ContactName $ContactAddSubject -ContactEmailAddress 'Address@Fake.com'
if($Test8.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Contact Test succeeded") -ForegroundColor Green   
    ("Add Contact Test succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add Contact Test failed") -ForegroundColor Red   
     foreach ($err in $Test8.Errors)
    {
        write-host $err
    }   
}


$Test9 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addappointmentitem" -SourceFolder $SourceFolderPath  -ItemSubject $AppointmentAddSubject -Start (Get-Date) -End (Get-Date).AddHours(1)
if($Test9.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Meeting Test succeeded") -ForegroundColor Green   
    ("Add Meeting Test succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add Meeting Test failed") -ForegroundColor Red   
     foreach ($err in $Test9.Errors)
    {
        write-host $err
    }   
}



$Test10 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addcustomitem" -SourceFolder $SourceFolderPath  -ItemSubject $CustomItemTest -ItemClass "IPM.StickyNote"
if($Test10.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Custom Item Test succeeded") -ForegroundColor Green   
    ("Add Custom Item Test succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add Custom Item Test failed") -ForegroundColor Red   
     foreach ($err in $Test10.Errors)
    {
        write-host $err
    }   
}

# Test Address Transform add a New Item





$Test11 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "transformaddress" -SourceFolder $SourceFolderPath  -ItemSubject $ItemAddSubject400 -Address $SourceSMTPAddresss -TargetAddress $TargetSMTPAddress
if($Test11.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("SMTP Address Rewriting Test succeeded") -ForegroundColor Green   
    ("SMTP Address Rewriting Test succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("SMTP Address Rewriting  Test failed") -ForegroundColor Red
      foreach ($err in $Test11.Errors)
    {
        write-host $err
    }   
}



$Test12 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "transformaddress" -SourceFolder $SourceFolderPath  -ItemSubject $ItemAddSubject -Address $Sourcex400Address -TargetAddress $Targetx400Address
if($Test12.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("X400 Address Rewriting Test succeeded") -ForegroundColor Green   
    ("X400 Address Rewriting Test succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("X400 Address Rewriting  Test failed") -ForegroundColor Red   
     foreach ($err in $Test12.Errors)
    {
        write-host $err
    }   
}

#New Item for Attachment Test


$Test13 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addmailitem" -SourceFolder $SourceFolderPath  -ItemSubject $ItemAddSubjectAttachment
if($Test13.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Item Test succeeded-4") -ForegroundColor Green 
    ("Add Item Test succeeded") | Out-File -Append -FilePath $TestLogFile      
}
else
{
    Write-Host ("Add Item Test failed") -ForegroundColor Red
      foreach ($err in $Test13.Errors)
    {
        write-host $err
    }   
}

#Add personal distribution List

$Test14 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "personaldlcontactlink" -SourceFolder $SourceFolderPath  -ContactName $contactNameDL -ContactEmailAddress 'Address@Fake.com' -DLName $dlName
if($Test14.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add PDL Test succeeded") -ForegroundColor Green  
    ("Add PDL Test succeeded")| Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Add PDL Test failed") -ForegroundColor Red   
     foreach ($err in $Test14.Errors)
    {
        write-host $err
    }   
}
###############################################################################################################################################
Write-Host ("Powershell will pause until Migration is complete - 1") -ForegroundColor Green
while(![System.IO.File]::Exists($StopFilePath1 )){
            Sleep -Seconds 10
} 
###############################################################################################################################################
#$SourceFolderPath
#$TargetFolderPath
#
#Sync-T2TPublicFolderHierarchy  -SourceMailbox $SourceMailbox -SourceCredentials $SoruceCreds -TargetMailbox $TargetMailbox -TargetCredentials $TargetCreds  -mappingfile $MappingFile -ProcessingPath c:\temp -SourcePublicFolderPath $SourceFolderPath -TargetPublicFolderPath $TargetfolderPath -DomainMappings $DomainMappings
#
#Copy-T2TPublicFolders -SourceMailbox $SourceMailbox -SourceCredentials $SoruceCreds -TargetMailbox $TargetMailbox -TargetCredentials $TargetCreds  -mappingfile $MappingFile -ProcessingPath c:\temp -Delta:$true -SourcePublicFolderPath $SourceFolderPath -TargetPublicFolderPath $Targetfolderpath 
########################################## Part 2 ############################################################################################
$TestSession.EnumeratePublicFolders()
#Validate Add Tests

#Validate and Mail Enable Folder

$ValidationTestmeAdd = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddfolder" -TestOutput $TestmeAdd.OutputObject
if($ValidationTestmeAdd.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Add Mail Folder succeeded-1") -ForegroundColor Green   
    ("Validate Add Mail Folder succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Add Mail Folder failed") -ForegroundColor Red
      foreach ($err in $ValidationTestmeAdd.Errors)
    {
        write-host $err
    }   
}
#Validate folder Tree
$ValidationTestmeAddFT = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddfoldertree" -TestOutput $TestmeAddFT.OutputObject
if($ValidationTestmeAddFT.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Add Mail FolderTree succeeded") -ForegroundColor Green   
    ("Validate Add Mail FolderTree succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Add Mail FolderTree failed") -ForegroundColor Red
     foreach ($err in $ValidationTestmeAddFT.Errors)
    {
        write-host $err
    }   
}
#Mail Enable the Folder


$TestME = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "mailenablefolder" -SourceFolder ($SourceFolderPath + "\" + $AddFolderTestNameMailEnabled) 
if($TestME.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Mail Enable Folder Test succeeded") -ForegroundColor Green   
    ("Mail Enable Folder Test succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Mail Enable Folder Test failed") -ForegroundColor Red   
    foreach ($err in $TestME.Errors)
    {
        write-host $err
    }   
}

#Add Send As Permissions and Proxy Address



$Testsa = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addsendaspermissionpublicfolder" -SourceFolder ($SourceFolderPath + "\" + $AddFolderTestNameMailEnabled)  -Username $SendAsSourceUserName -TargetUserName $TargetAsSourceUserName
if($Testsa.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add SendAS Permission Test succeeded") -ForegroundColor Green   
    ("Add SendAS Permission Test succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add SendAS Permission Test failed") -ForegroundColor Red   
     foreach ($err in $Testsa.Errors)
    {
        write-host $err
    }      
}



$Testap = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addproxyaddress" -SourceFolder ($SourceFolderPath + "\" + $AddFolderTestNameMailEnabled) -SourceProxyAddress $SourceProxyAddress -TargetProxyAddress $TargetProxyAddress
if($Testap.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Proxy Address Test succeeded") -ForegroundColor Green   
    ("Add Proxy Address Test succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add Proxy Address Test failed") -ForegroundColor Red   
      foreach ($err in $Testap.Errors)
    {
        write-host $err
    }      
}

# End  Send As Permissions and Proxy Address

$Validation3 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddfolder" -TestOutput $Test3.OutputObject
if($Validation3.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Add Mail Folder succeeded-2") -ForegroundColor Green   
    ("Validate Add Mail Folder succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Add Mail Folder failed") -ForegroundColor Red
      foreach ($err in $Validation3.Errors)
    {
        write-host $err
    }      
}
$Validation4 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddfolder" -TestOutput $Test4.OutputObject
if($Validation4.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Add Contacts Folder succeeded") -ForegroundColor Green   
   ("Validate Add Contacts Folder succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Add Contacts Folder failed") -ForegroundColor Red   
    foreach ($err in $Validation4.Errors)
    {
        write-host $err
    }        
}
$Validation5 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddfolder" -TestOutput $Test5.OutputObject
if($Validation5.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Add Calendar Folder succeeded") -ForegroundColor Green   
    ("Validate Add Calendar Folder succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Add Calendar Folder failed") -ForegroundColor Red   
       foreach ($err in $Validation5.Errors)
    {
        write-host $err
    }      
}
$Validation6 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddfolder" -TestOutput $Test6.OutputObject
if($Validation6.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Add Mail Folder succeeded-3") -ForegroundColor Green   
    ("Validate Add Mail Folder succeeded")| Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Add Mail Folder failed") -ForegroundColor Red   
      foreach ($err in $Validation6.Errors)
    {
        write-host $err
    }       
}
$Validation7 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "ValidateAddMailItem" -TestOutput $Test7.OutputObject
if($Validation7.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Add Item succeeded-1") -ForegroundColor Green   
    ("Validate Add Item succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Add Item failed") -ForegroundColor Red   
      foreach ($err in $Validation7.Errors)
    {
        write-host $err
    }      
}
$Validation8 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "ValidateAddMailItem" -TestOutput $Test8.OutputObject
if($Validation8.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Add Contact succeeded") -ForegroundColor Green   
    ("Validate Add Contact succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Add Contact failed") -ForegroundColor Red   
      foreach ($err in $Validation8.Errors)
    {
        write-host $err
    }        
}
$Validation9 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "ValidateAddMailItem" -TestOutput $Test9.OutputObject
if($Validation9.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Add Meeting succeeded") -ForegroundColor Green   
    ("Validate Add Meeting succeeded")| Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Add Meeting failed") -ForegroundColor Red   
      foreach ($err in $Validation9.Errors)
    {
        write-host $err
    }      
}
$Validation10 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "ValidateAddMailItem" -TestOutput $Test10.OutputObject
if($Validation10.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Custom Item succeeded") -ForegroundColor Green   
    ("Validate Custom Item succeeded")| Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Custom Item failed") -ForegroundColor Red   
      foreach ($err in $Validation10.Errors)
    {
        write-host $err
    }       
}
$Validation11 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatesmtptransformaddress" -TestOutput $Test11.OutputObject
if($Validation11.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate SMTP Transform succeeded") -ForegroundColor Green   
    ("Validate SMTP Transform succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate SMTP Transform failed") -ForegroundColor Red   
     foreach ($err in $Validation11.Errors)
    {
        write-host $err
    }       
}
$Validation12 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateextransformaddress" -TestOutput $Test12.OutputObject
if($Validation12.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate x400 Transform succeeded") -ForegroundColor Green   
     ("Validate x400 Transform succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate x400 Transform failed") -ForegroundColor Red   
      foreach ($err in $Validation12.Errors)
    {
        write-host $err
    }     
}
$Validation13 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "ValidateAddMailItem" -TestOutput $Test13.OutputObject
if($Validation13.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Add Item succeeded-2") -ForegroundColor Green   
     ("Validate Add Item succeeded")  | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Add Item failed") -ForegroundColor Red   
      foreach ($err in $Validation13.Errors)
    {
        write-host $err
    }      
}
$Validation14 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatetransformdistributionlist" -TestOutput $Test14.OutputObject
if($Validation14.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("move Test succeeded") -ForegroundColor Green   
    ("Validate PDL Test succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate PDL Test failed") -ForegroundColor Red   
      foreach ($err in $Validation14.Errors)
    {
        write-host $err
    }      
}

#Modification Tests
#Modify Folder Permissions
$Test15 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "modifydefaultfolderpermission" -SourceFolder ($SourceFolderPath + "\" + $AddFolderTestName) -Permission Editor
if($Test15.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Modify Folder Permissions Test succeeded") -ForegroundColor Green  
    ("Modify Folder Permissions Test succeeded")| Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Modify Folder Permissions Test failed") -ForegroundColor Red   
     foreach ($err in $Test15.Errors)
    {
        write-host $err
    }     
}

#Add Attachment and do binary comparision


$Test16 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addattachment" -SourceFolder $SourceFolderPath -ItemSubject $Validation13.ItemCheckedSubject -Attachment $AttachmentPath
if($Test16.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Attachment Test succeeded") -ForegroundColor Green  
    ("Add Attachment Test succeeded")| Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Add Attachment Test failed") -ForegroundColor Red   
    foreach ($err in $Test16.Errors)
    {
        write-host $err
    }     
}
#Modify Mail Item Subject


$Test17 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "modifyitemsubject" -SourceFolder $SourceFolderPath  -ItemSubject $Validation7.ItemCheckedSubject -NewItemSubject $ModifiedItemSubject
if($Test17.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Modify Item Test succeeded") -ForegroundColor Green  
    ("Modify Item Test succeeded")| Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Modify Item Test  Test failed") -ForegroundColor Red   
   foreach ($err in $Test17.Errors)
    {
        write-host $err
    }     
}

$Test18 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "modifycontactemail" -SourceFolder $SourceFolderPath  -ItemSubject $Validation8.ItemCheckedSubject -ContactEmailAddress 'NewAddress@fakeDomain.com'
if($Test18.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Modify Contact Item Test succeeded") -ForegroundColor Green  
    ("Modify Contact Item Test succeeded")| Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Modify Contact Item Test failed") -ForegroundColor Red   
    foreach ($err in $Test18.Errors)
    {
        write-host $err
    }     
}

#Modify StickNote
$Test19 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "modifyitemsubject" -SourceFolder $SourceFolderPath  -ItemSubject $Validation10.ItemCheckedSubject 
if($Test19.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Modify Sticky Note succeeded") -ForegroundColor Green  
    ("Modify Sticky Note succeeded")| Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Modify Sticky Note failed") -ForegroundColor Red   
   foreach ($err in $Test19.Errors)
    {
        write-host $err
    }     
}

#Rename Folder

$Test20 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "renamesourcefolder" -SourceFolder ($SourceFolderPath + "\" + $FolderTestNameR) -FolderName  $FolderTestNamer2
if($Test20.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Rename Folder succeeded") -ForegroundColor Green  
    ("Rename Folder succeeded")| Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Rename Folder failed") -ForegroundColor Red   
   foreach ($err in $Test20.Errors)
    {
        write-host $err
    }      
}

#Move Folder to Drafts
$Test21 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "movesourcefolder" -SourceFolder ($SourceFolderPath + "\" + $FolderTestName3) -DestinationFolderPath ($SourceFolderPath + "\" + $AddFolderTestName)   
if($Test21.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Move Folder succeeded") -ForegroundColor Green  
    ("Move Folder succeeded")| Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Move Folder failed") -ForegroundColor Red   
   foreach ($err in $Test21.Errors)
    {
        write-host $err
    }     
}

#Delete Folder

$Test22 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "deletesourcefolder" -SourceFolder ($SourceFolderPath + "\" + $FolderTestName2)   
if($Test22.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Delete Folder succeeded") -ForegroundColor Green  
    ("Delete Folder succeeded")| Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Delete Folder failed") -ForegroundColor Red   
   foreach ($err in $Test22.Errors)
    {
        write-host $err
    }        
}

#MoveItem

$Test23 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "moveitem" -SourceFolder $SourceFolderPath -ItemSubject $ItemAddSubjectm -TargetFolder ($SourceFolderPath + "\" + $AddFolderTestName)    
if($Test23.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Move Item succeeded") -ForegroundColor Green  
    ("Move Item succeeded")| Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Move Item failed") -ForegroundColor Red   
   foreach ($err in $Test23.Errors)
    {
        write-host $err
    }     
}

#Delete item

$Test24 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "deleteitem" -SourceFolder $SourceFolderPath -ItemSubject $ItemAddSubjectd  
if($Test24.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Delete Item succeeded") -ForegroundColor Green  
    ("Delete Item succeeded")| Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Delete Item failed") -ForegroundColor Red   
   foreach ($err in $Test24.Errors)
    {
        write-host $err
    }      
}
###############################################################################################################################################
Write-Host ("Powershell will pause until Migration is complete - 2") -ForegroundColor Green
while(![System.IO.File]::Exists($StopFilePath2 )){
            Sleep -Seconds 10
} 
###############################################################################################################################################
#
#Sync-T2TPublicFolderHierarchy  -SourceMailbox $SourceMailbox -SourceCredentials $SoruceCreds -TargetMailbox $TargetMailbox -TargetCredentials $TargetCreds  -mappingfile $MappingFile -ProcessingPath c:\temp -SourcePublicFolderPath $SourceFolderPath -TargetPublicFolderPath $TargetfolderPath -DomainMappings $DomainMappings
#
#Copy-T2TPublicFolders -SourceMailbox $SourceMailbox -SourceCredentials $SoruceCreds -TargetMailbox $TargetMailbox -TargetCredentials $TargetCreds  -mappingfile $MappingFile -ProcessingPath c:\temp -Delta:$true -SourcePublicFolderPath $SourceFolderPath -TargetPublicFolderPath $Targetfolderpath 
########################################## Part 2 ############################################################################################
########################################## Part 3 ############################################################################################


$TestSession.EnumeratePublicFolders()

#Run Validations

$Validation15 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatemodifydefaultfolderpermission" -TestOutput $Test15.OutputObject
if($Validation15.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Modify Folder Permissions succeeded") -ForegroundColor Green   
    ("Validate Modify Folder Permissions succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Modify Folder Permissions failed") -ForegroundColor Red   
   foreach ($err in $Validation15.Errors)
    {
        write-host $err
    }       
}

$Validation16 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddattachment" -TestOutput $Test16.OutputObject
if($Validation16.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Add Attachment succeeded") -ForegroundColor Green   
    ("Validate Add Attachment succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Add Attachment Permissions failed") -ForegroundColor Red   
     foreach ($err in $Validation16.Errors)
    {
        write-host $err
    }      
}

$Validation17 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatemodifyitemsubject" -TestOutput $Test17.OutputObject
if($Validation17.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Modify Item Subject succeeded") -ForegroundColor Green   
    ("Validate Modify Item Subject succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Modify Item Subject failed") -ForegroundColor Red   
    foreach ($err in $Validation17.Errors)
    {
        write-host $err
    }        
}

$Validation18 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatemodifycontactemail" -TestOutput $Test18.OutputObject
if($Validation18.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Modify Item contact succeeded") -ForegroundColor Green   
    ("Validate Modify Item contact succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Modify Item contact failed") -ForegroundColor Red   
    foreach ($err in $Validation18.Errors)
    {
        write-host $err
    }      
}

$Validation19 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatemodifyitemsubject" -TestOutput $Test19.OutputObject
if($Validation19.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Modify StickyNote succeeded") -ForegroundColor Green   
    ("Validate Modify StickyNote succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Modify StickyNote failed") -ForegroundColor Red   
     foreach ($err in $Validation19.Errors)
    {
        write-host $err
    }      
}

$Validation20 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validaterenamesourcefolder" -TestOutput $Test20.OutputObject
if($Validation20.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate rename folder succeeded") -ForegroundColor Green   
    ("Validate rename folder succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate rename folder failed") -ForegroundColor Red   
    foreach ($err in $Validation20.Errors)
    {
        write-host $err
    }         
}

$Validation21 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatemovesourcefolder" -TestOutput $Test21.OutputObject
if($Validation21.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate move folder succeeded") -ForegroundColor Green   
    ("Validate move folder succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate move folder failed") -ForegroundColor Red   
     foreach ($err in $Validation21.Errors)
    {
        write-host $err
    }      
}

$Validation22 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatedeletesourcefolder" -TestOutput $Test22.OutputObject
if($Validation22.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate delete folder succeeded") -ForegroundColor Green   
    ("Validate delete folder succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate delete folder failed") -ForegroundColor Red   
     foreach ($err in $Validation22.Errors)
    {
        write-host $err
    }        
}

$Validation23 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatemoveitem" -TestOutput $Test23.OutputObject
if($Validation23.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Move Item succeeded") -ForegroundColor Green   
    ("Move Item  succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Move Item  failed") -ForegroundColor Red   
    foreach ($err in $Validation23.Errors)
    {
        write-host $err
    }      
}

$Validation24 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatedeleteitem" -TestOutput $Test24.OutputObject
if($Validation24.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Delete Item succeeded") -ForegroundColor Green   
    ("Delete Item Succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Delete Item  failed") -ForegroundColor Red   
     foreach ($err in $Validation24.Errors)
    {
        write-host $err
    }         
}
#Validate Mail Enabled
$ValidationME = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatemailenablefolder" -TestOutput $TestME.OutputObject
if($ValidationME.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Mail Enable Folder succeeded") -ForegroundColor Green   
     ("Validate Mail Enable Folder succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Mail Enable Folder failed") -ForegroundColor Red   
    foreach ($err in $ValidationME.Errors)
    {
        write-host $err
    }       
}
#Validate SendAS

$Validationsa = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddsendaspermissionpublicfolder" -TestOutput $Testsa.OutputObject
if($Validationsa.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("validate Add SendAS Permission succeeded") -ForegroundColor Green   
     ("validate Add SendAS Permission succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("validate Add SendAS Permission validation failed") -ForegroundColor Red   
     foreach ($err in $Validationsa.Errors)
    {
        write-host $err
    }        
}

$Validationap = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddproxyaddress" -TestOutput $Testap.OutputObject
if($Validationap.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Add Proxy Address validation succeeded") -ForegroundColor Green   
     ("Validate Add Proxy Address validation succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Add Proxy Address validation failed") -ForegroundColor Red   
    foreach ($err in $Validationap.Errors)
    {
        write-host $err
    }         
}
