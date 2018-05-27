# TestTasks
Test tasks from companies which invite me to the job

# Test Task 1 & Task 1 Updater 
Need to write a program which can be updated by itself

# Test Task 2 
Need to write a program to work with xml

Write on WPF with MVVM

Main functionality:
  - Choose file and parse it with next format:
    - File struct: 
      <File FileVersion=«FileVersion»>
        <Name>Name</Name>
        <DateTime>ModifyDate</DateTime>
      </File> 
    - The file name is in the format «XX_YY_ZZ.xml», where:
      - XX – a set of Russian letters. Letters count <= 100
      - YY – a set of numbers. Numbers count –  1 or 10 or from 14 to 20
      - ZZ – any characters. Characters count <= 7. 
  - Upload parsed file information into DB
  - Show and Edit DB information
  - Extract DB to file
  - Logging
