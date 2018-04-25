param([string]$slogin = "BinaryTreePowerShellUser.BT-AutoQA2@btcorp7.onmicrosoft.com", [string]$spassword = "P@ssw0rd", [string]$tlogin = "BinaryTreePowerShellUser.BT-AutoQA1@btcorp9.onmicrosoft.com", [string]$tpassword = "P@ssw0rd", [string]$smailbox = "C7-Automation1@btcorp7.onmicrosoft.com", [string]$tmailbox = "C7-Automation1@btcorp9.onmicrosoft.com", [string]$StopFilePath1="C:\PFTests\MoveAndModifyStop1.txt", [string]$StopFilePath2="C:\PFTests\MoveAndModifyStop2.txt")
#Line5 - Please change the directory of btT2TestUtilPSModule.psd1 so that it matches your environment.
#Line6 - Please change the directory of btT2TPSModule.psd1 so that it matches your environment.
#Line199 - Please change the directory of the attachment and the attachment name so that it matches your environment.
#Line216 - Please change the amount of time to pause the powershell scripts while running migraion through UI.
import-module ($PSScriptRoot + "\PSTools\cmdtest\btT2TPSModule.psd1") -Force
import-module ($PSScriptRoot + "\PSTools\testutils\btT2TestUtilPSModule.psd1") -Force
$sourceUserName = $slogin
$sourcepasswd = ConvertTo-SecureString $spassword -AsPlainText -Force
$TargetUserName = $tlogin
$Targetpasswd = ConvertTo-SecureString $tpassword -AsPlainText -Force   

$SourceMailbox = $smailbox
$TargetMailbox = $tmailbox  

$LogPath = "C:\T2T_Logs"
$TestLogFile = $LogPath + "\" + "OverAllLog" + (Get-Date).ToString("yyyyMMddHHmm") + ".log"

$SoruceCreds = New-Object System.Management.Automation.PSCredential ($sourceUserName, $sourcepasswd)
$TargetCreds= New-Object System.Management.Automation.PSCredential ($TargetUserName, $Targetpasswd)

#Start the TestSession

$TestSession = Start-T2tMigrationTests -SourceMailbox $SourceMailbox -SourceCredentials $SoruceCreds -TargetMailbox $TargetMailbox -TargetCredentials $TargetCreds -LogFilePath $LogPath -TestArchive:$false 


#Add Folder to Test Folder Move and Rename

$FolderTestNameM = "1NewFolderToMove-" + (Get-Date).ToString("yyyyMMddHHmm");
$Test6M = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addfolder" -ParentSourceFolder "\\Inbox" -FolderName $FolderTestNameM -FolderClass "IPF.Note"
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
    ("Add Folder to Test Folder Move failed") | Out-File -Append -FilePath $TestLogFile       
}

$FolderTestNameR = "1NewFolderToRename-" + (Get-Date).ToString("yyyyMMddHHmm");
$Test6R = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addfolder" -ParentSourceFolder "\\Inbox" -FolderName $FolderTestNameR -FolderClass "IPF.Note"
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
    ("Add Folder to Test Folder Rename failed") | Out-File -Append -FilePath $TestLogFile       
}

#Add New Folder to test folder permission change
$FolderTestNameP= "Folder to test permission change-" + (Get-Date).ToString("yyyyMMddHHmm");
$Test6P = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addfolder" -ParentSourceFolder "\\Inbox" -FolderName $FolderTestNameP -FolderClass "IPF.Note"
if($Test6P.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Folder to test permission change succeeded") -ForegroundColor Green   
   ("Add Folder to test permission change succeeded")   | Out-File -Append -FilePath $TestLogFile  
}
else
{
    Write-Host ("Add Folder to test permission change failed") -ForegroundColor Red 
     foreach ($err in $Test6P.Errors)
    {
        write-host $err
    }      
    ("Add Folder to test permission change failed")  | Out-File -Append -FilePath $TestLogFile    
}

#Add message to test subject change

$ItemAddSubject = "Yoko's Message to test modifying subject " + (Get-Date).ToString("yyyyMMddHHmm");

$Test7 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addmailitem" -SourceFolder "\\Inbox" -ItemSubject $ItemAddSubject
if($Test7.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add message succeeded") -ForegroundColor Green  
    ("Add message succeeded") | Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Add messge failed") -ForegroundColor Red
     foreach ($err in $Test7.Errors)
    {
        write-host $err
    }      
    ("Add message failed")| Out-File -Append -FilePath $TestLogFile       
}

#Add contact to test modify email address
$ContactAddSubject = "Contact to test email address change " + (Get-Date).ToString("yyyyMMddHHmm");

$Test8 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addcontactitem" -SourceFolder "\\Contacts" -ContactName $ContactAddSubject -ContactEmailAddress 'Address@Fake.com'
if($Test8.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Contact to test email address change succeeded") -ForegroundColor Green   
    ("Add Contact to test email address change succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add Contact to test email address change failed") -ForegroundColor Red 
     foreach ($err in $Test8.Errors)
    {
        write-host $err
    }        
    ("Add Contact to test email address change failed") | Out-File -Append -FilePath $TestLogFile    
}

#Add Sticky Note to test modify subject

$AddStickyNoteSubject = "Sticky Note Add " + (Get-Date).ToString("yyyyMMddHHmm");
$Test10 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addcustomitem" -SourceFolder "\\Notes" -ItemSubject $AddStickyNoteSubject -ItemClass "IPM.StickyNote"
if($Test10.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Sticky Note succeeded") -ForegroundColor Green   
    ("Add Sticky Note succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add Sticky Note failed") -ForegroundColor Red   
     foreach ($err in $Test10.Errors)
    {
        write-host $err
    }      
    ("Add Sticky Note failed") | Out-File -Append -FilePath $TestLogFile    
}

#Add task to test subject change

$AddTaskSubject = "Task to test subject change " + (Get-Date).ToString("yyyyMMddHHmm");
$Test10T = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addcustomitem" -SourceFolder "\\Tasks" -ItemSubject $AddTaskSubject -ItemClass "IPM.Task"
if($Test10T.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Task succeeded") -ForegroundColor Green   
    ("Add Task succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add Task failed") -ForegroundColor Red  
     foreach ($err in $Test10T.Errors)
    {
        write-host $err
    }       
    ("Add Task failed") | Out-File -Append -FilePath $TestLogFile    
}

#Add Journal to test subject change

$AddJournalSubject = "Journal to test subject change " + (Get-Date).ToString("yyyyMMddHHmm");
$Test10J = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addcustomitem" -SourceFolder "\\Journal" -ItemSubject $AddJournalSubject -ItemClass "IPM.Activity"
if($Test10J.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Journal succeeded") -ForegroundColor Green   
    ("Add Journal succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add Journal failed") -ForegroundColor Red  
     foreach ($err in $Test10J.Errors)
    {
        write-host $err
    }       
    ("Add Journal failed") | Out-File -Append -FilePath $TestLogFile    
}

#Add message to test move to a different folder

$ItemAddSubjectMove = "Yoko's Message to test move to a different folder " + (Get-Date).ToString("yyyyMMddHHmm");

$Test7Move = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addmailitem" -SourceFolder "\\Inbox" -ItemSubject $ItemAddSubjectMove
if($Test7Move.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add message to test move succeeded") -ForegroundColor Green  
    ("Add message to test move succeeded") | Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Add messge to test move failed") -ForegroundColor Red
     foreach ($err in $Test7Move.Errors)
    {
        write-host $err
    }      
    ("Add message to test move failed")| Out-File -Append -FilePath $TestLogFile       
}

#Pause Powershell session for 15 minutes while running migration through UI

#Write-Host ("Powershell will pause 15 minutes while running migration through UI") -ForegroundColor Green
#start-sleep -Seconds 1080
#start-sleep -Seconds 30
#Run Delta Migration

###############################################################################################################################################
Write-Host ("Powershell will pause until Migration is complete - 1") -ForegroundColor Green
while(![System.IO.File]::Exists($StopFilePath1)){
            Sleep -Seconds 10
} 
###############################################################################################################################################

#Copy-T2TMailbox -SourceMailbox $SourceMailbox -SourceCredentials $SoruceCreds -TargetMailbox $TargetMailbox -TargetCredentials $TargetCreds  -mappingfile c:\temp\mappingC7toC9.csv -ProcessingPath c:\temp -Delta:$true
$TestSession.EnumberateFolders()

#Run Validation

$Validation6M = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddfolder" -TestOutput $Test6M.OutputObject
if($Validation6M.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Add Mail Folder succeeded") -ForegroundColor Green   
    ("Validate Add Mail Folder succeeded")| Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Add Mail Folder failed") -ForegroundColor Red  
      foreach ($err in $Validation6M.Errors)
    {
        write-host $err
    }      
    ("Validate Add Mail Folder failed")| Out-File -Append -FilePath $TestLogFile    
}

$Validation6R = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddfolder" -TestOutput $Test6R.OutputObject
if($Validation6R.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Add Mail Folder succeeded") -ForegroundColor Green   
    ("Validate Add Mail Folder succeeded")| Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Add Mail Folder failed") -ForegroundColor Red  
       foreach ($err in $Validation6R.Errors)
    {
        write-host $err
    }     
    ("Validate Add Mail Folder failed")| Out-File -Append -FilePath $TestLogFile    
}

$Validation6P = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddfolder" -TestOutput $Test6P.OutputObject
if($Validation6P.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Add Mail Folder succeeded") -ForegroundColor Green   
    ("Validate Add Mail Folder succeeded")| Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Add Mail Folder failed") -ForegroundColor Red  
       foreach ($err in $Validation6P.Errors)
    {
        write-host $err
    }     
    ("Validate Add Mail Folder failed")| Out-File -Append -FilePath $TestLogFile    
}

#Validate message migration to test subject modification

$Validation7 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "ValidateAddMailItem" -TestOutput $Test7.OutputObject
if($Validation7.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate message migration succeeded") -ForegroundColor Green   
    ("Validate message migration succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate message migration failed") -ForegroundColor Red  
       foreach ($err in $Validation7.Errors)
    {
        write-host $err
    }     
    ("Validate message migration failed")| Out-File -Append -FilePath $TestLogFile    
}

#Validate contact migration to test email address modification

$Validation8 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "ValidateAddMailItem" -TestOutput $Test8.OutputObject
if($Validation8.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Contact migration succeeded") -ForegroundColor Green   
    ("Validate Contact migration succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Contact migration failed") -ForegroundColor Red 
       foreach ($err in $Validation8.Errors)
    {
        write-host $err
    }      
    ("Validate Contact migration failed")| Out-File -Append -FilePath $TestLogFile    
}

#Validate sticky note migration to test subject change

$Validation10 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "ValidateAddMailItem" -TestOutput $Test10.OutputObject
if($Validation10.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Sticky Note migration succeeded") -ForegroundColor Green   
    ("Validate Sticky Note migration succeeded")| Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Sticky Note migration failed") -ForegroundColor Red  
       foreach ($err in $Validation10.Errors)
    {
        write-host $err
    }     
    ("Validate Sticky Note migration failed")| Out-File -Append -FilePath $TestLogFile    
}

#Validate Task migration to test subject change

$Validation10T = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "ValidateAddMailItem" -TestOutput $Test10T.OutputObject
if($Validation10T.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Task migration succeeded") -ForegroundColor Green   
    ("Validate Task migration succeeded")| Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Task migration failed") -ForegroundColor Red  
       foreach ($err in $Validation10T.Errors)
    {
        write-host $err
    }     
    ("Validate Task migration failed")| Out-File -Append -FilePath $TestLogFile    
}

#Validate Journal migration to test subject change

$Validation10J = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "ValidateAddMailItem" -TestOutput $Test10J.OutputObject
if($Validation10J.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Journal migration succeeded") -ForegroundColor Green   
    ("Validate Journal migration succeeded")| Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Journal migration failed") -ForegroundColor Red 
       foreach ($err in $Validation10J.Errors)
    {
        write-host $err
    }      
    ("Validate Journal migration failed")| Out-File -Append -FilePath $TestLogFile    
}

#Validate message migration to test move

$Validation7Move = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "ValidateAddMailItem" -TestOutput $Test7Move.OutputObject
if($Validation7Move.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate message migration succeeded") -ForegroundColor Green   
    ("Validate message migration succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate message migration failed") -ForegroundColor Red  
       foreach ($err in $Validation7Move.Errors)
    {
        write-host $err
    }     
    ("Validate message migration failed")| Out-File -Append -FilePath $TestLogFile    
}


#Modify Folders and Items

#Rename Folder
$FolderTestNameR2 = "Renamed 1NewFolder-" + (Get-Date).ToString("yyyyMMddHHmmss");
$Test20 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "renamesourcefolder" -SourceFolder ("\\Inbox\" + $FolderTestNameR) -FolderName  $FolderTestNameR2
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
    ("Rename Folder failed") | Out-File -Append -FilePath $TestLogFile    
}

#Move Folder to Drafts
$Test21 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "movesourcefolder" -SourceFolder ("\\Inbox\" + $FolderTestNameM) -DestinationFolderPath "\\Drafts"  
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
    ("Move Folder failed") | Out-File -Append -FilePath $TestLogFile    
}

$Test15 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "modifydefaultfolderpermission" -SourceFolder ("\\Inbox\" +$FolderTestNameP ) -Permission Editor
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
    ("Modify Folder Permissions  Test failed") | Out-File -Append -FilePath $TestLogFile    
}

#Modify Mail Item Subject

$Test17 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "modifyitemsubject" -SourceFolder "\\Inbox" -ItemSubject $Validation7.ItemCheckedSubject 
if($Test17.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Modify mail subject succeeded") -ForegroundColor Green  
    ("Modify mail subject succeeded")| Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Modify mail subject failed") -ForegroundColor Red   
      foreach ($err in $Test17.Errors)
    {
        write-host $err
    }   
    ("Modify mail subject failed") | Out-File -Append -FilePath $TestLogFile    
}

#Modify contact email address

$Test18 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "modifycontactemail" -SourceFolder "\\Contacts" -ItemSubject $Validation8.ItemCheckedSubject -ContactEmailAddress 'NewAddress@fakeDomain.com'
if($Test18.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Modify Contact email address Test succeeded") -ForegroundColor Green  
    ("Modify Contact email address Test succeeded")| Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Modify Contact email address Test failed") -ForegroundColor Red  
      foreach ($err in $Test18.Errors)
    {
        write-host $err
    }    
    ("Modify Contact email address failed") | Out-File -Append -FilePath $TestLogFile    
}

#Modify StickNote
$Test19 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "modifyitemsubject" -SourceFolder "\\Notes" -ItemSubject $Validation10.ItemCheckedSubject 
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
    ("Modify Sticky Notefailed") | Out-File -Append -FilePath $TestLogFile    
}

#Modify Task
$Test19T = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "modifyitemsubject" -SourceFolder "\\Tasks" -ItemSubject $Validation10T.ItemCheckedSubject 
if($Test19T.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Modify Task succeeded") -ForegroundColor Green  
    ("Modify Task succeeded")| Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Modify Task failed") -ForegroundColor Red   
      foreach ($err in $Test19T.Errors)
    {
        write-host $err
    }   
    ("Modify Task failed") | Out-File -Append -FilePath $TestLogFile    
}

#Modify Journal
$Test19J = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "modifyitemsubject" -SourceFolder "\\Journal" -ItemSubject $Validation10J.ItemCheckedSubject 
if($Test19J.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Modify Journal succeeded") -ForegroundColor Green  
    ("Modify Journal succeeded")| Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Modify Journal failed") -ForegroundColor Red
      foreach ($err in $Test19J.Errors)
    {
        write-host $err
    }      
    ("Modify Journal failed") | Out-File -Append -FilePath $TestLogFile    
}

#Move message to Draft folder
$Test23 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "moveitem" -SourceFolder "\\Inbox" -ItemSubject $ItemAddSubjectMove -TargetFolder "\\Drafts"   
if($Test23.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Move Message succeeded") -ForegroundColor Green  
    ("Move Message succeeded")| Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Move Message failed") -ForegroundColor Red  
      foreach ($err in $Test23.Errors)
    {
        write-host $err
    }    
    ("Move Message failed") | Out-File -Append -FilePath $TestLogFile    
}

#Run Delta Migration
#Pause Powershell session for 15 minutes while running migration through UI

#Write-Host ("Powershell will pause 15 minutes while running migration through UI") -ForegroundColor Green
#start-sleep -Seconds 1080
#start-sleep -Seconds 30

###############################################################################################################################################
Write-Host ("Powershell will pause until Migration is complete - 2") -ForegroundColor Green
while(![System.IO.File]::Exists($StopFilePath2)){
            Sleep -Seconds 10
} 
###############################################################################################################################################

#Copy-T2TMailbox -SourceMailbox $SourceMailbox -SourceCredentials $SoruceCreds -TargetMailbox $TargetMailbox -TargetCredentials $TargetCreds  -mappingfile c:\temp\mappingC7toC9.csv -ProcessingPath c:\temp -Delta:$true
$TestSession.EnumberateFolders()


#Validate Folder Rename
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
    ("Validate rename folder failed") | Out-File -Append -FilePath $TestLogFile    
}

#Validate Folder Move

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
    ("Validate move folder failed") | Out-File -Append -FilePath $TestLogFile    
}

#Validate Folder Permission change

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
    ("Validate Modify Folder Permissions failed") | Out-File -Append -FilePath $TestLogFile    
}

#Validate message subject change

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
    ("Validate Modify Item Subject failed") | Out-File -Append -FilePath $TestLogFile    
}

#Validate contact email address change

$Validation18 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatemodifycontactemail" -TestOutput $Test18.OutputObject
if($Validation18.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Modify contact email address succeeded") -ForegroundColor Green   
    ("Validate Modify contact email address succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Modify contact email address failed") -ForegroundColor Red 
     foreach ($err in $Validation18.Errors)
    {
        write-host $err
    }     
    ("Validate Modify contact email address failed") | Out-File -Append -FilePath $TestLogFile    
}

#Validate modify sticky Note

$Validation19 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatemodifyitemsubject" -TestOutput $Test19.OutputObject
if($Validation19.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Modify Sticky Note succeeded") -ForegroundColor Green   
    ("Validate Modify Sticky Note succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Modify Sticky Note failed") -ForegroundColor Red  
     foreach ($err in $Validation19.Errors)
    {
        write-host $err
    }    
    ("Validate Modify StickyNote failed") | Out-File -Append -FilePath $TestLogFile    
}

#Validate Modify Task

$Validation19T = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatemodifyitemsubject" -TestOutput $Test19T.OutputObject
if($Validation19T.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Modify Task succeeded") -ForegroundColor Green   
    ("Validate Modify Task succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Modify Task failed") -ForegroundColor Red 
     foreach ($err in $Validation19T.Errors)
    {
        write-host $err
    }     
    ("Validate Modify Task failed") | Out-File -Append -FilePath $TestLogFile    
}

#Validate Modify Journal

$Validation19J = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatemodifyitemsubject" -TestOutput $Test19J.OutputObject
if($Validation19J.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Modify Journal succeeded") -ForegroundColor Green   
    ("Validate Modify Journal succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Modify Journal failed") -ForegroundColor Red 
     foreach ($err in $Validation19J.Errors)
    {
        write-host $err
    }     
    ("Validate Modify Journal failed") | Out-File -Append -FilePath $TestLogFile    
}

#Validate Message Move

$Validation23 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatemoveitem" -TestOutput $Test23.OutputObject
if($Validation23.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Move Message succeeded") -ForegroundColor Green   
    ("Validate Move Message  succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Move Message  failed") -ForegroundColor Red 
     foreach ($err in $Validation23.Errors)
    {
        write-host $err
    }     
    ("Validate Move Message failed") | Out-File -Append -FilePath $TestLogFile    
}
