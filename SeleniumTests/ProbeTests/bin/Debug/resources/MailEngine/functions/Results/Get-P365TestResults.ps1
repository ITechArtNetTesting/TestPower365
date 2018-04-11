function Get-P365TestResults{
    param( 
	
    )  
 	Begin
	 {
		#Write-host ("Number of Test run : " + $Script:TestResults.Values.Count)
		return $Script:TestResults #.Values | Select TestCase,Description,TestLastRun,TestResult,ValidationLastRun,ValidationResult,OverAllResult | FT
     	}
}