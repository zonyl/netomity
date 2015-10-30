# Netomity

---

Netomity is an extensible device communication and automation system written in .NET and configured via Powershell. 
It's uses include home automation and lighting control but is certainly not limited to 
that.  It is supported on any platform that support .NET Core ( Windows, Mac OS-X, Linux, etc )

#### Supported
Netomity currently has support for the following hardware interfaces with 
more planned in the future.

   - [Insteon](http://www.insteon.com/) / X10 (2412N, 2412S, 2413U)
   - [WGL Designs](http://wgldesigns.com/w800.html) W800RF32 X10 RF receiver (W800/RS232)
   

### Future
   - [Radio Thermostat](http://www.radiothermostat.com/ ) WiFi Enabled Thermostat (CT30)
   - [Arduino](http://www.arduino.cc) Uno board (USB)
   - [X10](http://x10pro-usa.com/x10-home/controllers/wired-controllers/cm11a.html) CM11a (RS232)
   - [Spark I/O](http://www.spark.io) WiFi devices
   - Plum WiFi Devices

### FEATURES
   - Written in .NET / Powershell
   - REST API
   - Local Web access
   - Unique language to describe devices and actions
   - Smart objects: Doors, Lights, Motion, Photocell etc.
   - Optional “Mainloop” programming, for more complicated control
   - Optional “Event driven” programming, for complex actions when a device state changes
   - Time of day on and off control
   - Delays for time off
   - Idle command, device will return to "idle" state
   - Map one command to another with optional source and time
   - Good hardware support with more coming
   - Very easy to add new hardware drivers
   - Much more

---
###USAGE - PowerShell script

```
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

$ns.Run()
```
---

###INSTALLATION


#### DEPENDENCIES

Before you can create an instance and run Netomity automation software you must satisfy a few dependencies. Netomity is written in .NET and currently has been tested under versions 4.5

Netomity also requires the following packages to be installed for normal operation:
 p
#### Permissions
Like with all other interfaces. Make sure the powershell user account owns or otherwise has permissions to use the device. You may want to give your own usr account access as well.

INSTALL
=======
You are now ready to install Netomity. First, clone the netomity git repository. 


