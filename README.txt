Project Sub-modules:
ApplicationWatcher - This program checks the starting processes on the computer. If it sees chrome.exe start, then it starts the keylogger if the keylogger isn't active.
HookKeyLogger - This program is the keylogger hook that records keystrokes. The keystrokes and the application they were typed into are recorded in a log file called log.txt.

Start instructions:
ApplicationWatcher - Place the HookKeyLogger executable in the same folder as this program. 
HookKeyLogger - Launch the executable. 