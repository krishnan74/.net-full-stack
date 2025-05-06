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
     * Complete the 'plusMinus' function below.
     *
     * The function accepts INTEGER_ARRAY arr as parameter.
     */

    public static void plusMinus(List<int> arr)
    {
        int totalCount = arr.Count;
        int positiveCount = 0;
        int negativeCount = 0;
        int zeroCount = 0;
        
        foreach (int i in arr) {
            if( i == 0 ){
                zeroCount++;
            } 
            else if ( i > 0 ){
                positiveCount++; 
            }
            else{
                negativeCount++;
            }
        }
        
        
        Console.WriteLine($"{(double)positiveCount /totalCount:F6}");
        Console.WriteLine($"{(double)negativeCount /totalCount:F6}");
        Console.WriteLine($"{(double)zeroCount /totalCount:F6}");

    }

}

class Solution
{
    public static void Main(string[] args)
    {
        int n = Convert.ToInt32(Console.ReadLine().Trim());

        List<int> arr = Console.ReadLine().TrimEnd().Split(' ').ToList().Select(arrTemp => Convert.ToInt32(arrTemp)).ToList();
        
        Result.plusMinus(arr);
    }
}
