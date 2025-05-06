using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;

class Result
{

    /*
     * Complete the 'timeConversion' function below.
     *
     * The function is expected to return a STRING.
     * The function accepts STRING s as parameter.
     */

    public static string timeConversion(string s)
    {
        string hourString = s.Split(":")[0];
        int hourInt = Int32.Parse(hourString);
        string timeAt = s.Substring(8,2);
        
        if(timeAt == "AM" && hourInt == 12){
            hourInt = 0;
        }
        
        if(timeAt == "PM" && hourInt!= 12){
            hourInt = (12 + hourInt)%24;
        }
    
        return hourInt.ToString().PadLeft(2, '0') + s.Substring(2,6);
    }

}

class Solution
{
    public static void Main(string[] args)
    {
        TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

        string s = Console.ReadLine();

        string result = Result.timeConversion(s);

        textWriter.WriteLine(result);

        textWriter.Flush();
        textWriter.Close();
    }
}
