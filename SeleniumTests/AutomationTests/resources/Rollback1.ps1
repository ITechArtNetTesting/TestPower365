param([string]$slogin = "BinaryTreePowerShellUser.BT-AutoQA2@btcorp7.onmicrosoft.com", [string]$spassword = "P@ssw0rd", [string]$tlogin = "BinaryTreePowerShellUser.BT-AutoQA2@btcorp9.onmicrosoft.com", [string]$tpassword = "P@ssw0rd", [string]$smailbox = "C7-Automation3@btcorp7.onmicrosoft.com", [string]$tmailbox = "C7-Automation3@btcorp9.onmicrosoft.com", [string]$StopFilePath1="C:\PFTests\RollbackSyncStop1.txt", [string]$SourceMailbox1 = "C7O365SML20@btcorp7.onmicrosoft.com", [string]$TargetMailbox1 = "C7O365SML20@btcorp9.onmicrosoft.com", [string]$StopFilePathRollback1="C:\PFTests\RollbackStop1.txt", [string]$StopFilePath3="C:\PFTests\RollbackSyncStop2.txt", [string]$StopFilePathRollback4="C:\PFTests\RollbackStop2.txt")
#Line4 - Please change the directory of btT2TestUtilPSModule.psd1 so that it matches your environment.
#Line5 - Please change the directory of btT2TPSModule.psd1 so that it matches your environment.
#Line62  - Please change the amount of time to pause the powershell scripts while running migraion through UI.
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

#setup rollback
	$AddTargetFolder = "NewMailFolderAdd-" + (Get-Date).ToString("yyyyMMddHHmm");
	$addtfFolder = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addtargetfolder" -ParentSourceFolder "\\Inbox" -FolderName $AddTargetFolder -FolderClass "IPF.Note"
	if($addtfFolder.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
	{
		Write-Host ("Add New target MailFolder Check succeeded") -ForegroundColor Green   
	   ("Add New target MailFolder Check succeeded")   | Out-File -Append -FilePath $TestLogFile  
	}
	else
	{
		Write-Host ("Add New target MailFolder Check failed") -ForegroundColor Red 
		("Add New target MailFolder Check failed")  | Out-File -Append -FilePath $TestLogFile    
		$overallResults = "failed"
	}

	$addSourceFolder = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addsourcefolder" -ParentSourceFolder "\\Inbox" -FolderName $AddTargetFolder -FolderClass "IPF.Note"
	if($addSourceFolder.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
	{
		Write-Host ("Add New Source MailFolder Check succeeded") -ForegroundColor Green   
	   ("Add New Source MailFolder Check succeeded")   | Out-File -Append -FilePath $TestLogFile  
	}
	else
	{
		Write-Host ("Add New Source MailFolder Check failed") -ForegroundColor Red 
		("Add New Source MailFolder Check failed")  | Out-File -Append -FilePath $TestLogFile    
		$overallResults = "failed"
	}
	$SecondFolder = $AddTargetFolder + "SD"
	$addSourceFolder1 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addfolder" -ParentSourceFolder "\\Inbox" -FolderName $SecondFolder -FolderClass "IPF.Note"
	if($addSourceFolder1.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
	{
		Write-Host ("Add New Source MailFolder 2 Check succeeded") -ForegroundColor Green   
	   ("Add New Source MailFolder 2 Check succeeded")   | Out-File -Append -FilePath $TestLogFile  
	}
	else
	{
		Write-Host ("Add New Source MailFolder 2 Check failed") -ForegroundColor Red 
		("Add New Source MailFolder 2 Check failed")  | Out-File -Append -FilePath $TestLogFile    
		$overallResults = "failed"
	}
	$TestSession.EnumberateFolders()

	$ItemAddSubject = "Test New Items " + (Get-Date).ToString("yyyyMMddHHmmss");

	$ItemsTest = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addleftmailitems" -SourceFolder ("\\Inbox\" +$AddTargetFolder) -ItemSubject $ItemAddSubject -NewItemCount 10 -ReceivedDate (Get-Date)
	if($ItemsTest.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
	{
		Write-Host ("Add left items Test succeeded") -ForegroundColor Green  
		("Add left items Test succeeded") | Out-File -Append -FilePath $TestLogFile     
	}
	else
	{
		Write-Host ("Add left items Test failed") -ForegroundColor Red
		("Add left items Test failed")| Out-File -Append -FilePath $TestLogFile       
		 $overallResults = "failed"
	}

	$SourceItemsTest = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addmailitems" -SourceFolder ("\\Inbox\" +$AddTargetFolder)  -ItemSubject $ItemAddSubject -NewItemCount 10 -ReceivedDate (Get-Date)
	if($SourceItemsTest.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
	{
		Write-Host ("Add Source items Test succeeded") -ForegroundColor Green  
		("Add Source items Test succeeded") | Out-File -Append -FilePath $TestLogFile     
	}
	else
	{
		Write-Host ("Add Source items Test failed") -ForegroundColor Red
		("Add Source items Test failed")| Out-File -Append -FilePath $TestLogFile    
		$overallResults = "failed"   
	}
#AddItemTests
start-sleep -Seconds 80
	$AddTargetFolder = "NewMailFolderAdd-" + (Get-Date).ToString("yyyyMMddHHmm");
	$addtfFolder = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addtargetfolder" -ParentSourceFolder "\\Inbox" -FolderName $AddTargetFolder -FolderClass "IPF.Note"
	if($addtfFolder.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
	{
		Write-Host ("Add New target MailFolder Check succeeded") -ForegroundColor Green   
	   ("Add New target MailFolder Check succeeded")   | Out-File -Append -FilePath $TestLogFile  
	}
	else
	{
		Write-Host ("Add New target MailFolder Check failed") -ForegroundColor Red 
		("Add New target MailFolder Check failed")  | Out-File -Append -FilePath $TestLogFile    
		$overallResults = "failed"
	}

	$addSourceFolder = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addsourcefolder" -ParentSourceFolder "\\Inbox" -FolderName $AddTargetFolder -FolderClass "IPF.Note"
	if($addSourceFolder.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
	{
		Write-Host ("Add New Source MailFolder Check succeeded") -ForegroundColor Green   
	   ("Add New Source MailFolder Check succeeded")   | Out-File -Append -FilePath $TestLogFile  
	}
	else
	{
		Write-Host ("Add New Source MailFolder Check failed") -ForegroundColor Red 
		("Add New Source MailFolder Check failed")  | Out-File -Append -FilePath $TestLogFile    
		$overallResults = "failed"
	}
	$SecondFolder = $AddTargetFolder + "SD"
	$addSourceFolder1 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addfolder" -ParentSourceFolder "\\Inbox" -FolderName $SecondFolder -FolderClass "IPF.Note"
	if($addSourceFolder1.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
	{
		Write-Host ("Add New Source MailFolder 2 Check succeeded") -ForegroundColor Green   
	   ("Add New Source MailFolder 2 Check succeeded")   | Out-File -Append -FilePath $TestLogFile  
	}
	else
	{
		Write-Host ("Add New Source MailFolder 2 Check failed") -ForegroundColor Red 
		("Add New Source MailFolder 2 Check failed")  | Out-File -Append -FilePath $TestLogFile    
		$overallResults = "failed"
	}
	$TestSession.EnumberateFolders()

	$ItemAddSubject = "Test New Items " + (Get-Date).ToString("yyyyMMddHHmmss");

	$ItemsTest = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addleftmailitems" -SourceFolder ("\\Inbox\" +$AddTargetFolder) -ItemSubject $ItemAddSubject -NewItemCount 10 -ReceivedDate (Get-Date)
	if($ItemsTest.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
	{
		Write-Host ("Add left items Test succeeded") -ForegroundColor Green  
		("Add left items Test succeeded") | Out-File -Append -FilePath $TestLogFile     
	}
	else
	{
		Write-Host ("Add left items Test failed") -ForegroundColor Red
		("Add left items Test failed")| Out-File -Append -FilePath $TestLogFile       
		 $overallResults = "failed"
	}

	$SourceItemsTest = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addmailitems" -SourceFolder ("\\Inbox\" +$AddTargetFolder)  -ItemSubject $ItemAddSubject -NewItemCount 10 -ReceivedDate (Get-Date)
	if($SourceItemsTest.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
	{
		Write-Host ("Add Source items Test succeeded") -ForegroundColor Green  
		("Add Source items Test succeeded") | Out-File -Append -FilePath $TestLogFile     
	}
	else
	{
		Write-Host ("Add Source items Test failed") -ForegroundColor Red
		("Add Source items Test failed")| Out-File -Append -FilePath $TestLogFile    
		$overallResults = "failed"   
	}
#setup permissions

##Add Mailbox Permissions
 
	$Testsob = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addsendonbehalfpermission" -UserName $SourceMailbox1 -TargetUserName $TargetMailbox1 
	if($Testsob.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
	{
		Write-Host ("Add New Send on Behalf Permission Check succeeded") -ForegroundColor Green   
	   ("Add New Send on Behalf permission Check succeeded")   | Out-File -Append -FilePath $TestLogFile  
	}
	else
	{
		Write-Host ("Add New Send on Behalf Check Permission failed") -ForegroundColor Red 
		("Add New Send on Behalf Permission Check failed")  | Out-File -Append -FilePath $TestLogFile    
		$overallResults = "Failed"
	}

	$Testacf = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addsendaspermissionmailbox" -UserName $SourceMailbox1 -TargetUserName $TargetMailbox1 
	if($Testacf.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
	{
		Write-Host ("Add New SendAsPermission Check succeeded") -ForegroundColor Green   
	   ("Add New SendAsPermission Check succeeded")   | Out-File -Append -FilePath $TestLogFile  
	}
	else
	{
		Write-Host ("Add SendAsPermission Check failed") -ForegroundColor Red 
		("Add SendAsPermission Check failed")  | Out-File -Append -FilePath $TestLogFile    
		$overallResults = "Failed"
	}

	$Testamb = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addmailboxpermission" -UserName $SourceMailbox1 -TargetUserName $TargetMailbox1 -Permission FullAccess
	if($Testamb.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
	{
		Write-Host ("Add New Mailbox Permission Check succeeded") -ForegroundColor Green   
	   ("Add New Mailbox Check succeeded")   | Out-File -Append -FilePath $TestLogFile  
	}
	else
	{
		Write-Host ("Add Mailbox Check Permission failed") -ForegroundColor Red 
		("Add Mailbox Check failed")  | Out-File -Append -FilePath $TestLogFile    
		$overallResults = "Failed"
	}
#Pause Powershell session for 5 minutes while running migration through UI

#Write-Host ("Powershell will pause 15 minutes while running migration through UI") -ForegroundColor Green
#start-sleep -Seconds 1200
#start-sleep -Seconds 30
###############################################################################################################################################
Write-Host ("Powershell will pause until Migration is complete - 1") -ForegroundColor Green
while(![System.IO.File]::Exists($StopFilePath1)){
            Sleep -Seconds 10
} 
###############################################################################################################################################
$TestSession.EnumberateFolders()

#validate permissions
	$valTestacf = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddsendaspermissionmailbox"-TestOutput $Testacf.OutputObject
	if($valTestacf.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
	{
		Write-Host ("Validate SendAsPermission Check succeeded") -ForegroundColor Green   
	   ("Validate SendAsPermission Check succeeded")   | Out-File -Append -FilePath $TestLogFile  
	}
	else
	{
		Write-Host ("Validate SendAsPermission Check failed") -ForegroundColor Red 
		("Validate SendAsPermission Check failed")  | Out-File -Append -FilePath $TestLogFile    
		$overallResults = "Failed"
	}
	$valTestamb = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddmailboxpermission"-TestOutput $Testamb.OutputObject
	if($valTestacf.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
	{
		Write-Host ("Validate add Mailbox permission Check succeeded") -ForegroundColor Green   
	   ("Validate add Mailbox permission Check succeeded")   | Out-File -Append -FilePath $TestLogFile  
	}
	else
	{
		Write-Host ("Validate add Mailbox permission Check failed") -ForegroundColor Red 
		("Validate add Mailbox permission Check failed")  | Out-File -Append -FilePath $TestLogFile    
		$overallResults = "Failed"
	}
	$valTestsob = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddsendonbehalfpermission"-TestOutput $Testsob.OutputObject
	if($valTestsob.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
	{
		Write-Host ("Validate add Send on Behalf permission Check succeeded") -ForegroundColor Green   
	   ("Validate add Send on Behalf permission Check succeeded")   | Out-File -Append -FilePath $TestLogFile  
	}
	else
	{
		Write-Host ("Validate add Send on Behalf permission Check failed") -ForegroundColor Red 
		("Validate add Send on Behalf permission Check failed")  | Out-File -Append -FilePath $TestLogFile    
		$overallResults = "Failed"
	}
	
$SourceItemsTestRM = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "addmailitems" -SourceFolder ("\\Inbox\" + $SecondFolder)  -ItemSubject $ItemAddSubject -NewItemCount 10 -ReceivedDate (Get-Date)
if($SourceItemsTestRM.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Add Source items Test succeeded") -ForegroundColor Green  
    ("Add Source items Test succeeded") | Out-File -Append -FilePath $TestLogFile     
}
else
{
    Write-Host ("Add Source items Test failed") -ForegroundColor Red
    ("Add Source items Test failed")| Out-File -Append -FilePath $TestLogFile    
    $overallResults = "failed"   
}

#rollback needs to be executed and wait
###############################################################################################################################################
Write-Host ("Powershell will pause until Migration is complete - 2") -ForegroundColor Green
while(![System.IO.File]::Exists($StopFilePathRollback1)){
            Sleep -Seconds 10
} 
###############################################################################################################################################
$TestSession.EnumberateFolders()

$Validation10 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateadditems" -TestOutput $SourceItemsTest.OutputObject
if($Validation10.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Add items failed") -ForegroundColor Red   
    ("Validate Add items failed")| Out-File -Append -FilePath $TestLogFile    
    $overallResults = "failed"
}
else
{
        Write-Host ("Validate Add Items succeeded") -ForegroundColor Green   
    ("Validate Add items succeeded") | Out-File -Append -FilePath $TestLogFile   


}

$Validation9 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validatetargetaddfolder" -TestOutput $addtfFolder.OutputObject
if($Validation9.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Add target folder succeeded") -ForegroundColor Green   
    ("Validate Add target folder succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Add target folder failed") -ForegroundColor Red   
    ("Validate Add target folder failed")| Out-File -Append -FilePath $TestLogFile    
     $overallResults = "failed"
}
$Validation8 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddleftitems" -TestOutput $ItemsTest.OutputObject
if($Validation8.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Add left Items succeeded") -ForegroundColor Green   
    ("Validate Add left items succeeded") | Out-File -Append -FilePath $TestLogFile    
}
else
{
    Write-Host ("Validate Add left items failed") -ForegroundColor Red   
    ("Validate Add left items failed")| Out-File -Append -FilePath $TestLogFile    
    $overallResults = "failed"
}

$Validation12 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddfolder" -TestOutput $addSourceFolder1.OutputObject
if($Validation12.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
{
    Write-Host ("Validate Add target folder failed") -ForegroundColor Red   
    ("Validate Add target folder failed")| Out-File -Append -FilePath $TestLogFile    
     $overallResults = "failed"
}
else
{
            Write-Host ("Validate Add target folder succeeded") -ForegroundColor Green   
    ("Validate Add target folder succeeded") | Out-File -Append -FilePath $TestLogFile    
}
#validate permissions:
$valTestacf = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddsendaspermissionmailbox"-TestOutput $Testacf.OutputObject
	if($valTestacf.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
	{
		Write-Host ("Validate SendAsPermission Check succeeded") -ForegroundColor Green   
	   ("Validate SendAsPermission Check succeeded")   | Out-File -Append -FilePath $TestLogFile  
	}
	else
	{
		Write-Host ("Validate SendAsPermission Check failed") -ForegroundColor Red 
		("Validate SendAsPermission Check failed")  | Out-File -Append -FilePath $TestLogFile    
		$overallResults = "Failed"
	}
	$valTestamb = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddmailboxpermission"-TestOutput $Testamb.OutputObject
	if($valTestacf.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
	{
		Write-Host ("Validate add Mailbox permission Check succeeded") -ForegroundColor Green   
	   ("Validate add Mailbox permission Check succeeded")   | Out-File -Append -FilePath $TestLogFile  
	}
	else
	{
		Write-Host ("Validate add Mailbox permission Check failed") -ForegroundColor Red 
		("Validate add Mailbox permission Check failed")  | Out-File -Append -FilePath $TestLogFile    
		$overallResults = "Failed"
	}
	$valTestsob = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddsendonbehalfpermission"-TestOutput $Testsob.OutputObject
	if($valTestsob.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
	{
		Write-Host ("Validate add Send on Behalf permission Check succeeded") -ForegroundColor Green   
	   ("Validate add Send on Behalf permission Check succeeded")   | Out-File -Append -FilePath $TestLogFile  
	}
	else
	{
		Write-Host ("Validate add Send on Behalf permission Check failed") -ForegroundColor Red 
		("Validate add Send on Behalf permission Check failed")  | Out-File -Append -FilePath $TestLogFile    
		$overallResults = "Failed"
	}

Write-Host ("Powershell will pause until Migration is complete - 3") -ForegroundColor Green
while(![System.IO.File]::Exists($StopFilePath3)){
            Sleep -Seconds 10
} 

#rollback needs to be executed and wait (with permission rollback selected)
###############################################################################################################################################
Write-Host ("Powershell will pause until Migration is complete - 4") -ForegroundColor Green
while(![System.IO.File]::Exists($StopFilePathRollback4)){
            Sleep -Seconds 10
} 
###############################################################################################################################################
#validate permissions:
$valTestacf = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddsendaspermissionmailbox"-TestOutput $Testacf.OutputObject
	if($valTestacf.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
	{
		Write-Host ("Validate SendAsPermission Check failed") -ForegroundColor Red   
	   ("Validate SendAsPermission Check failed")   | Out-File -Append -FilePath $TestLogFile  
	}
	else
	{
		Write-Host ("Validate SendAsPermission Check succeeded") -ForegroundColor Green 
		("Validate SendAsPermission Check succeeded")  | Out-File -Append -FilePath $TestLogFile    
		$overallResults = "succeeded"
	}
	$valTestamb = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddmailboxpermission"-TestOutput $Testamb.OutputObject
	if($valTestacf.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
	{
		Write-Host ("Validate add Mailbox permission Check failed") -ForegroundColor Red   
	   ("Validate add Mailbox permission Check failed")   | Out-File -Append -FilePath $TestLogFile  
	}
	else
	{
		Write-Host ("Validate add Mailbox permission Check succeeded") -ForegroundColor Green 
		("Validate add Mailbox permission Check succeeded")  | Out-File -Append -FilePath $TestLogFile    
		$overallResults = "Failed"
	}
	$valTestsob = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "validateaddsendonbehalfpermission"-TestOutput $Testsob.OutputObject
	if($valTestsob.Result -eq [T2TMigrationTestUtil.Tests.TestResult]::Success)
	{
		Write-Host ("Validate add Send on Behalf permission Check failed") -ForegroundColor Red   
	   ("Validate add Send on Behalf permission Check failed")   | Out-File -Append -FilePath $TestLogFile  
	}
	else
	{
		Write-Host ("Validate add Send on Behalf permission Check succeeded") -ForegroundColor Green 
		("Validate add Send on Behalf permission Check succeeded")  | Out-File -Append -FilePath $TestLogFile    
		$overallResults = "Failed"
	}


