using System;
using System.Collections.Generic;



namespace JabrAPI
{
    public partial class BinaryNoisifier
    {
        public bool ImportFromBinary(List<Byte> data, bool throwExceptions = false)
        {
            try
            {
                Byte primaryCount = data[0];
                if (data.Count < primaryCount + 1)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data length is insufficient for the specified primaryNoiseCount" +
                            $" {primaryCount} from data[0]",
                            nameof(data)
                        );
                    return false;
                }

                _primaryNoise.Clear();
                _primaryNoise.AddRange(data.GetRange(1, primaryCount));


                Byte complexCount = data[primaryCount + 1];
                if (data.Count < complexCount + primaryCount + 2)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data length is insufficient for the specified complexNoiseCount" +
                            $" {complexCount} from data[{primaryCount + 2}]",
                            nameof(data)
                        );
                    return false;
                }

                _complexNoise.Clear();
                _complexNoise.AddRange(data.GetRange(primaryCount + 2, complexCount));
            }
            catch
            {
                if (throwExceptions) throw;
                return false;
            }
            return true;
        }


        public List<Byte> ExportAsBinary()
        {
            List<Byte> result = [];

            result.Add((Byte)(_primaryNoise.Count));
            result.AddRange(_primaryNoise);

            result.Add((Byte)(_complexNoise.Count));
            result.AddRange(_complexNoise);

            return result;
        }
    }
}