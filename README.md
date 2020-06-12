# EQWindowOpener
A simple tool to open Everquest Client Windows with a standard naming convention in the window title. This allows popular hotkey tools to be used in order to switch clients. 

## Configuration
Set EqDirectory paramater in the App.Config (EQWindowOpener.exe.config) file to point to the location where eqgame.exe resides.
```XML
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.7.2" />
    </startup>
  <appSettings>
    <add key="EqDirectory" value="C:\wfh_v2"></add>
  </appSettings>
</configuration>
```

## Usage
Run EQWindowOpener.exe. Each window will be given a sequential title (Client1,Client2,Client3, etc). The proccess will remain open in order to rename the window when the client periodically changes it back to "EverQuest". If running as administrator, the application must be in an accessible location (Example: C:\EQWindowOpener\). It's recommended to increase client background FPS in order to resolve issues with autofollow etc. '

## Example HotkeyNet Script
Below is example script for the [HotkeyNet](http://hotkeynet.com/) application. This uses the Numpad 1-5 keys to switch between clients. It also allows the use of left ctrl + F12 to open new clients.
```XML
<Hotkey Lctrl F12>
<SendPC local>

	<Run "EQWindowOpener.exe">
	<Wait 2000>

<hotkey Numpad1>
	<if WinExists "Client1">
		<TargetWin "Client1">
		<SetForegroundWin>
		<SendFocusWin>
	<else>
	<endif>
<hotkey Numpad2>
	<if WinExists "Client2">
		<TargetWin "Client2">
		<SetForegroundWin>
		<SendFocusWin>
	<else>
	<endif>
<hotkey Numpad3>
	<if WinExists "Client3">
		<TargetWin "Client3">
		<SetForegroundWin>
		<SendFocusWin>
	<else>
	<endif>
<hotkey Numpad4>
	<if WinExists "Client4">
		<TargetWin "Client4">
		<SetForegroundWin>
		<SendFocusWin>
	<else>
	<endif>
<hotkey Numpad5>
	<if WinExists "Client5">
		<TargetWin "Client5">
		<SetForegroundWin>
		<SendFocusWin>
	<else>
	<endif>
<hotkey NumpadHome>
	<if WinExists "HotkeyNet 0.1.45 build 210">
		<TargetWin "HotkeyNet 0.1.45 build 210">
		<SetForegroundWin>
		<SendFocusWin>
	<endif>
```