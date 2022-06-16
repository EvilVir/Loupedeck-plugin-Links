# Loupedeck plugin Links
[![Build Plugin](https://github.com/EvilVir/Loupedeck-plugin-Links/actions/workflows/main.yml/badge.svg)](https://github.com/EvilVir/Loupedeck-plugin-Links/actions/workflows/main.yml)

Plugin that adds missing functionality to Loupedeck software. Build-in **Run** action is useless, as it doesn't allow you to specify arguments for the executable, nor it pulls the application's icon (instead you have this ugly black box with purple bars and text in the middle). Same goes for **Application** action - no icons, ugly purple bars.

This plugin gives you ability to launch classic Windows applications, with arguments and pulls the icon from the .exe file to present on the Loupedeck's screen. Also you can launch modern Windows applications and have their icons on the screen as well.

---

## Using the plugin

1. Download plugin package from **Releases** page
2. In your Loupedeck software go for **Install Plugin from file**
3. You should get new Universal Plugin called **Links** enabled
4. On the **Links** tab you can add either Classic Application (.exe file) or Modern Application (from MS Store).
5. Enjoy :)

## Adding link to Classic Application

* Add parameters after space: `App.exe -Param1 -Param2`
* If path to your .exe contains spaces - enclose it in double quotes: `"C:\Program Files\App\App.exe" -Param1 -Param2`
* If you want to use other than default icon, add comma and icon index after .exe path: `"C:\Program Files\App\App.exe,1" -Param1 -Param2` (will load icon from index 1)

## Adding link to Modern Application

* Just select it from the list :)

## Known issues

* Modern applications list contains duplicates and dead links - not everything you can choose from it will work (thank you Microsoft for making such simple stuff as enumerating applications, a problem).