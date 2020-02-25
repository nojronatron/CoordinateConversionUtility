using CoordinateConversionUtility;
using System.Collections.Generic;
using System;

namespace CoordConverterUI
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("***** Fun with Coordinate Conversion Utility dll *****");
            Console.WriteLine();

            //  testing method to filter inputs for valid gridsquare lookups
            List<decimal> LattitudeInputs = new List<decimal>();
            LattitudeInputs.Add(49.21m);    //lynn lat
            LattitudeInputs.Add(-18.02m);    //lynn lon

            LattitudeInputs.Add(-53.75m);    //montevideo lat
            LattitudeInputs.Add(-12.50m);    //montevideo lon

            LattitudeInputs.Add(08.75m);    //munich lat
            LattitudeInputs.Add(37.50m);    //munich lon

            LattitudeInputs.Add(55.4m);     //waDC lat
            LattitudeInputs.Add(-03.80m);     //waDC lon

            LattitudeInputs.Add(-17.00m);    //welling lat
            LattitudeInputs.Add(44.70m);     //welling lon

            foreach (decimal LatMinsInput in LattitudeInputs)
            {
                decimal LattitudeSteps = 2.5m;
                //decimal MultipleOfInput = Math.Truncate(LatMinsInput / LattitudeSteps);
                //decimal LowEndMultiple = MultipleOfInput * LattitudeSteps;
                decimal LowEndMultiple = (Math.Truncate(LatMinsInput / LattitudeSteps)) * LattitudeSteps;
                decimal HighEndMultiple = LowEndMultiple + LattitudeSteps;
                decimal LowEndDifference = Math.Abs(LatMinsInput - LowEndMultiple);
                decimal HighEndDifference = Math.Abs(LatMinsInput - HighEndMultiple);
                decimal NearestMultipleLat = 0m;
                if (LatMinsInput % LattitudeSteps == 0)
                {
                    NearestMultipleLat = LatMinsInput;
                }
                else if (LowEndDifference < HighEndDifference)
                {
                    if (LatMinsInput < 0)
                    {
                        NearestMultipleLat = LowEndMultiple;
                    }
                    if (LatMinsInput > 0)
                    {
                        NearestMultipleLat = HighEndMultiple;
                    }
                }
                else if (LowEndDifference > HighEndDifference)
                {
                    if (LatMinsInput > 0)
                    {
                        NearestMultipleLat = HighEndMultiple;
                    }
                    if (LatMinsInput < 0)
                    {
                        NearestMultipleLat = LowEndMultiple;
                    }
                }
                else // LowEndDifference == HighEndDifference
                {
                    if (LatMinsInput > 0)
                    {
                        NearestMultipleLat = HighEndMultiple;
                    }
                    if (LatMinsInput < 0)
                    {
                        NearestMultipleLat = LowEndMultiple;
                    }
                }

                Console.WriteLine($"For input { LatMinsInput }, selected table entry is { NearestMultipleLat }.");
            }


            //result = cc.ConvertDDMtoGridsquare($"{munich[0].ToString()},{munich[1].ToString()}");
            //Console.WriteLine($"Converted DDM {munich[0].ToString()},{munich[1].ToString()} to gridsquare {result} (should be {munich[2].ToString()}).");

            //result = cc.ConvertDDMtoGridsquare($"{lynnwoodWA[0].ToString()},{lynnwoodWA[1].ToString()}");
            //Console.WriteLine($"Converted DDM {lynnwoodWA[0].ToString()},{lynnwoodWA[1].ToString()} to gridsquare {result} (should be {lynnwoodWA[2]}).");
            //Console.WriteLine();

            //result = cc.ConvertDDMtoGridsquare($"{montevideo[0].ToString()},{montevideo[1].ToString()}");
            //Console.WriteLine($"Converted DDM {montevideo[0].ToString()},{montevideo[1].ToString()} to gridsquare {result} (should be {montevideo[2]}).");
            //Console.WriteLine();

            //result = cc.ConvertDDMtoGridsquare($"{washingonDC[0].ToString()},{washingonDC[1].ToString()}");
            //Console.WriteLine($"Converted DDM {washingonDC[0].ToString()},{washingonDC[1].ToString()} to gridsquare {result} (should be {washingonDC[2]}).");
            //Console.WriteLine();

            //result = cc.ConvertDDMtoGridsquare($"{wellington[0].ToString()},{wellington[1].ToString()}");
            //Console.WriteLine($"Converted DDM {wellington[0].ToString()},{wellington[1].ToString()} to gridsquare {result} (should be {wellington[2]}).");
            //Console.WriteLine();

            //result = cc.ConvertDDtoGridsquare(lynnwoodDD);
            //Console.WriteLine($"Converted DD {lynnwoodDD} to gridsquare {result}.");
            //Console.WriteLine();

            //result = cc.ConvertDMStoGridsquare(lynnwoodDMS);
            //Console.WriteLine($"Converted DMS {lynnwoodDMS} to gridsquare {result}.");
            //Console.WriteLine();

            //result = cc.ConvertDDtoDDM(lynnwoodDD);
            //Console.WriteLine($"Converted DD {lynnwoodDD} to DDM {result} (should be {lynnwoodWA[0].ToString()},{lynnwoodWA[1].ToString()}).");
            //Console.WriteLine();

            //result = cc.ConvertDMStoDDM(lynnwoodDMS);
            //Console.WriteLine($"Converted DMS {lynnwoodDMS} to DDM {result}.");
            //Console.WriteLine();

            //result = cc.ConvertGridsquareToDDM($"{lynnwoodWA[2].ToString()}");
            //Console.WriteLine($"Converted gridsquare {lynnwoodWA[2].ToString()} to DDM {result} (should be {lynnwoodWA[0].ToString()},{lynnwoodWA[1].ToString()}).");
            //Console.WriteLine();

            //Console.WriteLine($"LatDirection is now {cc.LatDirection}.");
            //Console.WriteLine($"LonDirection is now {cc.LonDirection}.");
            //cc.ValidateDDMinput("");
            //cc.ValidateGridsquareInput("");
            


            Console.WriteLine("Press <Enter> to exit. . .");
            Console.ReadLine();


        }
    }
}
