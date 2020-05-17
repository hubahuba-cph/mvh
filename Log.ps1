Function Log {
    param(
        [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()][ValidateSet('Verbose', 'Debug', 'Info', 'Warning', 'Error')][String]$Level,
        [Parameter(Mandatory=$true)][String]$Msg
    )

    if(-not (Test-Path "Variable:\Script:LOGFILE")) {
        $logFile = "log.log"
        
    }
    else {
        $logFile = $Script:LOGFILE
    }

    # TODO: Add LogLevel Handling
    New-Item -Path .\log -ItemType Directory -ErrorAction SilentlyContinue | Out-Null
    Add-Content -Path .\log\$logFile -Encoding UTF8 -Value "$(Get-Date -format "yyyy-MM-ddTHH:mm:ss.FFF") [$($Level.ToUpper())]: $Msg" -Force
}