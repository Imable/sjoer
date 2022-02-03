# AR Facilitated Vessel Navigation (HoloLens 2)

## About

## Installation Guide

### Prerequisites
* Unity 2020.3.17f1
* Mixed Reality Toolkit 2.7.2 (included in the project)
* Visual Studio 2019 (16.3)

  _Include in installation_
  * Desktop development with C++
  * Universal Windows Platform (UWP) development
  * Windows 10 SDK version 10.0.19041.0 or 10.0.18362.0
  * USB Device Connectivity (required to deploy/debug to HoloLens over USB)
  * C++ (v142) Universal Windows Platform tools (required when using Unity)
* Git (to download the project)

### Enable Developer Mode on HoloLens 2
* Turn on your HoloLens 2 and navigate to `Settings` > `Updates` > `For Developers`
* Enable `Use Developer Features` in order to deploy applications from Visual Studio
* I recommend to enable `Windows Device Portal` in this menu too (https://docs.microsoft.com/en-us/windows/mixed-reality/develop/advanced-concepts/using-the-windows-device-portal)

### Enable Developer Mode on your PC
* Navigate to `Settings` > `Updates and Security` > `For Developers`
* Enable `Developer Mode` and click `Yes` to accept the change

### Get the project up and running
* Clone the project in a directory using `git clone https://github.com/Imable/ar-coastal-sailing.git`
* Go to https://www.barentswatch.no/minside/
  * Create an account
  * Create a client below "Mine klienter"
  * Create a file in `Assets/Resources/Config/` named `barentswatch_conf.json` with the following content, where you replace `...` with the credentials provided by Barentswatch
```
{
    "token_url": "...",
    "ais_url": "...",
    "auth_format": "client_id={0}&scope=api&client_secret={1}&grant_type=client_credentials",
    "client_id": "...",
    "client_secret": "..."
}
```
* Open the project in Unity
* Go to `File > Build Settings`
* Make sure the settings match the ones below

```
Target Device: HoloLens
Architecture: ARM64
Build Type: D3D Project
Target SDK Version: Latest Installed
Minimum Platform Version: 10.0.10240.0
Visual Studio Version: Latest Installed
Build and Run on: USB Device
Build Configuration: Release
```

_The remaining checkboxes should be deselected when creating a release build_

* Click `Build`
* Select a folder where the project should be built to
* Go into the export folder and open the Visual Studio project by double-clicking `ar-coastal-sailing.sln`
* Now connect the HoloLens 2 to your PC using USB and wait until Windows recognizes it
* In Visual Studio, switch `Debug` to `Release`, `ARM` to `ARM64` and `Remote Device` to `Device` (by clicking the small arrow on the right side of `ARM`)
* Then click the `Debug` tab on top of Visual Studio and select `Start without debugging`

_The project is now being build and compiled for HoloLens. This will take around 5-10 minutes, depending on your system. Leave the HoloLens plugged into your PC until it makes a sound and the project is opened automatically_

### Connect to GPS on your phone
* Download the NetGPS Android app (https://play.google.com/store/apps/details?id=com.meowsbox.netgps&hl=en&gl=US)
*  Disconnect your phone from WiFi and create a hotspot with your phone and connect the HoloLens to it

  _Note that this will use mobile data (but it should not be a crazy amount)_
* In NetGPS, open the second tab and create and enable a server with _type: TCP_, _port: 6000_
* Then click the arrow on top to show your IP address
* Insert the IP address from the app at `wlan0` into the config file in the Unity project at `Assets/Resources/Config/conf.json` (at the bottom of the file)
* If we now run the application on the HoloLens, it should connect to the phone

## Troubleshooting

### Error HRESULT E_FAIL has been returned from a call to a COM component
https://www.niceonecode.com/Question/20646/Error-HRESULT-E_FAIL-has-been-returned-from-a-call-to-a-COM-component
