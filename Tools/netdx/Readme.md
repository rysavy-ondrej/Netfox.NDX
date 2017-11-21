# NETDX
The tool for manipulating with captre files. It implements various operations from reading, decoding and transforming 
capture files. 

## Prepare-Trace

Reading cap file by tshark and prcessing JSON output with netdx can be done in a single command with the help of PowerShell:

```bash
C:\> powershell -Command "tshark -T ek -r file.cap | netdx Prepare-Trace -w file.dcap STDIN"
```

This seems only work for file data source. Reading data from live input seems not to work :(