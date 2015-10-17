#REQUIRES -VERSION 4.0

<#
.SYNOPSIS
	Netomity - Bud Henderson Main Instance
.DESCRIPTION
	Full House Implementation
.NOTES
	File Name      : bud-main.ps1
    Author         : Jason Sharpee <jason@sharpee.com>
    Prerequisite   : PowerShell V4 over Vista and upper.
    Copyright 2015 - Jason Sharpee

    ** Do Not Run in the ISE as threading model will not work.  Run in powershell directly.
.LINK
	http://www.netomity.com

#>
write-host "" 
$ErrorActionPreference = "Stop" 
$VerbosePreference = "Continue" 

#Import-Module C:\netomity\NetomityPS.dll -verbose
Import-Module C:\projects\Netomity\NetomityPS\bin\Debug\NetomityPS.dll -verbose

$logger = New-Logger -FileName "c:\temp\netomity-log.txt"
$ns = New-NetomitySystem
$insteonPort = New-SerialIO -Name "InsteonSerial" -PortName "COM5" -PortSpeed 19200
$w800Port = New-SerialIO -Name "W800Serial" -PortName "COM4" -PortSpeed 4800

$insteonTCP = New-TCPServer -Name "InsteonTCP" -PortNumber 3333
$w800TCP = New-TCPServer -Name "W800TCP" -PortNumber 3334

$insteonBC = New-BasicConnector -Name "InsteonBC" -Interfaces $insteonPort, $insteonTCP
$w800BC = New-BasicConnector -Name "W800BC" -Interfaces $w800Port, $w800TCP

#$insteonTask = $insteonBC.Open()
#$w800Task = $w800BC.Open()

$insteon = New-Insteon -Name "InsteonHA" -Interface $insteonPort

$wh = New-WebHost -Name "Web Host" -URL "http://localhost:8083/" -FilePath "C:\projects\Netomity\Netomity\Web\Content\"

$test_lamp = New-StateDevice -Name "Test Lamp 1" -Address "00.5B.5d" -Interface $insteon

$ns.Run()
