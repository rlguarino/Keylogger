# Simple Hook Keylogger

Project Dependencies:
* Visual Studio 2014 or greater
* NuGet Packages manager 3 or greater

# Installation

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
It's recommended to start the KeyLogger using Visual Studio because it's hard to find to shutdown once the window disappears.

# Architecture

There are Three main components of the project. Thee hook, the aggregator and the server.

The hook is responsible for intercepting the messages in the windows hook stack and forwarding the messages to the aggregation component.
The aggregation component processes streams of incoming KeyPress messages and extracts useful information from them.
It then sends the information extracted from the keypress stream to the server.'
The server saves the extracted confidential information in a way that the attacker has access to.
The server is the only component that is running on an entirely attacker controlled machine and not a target's machine.

## Project Sub-modules:

### Base

A Class Library which container base classes to be used by other parts of the application.

### ApplicationWatcher

This program checks the starting processes on the computer. If it sees chrome.exe start, then it starts the keylogger if the keylogger isn't active.

### KeypressAggregator

A process which receives the incoming stream of KeyPress messages and aggregates them, extracting useful information and forwarding it to the server.

### HookKeyLogger

A program installs the keylogger hook that records keystrokes. The keystrokes and their metadata are forwarded to the KeypressAggergator by this program.

### Server

A console program that listens for incoming information and stores it permanently. This program exists on an attacker controlled machine, it provides the attacker with the extracted information.

### PrintKeyLog

A console program that prints the KeyLog generated by the Hook in a Human Readable fashion.
