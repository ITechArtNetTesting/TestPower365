param([string]$slogin = "BinaryTreePowerShellUser.BT-AutoQA2@btcorp7.onmicrosoft.com", [string]$spassword = "P@ssw0rd", [string]$tlogin = "BinaryTreePowerShellUser.BT-AutoQA1@btcorp9.onmicrosoft.com", [string]$tpassword = "P@ssw0rd", [string]$smailbox = "C7-Automation1@btcorp7.onmicrosoft.com", [string]$tmailbox = "C7-Automation1@btcorp9.onmicrosoft.com", [string]$StopFilePath1="C:\PFTests\DeleteFoldersAndItemsStop1.txt", [string]$StopFilePath2="C:\PFTests\DeleteFoldersAndItemsStop2.txt")
#Line4 - Please change the directory of btT2TestUtilPSModule.psd1 so that it matches your environment.
#Line5 - Please change the directory of btT2TPSModule.psd1 so that it matches your environment.
#Line118 & 300 - Please change the amount of time to pause the powershell scripts while running migraion through UI.
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

#Add a message to test message delete 

$ItemAddSubjectD = "Banana Test New Mail Delete " + (Get-Date).ToString("yyyyMMddHHmm");
$Test7D = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addmailitem" -SourceFolder "\\Inbox" -ItemSubject $ItemAddSubjectD
if($Test7D.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Mail to test delete succeeded") -ForegroundColor Green  
    ("Add Mail to test delete succeeded") | Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Add Mail to test delete failed") -ForegroundColor Red
     foreach ($err in $Test7D.Errors)
    {
        write-host $err
    } 

    ("Add Mail to test delete failed")| Out-File -Append -FilePath $TestLogFile       
}

#Add a new Sticky Note to test delete

$StickyNoteSubject = "YM Sticky Note Added on " + (Get-Date).ToString("yyyyMMddHHmm");
$Test10D = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addcustomitem" -SourceFolder "\\Notes" -ItemSubject $StickyNoteSubject -ItemClass "IPM.StickyNote"
if($Test10D.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Stiky Note to test delete succeeded") -ForegroundColor Green   
    ("Add Sticky Note to test delete succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add Sticky Note to test failed") -ForegroundColor Red  
     foreach ($err in $Test10D.Errors)
    {
        write-host $err
    }  
    ("Add Sticky Note to test failed") | Out-File -Append -FilePath $TestLogFile    
}

#Add an Appointment to test appointment delete

$AppointmentAddSubject = "Banana Test Meeting " + (Get-Date).ToString("yyyyMMddHHmm");
$TestAddAppointment = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addappointmentitem" -SourceFolder "\\Calendar" -ItemSubject $AppointmentAddSubject -Start (Get-Date) -End (Get-Date).AddHours(1)
if($TestAddAppointment.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Appointment to test delete succeeded") -ForegroundColor Green   
    ("Add Appointment to test delete succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add Appointment to test delete failed") -ForegroundColor Red  
    foreach ($err in $TestAddAppointment.Errors)
    {
        write-host $err
    }   
    ("Add Appointment to test delete failed") | Out-File -Append -FilePath $TestLogFile    
}

#Add a Task to test task delete

$TaskAddSubjectD = "Banana New Task to be deleted " + (Get-Date).ToString("yyyyMMddHHmm");
$Test14TD = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addcustomitem" -SourceFolder "\\Tasks" -ItemSubject $TaskAddSubjectD -ItemClass "IPM.Task"
if($Test14TD.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Task to test delete succeeded") -ForegroundColor Green   
    ("Add Task to test delete succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add Task to test delete failed") -ForegroundColor Red  
     foreach ($err in $TaskAddSubjectD.Errors)
    {
        write-host $err
    }    
    ("Add Task to test delete failed") | Out-File -Append -FilePath $TestLogFile    
}

#Add a Journal to test journal delete

$JournalItemTestD = "Banana New Journal to be deleted " + (Get-Date).ToString("yyyyMMddHHmm");

$Test14JD = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addcustomitem" -SourceFolder "\\Journal" -ItemSubject $JournalItemTestD -ItemClass "IPM.Activity"
if($Test14JD.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Journal to test delete succeeded") -ForegroundColor Green   
    ("Add Journal to test delete succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add Journal to test delete failed") -ForegroundColor Red  
      foreach ($err in $Test14JD.Errors)
    {
        write-host $err
    }     
    ("Add Journal to test delete failed") | Out-File -Append -FilePath $TestLogFile    
}

#Add Contact to test delete

$ContactAddSubject = "Brian Arnold Added on " + (Get-Date).ToString("yyyyMMddHHmm");
$Test8D = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addcontactitem" -SourceFolder "\\Contacts" -ContactName $ContactAddSubject -ContactEmailAddress 'MyAddress@Fake.com'
if($Test8D.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Contact to test delete succeeded") -ForegroundColor Green   
    ("Add Contact to test delete succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Add Contact to test delete failed") -ForegroundColor Red  
     foreach ($err in $Test8D.Errors)
    {
        write-host $err
    }      
    ("Add Contact to test delete failed") | Out-File -Append -FilePath $TestLogFile    
}

#Pause Powershell session for 15 minutes while running migration through UI

#Write-Host ("Powershell will pause 15 minutes while running migration through UI") -ForegroundColor Green
#start-sleep -Seconds 1080
#start-sleep -Seconds 30

###############################################################################################################################################
Write-Host ("Powershell will pause until Migration is complete - 1") -ForegroundColor Green
while(![System.IO.File]::Exists($StopFilePath1)){
            Sleep -Seconds 10
} 
###############################################################################################################################################

#Copy-T2TMailbox -SourceMailbox $SourceMailbox -SourceCredentials $SoruceCreds -TargetMailbox $TargetMailbox -TargetCredentials $TargetCreds  -mappingfile c:\temp\mappingC7toC9.csv -ProcessingPath c:\temp -Delta:$true
$TestSession.EnumberateFolders()


#Run Validation

#Validate Mail migration

$Validation7D = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "ValidateAddMailItem" -TestOutput $Test7D.OutputObject
if($Validation7D.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Mail Migration succeeded") -ForegroundColor Green   
    ("Validate Mail Migration succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Mail Migration failed") -ForegroundColor Red   
     foreach ($err in $Validation7D.Errors)
    {
        write-host $err
    }      
    ("Validate Mail Migration failed")| Out-File -Append -FilePath $TestLogFile    
}

#Validate Sticky Note migration

$Validation10D = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "ValidateAddMailItem" -TestOutput $Test10D.OutputObject
if($Validation10D.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Sticky Note Migration succeeded") -ForegroundColor Green   
    ("Validate Sticky Note Migration succeeded")| Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Sticky Note Migration failed") -ForegroundColor Red  
     foreach ($err in $Validation10D.Errors)
    {
        write-host $err
    }       
    ("Validate Sticky Note Migration failed")| Out-File -Append -FilePath $TestLogFile    
}


#Validate Appoitment migration

$ValidationAddAppointment = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "ValidateAddMailItem" -TestOutput $TestAddAppointment.OutputObject
if($ValidationAddAppointment.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Appointment Migration succeeded") -ForegroundColor Green   
    ("Validate Appointment Migration succeeded")| Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Appointment Migration failed") -ForegroundColor Red   
    foreach ($err in $ValidationAddAppointment.Errors)
    {
        write-host $err
    }       
    ("Validate Appointment Migration failed")| Out-File -Append -FilePath $TestLogFile    
}

#Validate Task Item to be deleted Migration

$Validation14TD = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "ValidateAddMailItem" -TestOutput $Test14TD.OutputObject
if($Validation14TD.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Task Migration succeeded") -ForegroundColor Green   
    ("Validate Task Migration succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Task Migration failed") -ForegroundColor Red  
      foreach ($err in $Validation14TD.Errors)
    {
        write-host $err
    }        
    ("Validate Task Migration failed")| Out-File -Append -FilePath $TestLogFile    
}

#Validate Journal Migration

$Validation14JD = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "ValidateAddMailItem" -TestOutput $Test14JD.OutputObject
if($Validation14JD.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Journal Migration succeeded") -ForegroundColor Green   
    ("Validate Journal Migration succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Journal Migration failed") -ForegroundColor Red  
     foreach ($err in $Validation14JD.Errors)
    {
        write-host $err
    }         
    ("Validate Journal Migration failed")| Out-File -Append -FilePath $TestLogFile    
}

#Validate Contact Migration

$Validation8D = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "ValidateAddMailItem" -TestOutput $Test8D.OutputObject
if($Validation8D.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Contact Migration succeeded") -ForegroundColor Green   
    ("Validate Contact Migration succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Contact Migration failed") -ForegroundColor Red   
      foreach ($err in $Validation8D.Errors)
    {
        write-host $err
    }         
    ("Validate Contact Migration failed")| Out-File -Append -FilePath $TestLogFile    
}


#Delete Mail item in source

$Test24 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "deleteitem" -SourceFolder "\\Inbox" -ItemSubject $ItemAddSubjectd  
if($Test24.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Delete Mail succeeded") -ForegroundColor Green  
    ("Delete Mail succeeded")| Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Delete Mail failed") -ForegroundColor Red 
     foreach ($err in $Test24.Errors)
    {
        write-host $err
    }           
    ("Delete Mail failed") | Out-File -Append -FilePath $TestLogFile    
}

#Delete Sticky Note in source

$Test27 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "deleteitem" -SourceFolder "\\Notes" -ItemSubject $StickyNoteSubject -ItemClass "IPM.StickyNote" 
if($Test27.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Delete Sticky Note succeeded") -ForegroundColor Green  
    ("Delete Sticky Note succeeded")| Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Delete Sticky Note failed") -ForegroundColor Red 
      foreach ($err in $Test27.Errors)
    {
        write-host $err
    }             
    ("Delete Sticky Note failed") | Out-File -Append -FilePath $TestLogFile    
}

#Delete Appointment in source.

$TestDeleteAppointment = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "deleteitem" -SourceFolder "\\Calendar" -ItemSubject $AppointmentAddSubject -Start (Get-Date) -End (Get-Date).AddHours(1)  
if($TestDeleteAppointment.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Delete Appointment succeeded") -ForegroundColor Green  
    ("Delete Appointment succeeded")| Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Delete Appointment failed") -ForegroundColor Red  
     foreach ($err in $TestDeleteAppointment.Errors)
    {
        write-host $err
    }              
    ("Delete Appointment failed") | Out-File -Append -FilePath $TestLogFile    
}

#Delete Task Item. 

$Test25 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "deleteitem" -SourceFolder "\\Tasks" -ItemSubject $TaskAddSubjectD -ItemClass "IPM.Task" 
if($Test25.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Delete Task succeeded") -ForegroundColor Green  
    ("Delete Task succeeded")| Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Delete Task failed") -ForegroundColor Red  
      foreach ($err in $Test25.Errors)
    {
        write-host $err
    }          
    ("Delete Task failed") | Out-File -Append -FilePath $TestLogFile    
}

#Delete Journal Item.

$Test26 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "deleteitem" -SourceFolder "\\Journal" -ItemSubject $JournalItemTestD -ItemClass "IPM.Activity" 
if($Test26.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Delete Journal succeeded") -ForegroundColor Green  
    ("Delete Journal succeeded")| Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Delete Journal failed") -ForegroundColor Red  
      foreach ($err in $Test26.Errors)
    {
        write-host $err
    }          
    ("Delete Journal failed") | Out-File -Append -FilePath $TestLogFile    
}

#Delete Contact

$Test28 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "deleteitem" -SourceFolder "\\Contacts" -ItemSubject $ContactAddSubject
if($Test28.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Delete Contact succeeded") -ForegroundColor Green  
    ("Delete Contact succeeded")| Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Delete Contact failed") -ForegroundColor Red  
       foreach ($err in $Test28.Errors)
    {
        write-host $err
    }           
    ("Delete Contact failed") | Out-File -Append -FilePath $TestLogFile    
}


#Pause Powershell session for 5 minutes while running migration through UI

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

#Run Validation

#Validate Mail Item Delete

$Validation24 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatedeleteitem" -TestOutput $Test24.OutputObject
if($Validation24.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Delete Mail succeeded") -ForegroundColor Green   
    ("Validate Delete Mail Succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Delete Mail failed") -ForegroundColor Red  
        foreach ($err in $Validation24.Errors)
    {
        write-host $err
    }            
    ("Validate Delete Mail failed") | Out-File -Append -FilePath $TestLogFile    
}

#Validate Sticky Note Item Delete

$Validation27 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatedeleteitem" -TestOutput $Test27.OutputObject
if($Validation27.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Delete Sticky Note succeeded") -ForegroundColor Green   
    ("Validate Delete Sticky Note Succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Delete Sticky Note failed") -ForegroundColor Red  
    foreach ($err in $Validation27.Errors)
    {
        write-host $err
    }             
    ("Validate Delete Sticky Note failed") | Out-File -Append -FilePath $TestLogFile    
}

#Validate Meeting item delete

$ValidationDeleteAppointment = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatedeleteitem" -TestOutput $TestDeleteAppointment.OutputObject
if($ValidationDeleteAppointment.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Delete Appointment succeeded") -ForegroundColor Green   
    ("Validate Delete Appointment Succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Delete Appointment failed") -ForegroundColor Red  
    foreach ($err in $ValidationDeleteAppointment.Errors)
    {
        write-host $err
    }             
    ("Validate Delete Appointment failed") | Out-File -Append -FilePath $TestLogFile    
}

#Validate Task Item Delete

$Validation25 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatedeleteitem" -TestOutput $Test25.OutputObject
if($Validation25.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Delete Task succeeded") -ForegroundColor Green   
    ("Validate Delete Task Succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Delete Task failed") -ForegroundColor Red   
     foreach ($err in $Validation25.Errors)
    {
        write-host $err
    }       
    ("Validate Delete Task failed") | Out-File -Append -FilePath $TestLogFile    
}

#Validate Journal Item Delete

$Validation26 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatedeleteitem" -TestOutput $Test26.OutputObject
if($Validation26.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Delete Journal succeeded") -ForegroundColor Green   
    ("Validate Delete Journal Succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Delete Journal failed") -ForegroundColor Red   
     foreach ($err in $Validation26.Errors)
    {
        write-host $err
    }       
    ("Validate Delete Journal failed") | Out-File -Append -FilePath $TestLogFile    
}

#Validate Contact Item Delete

$Validation28 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatedeleteitem" -TestOutput $Test28.OutputObject
if($Validation28.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Delete Contact succeeded") -ForegroundColor Green   
    ("Validate Delete Contact Succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Delete Contact failed") -ForegroundColor Red   
     foreach ($err in $Validation28.Errors)
    {
        write-host $err
    }       
    ("Validate Delete Contact failed") | Out-File -Append -FilePath $TestLogFile    
}


