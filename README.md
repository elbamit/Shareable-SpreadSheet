# Shareable SpreadSheet Simulator

Simulator of multi-threaded operations on a shared SpreadSheet for OS course in SISE department of BGU University.

![image](https://user-images.githubusercontent.com/66309521/129537715-4fa292e4-837c-4099-b622-ea584662f0c0.png)


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
- Search in row/columns - Performs search in a specific row/column.
- Search in range - Performs search within a specific range of [row1:row2, col1:col2]
- Add row/column - Adds a new row/column to the sheet after a specified row/column.
- Get size - Returns the number of rows and columns of the Sheet.
- Set Concurrent Search Limit - Limits the number of threads that can run concurrently
- Save - Saves the SpreadSheet data onto a file.

## Synchronization & Thread Safety
