## Command Line Parsers

The following command will provide a list of available parsers. 

```ps1
    cli-parsers.exe --help
```

For help on a specific parser. 

```ps1
    cli-parsers.exe [ParserName] --help
```

Examples:

```ps1
    cli-parsers.exe OrderParser --input c:\temp\in\NL-Ordre.xlsx --output c:\temp\out\Orders.csv --header-line-no 1 --ws-name "Ny Ordre" --delimiter ','

    cli-parsers.exe ShippingLabelParser --input c:\temp\in\NL_Fraktetikett.xlsx --output c:\temp\out\ShippingLabels.csv --header-line-no 1 --ws-name "Ark1" --point-in-time-file c:\temp\cut-off.tmstmp
```

## Powershell Tools
Set-ExecutionPolicy: `powershell -Command "Set-ExecutionPolicy -ExecutionPolicy ByPass  -Scope CurrentUser"` (Maybe needed to run these CmdLet)

### Order Parser
Command:             `powershell .\OrderParser.ps1` (Run it from the folder where OrderParser.ps1 is located)

### Shipping Label Parser
Command:             `powershell .\ShippingLabelParser.ps1` (Run it from the folder where ShippingLabelParser.ps1 is located)
