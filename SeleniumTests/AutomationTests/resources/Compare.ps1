param([string]$slogin = "C7O365Admin@btcorp7.onmicrosoft.com", [string]$spassword = "BinTree123", [string]$tlogin = "corp9O365admin@btcorp9.onmicrosoft.com", [string]$tpassword = "BinTree123", [string]$smailbox = "C7-Automation2@btcorp7.onmicrosoft.com", [string]$tmailbox = "C7-Automation2@btcorp9.onmicrosoft.com")
#Please change SourceMailbox and TargetMailbox to your test mailbox.

import-module ($PSScriptRoot + "\PSTools\cmdtest\btT2TPSModule.psd1") -Force
import-module ($PSScriptRoot + "\PSTools\testutils\btT2TestUtilPSModule.psd1") -Force
#$sourceUserName = "C7O365admin@btcorp7.onmicrosoft.com"
#$sourcepasswd = ConvertTo-SecureString "BinTree123" -AsPlainText -Force
#$TargetUserName = "Corp9O365admin@btcorp9.onmicrosoft.com"
#$Targetpasswd = ConvertTo-SecureString "BinTree123" -AsPlainText -Force
$sourceUserName = $slogin
$sourcepasswd = ConvertTo-SecureString $spassword -AsPlainText -Force
$TargetUserName = $tlogin
$Targetpasswd = ConvertTo-SecureString $tpassword -AsPlainText -Force   

$SourceMailbox = $smailbox
$TargetMailbox = $tmailbox  

$LogPath = "C:\PSTools\T2T_Logs"

$TestLogFile = $LogPath + "\" + "OverAllLog" + (Get-Date).ToString("yyyyMMddHHmm") + ".log"

$SoruceCreds = New-Object System.Management.Automation.PSCredential ($sourceUserName, $sourcepasswd)
$TargetCreds= New-Object System.Management.Automation.PSCredential ($TargetUserName, $Targetpasswd)
#Start the TestSession
$TestSession = Start-T2tMigrationTests -SourceMailbox $SourceMailbox -SourceCredentials $SoruceCreds -TargetMailbox $TargetMailbox -TargetCredentials $TargetCreds -LogFilePath $LogPath -TestArchive:$false 

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
    Write-Host ("Folder existance Check failed") -ForegroundColor Red 
       foreach ($err in $Test1.Errors)
    {
        write-host $err
    }      
    ("Folder existance Check failed") | Out-File -Append -FilePath $TestLogFile  
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
    ("Source Target Item existance Check failed")  | Out-File -Append -FilePath $TestLogFile     
}