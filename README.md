# FolderSorter

Folder sorting tool, used to sort files into folders based on extensions, specifically within the user's Downloads folder. This is a remake of one of my previous repos (DownloadSorter), except in C# and with a very bare-bones GUI. The main reason for the remake was that I wanted an app that could run in system tray, rather than a console, and I wanted to experiment with C#.

FolderSorter will immediately run into the system tray, and will be active. Upon first run, it will create folders based on the extensions.csv file (more on this later). User can double click the icon to bring up the full GUI, where they can turn it on, turn it off, close out, etc. The GUI is very bare-bones; I'm thinking about adding new features, but there's really nothing I can think of that I'd want. Minimizing the window instantly puts it into system tray.

FolderSorter requires an extensions.csv file in the Downloads folder. This .csv file has folder names in the first column, and the extensions relating the folder names in the same row to the right of it. It can be easily expanded to add new folders/extensions.

The utility will run every 15 seconds.
This should be easy to setup to run on computer boot if the user wishes to.

Room to grow: 
* It only works with one directory; the user's Downloads folder. Adding a textbox so the user can modify the directory path would be really useful for alternative applications
* Having the program create the extensions.csv file in the chosen directory on first run would be handy for portability and ease of use. 
