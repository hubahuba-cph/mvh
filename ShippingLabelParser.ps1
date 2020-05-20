# Setup Variables and Environment
$inputFolderPath = "c:\github\mvh\in"
$headerLineNo = 1
$workSheetName = "Ark1"
$delimiter = ","

$expectedFileName = "NL_Fraktetikett.xlsx"
$timestampColumnName = "Timestamp"
$tmstmpFile = "ShippingLabelParser.tmstmp"

$ErrorActionPreference = "Stop"

$data = [System.Collections.ArrayList]@()

# $Script:LOGFILE = "ShippingLabelParser.{0:yyyy}{0:MM}{0:dd}{0:HH}.{0:mm}.{0:ss}.log" -f (Get-Date) # Set name of log file (default: log.log - Obtained by removing this line). Log file is saved to .\log.

# Including Functions
. .\functions\Log.ps1
. .\functions\WriteHeader.ps1

WriteHeader -Log $Script:LOGFILE -InPath $inputFolderPath -Output "./out/Fraktetiket.csv"

# Validations
if((Test-Path -Path $inputFolderPath) -eq $false) { 
    Write-Host "Input not found" 
    Exit 458 
}

# Set Last Timestamp

if((Test-Path -Path ".\$tmstmpFile") -eq $false) {
    $tmstmp = Get-Date
    #New-Item -Path ".\$tmstmpFile" -Force -Value $tmstmp.toString("yyyy-MM-ddTHH.mm.ss.fff")
}
else {
    $tmstmp = [DateTime]::ParseExact((Get-Content -Path ".\$tmstmpFile"), "yyyy-MM-ddTHH.mm.ss.fff", $null)
}

Write-Host "Fetching Shipping Labels using cut-off: $tmstmp"

# Actual Parsing Script 
Get-ChildItem -Path $inputFolderPath | Where-Object { $_.name -eq $expectedFileName } | ForEach-Object {
    $currentFile = "$inputFolderPath\$_"
    Log -Level "Info" -Msg "Parsing File: $currentFile"

    $objExcel = New-Object -ComObject Excel.Application
    $workBook = $objExcel.Workbooks.Open($currentFile)
    
    try { 
        Log -Level "Info" -Msg "Reading worksheets.."
        ForEach($workSheet in ($workBook.Sheets | Where-Object { $_.name -eq $workSheetName} )) {        
            $data.Clear()            
            $totalNoOfColumns = -1
    
            $dataObj = New-Object PSObject

            do {                  
                $totalNoOfColumns = $totalNoOfColumns + 1
    
                $columnName = $workSheet.Cells.Item($headerLineNo, $totalNoOfColumns + 1).text.trim().replace(' ','')
                $isValidColumn = [String]::IsNullOrEmpty($columnName) -eq $false
            
                if($isValidColumn) {
                    $dataObj | Add-Member -NotePropertyName $columnName -NotePropertyValue ""
                }
            } while ($isValidColumn)

            Log -Level "Debug" -Msg "Reading data lines.."

            $rowNum = $headerLineNo  
            $itemTmstmp = $tmstmp
            
            For($rowNum = 2; $rowNum -le ($workSheet.UsedRange.Rows).Count; $rowNum++)
            {
                $currentDataObj = $dataObj.PSObject.Copy()

                Log -Level "Debug" -Msg "Reading line no.: $($rowNum)"            
                
                For($j = 1; $j -le $totalNoOfColumns; $j++) {                    
                    $vName = $workSheet.Cells.Item($headerLineNo, $j).text.replace(' ','')
                    $Value = $workSheet.Cells.Item($rowNum, $j).text
                    
                    $currentDataObj.$vName = $Value.PsObject.Copy() 
                }

                $result = [DateTime]::TryParse($currentDataObj."$timestampColumnName", [ref]$itemTmstmp)

                if($result -eq $true) {
                    if($itemTmstmp -gt $tmstmp) {
                        Log -Level "Info" -Msg "Timestamp: $($itemTmstmp) included in output." 
    
                        $data.Add($currentDataObj)                        
                    }    
                }
            }                        
        }
        
        $data | Export-Csv -Path ./out/Fraktetikett.csv -Delimiter ';' -Force -NoTypeInformation -Encoding UTF8

        if($itemTmstmp -ne [DateTime]::MinValue) {
            New-Item -Path ".\$tmstmpFile" -Force -Value $itemTmstmp.toString("yyyy-MM-ddTHH.mm.ss.fff")
        }        
    }
    catch {
        Log -Level "Error" -Msg "An error occured.."                    
        Log -Level "Error" -Msg $PSItem.InvocationInfo
        Log -Level "Error" -Msg $PSItem.ScriptStackTrace
    }
    finally {
        Log -Level "Info" -Msg "Cleanup.."

        $workBook.Close();
        $objExcel.Quit();    
    }
}