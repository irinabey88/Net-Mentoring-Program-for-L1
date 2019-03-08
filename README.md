# **Module1: Intorduction to Net** [Tasks](https://github.com/irinabey88/Net-Mentoring-Program-for-L1/tree/master/Module1/NetIntroduction)

### *1. Create applications for the next platform:*
  - [x] .Net Core – Console

  - [x] .Net Framework – WinForms

  - [x] Mono – GTK#

  - [x] Xamarin – Android.Native
  
  ### *Acceptance criteria*
  - input text can be entered
  - result string should be displayed in a separate window or on the form
  
### *2. Deploy and configure environments to launch applications from the first task.*
  - [x] .Net Core – Windows, Linux

  - [x] .Net Framework – Windows

  - [x] Mono – GTK# – Windows, Linux

  - [x] Xamarin – Android.Native - Emulator
  
### *3. Create .Net Standard library that returns formated string with entered name and current time.*
   ### *Acceptance criteria*
  - all projects format the greeting with a call to the created library.

# **Module2: Advanced C#** [Tasks](https://github.com/irinabey88/Net-Mentoring-Program-for-L1/tree/develop/Module2/Advanced/FileManager)

### *1. Create class FileSystemVisitor that allows to get through the folder tree of the file system starting from the given path:*
  
  ### *Acceptance criteria*
  
  - [x] Return all found files and folders as a linear sequence, which  should be implemented like iterator (using the yield operator)

  - [x] The filtering algorithm should be set when the instance of FileSystemVisitor is initialised. The algorithm should be defined as a delegate/lambda.

  - [x] Generate notifications (through the event mechanism) about the stages of their work. Next events should be implemented

    - Start/Finish.

    - FileFound/DirectoryFound and FileFiltered/DirectoryFiltered. These events should allow (through setting special flags in the passed parameters):

      - interrupt search

      - exclude files/folders from the final list
      
 ### *2. Develop a test library that demonstrate the different modes of operation of FileSystemVisitor.*
