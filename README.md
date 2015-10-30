# Netomity

---

Netomity is an extensible device communication and automation system written in .NET and Powershell. It's uses 
include home automation and lighting control but is certainly not limited to 
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


