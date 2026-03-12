using System;
using System.Collections.Generic;


using AVcontrol;



namespace JabrAPI.Noise
{
    static internal partial class Internal
    {
        static private Byte[] RemovalRound(
            List<Byte> noised, ref bool ignoringIsActive,
            List<Byte> primaryNoise, List<Byte> complexNoise)
        {
            Int32 dataStartId = 0;
            List<Byte> dynamicResult = new(noised.Count);

            for (var i = 0; i < noised.Count; i++)
            {
                var curChar = noised[i];

                if (complexNoise.Contains(curChar))
                {
                    if (!ignoringIsActive)
                        dynamicResult.AddRange(
                            Utils.Interval(noised, dataStartId, i));

                    ignoringIsActive = !ignoringIsActive;
                    dataStartId = i + 1;
                    continue;
                }
                else if (primaryNoise.Contains(curChar))
                {
                    if (!ignoringIsActive)
                        dynamicResult.AddRange(
                            Utils.Interval(noised, dataStartId, i));

                    dataStartId = i + 1;
                    continue;
                }
            }

            if (!ignoringIsActive)
                dynamicResult.AddRange(
                    Utils.Interval(noised, dataStartId, noised.Count));

            return [.. dynamicResult];
        }
    }
}