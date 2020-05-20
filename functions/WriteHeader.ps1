Function WriteHeader {    
    [CmdletBinding()]
    param (        
        [Parameter()][String]$Log,
        [Parameter()][String]$InPath,
        [Parameter()][String]$Output
    )

    Write-Host ""
    Write-Host -ForegroundColor Green "OrderParser powered by hubahuba"
    Log -Level "Info" -Msg "OrderParser powered by hubahuba"

    Write-Host -ForegroundColor Green "--------------------------------------------------"
    Log -Level "Info" -Msg "--------------------------------------------------"

    if([String]::IsNullOrEmpty($Log)) {
        Write-Host -ForegroundColor Green "Log:     ./log.log"
        Log -Level "Info" -Msg "Log:     ./log.log"
    }
    else {
        Write-Host -ForegroundColor Green "Log:     $Log"    
        Log -Level "Info" -Msg "Log:     $Log"
    }

    Write-Host -ForegroundColor Green "Input:   $InPath"
    Log -Level "Info" -Msg "Input:   $InPath"

    Write-Host -ForegroundColor Green "Output:  $Output"
    Log -Level "Info" -Msg "Output:  $Output"
}
