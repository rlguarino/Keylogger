# Simple Hook Keylogger

Project Dependencies:
* Visual Studio 2014 or greater
* NuGet Packages manager 3 or greater

# Installation
Install .NET 4.5.2

## Source

The only way to acquire the project is to build it from source.

### Download Package Dependencies

Right click on the `HookKeylogger` solution in Visual Studio and click "Restore NuGet packages", this will download
all of the packages and dependencies required to build the solution automatically.

### (Optional) Build Probuf Source

The project make use of Google Protobuf and gRPC packages which generate code C# code from a simple domain language.
The generated files are distributed with the source so if you don't change any of the files with a ".proto" extension you can skip this step.

To build the protocol buffer generate source files run the 'gen.bat' program found in the base and KeypressAggergator projects.

### Build the solution

At this point you can build the solution using the Visual Studio build in build tools.

# Running the application

To start the Keylogger you need to start two programs, the KeyPressAggregator and the HookKeyLogger.
First start the KeyPressAggregator by running the executable then start the HookKeyLogger using visual studio.
You can instead only start the Application Watcher which will start the other two EXEs.
See be the sub-modules for parameter requirements for starting the EXEs.
It's recommended to start the KeyLogger using Visual Studio because it's hard to find to shutdown once the window disappears.

You can also start these applications by executing the EXE in the /bin/Debug folder with the command line and give the appropriate command line arguments. 
These EXEs must be started in the correct order KeyPressAggregator -> HookKeyLogger
You can also only start the ApplicationWatcher which will start the other two EXEs. 
See be the sub-modules for parameter requirements for starting the EXEs.

# Architecture

There are Three main components of the project. Thee hook, the aggregator and the server.

The hook is responsible for intercepting the messages in the windows hook stack and forwarding the messages to the aggregation component.
The aggregation component processes streams of incoming KeyPress messages and extracts useful information from them.
It then sends the information extracted from the keypress stream to the server.'
The server saves the extracted confidential information in a way that the attacker has access to.
The server is the only component that is running on an entirely attacker controlled machine and not a target's machine.

The application watcher program is responsible for initially starting services and restarting services that were killed by the user. 

## Project Sub-modules:

### Decrypt

This is for decrypting the encrypted files. We stopped using this when we switched over to using buffers for the aggregator service.

### Base

A Class Library which container base classes to be used by other parts of the application.

### ApplicationWatcher
PARAMETERS: This takes two parameters which are the location of the EXEs for the KeyPressAggregator and the HookKeyLogger.

This program starts up the KeypressAggregator and the HookKeyLogger. 
If either of these processes get terminated, then it waits 10 seconds and restarts them.

This program must be run with Admin privileges.

### KeypressAggregator
PARAMETERS: This takes one parameter which is the address of the server.

A process which receives the incoming stream of KeyPress messages and aggregates them, extracting useful information and forwarding it to the server.
Only characters which relate to possible passwords or credit cards are recorded.
This process will not upload if a blacklisted program is active such has Wireshark.

### HookKeyLogger

A program installs the keylogger hook that records keystrokes. The keystrokes and their metadata are forwarded to the KeypressAggergator by this program.
The data type, time, and application the characters were typed into are recorded.

### Server

A console program that listens for incoming information and stores it permanently. This program exists on an attacker controlled machine, it provides the attacker with the extracted information.

### Decrypt key
key = w8v*e!d#
