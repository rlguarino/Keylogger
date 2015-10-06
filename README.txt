Project Sub-modules:
ApplicationWatcher - This program checks the starting processes on the computer. If it sees chrome.exe start, then it starts the keylogger if the keylogger isn't active.
HookKeyLogger - This program is the keylogger hook that records keystrokes. The keystrokes and the application they were typed into are recorded in a log file called log.txt.
Server - This is a console program that listens for incoming messages.
ServerUpload - This is a console program that sends the logged keys to the attacker's server.

Start instructions:
ApplicationWatcher - Place the HookKeyLogger executable in the same folder as this program. 
HookKeyLogger - Launch the executable. 
Server - Launch the executable.
ServerUpload - Launch the executable with two parameters. The first paramter is the server IP address and the second parameter is the location of the keylogger log file.

All the executables are in the bin/Debug folder of their perspective module.