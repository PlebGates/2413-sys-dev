using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

public class ExcelClass
{
    public static bool IsFileinUse(FileInfo file) //https://www.codeproject.com/Answers/493096/c-pluscheckingplusifplusaplusfileplusisplusalrea
    {
        FileStream stream = null;

        try
        {
            stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None); //open file with rw access
        }
        catch (IOException Ex)
        {
            //the file is unavailable because it is:
            //still being written to
            //or being processed by another thread
            //or does not exist (has already been processed)
            return true;
        }
        finally
        {
            if (stream != null) //if it opened the file, close it again
                stream.Close();
        }
        return false;
    }
    public void ExcelCreate()
    {
        
        ExcelData xlData = new ExcelData(); //create new excel data
        OpenFileDialog dialog = new OpenFileDialog()
        {
            Filter = "xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*", //excel files, all files - open file window
            FilterIndex = 1 //set index to xlsx
        }; //new file dialog box
           //if (dialog.ShowDialog() == DialogResult.OK) //when ok button is clicked
        if (dialog.ShowDialog() == true) //when ok button is clicked
        {
            xlData.setFileName(dialog.FileName); //set exceldata filename to the selected spreadsheet
        }
        if (dialog.FileName == "") //if selections was cancelled
        {
            System.Environment.Exit(1); //skip the rest of the program
        }

        //check if file is in use
        if (IsFileinUse(new FileInfo(xlData.getFileName()))) //if the excel file is open
        {
            //collect garbage and close the file
            GC.Collect();
            GC.WaitForPendingFinalizers();
            MessageBox.Show("Please close any open instances of the spreadsheet before running this program.");
            System.Environment.Exit(1);
        }

        // creating COM objects for the excel sheet
        Excel.Application xlApp = new Excel.Application(); //open the excel com object
        Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(xlData.getFileName()); //open the target workbook
        Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1]; //open the first worksheet
        Excel.Range xlRange = xlWorksheet.UsedRange; //set the range to the used range
        Excel.Worksheet outputSheet = xlWorkbook.Worksheets.Add(After: xlWorksheet); //create a new output sheet

            xlWorkbook.Save(); //save the excel file to keep the output sheet

            //////////////////////////////////////cleanup///////////////////////////////////////////
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //rule of thumb for releasing com objects:
            //  never use two dots, all COM objects must be referenced and released individually
            //  ex: [somthing].[something].[something] is bad

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);
            Marshal.ReleaseComObject(outputSheet);

            //close and release
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
    }
}

public class ExcelData
{
    string fileName = null;

    public ExcelData(string filename)
    {
        fileName = filename;
    }
    public ExcelData()
    {

    }
    public void setFileName(string filename)
    {
        fileName = filename;
    }
    public string getFileName()
    {
        return fileName;
    }
}


        