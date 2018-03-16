param([string]$slogin = "BinaryTreePowerShellUser.BT-AutoQA2@btcorp7.onmicrosoft.com", [string]$spassword = "P@ssw0rd", [string]$tlogin = "BinaryTreePowerShellUser.BT-AutoQA1@btcorp9.onmicrosoft.com", [string]$tpassword = "P@ssw0rd", [string]$smailbox = "C7-Automation1@btcorp7.onmicrosoft.com", [string]$tmailbox = "C7-Automation1@btcorp9.onmicrosoft.com", [string]$StopFilePath1="C:\PFTests\AddStop1.txt")
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


#Add Folders

#Add Mail Folder

$AddFolderTestName = "NewMailFolderAdd-" + (Get-Date).ToString("yyyyMMddHHmm");
$Test3 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addfolder" -ParentSourceFolder "\\Inbox" -FolderName $AddFolderTestName -FolderClass "IPF.Note"
if($Test3.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add New MailFolder Check succeeded") -ForegroundColor Green   
   ("Add New MailFolder Check succeeded")   | Out-File -Append -FilePath $TestLogFile  
}
else
{
    Write-Host ("Add New MailFolder Check failed") -ForegroundColor Red 
       foreach ($err in $Test3.Errors)
    {
        write-host $err
    }     
    ("Add New MailFolder Check failed")  | Out-File -Append -FilePath $TestLogFile    
}

#Add Contact Folder

$FolderTestName = "NewContactFolderAdd-" + (Get-Date).ToString("yyyyMMddHHmm");
$Test4 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addfolder" -ParentSourceFolder "\\Contacts" -FolderName $FolderTestName -FolderClass "IPF.Contact"
if($Test4.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add New Contacts folder succeeded") -ForegroundColor Green   
    ("Add New Contacts Check succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add New Contacts folder failed") -ForegroundColor Red
      foreach ($err in $Test4.Errors)
    {
        write-host $err
    }     
    ("Add New Contacts Check failed") | Out-File -Append -FilePath $TestLogFile       
}

#Add Calendar folder

$FolderTestName = "NewCalendarFolderAdd-" + (Get-Date).ToString("yyyyMMddHHmm");
$Test5 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addfolder" -ParentSourceFolder "\\Calendar" -FolderName $FolderTestName -FolderClass "IPF.Appointment"
if($Test5.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add New Calendar folder succeeded") -ForegroundColor Green   
    ("Add New Calendar folder succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add New Calendar folder failed") -ForegroundColor Red  
      foreach ($err in $Test5.Errors)
    {
        write-host $err
    }      
    ("Add New Calendar folder failed")| Out-File -Append -FilePath $TestLogFile    
}

#Add Items

#Add Mail message

$ItemAddSubject = "Test New Mail " + (Get-Date).ToString("yyyyMMddHHmm");
$Test7 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addmailitem" -SourceFolder "\\Inbox" -ItemSubject $ItemAddSubject
if($Test7.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add New Mail succeeded") -ForegroundColor Green  
    ("Add New Mail succeeded") | Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Add New Mail failed") -ForegroundColor Red
      foreach ($err in $Test7.Errors)
    {
        write-host $err
    }     
    ("Add New mail failed")| Out-File -Append -FilePath $TestLogFile       
}

#Add contact

$ContactAddSubject = "Contact New Item " + (Get-Date).ToString("yyyyMMddHHmm");
$Test8 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addcontactitem" -SourceFolder "\\Contacts" -ContactName $ContactAddSubject -ContactEmailAddress 'Address@Fake.com'
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
    ("Add Contact Test failed") | Out-File -Append -FilePath $TestLogFile    
}

#Add Appointment

$AppointmentAddSubject = "Test Meeting " + (Get-Date).ToString("yyyyMMddHHmm");
$Test9 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addappointmentitem" -SourceFolder "\\Calendar" -ItemSubject $AppointmentAddSubject -Start (Get-Date) -End (Get-Date).AddHours(1)
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
    ("Add Meeting Test failed") | Out-File -Append -FilePath $TestLogFile    
}

#Add Sticky Note

$CustomItemTest = "Sticky Note Add " + (Get-Date).ToString("yyyyMMddHHmm");
$Test10 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addcustomitem" -SourceFolder "\\Notes" -ItemSubject $CustomItemTest -ItemClass "IPM.StickyNote"
if($Test10.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Stiky Nnote Test succeeded") -ForegroundColor Green   
    ("Add Sticky Note Test succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add Sticky Note Test failed") -ForegroundColor Red  
      foreach ($err in $Test10.Errors)
    {
        write-host $err
    }      
    ("Add Sticky Note Test failed") | Out-File -Append -FilePath $TestLogFile    
}

#Add Personal Distribution List

$dlName = "New PDL Test " + (Get-Date).ToString("yyyyMMddHHmm");
$contactName = "New Contact "+ (Get-Date).ToString("yyyyMMddHHmm");

$Test14 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "personaldlcontactlink" -SourceFolder "\\Contacts" -ContactName $contactName -ContactEmailAddress 'Address@Fake.com' -DLName $dlName
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
    ("Add PDL Test failed") | Out-File -Append -FilePath $TestLogFile   
}

#Add Task

$TaskAddSubject = "Eugeny New Task " + (Get-Date).ToString("yyyyMMddHHmm");
$Test14T = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addcustomitem" -SourceFolder "\\Tasks" -ItemSubject $TaskAddSubject -ItemClass "IPM.Task"
if($Test14T.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Task Item Test succeeded") -ForegroundColor Green   
    ("Add Task Item Test succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add Task Item Test failed") -ForegroundColor Red  
      foreach ($err in $Test14T.Errors)
    {
        write-host $err
    }      
    ("Add Task Item Test failed") | Out-File -Append -FilePath $TestLogFile    
}

#Add Journal Item

$JournalItemTest = "Eugeny New Journal " + (Get-Date).ToString("yyyyMMddHHmm");
$Test14J = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addcustomitem" -SourceFolder "\\Journal" -ItemSubject $JournalItemTest -ItemClass "IPM.Activity"
if($Test14J.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Journal Item Test succeeded") -ForegroundColor Green   
    ("Add Journal Item Test succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add Journal Item Test failed") -ForegroundColor Red 
      foreach ($err in $Test14J.Errors)
    {
        write-host $err
    }       
    ("Add Journal Item Test failed") | Out-File -Append -FilePath $TestLogFile    
}

#Add New message for Attachment Test

$ItemAddSubject = "Eugeny's test message " + (Get-Date).ToString("yyyyMMddHHmm");
$Test13 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addmailitem" -SourceFolder "\\Inbox" -ItemSubject $ItemAddSubject
if($Test13.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Mail for Attachment succeeded") -ForegroundColor Green 
    ("Add Mail for Attachment succeeded") | Out-File -Append -FilePath $TestLogFile      
}
else
{
    Write-Host ("Add Mail for Attachment failed") -ForegroundColor Red
     foreach ($err in $Test13.Errors)
    {
        write-host $err
    }       
    ("Add Mail for Attachment failed") | Out-File -Append -FilePath $TestLogFile       
}

#Add Attachment to the message

$AttachmentPath = $PSScriptRoot + "\attach.jpg"
$Test16 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addattachment" -SourceFolder "\\Inbox" -ItemSubject $ItemAddSubject -Attachment $AttachmentPath
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
    ("Add Attachment  Test failed") | Out-File -Append -FilePath $TestLogFile    
}


#Pause Powershell session for 5 minutes while running migration through UI

#Write-Host ("Powershell will pause 15 minutes while running migration through UI") -ForegroundColor Green
#start-sleep -Seconds 1500
#start-sleep -Seconds 30

###############################################################################################################################################
Write-Host ("Powershell will pause until Migration is complete - 1") -ForegroundColor Green
while(![System.IO.File]::Exists($StopFilePath1)){
            Sleep -Seconds 10
} 
###############################################################################################################################################
#Run Migration Validation

#Validate Add Mail folder

$Validation3 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddfolder" -TestOutput $Test3.OutputObject
if($Validation3.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Add Mail Folder succeeded") -ForegroundColor Green   
    ("Validate Add Mail Folder succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Add Mail Folder failed") -ForegroundColor Red
      foreach ($err in $Validation3.Errors)
    {
        write-host $err
    }   
    ("Validate Add Mail Folder failed")| Out-File -Append -FilePath $TestLogFile       
}

#Validate Add Contact folder

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
   ("Validate Add Contacts Folder failed") | Out-File -Append -FilePath $TestLogFile    
}


#Validate Add Calendar folder

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
     ("Validate Add Calendar Folder failed") | Out-File -Append -FilePath $TestLogFile    
}

#Validate Add Mail Message

$Validation7 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "ValidateAddMailItem" -TestOutput $Test7.OutputObject
if($Validation7.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Add Mail Message succeeded") -ForegroundColor Green   
    ("Validate Add Mail Message succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Add Mail Message failed") -ForegroundColor Red  
       foreach ($err in $Validation7.Errors)
    {
        write-host $err
    }     
    ("Validate Add Mail Message failed")| Out-File -Append -FilePath $TestLogFile    
}

#Validate Add Contact

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
    ("Validate Add Contact failed")| Out-File -Append -FilePath $TestLogFile    
}

#Validate Add Meeting

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
    ("Validate Add Meeting failed")| Out-File -Append -FilePath $TestLogFile    
}

#Validate Add Sticky Note

$Validation10 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "ValidateAddMailItem" -TestOutput $Test10.OutputObject
if($Validation10.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Add Sticky Note succeeded") -ForegroundColor Green   
    ("Validate Add Sticky Note succeeded")| Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Add Sticky Note failed") -ForegroundColor Red 
       foreach ($err in $Validation10.Errors)
    {
        write-host $err
    }      
    ("Validate Add Sticky Note failed")| Out-File -Append -FilePath $TestLogFile    
}

#Validate Add Personal Distribution List

$Validation14 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatetransformdistributionlist" -TestOutput $Test14.OutputObject
if($Validation14.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate PDL Test succeeded") -ForegroundColor Green   
    ("Validate PDL Test succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate PDL Test failed") -ForegroundColor Red   
       foreach ($err in $Validation14.Errors)
    {
        write-host $err
    }    
    ("Validate PDL Test failed") | Out-File -Append -FilePath $TestLogFile    
}

#Validate Add Task

$Validation14T = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "ValidateAddMailItem" -TestOutput $Test14T.OutputObject
if($Validation14T.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Migrate Task Item succeeded") -ForegroundColor Green   
    ("Validate Migrate Task Item succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Migrate Task Item failed") -ForegroundColor Red 
       foreach ($err in $Validation14T.Errors)
    {
        write-host $err
    }      
    ("Validate Migrate Task Item failed")| Out-File -Append -FilePath $TestLogFile    
}

#Validate Add Journal

$Validation14J = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "ValidateAddMailItem" -TestOutput $Test14J.OutputObject
if($Validation14J.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Migrate Journal Item succeeded") -ForegroundColor Green   
    ("Validate Add Item succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Migrate Journal Item failed") -ForegroundColor Red 
       foreach ($err in $Validation14J.Errors)
    {
        write-host $err
    }      
    ("Validate Add Item failed")| Out-File -Append -FilePath $TestLogFile    
}

#Validate Add Attachment

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
    ("Validate Add Attachment failed") | Out-File -Append -FilePath $TestLogFile    
}