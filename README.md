# Shareable SpreadSheet Simulator

Simulator of multi-threaded operations on a shared SpreadSheet for OS course in SISE department of BGU University. Solution for the readers-writers problem.

![image](https://user-images.githubusercontent.com/66309521/129537715-4fa292e4-837c-4099-b622-ea584662f0c0.png)

@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@CHANGE THE PICTURE@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@


## Table of Content
* [General Information](#General-Information)
* [Technologies](#Technologies)
* [Supported Operations](#Supported-Operations)
* [Synchronization & Thread Safety](#Synchronization-&-Thread-Safety)

## General Information
First part of the Shareable SpreadSheet app project - implements an operation simulator on a shared SpreadSheet that can be accessed and modified concurrently by multiple threads.
Sheet is a two-dimentional array of strings that holds the data. Multiple threads can access the sheet using a series of locks (mutexes and semaphores) to either read/modify data.

## Technologies
.NET Core 3.1

## Supported Operations
- Constructor - Creates a new custom size SpreadSheet.
- Get Cell - Returns the data on a specific cell in the Sheet.
- Set Cell - Modify the data on a specific cell in the Sheet.
- Search String - Searches the given string in the Sheet and returns it if found.
- Exchange rows/columns - Swaps the content between 2 rows/2 columns.
- Search in row/column - Performs search in a specific row/column.
- Search in range - Performs search within a specific range of [row1:row2, col1:col2]
- Add row/column - Adds a new row/column to the sheet after a specified row/column.
- Get size - Returns the number of rows and columns of the Sheet.
- Set Concurrent Search Limit - Limits the number of threads that can run concurrently
- Save - Saves the SpreadSheet data onto a file.
- Load - Loads an existing SpreadSheet file.

## Synchronization & Thread Safety
The SpreadSheet can handle multiple threads running concurrently and performing different operations. It uses a number of different locks according to the operation type being performed. The operation types are categorized as follow:

- Readers operations: Read-only operations that do not change the data of the sheet. There can be many readers operations that run simultaneously. The operations classified as "readers" are: Get Cell, Search String, Search in row/columns, Search in range, Get Size.

- Writers operations: Operations that modify the data of a single cell of the SpreadSheet. There can be many writers operations that run simultaneously. The only operation classified as "writer" is: Set Cell.

- Structural operations: Operations that modify the data of multiple cells/entire SpreadSheet. Structural operations are not allowed to run simultaneously because they require the entire SpreadSheet to be "locked". The operations classified as "structural" are: Exchange row/column, Add row/column, Set Concurrent Search Limit, Save, Load.

Every operation type has its own locks, and some locks are used to distinguish the operation type that will run next. Moreover, each type has its own Entry Section and Exit Section that locks/unlocks the correct mutexes and semaphores to allow the critical section of the code to run safely. Different operation types cannot run simultaneously.

![image](https://user-images.githubusercontent.com/66309521/129554906-a0b81151-846d-4251-bbb3-750e17ec917f.png)

