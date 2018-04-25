param([string]$slogin = "BinaryTreePowerShellUser.BT-AutoQA2@btcorp7.onmicrosoft.com", [string]$spassword = "P@ssw0rd", [string]$tlogin = "BinaryTreePowerShellUser.BT-AutoQA1@btcorp9.onmicrosoft.com", [string]$tpassword = "P@ssw0rd", [string]$smailbox = "C7-Automation1@btcorp7.onmicrosoft.com", [string]$tmailbox = "C7-Automation1@btcorp9.onmicrosoft.com", [string]$sx400 = "/o=ExchangeLabs/ou=Exchange Administrative Group (FYDIBOHF23SPDLT)/cn=Recipients/cn=6a84cbd5886341638d0a43ee93319ce0-C7-Automati", [string]$tx400 = "/o=ExchangeLabs/ou=Exchange Administrative Group (FYDIBOHF23SPDLT)/cn=Recipients/cn=e4003d7bc00c4f5da1129ae8865ffa66-C7-Automati", [string]$StopFilePath1="C:\PFTests\AddTransformStop1.txt")
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

# Test Address Transform add a New Item

$ItemAddSubject = "Test SMTP Address Transform New Item " + (Get-Date).ToString("yyyyMMddHHmm");
$SourceSMTPAddresss = $smailbox
$TargetSMTPAddress = $tmailbox


$Test11 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "transformaddress" -SourceFolder "\\Inbox" -ItemSubject $ItemAddSubject -Address $SourceSMTPAddresss -TargetAddress $TargetSMTPAddress
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
    ("SMTP Address Rewriting  Test failed") | Out-File -Append -FilePath $TestLogFile       
}

$ItemAddSubject = "Test x400 Address Transform New Item " + (Get-Date).ToString("yyyyMMddHHmm");
#$Sourcex400Address = '/o=ExchangeLabs/ou=Exchange Administrative Group (FYDIBOHF23SPDLT)/cn=Recipients/cn=6a84cbd5886341638d0a43ee93319ce0-C7-Automati'
#$Targetx400Address = '/o=ExchangeLabs/ou=Exchange Administrative Group (FYDIBOHF23SPDLT)/cn=Recipients/cn=04b653b41f274a41afafb42cc7465705-C7-Automati'
$Sourcex400Address = $sx400
$Targetx400Address = $tx400 


$Test12 = Invoke-T2TMigrationTest -TestSession $TestSession -TestName "transformaddress" -SourceFolder "\\Inbox" -ItemSubject $ItemAddSubject -Address $Sourcex400Address -TargetAddress $Targetx400Address
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
    ("X400 Address Rewriting  Test failed") | Out-File -Append -FilePath $TestLogFile    
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
#Run Migration Validation

#Validate SMTP Transform
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
   ("Validate SMTP Transform failed") | Out-File -Append -FilePath $TestLogFile    
}

#Validate X400 Transform
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
    ("Validate x400 Transform failed") | Out-File -Append -FilePath $TestLogFile    
}