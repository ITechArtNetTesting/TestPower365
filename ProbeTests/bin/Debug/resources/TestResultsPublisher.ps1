Param(
   [string]$testResultPath,
   [string]$collection,
   [string]$authUser,
   [string]$authPass,
   [string]$project,
   [string]$title,
   [string]$suiteId,
   [string]$configId,
   [string]$resultOwner
)

$tcmPath = "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\TCM.exe"

$trxFiles = Get-ChildItem -Path $testResultPath -Filter *.trx -Recurse
$fileCount = $trxFiles.Count
Write-Host "Found '$fileCount' trx files to publish."

foreach($file in $trxFiles)
{
	$filePath = $file.FullName

	$args = "run /publish /collection:`"$collection`" /login:`"$authUser,$authPass`" /noprompt /teamproject:`"$project`" /suiteid:$suiteId /configid:$configId /resultowner:`"$resultOwner`" /title:`"$title`" /resultsfile:`"$filePath`""
	Start-Process -FilePath $tcmPath -ArgumentList $args -NoNewWindow -Wait
}
