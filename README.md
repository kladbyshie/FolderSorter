# FolderSorter

Folder sorting tool, used to sort files into folders based on extensions. This is a remake of one of my previous projects (DownloadSorter, in Python), except in C# and with a very bare-bones GUI. The main reason for the remake was that I wanted an app that could run in system tray, rather than a console, and I wanted to experiment with C#.

FolderSorter will immediately run into the system tray, and will be active. On first run, user needs to select a directory to watch (default is Downloads folder). When the program is turned "on", it will create a CSV file in the location that will be used to sort files to folders accordingly. User can double click the icon to bring up the full GUI, where they can turn it on, turn it off, close out, etc. The Sorter will remember the folder the user last selected, and will remember if the user has checked the "auto-start on launch" checkbox. Minimizing the window instantly puts it into system tray.

FolderSorter requires an extensions.csv file in the Downloads folder, that it automatically creates if one is not already present. The user can modify this file, adding extra lines (folder names in the 1st column, all filetypes in columns 2 onwards)

The utility will run every 15 seconds.
This should be easy to setup to run on computer boot if the user wishes to, especially given that there is a "auto-on" feature.
