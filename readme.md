# Videogame Assay

An interactive closed loop environment.

## How to get things running

1. Install NVIDIA drivers
2. Install Visual Studio 2012 for Windows Desktop
3. Install Fly Capture (version now installed 2.11.3.121 SDK 64 bits), plug the blue cable in a red 3.1 USB port since it works better than a blue USB 3 port
4. Install Bonsai from the website in order to be able to allocate autimatically the dependency needed, aftre remove Bonsai since we will be using Bonsai64 in the Videogame folder (workflow needs to be opened using this version of Bonsai)
5. in case of Bonsai missing packages, look for  the missing one in videogame assay-bonsai-packages and grag the nupkg file in the package folder where it is missing
6. Power Shell is already installed in Windows, run it first as administrator as follow: in the commamd line write, Set-ExecutionPolicy Unrestricted and then Y, drag the videogame assay power shell file in the videogame folder on the power
   shell icon, right click on it to initialize the Bonsai workflow of interest
7. Download Ffmpeg zio file from Zeranoe ffmpeg website, go to bin and drag the ffmpeg.exe in videogame assay-bonsai-extensions
8. Install Opal Kelly from the Intan website- dowloads- interface software-USB interface board drivers, click on drivers and run the zip file, in windows pull the FrontPanelUSB file in downloads
9. Install RDH2000 evaluation system compiled version 1.5.1 for Windows, extract the content in Software....
10. Remove Windows 10 animations
11. St the refresh rate of the monitor (2) at 120Hz


1. Run Bonsai.exe inside the bonsai folder in order to bootstrap the environment.
2. Open Bonsai.Videogame and build the solution in Release mode.
3. Launch the Videogame-Assay powerscript and select your game.

## Useful links

- [Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet)
