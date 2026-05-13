using System;



namespace JabrAPI
{
    public class OutputInterval(double probability, Int32 minLength, Int32 maxLength)
    {
        public double Probability { get; set; } = probability;

        public Int32 MinLength { get; set; } = minLength;
        public Int32 MaxLength { get; set; } = maxLength;
    }
}