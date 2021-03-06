#REQUIRES -VERSION 4.0

<#
.SYNOPSIS
	Netomity - Bud Henderson Proxy Instance
.DESCRIPTION
	Sets up this computer to be a TCP -> Serial port gateway to another netomity instance
.NOTES
	File Name      : bud-proxy.ps1
    Author         : Jason Sharpee <jason@sharpee.com>
    Prerequisite   : PowerShell V4 over Vista and upper.
    Copyright 2015 - Jason Sharpee
.LINK
	http://www.netomity.com
#>
write-host "" 
$ErrorActionPreference = "Stop" 
$VerbosePreference = "Continue" 

Import-Module C:\netomity\NetomityPS.dll -verbose

$logger = New-Logger -FileName "c:\temp\netomity-log.txt"
$ns = New-NetomitySystem
$insteonPort = New-SerialIO -Name "InsteonSerial" -PortName "COM5" -PortSpeed 19200
$w800Port = New-SerialIO -Name "W800Serial" -PortName "COM4" -PortSpeed 4800

$insteonTCP = New-TCPServer -Name "InsteonTCP" -PortNumber 3333
$w800TCP = New-TCPServer -Name "W800TCP" -PortNumber 3334

$insteonBC = New-BasicConnector -Name "InsteonBC" -Interfaces $insteonPort, $insteonTCP
$w800BC = New-BasicConnector -Name "W800BC" -Interfaces $w800Port, $w800TCP

$insteonTask = $insteonBC.Open()
$w800Task = $w800BC.Open()

$ns.Run()
