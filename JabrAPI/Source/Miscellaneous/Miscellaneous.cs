using System;



namespace JabrAPI.Noise
{
    static internal partial class Miscellaneous
    {
        private class SyntropyBiases(double valueBias, double powerBias)
        {
            public double valueBias = valueBias;
            public double powerBias = powerBias;
        }



        static public Int32 CalculateMaxNonEntropy(
            ExpectedEntropy entropySetting,
            Int32 initial, Int32 extending)
        {
            return entropySetting == ExpectedEntropy.L0_Fast_Anything ?
                initial : MaxSyntropy(initial, extending,
                entropySetting switch
                {
                    ExpectedEntropy.L1_Fast_Low =>
                        new SyntropyBiases(1.8, 1.55),
                    ExpectedEntropy.L2_Fast_Low =>
                        new SyntropyBiases(1.6, 1.5),


                    ExpectedEntropy.C0_Medium =>
                        new SyntropyBiases(1.5, 1.4),
                    ExpectedEntropy.C1_Medium =>
                        new SyntropyBiases(1.4, 1.33),
                    ExpectedEntropy.C2_Medium =>
                        new SyntropyBiases(1.4, 1.2),


                    ExpectedEntropy.H0_Slow_High =>
                        new SyntropyBiases(1.3, 1.2),
                    ExpectedEntropy.H1_Slow_High =>
                        new SyntropyBiases(1.2, 1.15),
                    ExpectedEntropy.H2_Slow_Maximal =>
                        new SyntropyBiases(1, 1.01),

                    _ => throw new NotImplementedException()
                });
        }

        static private Int32 MaxSyntropy(
            Int32 initial, Int32 extending,
            SyntropyBiases syntropy)
                => (Int32)Math.Ceiling
                (
                    Math.Pow
                    (
                        initial * syntropy.valueBias /
                        (
                            extending - initial + 1
                        ),
                        syntropy.powerBias
                    )
                );
    }
}