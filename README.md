# MacroCoach
Macro Coach is an app that runs in the background while you are playing Starcraft 2 (or any game, I guess) and monitors your keystrokes 
and reminds you to press the right keys if you fall behind on your macro.

## How it works
The UI lets you create alerts.

An alert is 3 things:
1. A key to watch (such as the hotkey you group your production buildings to) 
2. How long to wait in between presses of this key before alerting
3. A message to read via Text-To-Speech

You can have as many alerts as you want, but typically one or two is sufficient. 

## Alerts I like to set
I have two relevant hotkeys: 4 is my SCV production and 5 is my marine production. SCVs take 12 seconds, so I create a 12 second alert
on 4 saying "Create SCV". Marines take 18 seconds so I create an 18 second alert on 5 saying "build marines". As soon as I get to 70
SCV I stop all alerts using a Autohotkey as described below.

## Starting / Stopping
You can use the start and stop buttons on the UI but that's not advisable during a game. When the app is running you can control it
through the use of commandline arguments. While the app is running simply run it again passing an action argument. For example, to stop
it you would do: MacroCoach.exe action:stop

There are 3 supported actions:
* action: start - starts alerting
* action: stop - stops alerting
* action: toggle - switches the state

Doing it as a commandline argument means you can use your mouse buttons, Autohotkey, or any other tools to control it.

## Autohotkey example
This is how I control it. In fact, the first version of this project was just a collection of Autohotkey scripts. My setup:
1. Install Autohotkey https://www.autohotkey.com/
2. Create a hotkey for Ctrl+Alt+T that runs the program passing action:toggle On my machine that looks like:
```#NoEnv  ; Recommended for performance and compatibility with future AutoHotkey releases.
; #Warn  ; Enable warnings to assist with detecting common errors.
SendMode Input  ; Recommended for new scripts due to its superior speed and reliability.
SetWorkingDir %A_ScriptDir%  ; Ensures a consistent starting directory.
^!t::
run, C:\Users\henry\apps\MacroCoach\MacroCoach.exe action:toggle
```

## Misc. other notes
* The hotkey debugger window will run the keystroke listener and dump the keys it sees to the screen. This can be used if you have
a strange device and wish to find what keys your keystrokes map to
* Most buttons in Starcraft 2 serve multiple functions. This is why I believe hotkey groups are the best target for this
