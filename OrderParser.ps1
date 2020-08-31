# Setup Variables and Environment
$inputFolderPath = "c:\github\mvh\in"
$outputFolderPath = ".\out"

$headerLineNo = 1
$workSheetName = "Ny Ordre"
$delimiter = ","

$expectedFileName = "NL-Ordre.xlsx"
$quantityColumnName = "Quantity"
$productColumnName = "ProductName"
$priceColumnName = "ProductSalesPrice"

$ErrorActionPreference = "Stop"
$data = [System.Collections.ArrayList]@()

# $Script:LOGFILE = "OrderParser.{0:yyyy}{0:MM}{0:dd}{0:HH}.{0:mm}.{0:ss}.log" -f (Get-Date) # Set name of log file (default: log.log - Obtained by removing this line). Log file is saved to .\log.

# Including Functions
. .\functions\Log.ps1
. .\functions\WriteHeader.ps1

WriteHeader -Log $Script:LOGFILE -InPath $inputFolderPath -Output "$outputFolderPath\Orders.csv"

# Validations
if((Test-Path -Path $inputFolderPath) -eq $false) { 
    Write-Host "Input not found" 
    Exit 458 
}

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
    
                $columnName = $workSheet.Cells.Item($headerLineNo, $totalNoOfColumns + 1).text.trim()
                $isValidColumn = [String]::IsNullOrEmpty($columnName) -eq $false
            
                if($isValidColumn) {
                    $dataObj | Add-Member -NotePropertyName $columnName -NotePropertyValue ""
                }
            } while ($isValidColumn)
                    
            Log -Level "Debug" -Msg "Reading data lines.."
            
            $rowNum = $headerLineNo        
            For($rowNum = 2; $rowNum -le ($workSheet.UsedRange.Rows).Count; $rowNum++)
            {
                $currentDataObj = $dataObj.PSObject.Copy()

                Log -Level "Debug" -Msg "Reading line no.: $($rowNum)"            
                For($j = 1; $j -le $totalNoOfColumns; $j++) {
                    $vName = $workSheet.Cells.Item($headerLineNo, $j).text
                    $Value = $workSheet.Cells.Item($rowNum, $j).text
    
                    $currentDataObj.$vName = $Value 
                }
                
                $currentQuantity = $currentDataObj."$quantityColumnName"
                Log -Level "Info" -Msg "Order, Quantity: $currentQuantity"

                $hasDelimiters = $currentDataObj."$productColumnName".indexOf($delimiter) -gt 0

                $currentDataObj."$quantityColumnName" = 1
                if($hasDelimiters -eq $true) {
                    $productArray = ($currentDataObj."$productColumnName").split($delimiter)
                    $priceArray = ($currentDataObj."$priceColumnName").split($delimiter)                                

                    for ($l = 0; $l -lt $currentQuantity; $l++) {
                        $currentDataObj."$productColumnName" = $productArray[$l].Trim()
                        $currentDataObj."$priceColumnName" = $priceArray[$l].Trim()
    
                        $data.Add($currentDataObj)
                    }        
                } else {
                    $data.Add($currentDataObj)                        
                }
            }            
        }
    
        Log -Level "Info" -Msg "File parsed"

        $data | Export-Csv -Path "$outputFolderPath\Orders.csv" -Delimiter ';' -Force -NoTypeInformation -Encoding UTF8
    }
    catch {
        Log -Level "Error" -Msg "An error occured.."                    
        $PSItem.InvocationInfo | Format-List *
        $PSItem.ScriptStackTrace
    }
    finally {
        Log -Level "Info" -Msg "Cleanup.."

        $workBook.Close();
        $objExcel.Quit();    
    }
}