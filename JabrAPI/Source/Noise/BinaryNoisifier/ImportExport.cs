using System;
using System.Collections.Generic;



namespace JabrAPI
{
    public partial class BinaryNoisifier
    {
        public bool ImportFromBinary(List<Byte> exportData, bool throwExceptions = false)
            => ImportFromBinary(exportData.ToArray(), throwExceptions);
        public bool ImportFromBinary(Byte[] exportData, bool throwExceptions = false)
        {
            try
            {
                Byte primaryCount = exportData[0];
                if (exportData.Length < primaryCount + 1)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data length is insufficient for the specified primaryNoiseCount" +
                            $" {primaryCount} from exportData[0]",
                            nameof(exportData)
                        );
                    return false;
                }

                _primaryNoise.Clear();
                _primaryNoise.AddRange(exportData[1..(primaryCount + 1)]);


                Byte complexCount = exportData[primaryCount + 1];
                if (exportData.Length < complexCount + primaryCount + 2)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data length is insufficient for the specified complexNoiseCount" +
                            $" {complexCount} from exportData[{primaryCount + 2}]",
                            nameof(exportData)
                        );
                    return false;
                }

                _complexNoise.Clear();
                _complexNoise.AddRange(exportData[(primaryCount + 2)..(complexCount + primaryCount + 2)]);
            }
            catch
            {
                if (throwExceptions) throw;
                return false;
            }
            return true;
        }


        public Byte[] ExportAsBinary()
        {
            return
            [
                (Byte)_primaryNoise.Count,
                .. _primaryNoise,

                (Byte)_complexNoise.Count,
                .. _complexNoise
            ];
        }
        public List<Byte> ExportAsBinaryList()
        {
            return
            [
                (Byte)_primaryNoise.Count,
                .. _primaryNoise,

                (Byte)_complexNoise.Count,
                .. _complexNoise
            ];
        }
    }
}