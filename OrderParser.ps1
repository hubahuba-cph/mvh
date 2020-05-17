# Setup Variables and Environment
$inputFolderPath = 'c:\github\mvh\in'
$headerLineNo = 1
$workSheetName = "Ny Ordre"

$ErrorActionPreference = "Stop"

# $Script:LOGFILE = "{0:yyyy}{0:MM}{0:dd}{0:HH}.{0:mm}.{0:ss}.log" -f (Get-Date) # Set name of log file (default: log.log - Obtained by removing this line). Log file is saved to .\log.

# Including Functions
. .\Log.ps1

# Validations
if((Test-Path -Path $inputFolderPath) -eq $false) { Exit 458 }

# Actual Parsing Script 
Get-ChildItem -Path $inputFolderPath | ForEach-Object {
    $currentFile = "$inputFolderPath\$_"
    Log -Level "Info" -Msg "Parsing File: $currentFile"

    $objExcel = New-Object -ComObject Excel.Application
    $workBook = $objExcel.Workbooks.Open($currentFile)

    $workBook.sheets | Select-Object -Property Name

    Log -Level "Info" -Msg "Reading worksheets.."
    ForEach($workSheet in ($workBook.Sheets | Where { $_.name -eq $workSheetName} )) {        
        $data = $null
        $totalNoOfItems = $totalNoOfRecords -1
        $totalNoOfColumns = -1

        Log -Level "Debug" -Msg "TotalNoOfItems = $totalNoOfItems"

        $dataObj = New-Object PSObject

        do {                  
            $totalNoOfColumns++;

            $columnName = $workSheet.Cells.Item($headerLineNo, $totalNoOfColumns + 1).text.trim()
            $isValidColumn = [String]::IsNullOrEmpty($columnName) -eq $false
        
            if($isValidColumn) {
                $dataObj | Add-Member -NotePropertyName $columnName -NotePropertyValue ""
            }
        } while ($isValidColumn)
            
        $dataObj | Get-Member | Format-Table -AutoSize

        Log -Level "Debug" -Msg "Reading data lines.."
        
        $rowNum = $headerLineNo        
        ForEach($row in ($workSheet.UsedRange.Rows|Select -skip $headerLineNo))
        {
            Log -Level "Debug" -Msg "Reading line no.: $($rowNum)"            
            For($j = 1; $j -le $totalNoOfColumns; $j++) {
                $vName = $workSheet.Cells.Item($headerLineNo, $j).text
                $Value = $row.Cells.Item($rowNum, $j).text

                $dataObj.$vName = $Value 
            }
            
            $data += $dataObj
            $rowNum++
        }            
    }

    Log -Level "Info" -Msg "File parsed"
}
