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

Import-Module C:\netomity\NetomityPS.dll -verbose
#Import-Module C:\projects\Netomity\NetomityPS\bin\Debug\NetomityPS.dll -verbose

$logger = New-Logger -FileName "c:\temp\netomity-log.txt"
$ns = New-NetomitySystem
$insteonPort = New-SerialIO -Name "InsteonSerial" -PortName "COM5" -PortSpeed 19200
$w800Port = New-SerialIO -Name "W800Serial" -PortName "COM4" -PortSpeed 4800

$insteonTCP = New-TCPServer -Name "InsteonTCP" -PortNumber 3333
$w800TCP = New-TCPServer -Name "W800TCP" -PortNumber 3334

$insteonBC = New-BasicConnector -Name "InsteonBC" -Interfaces $insteonPort, $insteonTCP
$w800BC = New-BasicConnector -Name "W800BC" -Interfaces $w800Port, $w800TCP

$insteon = New-Insteon -Name "InsteonHA" -Interface $insteonPort
$w800 = New-W800 -Name "W800HA" -Interface $w800Port

#netsh http add urlacl url=http://+:8082/ user=Everyone
#netsh http add urlacl url=http://192.168.12.161:8083/ user=Everyone
$wh = New-WebHost -Name "Web Host" -URL "http://192.168.12.161:8083/" -FilePath "C:\netomity\Content\"

######
# Devices
######
$test_lamp = New-StateDevice -Name "Test Lamp 1" -Address "00.5B.5d" -Interface $insteon
$test_motion = New-StateDevice -Name "Test Motion 1" -Address "j1" -Interface $w800

# Master Bedroom Devices
$master_fan = New-StateDevice -Name "Master Fan" -Address "1f.ad.76" -Interface $insteon
$master_light = New-StateDevice -Name "Master Light" -Address "38.2e.a2" -Interface $insteon

$family_door = New-StateDevice -Name "Family Door" -Address "32.7E.40" -Interface $insteon
$kitchen_door = New-StateDevice -Name "Master Fan" -Address "32.7A.34" -Interface $insteon


$ns.Run()