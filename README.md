# FileAccessManipulator

A Windows console app that lets the current user access a file only during
chosen time intervals, by rewriting the file's NTFS permissions.

## How it works

Menu (keys `1`–`5`; `0` returns to the menu):

1. Set the path to the file.
2. Add a time interval in the form `HH:MM:SS - HH:MM:SS`.
3. Delete one of the added intervals.
4. Turn the monitor on.
5. Turn the monitor off (pause).

While the monitor is on, a background thread checks the clock every second.
Inside any of the intervals the current Windows user gets an explicit
*Allow Full Control* entry on the file; outside them it is replaced with
*Deny Full Control*, so the file can't be opened.

Intervals are kept in memory only and apply to the day they were entered.

> **Note.** The app edits the file's access control list directly and doesn't
> restore it. If you quit while access is denied, the Deny entry stays on the
> file; remove it in the file's Properties → Security → Advanced. Try it on a
> test file.

## Tech

- C#, .NET Framework 4.5.2, console application
- `System.Security.AccessControl` (`FileSecurity`, `FileSystemAccessRule`)
- Windows / NTFS only

## Build and run

Open `MBKS_Galkin_01.sln` in Visual Studio (originally Visual Studio 2015),
build and run.

## Context

University project, Omsk State University, 2017: lab 1 of the
"Security Models of Computer Systems" (MBKS) course. Labs 2 and 3:
[DirectoryAccessMatrixManipulator](https://github.com/megarohas/DirectoryAccessMatrixManipulator),
[DirectoryAccessMandatoryManipulator](https://github.com/megarohas/DirectoryAccessMandatoryManipulator).
