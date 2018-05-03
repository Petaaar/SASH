# SASH project.
by Petar Angelov - [Petaaar](https://github.com/Petaaar)

### The SASH (or Simple Automated Script Headers) Project.

The SASH (or Simple Automated Script Headers) Project is a project, where I try to 
create simple, yet powerfull command-based system for operating with a files, folders, ect..

### Currently added commands:
1. RUN Program <WindowStyle> -> Runs a program. Via the command the program window can be modified as follows:  
```  
run cmd.exe MINIMIZED  
run cmd.exe MAXIMIZED  
run cmd.exe HIDDEN  
run cmd.exe NORMAL  
```  

The __*<CAPS_CASE>*__ words are formal parameter(s), __*case-insensitive*__. If the program don't find any parameter, the window size will be set to **normal**.  

2. DELETE -> Deletes one or many files in a specified directory.  
```  
delete FILE in FOLDER  
delete * in FOLDER  
delete FILE(*) in path  
```  
Where the __*FILE*__ is a full file name with it's extension.  
The __*FOLDER*__ is the path, where we will delete a given file.  
If you use __*__ the program will delete __*ALL*__ files in the given folder.  

3. CREATE -> Creates a file or directory, relative to a given arguments.
> Not implemented yet.

4. CLEAR -> Clears the console.  

5. EXIT -> Kills the program. Determines all processes, connected to it.
