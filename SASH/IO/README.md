
# SASH.IO namespace.

### In this namespace are contained a number of commands(separated in different classes) for creating, destroying, and running external files / programs.

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
delete * in FOLDER(path) where LIKE  
```  
Where the __*FILE*__ is a full file name with it's extension.  
The __*FOLDER*__ is the path, where we will delete a given file.  
The "path" is the current path.
If you use __*__ the program will delete __*ALL*__ files in the given folder.  
The __*LIKE*__ is a common sign in between all file names in the specified folder.  

### Example: delete * in path where test
> This example will search for every file, containing "test" in it's name, regardless it's extension and will delete it.

3. CREATE -> Creates a file or directory, relative to a given arguments.  
```  
create FILE  
create FILE in FOLDER  
```  
Where the __*FILE*__ is a full file name with it's extension.  
The __*FOLDER*__ is the path, where we will delete a given file. 

If the __*FILE*__ __does__ exsist in the specified __*FOLDER*__ you'll get __ERROR__ message.  

4. COPY -> copyes a file/directory from one place to another. 
```  
copy FILE  
copy FILE in DIRECTORY  
copy -d DIRECTORY  
copy -d DIRECTORY in ANOTHER_DIRECTORY  
```  
If FILE __doesn't exist__ you will get __error__ message.  
If FILE __exists__ in DIRECTORY you will get __error__ message. If there's no specified directory the file will be copyed in the __boot directory__.  
If DIRECTORY __doesn't exist__ or is empty you will get __error__ message.  
If DIRECTORY __exists__ in ANOTHER_DIRECTORY you will get __error__ message. If there's no specified directory the file will be copyed in the __boot directory__.  

