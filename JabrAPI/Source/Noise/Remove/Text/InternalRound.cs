using System;
using System.Text;


using AVcontrol;



namespace JabrAPI.Noise
{
    static internal partial class Internal
    {
        static private string RemovalRound(
            string noised, ref bool ignoringIsActive,
            string primaryNoise, string complexNoise)
        {
            Int32 dataStartId = 0;
            StringBuilder dynamicResult = new(noised.Length);

            for (var i = 0; i < noised.Length; i++)
            {
                var curChar = noised[i];

                if (complexNoise.Contains(curChar))
                {
                    if (!ignoringIsActive)
                        dynamicResult.Append(
                            Utils.Interval(noised, dataStartId, i));

                    ignoringIsActive = !ignoringIsActive;
                    dataStartId = i + 1;
                    continue;
                }
                else if (primaryNoise.Contains(curChar))
                {
                    if (!ignoringIsActive)
                        dynamicResult.Append(
                            Utils.Interval(noised, dataStartId, i));

                    dataStartId = i + 1;
                    continue;
                }
            }

            if (!ignoringIsActive)
                dynamicResult.Append(
                    Utils.Interval(noised, dataStartId, noised.Length));

            return dynamicResult.ToString();
        }
    }
}