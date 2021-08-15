using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;

namespace Simulator
{
    //Program that simulates a ShareableSpreaSheet object and many users (thread) that performs operations on it
    class Program
    {
        static ShareableSpreadSheet sheet;
        static int active_threads;

        static void Main(string[] args)
        {
            //Checks that number of arguments received is correct
            if (args.Length != 4)
            {
                Console.WriteLine("Wrong input - format is:");

                Console.WriteLine("Simulator <rows> <cols> <nThreads> <nOperations>");
                return;
            }

            //Extract data
            int row = Convert.ToInt32(args[0]);
            int col = Convert.ToInt32(args[1]);
            int nThreads = Convert.ToInt32(args[2]);
            int nOperations = Convert.ToInt32(args[3]);
            active_threads = nThreads;

            //Create new shareable spreadsheet
            sheet = new ShareableSpreadSheet(row, col);
            sheet.set_Thread_number(nThreads);

            //Fill the spreadsheet with data ("testcellXY")
            sheet.Test_fill();


            for (int i = 0; i < nThreads; i++)
            {
                //Creates and starts a new thread with the array of int as argument
                Thread t = new Thread(Program.Run_Operations);
                t.Start(nOperations);
            }

            //Infinite loop until all active thread finish their work
            while (active_threads > 0)
            {
            }

            //Saves the current sheet
            String path = "spreadsheet.dat";
            sheet.save(path);
        }

        

        //Function that runs nOperations random Operations on the spreadsheet
        static void Run_Operations(Object o)
        {
            //Extract number of operations
            int nOperations = (int) o;

            //Iterates nOperations times - each iterations a random number is choosed between 0-12, each number corresponds to an operation on the spreadsheet
            for (int i = 0; i < nOperations; i++)
            {
                Random rand = new Random();
                int num = rand.Next(0,13);

                //Choose an operation to executes on the spreadsheet from a random number - num;
                Choose_operation(num);
                Thread.Sleep(100);
            }

            //Decrement atomically the number of active threads
            Interlocked.Decrement(ref active_threads);

        }


        //Function that receives a random number n and according to it - performs an operation on the spreadsheet 
        static void Choose_operation(int n)
        {
            int caseSwitch = n;
            Random rand = new Random();

            switch (caseSwitch)
            {
                //Get cell operation - chooses a random cell
                case 0:
                    int row = 0;
                    int col = 0;
                    sheet.getSize(ref row, ref col);
                    int rand_row = rand.Next(1, row);
                    int rand_col = rand.Next(1, col);
                    String str_res = sheet.getCell(rand_row, rand_col);
                    Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: string \"" + str_res + "\" found in cell [" + rand_row + "," + rand_col + "].");
                    break;

                //Set cell operation - set a random cell with "testcellXY" (XY are 2 more random numbers)
                case 1:
                    row = 0;
                    col = 0;
                    sheet.getSize(ref row, ref col);
                    rand_row = rand.Next(1, row);
                    rand_col = rand.Next(1, col);
                    int rand_input1 = rand.Next(1, row);
                    int rand_input2 = rand.Next(0, col);
                    sheet.setCell(rand_row, rand_col, "testcell" + rand_input1 + rand_input2);
                    Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: string \"" + "testcell" + rand_input1 + rand_input2 + "\" set in cell [" + rand_row + "," + rand_col + "].");
                    break;

                //Search String operation - a random string "testcellXY" (XY are 2 random numbers) is searched
                case 2:
                    row = 0;
                    col = 0;
                    sheet.getSize(ref row, ref col);
                    rand_row = rand.Next(1, row);
                    rand_col = rand.Next(1, col);
                    String str_to_find = "testcell" + rand_row + rand_col;
                    Boolean bool_res = sheet.searchString(str_to_find, ref row, ref col);

                    //String is found
                    if (bool_res)
                    {
                        Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: string \"" + str_to_find + "\" found in cell [" + row + "," + col + "].");
                    }

                    //String is not found
                    else
                    {
                        Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: string \"" + str_to_find + "\" not found in sheet.");
                    }
                    break;

                //Exchange Rows operation - exchange between 2 random rows
                case 3:
                    row = 0;
                    col = 0;
                    sheet.getSize(ref row, ref col);
                    rand_row = rand.Next(1, row);
                    rand_col = rand.Next(1, row);
                    bool_res = sheet.exchangeRows(rand_row, rand_col);

                    //Exchange has been done
                    if (bool_res)
                    {
                        Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: rows [" + rand_row + "] and [" + rand_col + "] exchanged successfully.");
                    }

                    //Couldn't make the exchange
                    else
                    {
                        Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: could not exchange rows [" + rand_row + "] and [" + rand_col + "].");
                    }
                    break;

                //Exchange Columns operation - exchange between 2 random columns 
                case 4:
                    row = 0;
                    col = 0;
                    sheet.getSize(ref row, ref col);
                    rand_row = rand.Next(1, col);
                    rand_col = rand.Next(1, col);
                    bool_res = sheet.exchangeCols(rand_row, rand_col);

                    //Exchange has been done
                    if (bool_res)
                    {
                        Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: columns [" + rand_row + "] and [" + rand_col + "] exchanged successfully.");
                    }

                    //Couldn't make the exchange
                    else
                    {
                        Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: could not exchange columns [" + rand_row + "] and [" + rand_col + "].");
                    }
                    break;


                //Search In Row operation - Searches a random string in a random row
                case 5:
                    row = 0;
                    col = 0;
                    sheet.getSize(ref row, ref col);
                    rand_row = rand.Next(1, row);
                    rand_col = rand.Next(1, col);
                    str_to_find = "testcell" + rand_row + rand_col;
                    bool_res = sheet.searchInRow(rand_row, str_to_find, ref col);

                    //String is found
                    if (bool_res)
                    {
                        Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: string \"" + str_to_find + "\" found in cell [" + rand_row + "," + col + "].");
                    }

                    //String is not found
                    else
                    {
                        Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: string \"" + str_to_find + "\" not found in sheet in row " + rand_row + ".");
                    }

                    break;


                //Search in Column operation - Searches a random string in a random column 
                case 6:
                    row = 0;
                    col = 0;
                    sheet.getSize(ref row, ref col);
                    rand_row = rand.Next(1, row);
                    rand_col = rand.Next(1, col);
                    str_to_find = "testcell" + rand_row + rand_col;
                    bool_res = sheet.searchInCol(rand_col, str_to_find, ref row);

                    //String is found
                    if (bool_res)
                    {
                        Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: string \"" + str_to_find + "\" found in cell [" + row + "," + rand_col + "].");
                    }

                    //String is not found
                    else
                    {
                        Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: string \"" + str_to_find + "\" not found in sheet in column " + rand_col + ".");
                    }

                    break;

                //Search In Range operation - Searches a random string in a random range of [row:row2 - col:col2]
                case 7:
                    row = 0;
                    col = 0;
                    sheet.getSize(ref row, ref col);
                    rand_row = rand.Next(1, row);
                    rand_col = rand.Next(1, col);
                    int rand_row2 = rand.Next(1, row);
                    int rand_col2 = rand.Next(1, col);
                    str_to_find = "testcell" + rand_row + rand_col;
                    bool_res = sheet.searchInRange(rand_col, rand_col2, rand_row, rand_row2, str_to_find, ref row, ref col);

                    //String is found
                    if (bool_res)
                    {
                        Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: string \"" + str_to_find + "\" found in cell [" + row + "," + col + "].");
                    }

                    //String is not found
                    else
                    {
                        Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: unable to find string \"" + str_to_find + "\" in sheet.");
                    }

                    break;


                //Add row operation - Adds a new row to the spreadsheet after a randomly choosed row @@@@@@@@@@@@@@@@what to fill the new row with?@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ + (i changed the index of the loop)
                case 8:
                    row = 0;
                    col = 0;
                    sheet.getSize(ref row, ref col);
                    rand_row = rand.Next(1, row);
                    bool_res = sheet.addRow(rand_row);

                    //Row has been added successfully
                    if (bool_res)
                    {
                        Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: a new row was added after row " + rand_row + ".");
                    }

                    //Row has not been added
                    else
                    {
                        Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: unable to add a new row after row " + rand_row + ".");
                    }

                    break;


                //Add column operation - Adds a new column to the spreadsheet after a randomly choosed column
                case 9:
                    row = 0;
                    col = 0;
                    sheet.getSize(ref row, ref col);
                    rand_col = rand.Next(1, col);
                    bool_res = sheet.addCol(rand_col);

                    //Column has been added successfully
                    if (bool_res)
                    {
                        Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: a new column was added after column " + rand_col + ".");
                    }

                    //Column has not been added
                    else
                    {
                        Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: unable to add a new column after column " + rand_col + ".");
                    }

                    break;


                //Get size operation - returns the size of the spreadsheet
                case 10:
                    row = 0;
                    col = 0;
                    sheet.getSize(ref row, ref col);
                    Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: sheet size is: " + row + "X" + col + ".");
                    break;


                //Set Concurrent Search Limit operation - Limits the number of search operations that can be done simultaneously
                case 11:
                    int number_users = rand.Next(1, 2 * sheet.get_Thread_number());
                    bool_res = sheet.setConcurrentSearchLimit(number_users);

                    //Limit has been set successfully
                    if (bool_res)
                    {
                        Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: Number of concurrent search operations limited to: " + number_users + ".");
                    }

                    //Unable to set a limit
                    else
                    {
                        Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: Unable to limit the number of concurrent search operations.");
                    }
                    break;


                //Save operation - Saves the current spreadsheet to 
                case 12:
                    String path = "spreadsheet.txt";
                    bool_res = sheet.save(path);
                    if (bool_res)
                    {
                        Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: file was successfully saved.");
                    }
                    else
                    {
                        Console.WriteLine("User[" + Thread.CurrentThread.ManagedThreadId + "]: unable to save the file.");
                    }

                    break;


            }
        }
    }
}
