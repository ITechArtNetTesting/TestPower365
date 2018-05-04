import-module './btT2TPSModule.psd1' -Force
$ResultsFiles = 'c:\temp\mbReports4.csv'
$sourceUserName = "admin@Md2.onmicrosoft.com"
$sourcepasswd = ConvertTo-SecureString "password" -AsPlainText -Force
$TargetUserName = "admin@Md1.onmicrosoft.com"
$Targetpasswd = ConvertTo-SecureString "password" -AsPlainText -Force

$SoruceCreds = New-Object System.Management.Automation.PSCredential ($sourceUserName, $sourcepasswd)
$TargetCreds= New-Object System.Management.Automation.PSCredential ($TargetUserName, $Targetpasswd)
$Script:Results = @()
Import-Csv -path c:\temp\CopyBatch1.csv | ForEach-Object {
    Write-Host ("Copy Mailbox " + $_.SourceMailbox + " To " + $_.TargetMailbox)
    $Result = Copy-T2TMailbox -SourceMailbox $_.SourceMailbox -SourceCredentials $SoruceCreds -TargetMailbox $_.TargetMailbox -TargetCredentials $TargetCreds  -mappingfile c:\temp\mapping.csv
    $Result | select "OverAllResult","ExportId","SourceMailbox","TargetMailbox","isArchive","ErrorCount","FirstPassItemsProcessed","FirstPassItemExist","SecondPassItemsProcessed","FoldersProcessed","ConfirmationFoldersCount","DownloadRate","UploadRate","DownloadedMB","UploadedMB","CopyRate","StartTime","EndTime","TotalMinutes","Exception","LogFilePath","ErrorLogFilePath" | Export-Csv -Path $ResultsFiles -Append -NoTypeInformation 
}